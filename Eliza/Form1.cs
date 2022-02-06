using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eliza
{
    public partial class Form1 : Form
    {
        WebBrowser webBrowser;
        SpeechSynthesizer synthesizer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser = new WebBrowser();
            webBrowser.Navigate("https://www.masswerk.at/elizabot/");
            webBrowser.Navigated += WebBrowser_Navigated;

            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 1;     // -10...10
            try { synthesizer.SelectVoice(synthesizer.GetInstalledVoices()[1].VoiceInfo.Name); } catch { }
        }

        private async void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            await Task.Delay(1000);
            BigBox.Text = webBrowser.Document.GetElementsByTagName("textarea")[0].GetAttribute("value");
            synthesizer.SpeakAsync(BigBox.Text.Substring(BigBox.Text.LastIndexOf(":")));
            SendButton.Enabled = true;

            QuerryBox.TabIndex = 0;
            QuerryBox.Focus();
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            webBrowser.Document.GetElementsByTagName("input")[0].SetAttribute("value", QuerryBox.Text.Trim());
            webBrowser.Document.GetElementsByTagName("input")[1].InvokeMember("click");
            QuerryBox.Text = "";

            await Task.Delay(100);

            BigBox.Text = webBrowser.Document.GetElementsByTagName("textarea")[0].GetAttribute("value");
            synthesizer.SpeakAsync(BigBox.Text.Substring(BigBox.Text.LastIndexOf(":")));

            BigBox.SelectionStart = BigBox.Text.Length;
            BigBox.ScrollToCaret();

            QuerryBox.TabIndex = 0;
            QuerryBox.Focus();
        }

        private void QuerryBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                SendButton_Click(null, null);
        }
    }
}
