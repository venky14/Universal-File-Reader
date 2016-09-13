using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GlobalFileReader
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text != "" && txtPassword.Text != "")
            {
                if (txtUserName.Text == "admin" && txtPassword.Text == "admin")
                {
                    frmMain fM = new frmMain();
                    this.Hide();
                    fM.Show();
                }
                else
                {
                    MessageBox.Show("Please enter proper UserName and Password");
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    txtUserName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter UserName and Password");
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtUserName.Focus();
            }
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (txtUserName.Text != "" && txtPassword.Text != "")
            {
                if (txtUserName.Text == "admin" && txtPassword.Text == "admin")
                {
                    frmMain fM = new frmMain();
                    this.Hide();
                    fM.Show();
                }
                else
                {
                    MessageBox.Show("Please enter correct UserName and Password");
                    txtUserName.Text = "";
                    txtPassword.Text = "";
                    txtUserName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter UserName and Password");
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtUserName.Focus();
            }
        }

        
    }
}
