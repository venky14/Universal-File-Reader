using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using Microsoft.DirectX.AudioVideoPlayback;


namespace GlobalFileReader
{
    public partial class Form2 : Form
    {
       // private Video _video = null;
        private string _command;
        private bool isOpen;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand,StringBuilder strReturn,int iReturnLength, IntPtr hwndCallback);
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    Image img = Image.FromFile(openFileDialog1.FileName);
            //    Clipboard.SetImage(img);
                
            //    richTextBox1.SelectionStart = 0;
            //    richTextBox1.Paste();

            //    Clipboard.Clear();
            //}

            //DataTable dt;
            //process.funcExcelFileLogic("H:\\Kalpesh\\Projects\\Project List & Sysnopsis\\ProjectList",  out dt);

            //dataGridViewExcel.DataSource = dt.DefaultView;

           // string sFileName = "K:\\songs\\01_Kalpesh";   
           // _command = "open \"" + sFileName + "\" type mpegvideo alias MediaFile";

           // mciSendString(_command, null, 0, IntPtr.Zero);

           //_command = "play MediaFile";

    
           // _command += " REPEAT";

           // mciSendString(_command, null, 0, IntPtr.Zero);
          /*  if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // open the video

                // remember the original dimensions of the panel
                int height = videoPanel.Height;
                int width = videoPanel.Width;

                // dispose of the old video
                if (_video != null)
                {
                    _video.Dispose();
                }

                // open a new video
                _video = new Video(openFileDialog1.FileName);

                // assign the win form control that will contain the video
                _video.Owner = videoPanel;

                // resize to fit in the panel
                videoPanel.Width = width;
                videoPanel.Height = height;

                // play the first frame of the video so we can identify it
                _video.Play();
                _video.Pause();*/
            }

  

        
    }
}
