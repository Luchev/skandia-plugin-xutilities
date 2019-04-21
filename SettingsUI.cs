using Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xUtilities
{
    public partial class SettingsUI : Form
    {
        public SettingsUI()
        {
            InitializeComponent();
            UpdateSellerProfiles();
            UpdateSettingsUI();
            MaximizeBox = false;

        }

        private void UpdateSellerProfiles()
        {
            var sellerProfiles = H.GetSellerProfiles();

            foreach (var item in sellerProfiles)
            {
                comboBox1.Items.Add(item);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Main.settings.MainUIHeight = (int)numericUpDown1.Value;
            Main.mainUI.ChangeSizes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.mainUI.RefreshUI();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Main.settings.SellerProfile = comboBox1.SelectedItem.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            H.LoadSettings(Skandia.Me.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            H.SaveSettings(Skandia.Me.Name);
        }

        public void UpdateSettingsUI()
        {
            numericUpDown1.Value = Main.settings.MainUIHeight;
            comboBox1.SelectedIndex = comboBox1.FindStringExact(Main.settings.SellerProfile);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Main.settings.GetInfoAboutNPCsAround = checkBox1.Checked;
        }

        internal void RefreshUI()
        {
            checkBox1.Checked = Main.settings.GetInfoAboutNPCsAround;
            Main.PendingSettingsUIRefresh = false;
        }

        private void SettingsUI_Load(object sender, EventArgs e)
        {
            if (Main.PendingSettingsUIRefresh)
                RefreshUI();
        }
    }
}
