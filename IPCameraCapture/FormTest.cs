using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPCameraCapture
{
    public partial class FormTest : Form
    {
        private readonly HttpClient _httpClient;
        private const string CameraImageUrl = "http://192.168.1.9/axis-cgi/jpg/image.cgi";
        private const string CameraUname = "root";
        private const string CameraPword = "root";

        public FormTest()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            tbUrl.Text = CameraImageUrl;
            tbUname.Text = CameraUname;
            tbPword.Text = CameraPword;
        }

        private Image downloadedImage;
        private async void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                var credentials = new NetworkCredential( CameraUname, CameraPword); // Replace with actual username and password
                var handler = new HttpClientHandler { Credentials = credentials };

                using (var httpClient = new HttpClient(handler))
                using (var response = await httpClient.GetAsync(CameraImageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            if (downloadedImage != null) 
                            {
                                downloadedImage.Dispose();
                            }
                            downloadedImage = Image.FromStream(stream);
                            pictureBox.Image = downloadedImage;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to download the image.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }   
    }
}
