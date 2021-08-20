using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CustomAlertBoxDemo
{
    public partial class Form1 : Form
    {
        HubConnection connection;
        public Form1()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/hubs/todo").Build();
        }
        public void Alert(string msg, Form_Alert.enmType type)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg,type);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Alert("Success Alert", Form_Alert.enmType.Success);
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            connection.On<string>("NotificationAdd", (message) =>
            {
                this.Invoke((Action)(() =>
                {
                    this.Alert(message, Form_Alert.enmType.Success);
                }));
            });

            connection.On<string>("NotificationDelete", (message) =>
            {
                this.Invoke((Action)(() =>
                {
                    this.Alert(message, Form_Alert.enmType.Warning);
                }));
            });

            connection.On<string>("NotificationUpdate", (message) =>
            {
                this.Invoke((Action)(() =>
                {
                    this.Alert(message, Form_Alert.enmType.Success);
                }));
            });

            try
            {
                await connection.StartAsync();
                this.Alert("Connection started", Form_Alert.enmType.Success);
                //connectButton.IsEnabled = false;
                //sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message, Form_Alert.enmType.Error);
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            bool cursorNotInBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
