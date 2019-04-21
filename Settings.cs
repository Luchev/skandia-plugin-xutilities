using Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xUtilities
{
    public class Settings
    {
        [XmlIgnore]
        public int MainUIWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
        [XmlIgnore]
        public Color MainUIForeColor = Color.FromArgb(217, 217, 17);
        [XmlIgnore]
        public Color MainUIBackColor = Color.FromArgb(42, 42, 44);
        public int MainUIHeight { get; set; }
        public int BackgroundTimerCooldown { get; set; }
        // In milliseconds ↓↑
        public int MainTimerCooldown { get; set; }
        [XmlIgnore]
        public bool MainUIMaximized = true;
        public string SellerProfile { get; set; }
        public int LogLevel { get; set; }
        public bool GetInfoAboutNPCsAround { get; set; }

        public Settings()
        {
            RestoreDefaults();
        }

        public void RestoreDefaults()
        {
            MainUIHeight = 30;
            BackgroundTimerCooldown = 100;
            MainTimerCooldown = 5000;
            SellerProfile = "Default";
            LogLevel = 0;
            GetInfoAboutNPCsAround = false;
        }
    }
}
