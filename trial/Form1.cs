using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;


namespace GlobalFileReader
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        void fullscreen(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == (char)Keys.Space)
            {
                
                    this.Hide();

                    frmMain.ActiveForm.Show();
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.KeyPress += new KeyPressEventHandler(fullscreen);
        }
        
    }
}
