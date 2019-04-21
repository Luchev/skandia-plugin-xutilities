using Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace xUtilities
{
    public static class H
    {
        private static XmlSerializer serializer;
        public static T DeserializeFromFile<T>(string file, bool useCutomDirectory = false)
        {
            try
            {
                if (!file.EndsWith(".xml"))
                    file = file + ".xml";

                serializer = new XmlSerializer(typeof(T));
                if (useCutomDirectory)
                    using (var reader = new StreamReader(file))
                        return (T)serializer.Deserialize(reader);
                else
                    using (var reader = new StreamReader(Path.Combine(ProfilesDirectory, file)))
                        return (T)serializer.Deserialize(reader);
            }
            catch
            {
                Log(0, "Failed to deserialize file");
                return default(T);
            }
        }
        public static void SerializeToFile<T>(string file, T instance, bool useCustomDirectory = false)
        {
            try
            {
                if (!file.EndsWith(".xml"))
                    file = file + ".xml";

                serializer = new XmlSerializer(typeof(T));
                if (useCustomDirectory)
                    using (var writer = new StreamWriter(file))
                        serializer.Serialize(writer, instance);
                else
                    using (var writer = new StreamWriter(Path.Combine(ProfilesDirectory, file)))
                        serializer.Serialize(writer, instance);
            }
            catch
            {
                Log(0, "Failed to serialize file");
            }
        }
        public static string SerializeToString<T>(T instance)
        {
            try
            {
                serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, instance);
                    return writer.ToString();
                }
            }
            catch
            {
                Log(0, "Failed to serialize string");
                return "";
            }
        }
        public static T DeserializeFromString<T>(string data)
        {
            try
            {
                serializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(data))
                    return (T)serializer.Deserialize(reader);
            }
            catch
            {
                Log(0, "Failed to deserialize from string");
                return default(T);
            }
        }
        public static string ProfilesDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(Directory.GetParent(Path.GetDirectoryName(path)).FullName, "profiles", "plugins", "xUtilities");
            }
        }
        public static string GenerateProfileName(string file)
        { 
            if (!file.EndsWith(".xml"))
                file = file + ".xml";
            return Path.Combine(ProfilesDirectory, file);
        }
        public static string SellerProfilesDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(Directory.GetParent(Path.GetDirectoryName(path)).FullName, "profiles", "seller");
            }
        }
        public static List<string> GetSellerProfiles()
        {
            List<string> files = Directory.GetFiles(SellerProfilesDirectory, "*.akss").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = Path.GetFileNameWithoutExtension(files[i]);
            }
            return files;
        }
        public static string PluginsDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.Combine(Directory.GetParent(Path.GetDirectoryName(path)).FullName, "plugins");
            }
        }
        // For color management tweak the CustomColorTable ↓
        public class CustomToolStripMenuRenderer : ToolStripProfessionalRenderer
        {
            public CustomToolStripMenuRenderer() : base(new CustomColorTable()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
                Color c = e.Item.Selected ? Main.settings.MainUIBackColor : Main.settings.MainUIBackColor;
                using (SolidBrush brush = new SolidBrush(c))
                    e.Graphics.FillRectangle(brush, rc);
            }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = Main.settings.MainUIForeColor;
                base.OnRenderArrow(e);
            }
        }
        // Used by the Custom ToolStripMenuRenderer ↑
        public class CustomColorTable : ProfessionalColorTable
        {
            public override Color MenuItemBorder
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color MenuBorder
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color MenuItemSelected
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Main.settings.MainUIBackColor; }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Main.settings.MainUIBackColor; }
            }

            public override Color MenuItemPressedGradientBegin
            {
                get { return Main.settings.MainUIBackColor; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripContentPanelGradientBegin
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripContentPanelGradientEnd
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripGradientBegin
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripGradientEnd
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripPanelGradientBegin
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripPanelGradientEnd
            {
                get { return Main.settings.MainUIBackColor; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return Main.settings.MainUIBackColor; }
            }
        }
        public static void Log(int level, params object[] values)
        {
            if (Main.settings?.LogLevel != null && Main.settings.LogLevel >= level)
            {
                string message = "[xUtilities]";
                foreach (var item in values)
                {
                    message += item.ToString();
                }
                Skandia.MessageLog(message);
            }
        }
        public static void LoadSettings(string fileName = "")
        {
            if (fileName != "" && File.Exists(Path.Combine(ProfilesDirectory, fileName + ".xml")))
            {
                Main.settings = DeserializeFromFile<Settings>(fileName);
                Log(0, "[H]Loading settings for character ", fileName);
            }
            else
            {
                Log(0, "[H]Loading default settings");
                Main.settings = new Settings();
                if (fileName != "")
                    SaveSettings(fileName);
            }
            UpdateSettings();
        }
        private static void UpdateSettings()
        {
            Log(0, "[H]Updating settings");
            if (Main.settingsUI != null)
            {
                Main.settingsUI.RefreshUI();
            }
            else
            {
                Main.PendingSettingsUIRefresh = true;
            }
            if (Main.mainUI != null)
            {
                Main.mainUI.RefreshUI();
            }
            else
            {
                Log(0, "[ERROR]Failed to update settings");
            }
        }
        public static void SaveSettings(string fileName)
        {
            SerializeToFile(fileName, Main.settings);
            Log(0, "[H]Saving settings for character ", fileName);
        }
        public class CustomMenuStrip : MenuStrip
        {
            // Used to make the controls clickable without focus
            const uint WM_LBUTTONDOWN = 0x201;
            const uint WM_LBUTTONUP = 0x202;

            static private bool down = false;

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_LBUTTONUP && !down)
                {
                    m.Msg = (int)WM_LBUTTONDOWN; base.WndProc(ref m);
                    m.Msg = (int)WM_LBUTTONUP;
                }

                if (m.Msg == WM_LBUTTONDOWN) down = true;
                if (m.Msg == WM_LBUTTONUP) down = false;

                base.WndProc(ref m);
            }
        }
        //public static List<GameEvent> ConvertGameEventSerializeableListToGameEventList(List<GameEventSerializeable> gameEventSerializeableList)
        //{
        //    List<GameEvent> list = new List<GameEvent>();
        //    foreach (var item in gameEventSerializeableList)
        //    {
        //        list.Add(ConvertGameEventSerializeableToGameEvent(item));
        //    }
        //    return list;
        //}

        //public static List<GameEventSerializeable> ConvertGameEventListToGameEventSerializeableList(List<GameEvent> gameEventList)
        //{
        //    List<GameEventSerializeable> list = new List<GameEventSerializeable>();
        //    foreach (var item in gameEventList)
        //    {
        //        list.Add(ConvertGameEventToGameEventSerializeable(item));
        //    }
        //    return list;
        //}
        //public static GameEvent ConvertGameEventSerializeableToGameEvent(GameEventSerializeable gameEventSerializeable)
        //{
        //    var e = new GameEvent();
        //    e.Name = gameEventSerializeable.Name;
        //    e.Bait = gameEventSerializeable.Bait;
        //    e.Map = gameEventSerializeable.Map;
        //    e.Location2D = gameEventSerializeable.Location2D;
        //    e.Location3D = gameEventSerializeable.Location3D;
        //    e.Type = gameEventSerializeable.Type;
        //    e.Days = gameEventSerializeable.Days;
        //    e.Tooltip = gameEventSerializeable.Tooltip;
        //    e.Note = "";

        //    e.StartTimes = new List<TimeSpan>();
        //    e.EndTimes = new List<TimeSpan>();

        //    foreach (int f in gameEventSerializeable.StartTimes)
        //    {
        //        e.StartTimes.Add(TimeSpan.FromSeconds(f));
        //    }
        //    foreach (int y in gameEventSerializeable.EndTimes)
        //    {
        //        e.EndTimes.Add(TimeSpan.FromSeconds(y));
        //    }

        //    return e;
        //}
        //public static GameEventSerializeable ConvertGameEventToGameEventSerializeable(GameEvent gameEvent)
        //{
        //    var e = new GameEventSerializeable();
        //    e.Name = gameEvent.Name;
        //    e.Bait = gameEvent.Bait;
        //    e.Map = gameEvent.Map;
        //    e.Location2D = gameEvent.Location2D;
        //    e.Location3D = gameEvent.Location3D;
        //    e.Type = gameEvent.Type;
        //    e.Days = gameEvent.Days;
        //    e.Tooltip = gameEvent.Tooltip;
        //    e.Note = "";

        //    foreach (var f in gameEvent.StartTimes)
        //    {
        //        e.StartTimes.Add((int)f.TotalSeconds);
        //    }
        //    foreach (var y in gameEvent.EndTimes)
        //    {
        //        e.EndTimes.Add((int)y.TotalSeconds);
        //    }

        //    return e;
        //}

        //public static List<GameEvent> DefaultGameEvents()
        //{
        //    List<GameEventSerializeable> s = DeserializeFromString<List<GameEventSerializeable>>(Properties.Resources.Timers);
        //    return ConvertGameEventSerializeableListToGameEventList(s);

        //}
    }
}
