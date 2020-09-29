using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubDarkAPI;
using System.Runtime.InteropServices;

namespace CodeSploit
{
    public partial class Form2 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        public static ExploitAPI api = new ExploitAPI();
        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Author: Maks1116 from KaMaO Studio", "CodeSploit v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Functions.openfiledialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    string MainText = File.ReadAllText(Functions.openfiledialog.FileName);
                    webBrowser1.Document.InvokeScript("SetText", new object[]
                    {
                          MainText
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            if (this.listBox1.SelectedIndex != -1)
            {
                this.webBrowser1.Document.InvokeScript("SetText", new object[1]
                {
            (object) System.IO.File.ReadAllText("scripts\\" + this.listBox1.SelectedItem.ToString())
                });
            }
            else
            {
                int num = (int)MessageBox.Show("Please select a script from the list before trying to loading it in tab.", "Name");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument document = webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();
            api.ExecuteScript(script);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                ""
            });
        }
        WebClient wc = new WebClient();
        private string defPath = Application.StartupPath + "//Monaco//";

        private void addIntel(string label, string kind, string detail, string insertText)
        {
            string text = "\"" + label + "\"";
            string text2 = "\"" + kind + "\"";
            string text3 = "\"" + detail + "\"";
            string text4 = "\"" + insertText + "\"";
            webBrowser1.Document.InvokeScript("AddIntellisense", new object[]
            {
                label,
                kind,
                detail,
                insertText
            });
        }

        private void addGlobalF()
        {
            string[] array = File.ReadAllLines(this.defPath + "//globalf.txt");
            foreach (string text in array)
            {
                bool flag = text.Contains(':');
                if (flag)
                {
                    this.addIntel(text, "Function", text, text.Substring(1));
                }
                else
                {
                    this.addIntel(text, "Function", text, text);
                }
            }
        }

        private void addGlobalV()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalv.txt"))
            {
                this.addIntel(text, "Variable", text, text);
            }
        }

        private void addGlobalNS()
        {
            foreach (string text in File.ReadLines(this.defPath + "//globalns.txt"))
            {
                this.addIntel(text, "Class", text, text);
            }
        }

        private void addMath()
        {
            foreach (string text in File.ReadLines(this.defPath + "//classfunc.txt"))
            {
                this.addIntel(text, "Method", text, text);
            }
        }

        private void addBase()
        {
            foreach (string text in File.ReadLines(this.defPath + "//base.txt"))
            {
                this.addIntel(text, "Keyword", text, text);
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Proxy = null;
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                bool flag2 = registryKey.GetValue(friendlyName) == null;
                if (flag2)
                {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
                registryKey = null;
                friendlyName = null;
            }
            catch (Exception)
            {
            }
            webBrowser1.Url = new Uri(string.Format("file:///{0}/Monaco/Monaco.html", Directory.GetCurrentDirectory()));
            await Task.Delay(500);
            webBrowser1.Document.InvokeScript("SetTheme", new string[]
            {
                   "Dark" 
                   /*
                    There are 2 Themes Dark and Light
                   */
            });
            addBase();
            addMath();
            addGlobalNS();
            addGlobalV();
            addGlobalF();
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                 "--CodeSploit v1.0--"
            });
            listBox1.Items.Clear();
            Functions.PopulateListBox(listBox1, "./Scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./Scripts", "*.lua");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            api.LaunchExploit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBox1.Checked;
        }

        private void loadInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                this.webBrowser1.Document.InvokeScript("SetText", new object[1]
                {
                    (object) System.IO.File.ReadAllText("scripts\\" + this.listBox1.SelectedItem.ToString())
                });
            }
        }

        private void executeScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                api.ExecuteScript(System.IO.File.ReadAllText("scripts\\" + this.listBox1.SelectedItem.ToString()));
            }
        }

        private void refreshListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Functions.PopulateListBox(listBox1, "./Scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./Scripts", "*.lua");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            api.KillRoblox();
        }
    }
}
