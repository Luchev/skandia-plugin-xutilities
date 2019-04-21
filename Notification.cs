using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xUtilities
{
    public partial class NotificationWindow : Form
    {
        NotificationType type;
        Label label1;
        Button button1;
        Button button2;
        TextBox textBox1;
        public NotificationWindow()
        {
            InitializeComponent();
            type = NotificationType.YesNo;
            Font = new Font("Maiandra GD", 12);
            BackColor = Main.settings.MainUIBackColor;
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(button2);
            button1.Font = Font;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderColor = Main.settings.MainUIForeColor;
            button1.FlatAppearance.BorderSize = 1;
            button2.Font = Font;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderColor = Main.settings.MainUIForeColor;
            button2.FlatAppearance.BorderSize = 1;
            label1.Font = Font;
        }
        private void RefreshUI(int _heigh)
        {
            Height = _heigh;
            Width = 250;
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width) / 2, Main.settings.MainUIHeight);
            label1.ForeColor = Main.settings.MainUIForeColor;
            label1.Location = new Point(0, 0);
            label1.Width = Width;
            label1.Height = 30;
            label1.AutoSize = false;
            label1.TextAlign = ContentAlignment.TopCenter;
            label1.Dock = DockStyle.Fill;
            // TODO
            if (type == NotificationType.Input)
            {
                textBox1.Font = Font;
                textBox1.ForeColor = Main.settings.MainUIForeColor;
                textBox1.Width = Width;
                textBox1.Height = 30;
                textBox1.Location = new Point(0, 30);
                textBox1.Text = "Input here";
            }
            if (type == NotificationType.YesNo || type == NotificationType.OkCancel)
            {
                button1.ForeColor = Main.settings.MainUIForeColor;
                button1.Location = new Point(0, 30);
                button2.ForeColor = Main.settings.MainUIForeColor;
                button2.Location = new Point(0, 70);
                button1.Show();
            }
        }
        public void SetType(NotificationType _notificationType)
        {
            type = _notificationType;
            if (_notificationType == NotificationType.Text)
            {
                button1.Hide();
                button2.Hide();
                textBox1.Hide();
                RefreshUI(30);
            }
            else if (_notificationType == NotificationType.OkCancel)
            {
                button1.Show();
                button1.Text = "OK";
                button2.Show();
                button2.Text = "Cancel";
                textBox1.Hide();
                Height = 60;
            }
            else if (_notificationType == NotificationType.YesNo)
            {
                //button1.Show();
                button1.Text = "Yes";
                //button2.Show();
                button2.Text = "No";
                textBox1.Hide();
                Height = 60;
            }
            else if (_notificationType == NotificationType.Input)
            {
                button1.Show();
                button2.Show();
                textBox1.Show();
                Height = 90;
            }
        }
        public void Send(string _text)
        {
            label1.Text = _text;
        }
    }
}
