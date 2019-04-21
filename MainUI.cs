using Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace xUtilities
{
    public partial class MainUI : Form
    {
        public H.CustomMenuStrip menu;
        public PictureBox pictureBoxHide;
        public MainUI()
        {
            InitializeComponent();
            MinimumSize = new Size(1, 1);
            Location = new Point(0, 0);
            IsMdiContainer = true;
            RefreshUI();
        }
        public void RefreshUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    PrivateBuildUI();
                });
            }
            else
            {
                PrivateBuildUI();
            }
        }
        private void PrivateBuildUI()
        {
            pictureBoxHide = new PictureBox();
            menu = new H.CustomMenuStrip();

            pictureBoxHide.Height = Main.settings.MainUIHeight;
            pictureBoxHide.Width = Main.settings.MainUIHeight;
            pictureBoxHide.Image = Properties.Resources.Minimize;
            pictureBoxHide.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxHide.BackColor = Main.settings.MainUIBackColor;
            pictureBoxHide.Click += pictureBoxHide_Click;

            menu.Font = new Font("Maiandra GD", (Main.settings.MainUIHeight/(float)30) * 10);
            // Important for the buttons to take the whole space provided
            menu.Padding = new Padding(0);
            // Needed for controllable size from the Settings
            menu.AutoSize = false;
            menu.Height = Main.settings.MainUIHeight;
            menu.RightToLeft = RightToLeft.Yes;
            menu.Dock = DockStyle.Top;

            // Misc
            ToolStripMenuItem general = new ToolStripMenuItem("Misc", null, new EventHandler(menuItemClick), "misc");
            ToolStripMenuItem settings = new ToolStripMenuItem("Settings", null, new EventHandler(menuItemClick), "settings");
            settings.Image = Properties.Resources.Settings;
            ToolStripMenuItem chat = new ToolStripMenuItem("Chat", null, new EventHandler(menuItemClick), "chat");
            ToolStripMenuItem speedHack = new ToolStripMenuItem("Speed Hack", null, new EventHandler(menuItemClick), "speedHack");
            ToolStripMenuItem flyHack = new ToolStripMenuItem("Fly Hack", null, new EventHandler(menuItemClick), "flyHack");
            ToolStripMenuItem autoRevive = new ToolStripMenuItem("Auto Revive", null, new EventHandler(menuItemClick), "autoRevive");
            ToolStripMenuItem zoomHack = new ToolStripMenuItem("Zoom Hack", null, new EventHandler(menuItemClick), "zoomHack");
            ToolStripMenuItem eidolon = new ToolStripMenuItem("Auto Eidolon", null, new EventHandler(menuItemClick), "autoEidolon");
            general.DropDownItems.Add(settings);
            general.DropDownItems.Add(chat);
            general.DropDownItems.Add(speedHack);
            general.DropDownItems.Add(flyHack);
            general.DropDownItems.Add(autoRevive);
            general.DropDownItems.Add(zoomHack);
            general.DropDownItems.Add(eidolon);

            // Skandia
            ToolStripMenuItem skandia = new ToolStripMenuItem("Skandia", null, new EventHandler(menuItemClick));
            ToolStripMenuItem skandiaSeller = new ToolStripMenuItem("Seller", null, new EventHandler(menuItemClick), "seller");
            ToolStripMenuItem skandiaFishing = new ToolStripMenuItem("Fishing", null, new EventHandler(menuItemClick), "fishing");
            ToolStripMenuItem skandiaArcheology = new ToolStripMenuItem("Archeology", null, new EventHandler(menuItemClick), "archeology");
            ToolStripMenuItem skandiaFight = new ToolStripMenuItem("Fight", null, new EventHandler(menuItemClick), "fight");
            ToolStripMenuItem fishingVendor = new ToolStripMenuItem("Vendor", null, new EventHandler(menuItemClick), "fishingVendor");
            ToolStripMenuItem fishingAssist = new ToolStripMenuItem("Fishing Assist", null, new EventHandler(menuItemClick), "fishingAssist");
            ToolStripMenuItem fishingBot = new ToolStripMenuItem("Fishing Bot", null, new EventHandler(menuItemClick), "fishingBot");
            ToolStripMenuItem archeologyBot = new ToolStripMenuItem("Archeology Bot", null, new EventHandler(menuItemClick), "archeologyBot");
            ToolStripMenuItem archeologyVendor = new ToolStripMenuItem("Vendor", null, new EventHandler(menuItemClick), "archeologyVendor");
            ToolStripMenuItem farmBot = new ToolStripMenuItem("Farm Bot", null, new EventHandler(menuItemClick), "farmBot");
            ToolStripMenuItem farmBotVendor = new ToolStripMenuItem("Vendor", null, new EventHandler(menuItemClick), "farmBotVendor");
            ToolStripMenuItem selfDefence = new ToolStripMenuItem("Self Defence", null, new EventHandler(menuItemClick), "selfDefence");
            ToolStripMenuItem autoSkill = new ToolStripMenuItem("Auto Skill", null, new EventHandler(menuItemClick), "autoSkill");
            skandia.DropDownItems.Add(skandiaSeller);
            var SellerProfilesList = H.GetSellerProfiles();
            foreach (var item in SellerProfilesList)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(item, null, new EventHandler(menuItemClick));
                newItem.Tag = "SellerProfile";
                skandiaSeller.DropDownItems.Add(newItem);
            }
            skandiaFight.DropDownItems.Add(farmBot);
            skandiaFight.DropDownItems.Add(farmBotVendor);
            skandiaFight.DropDownItems.Add(selfDefence);
            skandiaFight.DropDownItems.Add(autoSkill);
            skandia.DropDownItems.Add(skandiaFight);
            skandiaArcheology.DropDownItems.Add(archeologyBot);
            skandiaArcheology.DropDownItems.Add(archeologyVendor);
            skandia.DropDownItems.Add(skandiaArcheology);
            skandiaFishing.DropDownItems.Add(fishingBot);
            skandiaFishing.DropDownItems.Add(fishingVendor);
            skandiaFishing.DropDownItems.Add(fishingAssist);
            skandia.DropDownItems.Add(skandiaFishing);

            // Plugins
            ToolStripMenuItem plugins = new ToolStripMenuItem("Plugins", null, new EventHandler(menuItemClick));
            var pluginsList = Skandia.Core.GetPlugins();
            foreach (var item in pluginsList.Keys)
            {
                if (item == "xUtilities")
                    continue;
                ToolStripMenuItem newItem = new ToolStripMenuItem(item.ToString(), null, new EventHandler(menuItemClick), "Plugin" + item);
                newItem.Tag = "Plugin";
                ToolStripMenuItem newItemSettigs = new ToolStripMenuItem("Settings", null, new EventHandler(menuItemClick));
                newItemSettigs.Tag = "PluginSettings";
                newItem.DropDownItems.Add(newItemSettigs);
                plugins.DropDownItems.Add(newItem);
            }

            // Developer
            ToolStripMenuItem developer = new ToolStripMenuItem("Developer", null, new EventHandler(menuItemClick));
            ToolStripMenuItem test = new ToolStripMenuItem("Test", null, new EventHandler(menuItemClick), "test");
            developer.DropDownItems.Add(test);
            
            // Render everything from scratch and set the menu
            menu.Items.Add(general);
            menu.Items.Add(skandia);
            menu.Items.Add(plugins);
            menu.Items.Add(developer);
            MainMenuStrip = menu;
            menu.RenderMode = ToolStripRenderMode.Professional;
            menu.Renderer = new H.CustomToolStripMenuRenderer();
            Controls.Clear();
            Controls.Add(pictureBoxHide);
            Controls.Add(menu);
            ChangeSizes();
            UpdateColors();
            H.Log(0, "[UI]Building the UI");
            H.Log(0, "[UI]Finished building the UI");
        }

        private void UpdateColors()
        {
            _restoreForeColors();
            var stripItems = GetAllToolStripMenuItems();
            // Misc
            var chat = stripItems.FirstOrDefault(x => x.Name == "chat");
            if (chat != null && Skandia.Core.GetChatFilterState())
                chat.ForeColor = Color.Green;
            var speedHack = stripItems.FirstOrDefault(x => x.Name == "speedHack");
            if (speedHack != null && Skandia.Core.GetSpeedHackState())
                speedHack.ForeColor = Color.Green;
            var flyHack = stripItems.FirstOrDefault(x => x.Name == "flyHack");
            if (flyHack != null && Skandia.Core.GetFlyHackState())
                    flyHack.ForeColor = Color.Green;
            var autoRevive = stripItems.FirstOrDefault(x => x.Name == "autoRevive");
            if (autoRevive != null && Skandia.Core.GetAutoRessurectState())
                    autoRevive.ForeColor = Color.Green;
            var eidolon = stripItems.FirstOrDefault(x => x.Name == "autoEidolon");
            if (eidolon != null && Skandia.Core.GetEidolonLinkingState())
                    eidolon.ForeColor = Color.Green;
            // Plugins
            var pluginsList = Skandia.Core.GetPlugins();
            foreach (var item in pluginsList.Keys)
            {
                var _pluginStrip = stripItems.FirstOrDefault(x => x.Name == "Plugin" + item);
                if (_pluginStrip != null)
                {
                    if (pluginsList[item])
                        _pluginStrip.ForeColor = Color.Green;
                    else
                        _pluginStrip.ForeColor = Main.settings.MainUIForeColor;
                }
            }
            // Skandia
            var seller = stripItems.FirstOrDefault(x => x.Name == "seller");
            if (seller != null && Skandia.Core.Seller.IsSelling())
                seller.ForeColor = Color.Green;
            var fishing = stripItems.FirstOrDefault(x => x.Name == "fishing");
            if (fishing != null && Skandia.Core.GetFishingBotState())
                fishing.ForeColor = Color.Green;
            var archeology = stripItems.FirstOrDefault(x => x.Name == "archeology");
            if (archeology != null && Skandia.Core.GetArchaeologyBotState())
                archeology.ForeColor = Color.Green;
            var archeologyBot = stripItems.FirstOrDefault(x => x.Name == "archeologyBot");
            if (archeologyBot != null && Skandia.Core.GetArchaeologyBotState())
                archeologyBot.ForeColor = Color.Green;
            var archeologyVendor = stripItems.FirstOrDefault(x => x.Name == "archeologyVendor");
            if (archeologyVendor != null && Skandia.Core.GetArchaeologyBotVendorId() != 0)
                archeologyVendor.Text = "Vendor " + ObjectManager.GetTemplateInfo(Skandia.Core.GetArchaeologyBotVendorId()).Name;
            var fight = stripItems.FirstOrDefault(x => x.Name == "fight");
            if (fight != null && Skandia.Core.GetFarmBotState())
                fight.ForeColor = Color.Green;
            var farmBot = stripItems.FirstOrDefault(x => x.Name == "farmBot");
            if (farmBot != null && Skandia.Core.GetFarmBotState())
                farmBot.ForeColor = Color.Green;
            var farmBotVendor = stripItems.FirstOrDefault(x => x.Name == "farmBotVendor");
            if (farmBotVendor != null && Skandia.Core.GetArchaeologyBotVendorId() != 0)
                farmBotVendor.Text = "Vendor " + ObjectManager.GetTemplateInfo(Skandia.Core.GetFarmBotVendorId()).Name;
            var fishingVendor = stripItems.FirstOrDefault(x => x.Name == "fishingVendor");
            if (fishingVendor != null && Skandia.Core.GetArchaeologyBotVendorId() != 0)
                fishingVendor.Text = "Vendor " + ObjectManager.GetTemplateInfo(Skandia.Core.GetFishingBotVendorId()).Name;
            var fishingAssist = stripItems.FirstOrDefault(x => x.Name == "fishingAssist");
            if (fishingAssist != null && Skandia.Core.GetFishingAssistState())
                fishingAssist.ForeColor = Color.Green;
            var fishingBot = stripItems.FirstOrDefault(x => x.Name == "fishingBot");
            if (fishingBot != null && Skandia.Core.GetFishingBotState())
                fishingBot.ForeColor = Color.Green;
            var selfDefence = stripItems.FirstOrDefault(x => x.Name == "selfDefence");
            if (selfDefence != null && Skandia.Core.GetSelfDefenseState())
                selfDefence.ForeColor = Color.Green;
            var autoSkill = stripItems.FirstOrDefault(x => x.Name == "autoSkill");
            if (autoSkill != null && Skandia.Core.GetAutoBuffState())
                autoSkill.ForeColor = Color.Green;
        }

        private void menuItemClick(object sender, EventArgs e)
        {
            var menuItemClicked = sender as ToolStripMenuItem;
            //Misc
            if (menuItemClicked.Name == "test")
            {
                NotificationManager.SendNotification(NotificationType.YesNo,"Test");
            }
            else if (menuItemClicked.Name == "settings")
            {
                Main.SettingsClick();
            }
            else if (menuItemClicked.Name == "chat")
            {
                if (Skandia.Core.GetChatFilterState())
                {
                    Skandia.Core.ToggleChatFilterBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleChatFilterBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "speedHack")
            {
                // TODO
                if (Skandia.Core.GetSpeedHackState())
                {
                    Skandia.Core.ToggleSpeedHackBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleSpeedHackBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "flyHack")
            {
                if (Skandia.Core.GetFlyHackState())
                {
                    Skandia.Core.ToggleFlyHackBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFlyHackBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "autoRevive")
            {
                if (Skandia.Core.GetAutoRessurectState())
                {
                    Skandia.Core.ToggleAutoRessurect(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleAutoRessurect(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "zoomHack")
            {
                // TODO
            }
            else if (menuItemClicked.Name == "autoEidolon")
            {
                if (Skandia.Core.GetEidolonLinkingState())
                {
                    Skandia.Core.ToggleEidolonLinking(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleEidolonLinking(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            // Plugins
            else if (menuItemClicked.Tag != null && menuItemClicked.Tag.ToString() == "Plugin")
            {
                string pluginName = menuItemClicked.Text;
                if (menuItemClicked.ForeColor == Color.Green)
                {
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    Skandia.Core.StopPlugin(pluginName);
                }
                else
                {
                    menuItemClicked.ForeColor = Color.Green;
                    Skandia.Core.StartPlugin(pluginName);
                }
            }
            else if (menuItemClicked.Tag != null && menuItemClicked.Tag.ToString() == "PluginSettings")
            {
                string pluginName = menuItemClicked.OwnerItem.Text;
                Skandia.Core.TogglePluginSettings(pluginName);
            }
            else if (menuItemClicked.Name == "seller")
            {
                if (Skandia.Core.Seller.IsSelling())
                {
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    Skandia.Core.Seller.StopSelling();
                }
                else
                {
                    // TODO
                }
            }
            // Skandia -> Seller
            else if (menuItemClicked.Tag != null && menuItemClicked.Tag.ToString() == "SellerProfile")
            {
                if (Skandia.Core.Seller.IsIdle() && Skandia.Me.GotTarget)
                {
                    H.Log(0, "Starting selling using {0} profile to {1}", menuItemClicked.Text, Skandia.Me.CurrentTarget.Name);
                    Skandia.Core.Seller.StartSelling(menuItemClicked.Text.ToString(), Skandia.Me.CurrentTarget.Template.Id);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.OwnerItem.ForeColor = Color.Green;
                    menuItemClicked.OwnerItem.OwnerItem.ForeColor = Color.Green;
                }
                else
                {
                    H.Log(0, "Seller Stopped");
                    Skandia.Core.Seller.StopSelling();
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.OwnerItem.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.OwnerItem.OwnerItem.ForeColor = Main.settings.MainUIForeColor;
                }
            }
            else if (menuItemClicked.Name == "fishing")
            {
                if (Skandia.Core.GetFishingBotState())
                {
                    Skandia.Core.ToggleFishingBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.DropDownItems.Find("fishingBot", true).First().ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFishingBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.DropDownItems.Find("fishingBot", true).First().ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "fishingBot")
            {
                if (Skandia.Core.GetFishingBotState())
                {
                    Skandia.Core.ToggleFishingBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.OwnerItem.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFishingBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.OwnerItem.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "fishingVendor")
            {
                if (Skandia.Me.GotTarget && Skandia.Me.CurrentTarget.IsValid && Skandia.Me.CurrentTarget.Template != null)
                {
                    Skandia.Core.SetFishingBotVendorId(Skandia.Me.CurrentTarget.Template.Id);
                    menuItemClicked.Text = "Vendor: " + Skandia.Me.CurrentTarget.Template.Name;
                }
            }
            else if (menuItemClicked.Name == "archeologyVendor")
            {
                if (Skandia.Me.GotTarget && Skandia.Me.CurrentTarget.IsValid && Skandia.Me.CurrentTarget.Template != null)
                {
                    Skandia.Core.SetArchaeologyBotVendorId(Skandia.Me.CurrentTarget.Template.Id);
                    menuItemClicked.Text = "Vendor: " + Skandia.Me.CurrentTarget.Template.Name;
                }
            }
            else if (menuItemClicked.Name == "farmBotVendor")
            {
                if (Skandia.Me.GotTarget && Skandia.Me.CurrentTarget.IsValid && Skandia.Me.CurrentTarget.Template != null)
                {
                    Skandia.Core.SetFarmBotVendorId(Skandia.Me.CurrentTarget.Template.Id);
                    menuItemClicked.Text = "Vendor: " + Skandia.Me.CurrentTarget.Template.Name;
                }
            }
            else if (menuItemClicked.Name == "fishingAssist")
            {
                if (Skandia.Core.GetFishingAssistState())
                {
                    Skandia.Core.ToggleFishingAssist(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFishingAssist(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "autoSkill")
            {
                if (Skandia.Core.GetAutoBuffState())
                {
                    Skandia.Core.ToggleAutoBuffBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleAutoBuffBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "selfDefence")
            {
                if (Skandia.Core.GetSelfDefenseState())
                {
                    Skandia.Core.ToggleSelfDefenseBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleSelfDefenseBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "archeology")
            {
                if (Skandia.Core.GetArchaeologyBotState())
                {
                    Skandia.Core.ToggleArchaeologyBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.DropDownItems.Find("archeologyBot", true).First().ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleArchaeologyBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.DropDownItems.Find("archeologyBot", true).First().ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "archeologyBot")
            {
                if (Skandia.Core.GetArchaeologyBotState())
                {
                    Skandia.Core.ToggleArchaeologyBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.OwnerItem.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleArchaeologyBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.OwnerItem.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "farmBot")
            {
                if (Skandia.Core.GetFarmBotState())
                {
                    Skandia.Core.ToggleFarmBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.OwnerItem.ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFarmBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.OwnerItem.ForeColor = Color.Green;
                }
            }
            else if (menuItemClicked.Name == "fight")
            {
                if (Skandia.Core.GetFarmBotState())
                {
                    Skandia.Core.ToggleFarmBot(false);
                    menuItemClicked.ForeColor = Main.settings.MainUIForeColor;
                    menuItemClicked.DropDownItems.Find("farmBot", true).First().ForeColor = Main.settings.MainUIForeColor;
                }
                else
                {
                    Skandia.Core.ToggleFarmBot(true);
                    menuItemClicked.ForeColor = Color.Green;
                    menuItemClicked.DropDownItems.Find("farmBot", true).First().ForeColor = Color.Green;
                }
            }
        }
        // Changes the colors for all the items of the mainUI - used by RefreshColors
        public void _restoreForeColors()
        {
            menu.BackColor = Main.settings.MainUIBackColor;
            menu.ForeColor = Main.settings.MainUIForeColor;
            var list = GetAllToolStripMenuItems();

            if (list.Count == 0)
                return;
            foreach (var item in list)
            {
                item.BackColor = Main.settings.MainUIBackColor;
                item.ForeColor = Main.settings.MainUIForeColor;
            }
        }
        // Used in methods that make changes to the every MainUI element
        private List<ToolStripMenuItem> GetAllToolStripMenuItems()
        {
            List<ToolStripMenuItem> allItems = new List<ToolStripMenuItem>();
            foreach (ToolStripMenuItem toolItem in menu.Items)
            {
                allItems.Add(toolItem);
                allItems.AddRange(_getToolStripDropDownItems(toolItem));
            }
            return allItems;
        }
        // Used to return all ToolStripMenuItem
        private IEnumerable<ToolStripMenuItem> _getToolStripDropDownItems(ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem dropDownItem in item.DropDownItems)
            {
                if (dropDownItem.HasDropDownItems)
                {
                    foreach (ToolStripMenuItem subItem in _getToolStripDropDownItems(dropDownItem))
                        yield return subItem;
                }
                yield return dropDownItem;
            }
        }
        public void ChangeSizes()
        {
            Size = new Size(Main.settings.MainUIWidth, Main.settings.MainUIHeight);
            if (menu != null)
                menu.Height = Main.settings.MainUIHeight;
            if (pictureBoxHide != null)
            {
                pictureBoxHide.Height = Main.settings.MainUIHeight;
                pictureBoxHide.Width = Main.settings.MainUIHeight;
            }
        }

        // Disallow window closing with Alt+F4 or any other way except from within the code
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        // Hide window from Alt+Tab
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }
        
        // Maybe to be used as running from the same thread and with Invoker like the Start/Stop
        public void SafeShow()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Show();
                });
            }
            else
            {
                Show();
            }
            H.Log(0, "[UI]Showing MainUI");
        }
        
        public void SafeHide()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    Hide();
                });
            }
            else
            {
                Hide();
            }
            H.Log(0, "[UI]Hiding MainUI");
        }
        
        private void pictureBoxHide_Click(object sender, EventArgs e)
        {
            if (Main.settings.MainUIMaximized)
                Minimize();
            else
                Maximize();
            Main.settings.MainUIMaximized = !Main.settings.MainUIMaximized;
        }

        public void Maximize()
        {
            Size = new Size(Main.settings.MainUIWidth, Main.settings.MainUIHeight);
            pictureBoxHide.Image = Properties.Resources.Minimize;
            H.Log(0, "[UI]Maximizing MainUI");
            Show();
            menu.Visible = true;
        }

        public void Minimize()
        {
            Size = new Size(Main.settings.MainUIHeight, Main.settings.MainUIHeight);
            pictureBoxHide.Image = Properties.Resources.Maximize;
            H.Log(0, "[UI]Minimizing MainUI");
            Hide();
            menu.Visible = false;
        }

        private void MainUI_Load(object sender, EventArgs e)
        {
            Main.KillSplashScreen();
        }
    }
}
