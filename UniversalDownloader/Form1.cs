using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalDownloader
{
    public partial class Form1 : Form
    {
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
    }
}
