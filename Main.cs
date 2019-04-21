using System;
using PluginsCommon;
using System.Threading;
using Plugins;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xUtilities
{
    public class Main : IPlugin, IPluginChat
    {
        public static MainUI mainUI { get; set; }
        public static Thread mainUIThread { get; set; }
        public static SettingsUI settingsUI { get; set; }
        public static Thread settingsUIThread { get; set; }
        public static Splash splash { get; set; }
        public static Settings settings;
        public static string Character;
        private Stopwatch BackgroundTimer;
        public static bool PendingSettingsUIRefresh = false;
        public List<Other.NPC> NPCs;
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        public string Author
        {
            get
            {
                return "Hachiman";
            }
        }
        public string Description
        {
            get
            {
                return "Extra Utilities for Skandia";
            }
        }
        public string Name
        {
            get
            {
                return "xUtilities";
            }
        }
        public Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
        public void OnButtonClick()
        {
            SettingsClick();
        }
        public static void SettingsClick()
        {
            // Handle all cases of SettingsUI
            if (settingsUI != null && settingsUI.Visible == false)
            {
                settingsUI.Show();
                Skandia.MessageLog("[Main]Showing SettingsUI");
            }
            else if (settingsUI != null && settingsUI.Visible == true)
            {
                settingsUI.Hide();
                Skandia.MessageLog("[Main]Hiding SettingsUI");
            }
            else
            {
                StartSettingsUIThread();
            }
        }
        public void OnChatMessage(string message, uint type, bool isWhisper)
        {
            // TODO
        }
        public void OnStart()
        {
            CreateSplash();
            settings = new Settings();
            BackgroundTimer = new Stopwatch();
            Character = "  ";
            NotificationManager.Initialize();
            StartMainUIThread();
            if (!Directory.Exists(H.ProfilesDirectory))
                Directory.CreateDirectory(H.ProfilesDirectory);
            NPCs = new List<Other.NPC>();
        }
        private void CreateSplash()
        {
            var s = new Thread(privateTest);
            s.SetApartmentState(ApartmentState.STA);
            s.Start();
        }

        private void privateTest()
        {
            splash = new Splash();
            splash.ShowDialog();
        }
        public static void StartSettingsUIThread()
        {
            if (settingsUIThread == null || 
            settingsUIThread.ThreadState == System.Threading.ThreadState.Aborted ||
            settingsUIThread.ThreadState == System.Threading.ThreadState.Stopped ||
            settingsUIThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                settingsUIThread = new Thread(SettingsUIThreadStartMethod);
                settingsUIThread.SetApartmentState(ApartmentState.STA);
                settingsUIThread.Start();
                return;
            }
            else
            {
                H.Log(0, "[ERROR]SettingsUI failed to initialize");
                return;
            }
        }
        // Used by settingUIThread as starting method
        public static void SettingsUIThreadStartMethod()
        {
            if (settingsUI == null)
            {
                settingsUI = new SettingsUI();
            }
            settingsUI.ShowDialog();
            H.Log(0, "[Main]Starting SettingsUI");
        }
        public void OnStop(bool off)
        {
            // Stop NotificationManager
            NotificationManager.Terminate();
            // Stop SettingsUI
            if (settingsUI != null && settingsUI.InvokeRequired)
            {
                settingsUI.Invoke((MethodInvoker)delegate
                {
                    settingsUI.Close();
                    settingsUI.Dispose();
                });
            }
            else
            {
                if (settingsUI != null && !settingsUI.IsDisposed)
                {
                    settingsUI.Close();
                    settingsUI.Dispose();
                }
            }
            H.Log(0, "[Main]Stopping SettingsUI");
            // Stop MainUI
            if (mainUI != null && mainUI.InvokeRequired)
            {
                mainUI.Invoke((MethodInvoker)delegate
                {
                    mainUI.Close();
                    mainUI.Dispose();
                });
            }
            else
            {
                if (mainUI != null && !mainUI.IsDisposed)
                {
                    mainUI.Close();
                    mainUI.Dispose();
                }
            }
            H.Log(0, "[Main]Stopping MainUI");
        }
        public void Pulse()
        {
            Skandia.Update();
            if (!Skandia.IsInGame || splash != null)
            {
                return;
            }
            if (Character == "" || Character != Skandia.Me.Name)
            {
                // Auto Load settings for character
                Character = Skandia.Me.Name;
                H.LoadSettings(Character);
            }
            if (!BackgroundTimer.IsRunning)
                BackgroundTimer.Start();
            // Thigs that run in the background (Mainly dev stuff)
            if (BackgroundTimer.ElapsedMilliseconds > settings.BackgroundTimerCooldown)
            {
                // Handle the auto-hide minimized MainUI
                if (mainUI != null)
                {
                    if (Cursor.Position.X == 0 && Cursor.Position.Y == 0 && mainUI.Visible == false || settings.MainUIMaximized == true && mainUI.Visible == false)
                        mainUI?.SafeShow();
                    else if ((mainUI?.Visible == true && settings.MainUIMaximized == false && (Cursor.Position.X > settings.MainUIHeight || Cursor.Position.Y > settings.MainUIHeight)))
                        mainUI?.SafeHide();
                }
                BackgroundTimer.Restart();
            }
            // TODO Scan NPCs around
            if (settings.GetInfoAboutNPCsAround)
            {
                if (File.Exists(H.GenerateProfileName("NPCs")) && NPCs.Count == 0)
                {
                    H.Log(0, "Loading NPCs");
                    NPCs = H.DeserializeFromFile<List<Other.NPC>>("NPCs");
                }
                var objectss = ObjectManager.ObjectList;
                foreach (var item in objectss)
                {
                    if (item.IsValid && item.Template != null)
                        if (!NPCs.Exists(x => x.Id == item.Template.Id))
                        {
                            NPCs.Add(new Other.NPC(item.Template.Name, item.Guild, item.Template.Id));
                            H.SerializeToFile("NPCs", NPCs);
                            H.Log(0, "ADDING NPC");
                        }
                }
            }
                
        }
        public static void KillSplashScreen()
        {
            if (splash != null)
                Task.Delay(TimeSpan.FromMilliseconds(1000)).ContinueWith((Task) =>
                {
                    if (splash.InvokeRequired)
                    {
                        splash.Invoke((MethodInvoker)delegate {
                            splash.Hide();
                            splash.Dispose();
                        });
                    }
                    else
                    {
                        splash.Hide();
                        splash.Dispose();
                    }
                    splash = null;
                });
        }
        // Create a thread for the MainUI
        public void StartMainUIThread()
        {
            if (mainUIThread == null || 
                mainUIThread.ThreadState == System.Threading.ThreadState.Aborted ||
                mainUIThread.ThreadState == System.Threading.ThreadState.Stopped ||
                mainUIThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                mainUIThread = new Thread(MainUIThreadStartMethod);
                mainUIThread.SetApartmentState(ApartmentState.STA);
                mainUIThread.Start();
                return;
            }
            else
            {
                H.Log(0, "[ERROR]MainUI failed to initialize");
                return;
            }
        }
        // Used by mainUIThread as starting method
        private void MainUIThreadStartMethod()
        {
            if (mainUI == null)
            {
                mainUI = new MainUI();
            }
            if (mainUI.InvokeRequired)
            {
                mainUI.Invoke((MethodInvoker)delegate
                {
                    mainUI.ShowDialog();
                });
            }
            else
            {
                mainUI.ShowDialog();
            }
            H.Log(0, "[Main]Starting MainUI");
        }
    }
}
