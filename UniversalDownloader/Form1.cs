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
        public Form1()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {

        }

        private void webClientControl_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {

        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = functions.GetFileName();
            DialogResult result = saveFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                functions.destination = result.ToString();
            }
        }
    }
}
