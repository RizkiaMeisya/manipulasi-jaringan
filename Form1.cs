using System.Diagnostics;
using System.Security.Principal;
using System.Net;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!IsAdmin())
            {
                RestartElevated();
            }
            bsi_hotspot( null, null, false);
            Application.Exit();
        }

        private void bsi_hotspot(string ssid, string key, bool status)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);

            if (process != null)
            {
                if (status)
                {
                    process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + ssid + " key=" + key);
                    // pemangilan untuk membuat SSID
                    process.StandardInput.WriteLine("netsh wlan start hosted network"); process.StandardInput.Close();
                }

                else
                {
                    process.StandardInput.WriteLine("netsh wlan stop hostednetwork");
                    process.StandardInput.Close();
                }
            }
        }

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id); return
            p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void RestartElevated()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
            }
            catch
            { }
            System.Windows.Forms.Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ssid = textBox1.Text, key = textBox2.Text;
            bool connect = false; //tambahan mendeskripsikan connect
            if (!connect)
            {
                if (textBox1.Text == null || textBox1.Text == "")
                {
                    MessageBox.Show("Anda Belum Mengisi SSID !", "Informasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (textBox2.Text == null || textBox2.Text == "")
                    {
                        MessageBox.Show("Anda Belum Mengisi Password !", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {

                        if (key.Length >= 6)
                        {
                            bsi_hotspot(ssid, key, true);
                            textBox1.Enabled = false;
                            textBox2.Enabled = false;
                            button1.Text = "Berhenti";
                            connect = true;
                        }
                        else
                        {
                            MessageBox.Show("Password harus 6 karakter atau lebih !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                bsi_hotspot(null, null, false);
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Text = "START";
                bool X = false;
            }
        }

    }
}
