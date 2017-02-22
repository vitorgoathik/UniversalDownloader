using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalDownloaderAgoda;

namespace UniversalDownloader
{
    public partial class Form1 : Form
    {
        Functions functions = new Functions();
        private System.Net.WebClient webClientControl;
        public Form1()
        {
            InitializeComponent();
            DefineWebClient();
        }

        /// <summary>
        /// WebClient needs to be defined here, 
        /// so that it won't autogenerate 
        /// non-buildable deprecated properties in the design
        /// </summary>
        private void DefineWebClient()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.webClientControl = new System.Net.WebClient();
            this.webClientControl.BaseAddress = "";
            this.webClientControl.CachePolicy = null;
            this.webClientControl.Credentials = null;
            this.webClientControl.Encoding = ((System.Text.Encoding)(resources.GetObject("webClientControl.Encoding")));
            this.webClientControl.Headers = ((System.Net.WebHeaderCollection)(resources.GetObject("webClientControl.Headers")));
            this.webClientControl.QueryString = ((System.Collections.Specialized.NameValueCollection)(resources.GetObject("webClientControl.QueryString")));
            this.webClientControl.UseDefaultCredentials = false;
            this.webClientControl.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(this.webClientControl_DownloadProgressChanged);
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {

        }

        private void webClientControl_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {

        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                functions.destination = result.ToString();
            }
        }
    }
}
