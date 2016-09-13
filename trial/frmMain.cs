using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using System.Linq;



namespace GlobalFileReader
{
    public partial class frmMain : Form
    {

        PictureBox pictureBox;
        Point mouseDown;
        int startx = 0;             // offset of image when mouse was pressed
        int starty = 0;
        int imgx = 0;               // current offset of image
        int imgy = 0;

        bool mousepressed = false;  // true as long as left mousebutton is pressed
        float zoom = 1; 

        TreeNode root = new TreeNode("Desktop");
        TreeNode doc = new TreeNode("My Documents");
        TreeNode comp = new TreeNode("My Computer ");
        TreeNode drivenode;
        TreeNode filenode;
        DirectoryInfo dir;
        string path = "";





        public Boolean FullScreen = false;
      //  public Timer at1 = new Timer();
    //    public Timer vt1 = new Timer();
      //  public Label vtim, atim;
      //  public TrackBar vtbar, atbar;
        public TabPage tbPage;
      
        public RichTextBox richTxtBox;
      
        public DataGridView dataGridViewExcel;
        public DataTable dt;
        //public Button btnStop, btnPlay, btnPause, vplay, vstop, vpause;
        //public HScrollBar vhScroller, ahScroller;
        //public Video video;
        //public Audio audio;
       // public Panel pnl;
        public int newtab = 0;
        public Font verdana10Font;
        public StreamReader reader;
        PrintDocument pd = new PrintDocument();
        static int i;
        public string str;
        public string[] filePath = new string[10];
        //  [DllImport("winmm.dll")]
        // private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public frmMain()
        {
            InitializeComponent();
            //tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
            
        }

        void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Text != "")

                filePath[newtab] = e.TabPage.Text;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //pictureBoxBackground.Visible = false;
                //////////////////////////////////////////////////////////////////////////////
                
                //img = Image.FromFile(openFileDialog.FileName);
                treeView1.Nodes.Clear();
                showtree(Path.GetDirectoryName(openFileDialog.FileName));

                treeView1.CollapseAll();
                TreeNode[] searchNode = treeView1.Nodes.Find(Path.GetDirectoryName(openFileDialog.FileName), true);
                foreach (TreeNode n in searchNode)
                {
                    if (n != null)
                        n.Expand();
                }
              //  drivenode = new TreeNode(openFileDialog.FileName);
             //   treeView1.Nodes.Add(drivenode);
               // MessageBox.Show(Path.GetDirectoryName(openFileDialog.FileName));
               // getFilesAndDir(drivenode, new DirectoryInfo(Path.GetDirectoryName(openFileDialog.FileName)));

                SwitchLogic(openFileDialog.FileName);

                //this.pictureBox.MouseDown +=
                //  new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseDown);
                //this.pictureBox.MouseMove +=
                //  new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
                //this.pictureBox.MouseUp +=
                //  new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseUp);
                //this.pictureBox.MouseUp +=
                //  new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseEnter);

                //pictureBox.MouseWheel += new MouseEventHandler(pictureBox_MouseWheel);
                ////this.MouseWheel += new System.Windows.Forms.MouseEventHandler(form1_MouseWheel);

                //Graphics g = this.CreateGraphics();

                ////// Fit whole image
                ////zoom = Math.Min(
                ////  ((float)pictureBox.Height / (float)img.Height) * (img.VerticalResolution / g.DpiY),
                ////  ((float)pictureBox.Width / (float)img.Width) * (img.HorizontalResolution / g.DpiX)
                ////);

                //// Fit width
                //zoom = ((float)pictureBox.Width / (float)img.Width) *
                //        (img.HorizontalResolution / g.DpiX);

                //pictureBox.Paint += new PaintEventHandler(imageBox_Paint);
            }
        }

        public void SwitchLogic(string str)
        {
            newtab = newtab + 1;
            filePath[newtab] = str;
          string str1 = Path.GetExtension(str);
          switch (str1)
            {
                case ".java":
                case ".cs":
                case ".txt": try
                    {

                        funcAddRichTextBox(out richTxtBox);
                        richTxtBox.Text = process.funcTxtFileLogic(filePath[newtab]);


                    }
                    catch (Exception e1)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e1.Message);
                    }

                    break;
                case ".docx":
                case ".doc": try
                    {

                        funcAddRichTextBox(out richTxtBox);

                        process.funcWordFileLogic(filePath[newtab]);

                        richTxtBox.Paste();

                        Clipboard.Clear();

                    }
                    catch
                    {
                    }
                    break;

                case ".htm":
                case ".HTML":
                case ".html": funcAddWebBrowser(str);
                   // webBrowser.Navigate(str);
                    break;


                case ".jpeg":
                case ".JPEG":
                case ".gif":
                case ".GIF":
                case ".png":
                case ".PNG":
                case ".bmp":
                case ".BMP":
                case ".jpg":
                case ".JPG":
                   

                    funcAddPictureBox(str);
                    //pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                   
                    break;
                case ".xml":
                case ".XML":
                    funcAddRichTextBox(out richTxtBox);
                    richTxtBox.Text = process.funcXmlFileLogic(filePath[newtab]);
                    break;
                case ".pdf":
                    funcAddRichTextBox(out richTxtBox);
                    richTxtBox.Text = process.funcPdfFileLogic(filePath[newtab]);
                    richTxtBox.Enabled = false;
                    break;
                case ".csv":
                case ".xls":
                case ".xlsx":
                    funcAddDataGridViewer(out dataGridViewExcel);
                    process.funcExcelFileLogic(filePath[newtab], out dt);
                    dataGridViewExcel.DataSource = dt.DefaultView;
                    break;

                case ".mp3":
                    funcAddButton(filePath);
                    // process.funcMP3FileLogic(filePath);
                    break;
                case ".mkv":
                case ".mpeg":
                case ".avi":
                case ".mp4":
                case ".3gp":
                case ".flv":
                case ".mov":
                case ".wmv":
                case ".VOB":

                    funcAddMedia(filePath);

                    //  process.funcVideoFileLogic(video, tbar);

                    break;
                default: if (i == 0)
                        //pictureBoxBackground.Visible = true; 
                        MessageBox.Show("File Format Not Supported", "File Format Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////
          
            toolStripStatusLabelValue.Text = i.ToString();
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e, PictureBox pictureBox)
        {
            pictureBox.Focus();
        }

        private void pictureBox_MouseMove(object sender, EventArgs e, PictureBox pictureBox)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                imgy = (int)(starty + (deltaY / zoom));

                pictureBox.Refresh();
            }
        }

        private void imageBox_MouseDown(object sender, EventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (!mousepressed)
                {
                    mousepressed = true;
                    mouseDown = mouse.Location;
                    startx = imgx;
                    starty = imgy;
                }
            }
        }

        private void imageBox_MouseUp(object sender, EventArgs e)
        {
            mousepressed = false;
        }

        //protected override void OnMouseWheel(MouseEventArgs e)
        private void pictureBox_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e, PictureBox pictureBox)
        {
            float oldzoom = zoom;

            if (e.Delta > 0)
            {
                zoom += 0.1F;
            }

            else if (e.Delta < 0)
            {
                zoom = Math.Max(zoom - 0.1F, 0.01F);
            }

            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - pictureBox.Location.X;    // Where location of the mouse in the pictureframe
            int y = mousePosNow.Y - pictureBox.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / zoom);

            imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
            imgy = newimagey - oldimagey + imgy;

            pictureBox.Refresh();  // calls imageBox_Paint
        }

        private void imageBox_Paint(object sender, PaintEventArgs e,Image img)
        {

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.ScaleTransform(zoom, zoom);
            e.Graphics.DrawImage(img, imgx, imgy);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
          //  PictureBox pictureBox;
           // pictureBox = this.Controls.Find("pictureBox" + tabControl1.SelectedIndex.ToString(), true).FirstOrDefault() as PictureBox;
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Right:
                        imgx -= (int)(pictureBox.Width * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Left:
                        imgx += (int)(pictureBox.Width * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Down:
                        imgy -= (int)(pictureBox.Height * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.Up:
                        imgy += (int)(pictureBox.Height * 0.1F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageDown:
                        imgy -= (int)(pictureBox.Height * 0.90F / zoom);
                        pictureBox.Refresh();
                        break;

                    case Keys.PageUp:
                        imgy += (int)(pictureBox.Height * 0.90F / zoom);
                        pictureBox.Refresh();
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void funcAddPictureBox(string str)
        {
            //InitializeComponent(); 
            //string path = openFileDialog.FileName;
            Image img;
           // PictureBox pictureBox;
            tbPage = new TabPage(Path.GetFileName(str));
            pictureBox = new PictureBox();
           // pictureBox.Name = "pictureBox" + i.ToString();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

            img = Image.FromFile(str);

            pictureBox.Visible = true;

            Graphics g = this.CreateGraphics();

            zoom = ((float)pictureBox.Width / (float)img.Width) *
                    (img.HorizontalResolution / g.DpiX);

            pictureBox.MouseDown += (sender, e) => imageBox_MouseDown(sender, e);
            pictureBox.MouseMove += (sender, e) => pictureBox_MouseMove(sender, e, pictureBox);
            pictureBox.MouseUp += (sender, e) => imageBox_MouseUp(sender, e);
            pictureBox.MouseEnter += (sender, e) => pictureBox_MouseEnter(sender, e,pictureBox);
            pictureBox.MouseWheel += (sender, e) => pictureBox_MouseWheel(sender, e,pictureBox);
            pictureBox.Paint += (sender, e) => imageBox_Paint(sender, e,img);


           // MessageBox.Show(pictureBox.Name);

            //this.pictureBox.MouseDown +=
            //new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseDown);
            //this.pictureBox.MouseMove +=
            //  new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            //this.pictureBox.MouseUp +=
            //  new System.Windows.Forms.MouseEventHandler(this.imageBox_MouseUp);
            //this.pictureBox.MouseUp +=
            //  new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseEnter);
            //pictureBox.MouseWheel += new MouseEventHandler(pictureBox_MouseWheel);

            //this.MouseWheel += new System.Windows.Forms.MouseEventHandler(form1_MouseWheel);

           

           // pictureBox.Paint += new PaintEventHandler(imageBox_Paint);




            //pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
            //pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseMove);
            tbPage.Controls.Add(pictureBox);
            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);

        }

        private void funcAddWebBrowser(string str)
        {
            //string path = openFileDialog.FileName;
            //tbPage = new TabPage(Path.GetFileName(path));
             WebBrowser webBrowser;
            tbPage = new TabPage("Browse");
            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Bottom;
            webBrowser.Height = Convert.ToInt32(this.Height * 0.8);
            webBrowser.Name = "webBrowser" + i.ToString();


            TextBox textBox1 = new TextBox();
            textBox1.Name = "txtUrl" + i.ToString();
           // textBox1.Name = "txtUrl";
            textBox1.Width = 290;
            // textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
          
            textBox1.Left = 80;

            Button button1 = new Button();
            button1.Text = "Go";
            button1.Name = "btnSearch" + i.ToString();
            //button1.Name = "btnSearch";
          //  button1.Click += new EventHandler(button1_Click);
          
            button1.Left = 320;
            button1.Width = 50;


            textBox1.KeyDown += (sender, e) => textBox1_KeyDown(sender, e, button1);

            button1.Click += (sender, e) => button1_Click(sender, e, webBrowser, textBox1);


            Button button2 = new Button();
            button2.Text = "Refresh";
            button2.Name = "btnRefresh" + i.ToString();
           // button2.Name = "btnRefresh";
           // button2.Click += new EventHandler(button2_Click);
            button2.Click += (sender, e) => button2_Click(sender, e, webBrowser);
            button2.Left = 0;

            Button button3 = new Button();
            button3.Text = "Back";
            button3.Name = "btnBack" + i.ToString();
          //  button3.Name = "btnBack";
          //  button3.Click += new EventHandler(button3_Click);
            button3.Click += (sender, e) => button3_Click(sender, e, webBrowser);
            button3.Left = 480;

            Button button4 = new Button();
            button4.Text = "Forward";
            button4.Name = "btnForward" + i.ToString();
           // button4.Name = "btnForward";
          //  button4.Click += new EventHandler(button4_Click);
            button4.Click += (sender, e) => button4_Click(sender, e, webBrowser);
            button4.Left = 560;

            Button button5 = new Button();
            button5.Text = "Stop";
            button5.Name = "btnStop" + i.ToString();
          //  button5.Name = "btnStop";
           // button5.Click += new EventHandler(button5_Click);
            button5.Click += (sender, e) => button5_Click(sender, e, webBrowser);
            button5.Left = 400;



            tbPage.Controls.Add(button1);
            tbPage.Controls.Add(button2);
            tbPage.Controls.Add(button3);
            tbPage.Controls.Add(button4);
            tbPage.Controls.Add(button5);
            tbPage.Controls.Add(textBox1);
            tbPage.Controls.Add(webBrowser);
            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);

            textBox1.Select();
            webBrowser.Visible = true;

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e, Button button1)
        {
            if (e.KeyCode == Keys.Enter)
            {
               // Button button1 = (Button)this.Controls.Find("btnSearch", true)[0];
                button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e, WebBrowser web1, TextBox txtName1)
        {
           // TextBox txtName = (TextBox)this.Controls.Find("txtUrl", true)[0];

            if (txtName1 != null)
            {
                web1.Navigate(txtName1.Text);
                //webBrowser.Navigate(txtName.Text);
            }
           //   Button Btn = (Button)sender;
           //   MessageBox.Show(web1.Name + "::" + txtName1.Name + ":::" + Btn.Name);
        }

        private void button2_Click(object sender, EventArgs e, WebBrowser webBrowser1)
        {
            webBrowser1.Refresh();
          //  MessageBox.Show(webBrowser1.Name);
        }

        private void button3_Click(object sender, EventArgs e, WebBrowser webBrowser1)
        {
            webBrowser1.GoBack();
          //  MessageBox.Show(webBrowser1.Name);
        }

        private void button4_Click(object sender, EventArgs e, WebBrowser webBrowser1)
        {
            webBrowser1.GoForward();
        }

        private void button5_Click(object sender, EventArgs e, WebBrowser webBrowser1)
        {
            webBrowser1.Stop();
        }

        private void funcAddDataGridViewer(out DataGridView dataGridViewExcel)
        {
            string path = openFileDialog.FileName;
            tbPage = new TabPage(Path.GetFileName(path));
            dataGridViewExcel = new DataGridView();
            dataGridViewExcel.Dock = DockStyle.Fill;
            tbPage.Controls.Add(dataGridViewExcel);
            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);

        }


        private void funcAddRichTextBox(out RichTextBox richTxtBox)
        {
            string path = openFileDialog.FileName;
            tbPage = new TabPage(Path.GetFileName(path));
            richTxtBox = new RichTextBox();
            richTxtBox.Name = i.ToString();
            richTxtBox.Dock = DockStyle.Fill;
            // richTxtBox.Enabled = false;

            richTxtBox.ForeColor = Color.Black;
            richTxtBox.Font = new Font("Times New Roman", 14);


            tbPage.Controls.Add(richTxtBox);

            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);

        }

        //Point mouseDownLoc;

        //private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //        mouseDownLoc = e.Location;
        //}

        //private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        Point currentMousePos = e.Location;
        //        int distanceX = currentMousePos.X - mouseDownLoc.X;
        //        int distanceY = currentMousePos.Y - mouseDownLoc.Y;
        //        int newX = pictureBox.Location.X + distanceX;
        //        int newY = pictureBox.Location.Y + distanceY;

        //        if (newX + pictureBox.Image.Width < pictureBox.Image.Width && pictureBox.Image.Width + newX > pictureBox.Width)
        //            pictureBox.Location = new Point(newX, pictureBox.Location.Y);
        //        if (newY + pictureBox.Image.Height < pictureBox.Image.Height && pictureBox.Image.Height + newY > pictureBox.Height)
        //            pictureBox.Location = new Point(pictureBox.Location.X, newY);
        //    }
        //}

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            i--;
            if (i == 0)
                //pictureBoxBackground.Visible = true;

                toolStripStatusLabelValue.Text = i.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabelValue.Text = i.ToString();
            //listView1.LabelEdit = true;
            //listView1.FullRowSelect = true;
            //listView1.Sorting = SortOrder.Ascending;
          //  treeView1.Nodes.Add(root);
         //   doc.ImageIndex = 5;
       //     comp.ImageIndex = 4;
       //     treeView1.Nodes.Add(doc);
       //     treeView1.Nodes.Add(comp);
            //   GetDrives();

            showtree("");

           
        }

        public void showtree(string path)
        {
            string[] drives;
            if (path == "")
            {
                drives = Environment.GetLogicalDrives();
                foreach (string drive in drives)
                {
                    DriveInfo di = new DriveInfo(drive);
                    int driveImage;

                    switch (di.DriveType)    //set the drive's icon
                    {
                        case DriveType.CDRom:
                            driveImage = 3;
                            break;
                        case DriveType.Network:
                            driveImage = 6;
                            break;
                        case DriveType.NoRootDirectory:
                            driveImage = 8;
                            break;
                        case DriveType.Unknown:
                            driveImage = 8;
                            break;
                        default:
                            driveImage = 2;
                            break;
                    }

                    TreeNode node = new TreeNode(drive.Substring(0, 1), driveImage, driveImage);
                    node.Tag = drive;

                    if (di.IsReady == true)
                        node.Nodes.Add("...");

                    treeView1.Nodes.Add(node);
                }
            }
            else
            {
                DriveInfo di = new DriveInfo(path);
                int driveImage;

                switch (di.DriveType)    //set the drive's icon
                {
                    case DriveType.CDRom:
                        driveImage = 3;
                        break;
                    case DriveType.Network:
                        driveImage = 6;
                        break;
                    case DriveType.NoRootDirectory:
                        driveImage = 8;
                        break;
                    case DriveType.Unknown:
                        driveImage = 8;
                        break;
                    default:
                        driveImage = 2;
                        break;
                }

                TreeNode node = new TreeNode(di.Name.Substring(0, 1), driveImage, driveImage);
                node.Tag = di.Name;

                if (di.IsReady == true)
                    node.Nodes.Add("...");

                treeView1.Nodes.Add(node);
            }
            
          
        }

        #region old_code
        private void GetDrives()
        {
            DriveInfo[] drive = DriveInfo.GetDrives();
            foreach (DriveInfo d in drive)
            {
                drivenode = new TreeNode(d.Name);
                dir = d.RootDirectory;
                comp.Nodes.Add(drivenode);
                switch (d.DriveType)
                {
                    case DriveType.CDRom:
                        drivenode.ImageIndex = 3;
                        break;
                    case DriveType.Network:
                        drivenode.ImageIndex = 6;
                        break;
                    case DriveType.NoRootDirectory:
                        drivenode.ImageIndex = 8;
                        break;
                    case DriveType.Unknown:
                        drivenode.ImageIndex = 8;
                        break;
                    default:
                        drivenode.ImageIndex = 2;
                        break;
                }
                getFilesAndDir(drivenode, dir);
            }
        }

        private void getFilesAndDir(TreeNode node, DirectoryInfo dirname)
        {
            try
            {
                foreach (FileInfo fi in dirname.GetFiles())
                {
                    filenode = new TreeNode(fi.Name);
                    filenode.Name = fi.FullName;
                    getFileExtension(filenode.Name);
                    node.Nodes.Add(filenode);
                }
                try
                {
                    foreach (DirectoryInfo di in dirname.GetDirectories())
                    {
                        TreeNode dirnode = new TreeNode(di.Name);
                        dirnode.ImageIndex = 2;
                        dirnode.Name = di.FullName;
                        node.Nodes.Add(dirnode);
                        getFilesAndDir(dirnode, di); //Recursive Functioning
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void getFileExtension(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".txt":
                case ".rtf":
                    filenode.ImageIndex = 2;
                    break;
                case ".doc":
                case ".docx":
                    filenode.ImageIndex = 0;
                    break;
                case ".html":
                case ".htm":
                    filenode.ImageIndex = 1;
                    break;
                case ".rar":
                case ".zip":
                    filenode.ImageIndex = 3;
                    break;
                case ".java":
                    filenode.ImageIndex = 4;
                    break;
                default:
                    filenode.ImageIndex = 2;
                    break;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //listView1.Items.Clear();
                TreeNode selectednode = e.Node;
                treeView1.SelectedNode.ImageIndex = e.Node.ImageIndex;
                selectednode.Expand();
                // textBox1.Text = selectednode.FullPath;
                //getFilesAndDir(selectednode, );
                if (selectednode.Nodes.Count > 0)
                {
                    foreach (TreeNode n in selectednode.Nodes)
                    {
                        ListViewItem lst = new ListViewItem(n.Text, n.ImageIndex);
                        lst.Name = n.FullPath.Substring(13);
                       // MessageBox.Show("List Node : " + lst.Name);
                        //listView1.Items.Add(lst);
                    }
                }
                else
                {
                    //listView1.Items.Add(selectednode.FullPath, selectednode.Text, selectednode.ImageIndex);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }


        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  for (int i = 0; i < listView1.Items.Count; i++)
            {
                // if (listView1.Items[i].Selected == true)
                {
                    // path = listView1.Items[i].Name;
                    // textBox1.Text = path;
                    //listView1.Items.Clear();
                    LoadFilesAndDir(path);
                }
            }
        }


        private void LoadFilesAndDir(string address)
        {
            DirectoryInfo di = new DirectoryInfo(address);
            try
            {
                foreach (FileInfo fi in di.GetFiles())
                {
                    //listView1.Items.Add(fi.Name, filenode.ImageIndex);
                }
                try
                {
                    foreach (DirectoryInfo listd in di.GetDirectories())
                    {
                        //listView1.Items.Add(listd.FullName, listd.Name, 2);
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        #endregion


        #region new code

        private void PopulateTreeView()
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(@"../..");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                   // GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }




        //private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        //{
        //    if (e.Node.Nodes.Count > 0)
        //    {
        //        if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
        //        {
        //            e.Node.Nodes.Clear();

        //            //get the list of sub direcotires
        //            string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

        //            foreach (string dir in dirs)
        //            {
        //                DirectoryInfo di = new DirectoryInfo(dir);
        //                TreeNode node = new TreeNode(di.Name, 0, 1);

        //                try
        //                {
        //                    node.Tag = dir;  //keep the directory's full path in the tag for use later

        //                    //if the directory has any sub directories add the place holder
        //                    if (di.GetDirectories().Count() > 0)
        //                        node.Nodes.Add(null, "...", 0, 0);
        //                }
        //                catch (UnauthorizedAccessException)
        //                {
        //                    //if an unauthorized access exception occured display a locked folder
        //                    node.ImageIndex = 12;
        //                    node.SelectedImageIndex = 12;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message, "DirectoryLister", MessageBoxButtons.OK,
        //                        MessageBoxIcon.Error);
        //                }
        //                finally
        //                {
        //                    e.Node.Nodes.Add(node);
        //                }
        //            }
        //        }
        //    }
        //}


        #endregion





        #region functions

        private void foToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.ShowDialog();
            fontDialog.ShowColor = true;
            fontDialog.ShowApply = true;
            fontDialog.ShowEffects = true;
            fontDialog.ShowHelp = true;
            try
            {
                int tab = tabControl1.SelectedIndex;


                this.richTxtBox.Font = fontDialog.Font;
                this.richTxtBox.ForeColor = fontDialog.Color;
            }
            catch
            {
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Text File|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.richTxtBox.SaveFile(saveFileDialog.FileName);
                MessageBox.Show("file saved succ");
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (filePath[newtab] != null)
            {
                str = filePath[newtab];
                string str1 = Path.GetExtension(str);
                switch (str1)
                {
                    case ".java":
                    case ".cs":
                    case ".txt":
                    case ".xml":
                    case ".XML":
                        string filename = this.richTxtBox.Text;
                        reader = new StreamReader(filePath[newtab]);
                        verdana10Font = new Font("Verdana", 10);

                        pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
                        pd.Print();
                        if (reader != null)
                            reader.Close();

                        break;
                    case ".pdf":
                    case ".doc":
                    case ".docx":
                        PrintWordFileHandler(filePath[newtab]);
                        break;
                    case ".jpg":

                        pd.PrintPage += new PrintPageEventHandler(this.PrintImageFileHandler);
                        pd.Print();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please open a file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {

            Graphics g = ppeArgs.Graphics;
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ppeArgs.MarginBounds.Left;
            float topMargin = ppeArgs.MarginBounds.Top;
            string line = null;
            verdana10Font = new Font("Verdana", 10);
            linesPerPage = ppeArgs.MarginBounds.Height / verdana10Font.GetHeight(g);


            while (count < linesPerPage && ((line = reader.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                verdana10Font.GetHeight(g));
                g.DrawString(line, verdana10Font, Brushes.Black,
                leftMargin, yPos, new StringFormat());
                count++;

            }

            if (line != null)
            {
                ppeArgs.HasMorePages = true;
            }

            else
            {
                ppeArgs.HasMorePages = false;
            }
        }

        public void PrintImageFileHandler(object o, PrintPageEventArgs e)
        {
            Image i = Image.FromFile(filePath[newtab]);
            Point p = new Point(0, 0);


            e.Graphics.DrawImageUnscaled(i, p);
        }

        public void PrintWordFileHandler(string filepath)
        {

            //e.Graphics.DrawString(this.richTxtBox.Text, new Font("Verdana", 10), Brushes.Black, new PointF(0, 0));

            Microsoft.Office.Interop.Word.ApplicationClass wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            object file = filepath; // Specify path for word file
            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref file, ref missing, ref missing,
                                                                                ref missing, ref missing, ref missing,
                                                                                ref missing, ref missing, ref missing,
                                                                                ref missing, ref missing, ref missing,
                                                                                ref missing, ref missing, ref missing, ref missing);

            object copies = "1";
            object pages = "";
            object range = Word.WdPrintOutRange.wdPrintAllDocument;
            object items = Word.WdPrintOutItem.wdPrintDocumentContent;
            object pageType = Word.WdPrintOutPages.wdPrintAllPages;
            object oTrue = true;
            object oFalse = false;

            doc.PrintOut(ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
                ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
                ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);

        }

        public void funcAddMedia( string[] filepath)
        {
             HScrollBar vhScroller;
             Video video;
                 Timer vt1 = new Timer();
            
            tbPage = new TabPage(Path.GetFileName(filePath[newtab]));
            Button vplay = new Button();
            Button vstop = new Button();
            Button vpause = new Button();
            TrackBar vtbar = new TrackBar();
            Label vtim = new Label();
            Panel pnl = new Panel();
            vhScroller = new HScrollBar();
            vplay.Name = "vplay" + i.ToString();
            vstop.Name = "vstop" + i.ToString();
            vpause.Name = "vpause" + i.ToString();
            vstop.Text = "Stop";
            vplay.Text = "Play";
            vpause.Text = "Pause";
            vhScroller.Minimum = 0;
            vhScroller.Maximum = 6000;
            video = new Video(filePath[newtab]);
            video.Owner = pnl;
            video.Audio.Volume = -6000;
            vtbar.Maximum = (int)video.Duration;
            vtbar.Minimum = (int)video.CurrentPosition;

            //int cx = ClientRectangle.Width / 8;
            //int cy = ClientRectangle.Height / 8;
            //vstop.SetBounds(cx * 0, cy * 0, cx, cy);
            //vplay.SetBounds(cx * 1, cy * 0, cx, cy);
            //vpause.SetBounds(cx * 2, cy * 0, cx, cy);
            //vtim.SetBounds(cx * 3, cy * 1, cx * 4, cy);
            //vtbar.SetBounds(cx * 3, cy * 0, cx * 4, cy);
            //vhScroller.SetBounds(cx * 7, cy * 0, cx, cy);
            //pnl.SetBounds(cx * 0, cy * 2, cx * 8, cy * 6);

            vstop.AutoSize = true;
            vplay.AutoSize = true;
            vpause.AutoSize = true;
            vtim.AutoSize = true;
            vtbar.AutoSize = true;
            vhScroller.AutoSize = true;
            pnl.AutoSize = true;

            vstop.Location = new System.Drawing.Point(10, 20);
            vplay.Location = new System.Drawing.Point(90, 20);
            vpause.Location = new System.Drawing.Point(170, 20);
            vtim.Location = new System.Drawing.Point(260, 20);
            vtbar.Location = new System.Drawing.Point(350, 20);
            vtbar.Size = new System.Drawing.Size(700, 20);
            vhScroller.Location = new System.Drawing.Point(1050, 20);
            vhScroller.Size = new System.Drawing.Size(110, 10);
            pnl.Location = new System.Drawing.Point(10, 50);
            pnl.Size = new System.Drawing.Size(1330, 560);

           // vstop.Click += new EventHandler(vstop_Click);
            vstop.Click += (sender, e) => vstop_Click(sender, e, video,vt1);
           // vplay.Click += new EventHandler(vplay_Click);
            vplay.Click += (sender, e) => vplay_Click(sender, e, video,vtim,vt1);
           // vpause.Click += new EventHandler(vpause_Click);
            vpause.Click += (sender, e) => vpause_Click(sender, e, video,vt1);
           // vtbar.Scroll += new System.EventHandler(vid_Scroll);
            vtbar.Scroll += (sender, e) => vid_Scroll(sender, e, video,vtbar);
            //vt1.Tick += new EventHandler(vid_Tick);
            vt1.Tick += (sender, e) => vid_Tick(sender, e, video,vtbar);
          //  vhScroller.ValueChanged += new System.EventHandler(vidvol_scroll);
            vhScroller.ValueChanged += (sender, e) => vidvol_scroll(sender, e, video);
            //tabControl1.KeyPress += new KeyPressEventHandler(fullscreen);
            tbPage.MouseDoubleClick += (sender, e) => fullscreen(sender, e, video, pnl);

            tbPage.Controls.Add(pnl);
            tbPage.Controls.Add(vplay);
            tbPage.Controls.Add(vstop);
            tbPage.Controls.Add(vpause);
            tbPage.Controls.Add(vtim);
            tbPage.Controls.Add(vtbar);
            tbPage.Controls.Add(vhScroller);
            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);

        }

        public void funcAddButton(string[] filepath)
        {
            Timer at1 = new Timer();
            Audio audio;
            HScrollBar  ahScroller;
            tbPage = new TabPage(Path.GetFileName(filePath[newtab]));
            Button btnStop = new Button();
            Button btnPlay = new Button();
            Button btnPause = new Button();
            Label atim = new Label();
            TrackBar atbar = new TrackBar();
            ahScroller = new HScrollBar();
            btnStop.Name = "btnStop" + i.ToString();
            btnPlay.Name = "btnPlay" + i.ToString();
            btnPause.Name = "btnPause" + i.ToString();
            btnStop.Text = "Stop";
            btnPlay.Text = "Play";
            btnPause.Text = "Pause";
            btnStop.Location = new System.Drawing.Point(10, 20);
            btnPlay.Location = new System.Drawing.Point(90, 20);
            btnPause.Location = new System.Drawing.Point(170, 20);
            atim.Location = new System.Drawing.Point(260, 20);
            atbar.Location = new System.Drawing.Point(350, 20);
            atbar.Size = new System.Drawing.Size(700, 20);
            ahScroller.Location = new System.Drawing.Point(1050, 20);
            ahScroller.Size = new System.Drawing.Size(110, 10);
            audio = Audio.FromFile(filepath[newtab]);
            audio.Volume = -6000;
            atbar.Maximum = (int)audio.Duration;
            atbar.Minimum = (int)audio.CurrentPosition;
            ahScroller.Minimum = 0;
            ahScroller.Maximum = 6000;
           // btnStop.Click += new EventHandler(btnStop_Click);
            btnStop.Click += (sender, e) => btnStop_Click(sender, e, audio,at1);
           // btnPlay.Click += new EventHandler(btnPlay_Click);
            btnPlay.Click += (sender, e) => btnPlay_Click(sender, e, audio,atim,at1);
           // btnPause.Click += new EventHandler(btnPause_Click);
            btnPause.Click += (sender, e) => btnPause_Click(sender, e, audio,at1);
           // atbar.Scroll += new System.EventHandler(aud_Scroll);
            atbar.Scroll += (sender, e) => aud_Scroll(sender, e, audio,atbar);
            //at1.Tick += new EventHandler(aud_Tick);
            at1.Tick += (sender, e) => aud_Tick(sender, e, audio,atbar);
            //ahScroller.ValueChanged += new System.EventHandler(audvol_scroll);
            ahScroller.ValueChanged += (sender, e) => audvol_scroll(sender, e, audio);

            
            

            tbPage.Controls.Add(btnStop);
            tbPage.Controls.Add(btnPlay);
            tbPage.Controls.Add(btnPause);
            tbPage.Controls.Add(atim);
            tbPage.Controls.Add(atbar);
            tbPage.Controls.Add(ahScroller);

            tabControl1.TabIndex = i;
            tabControl1.TabPages.Add(tbPage);
            tabControl1.SelectTab(i++);


        }

        void vidvol_scroll(object sender, EventArgs e, Video video)
        {
            try
            {
                HScrollBar vhScroller = (HScrollBar)sender;
                TabPage tp = (TabPage)vhScroller.Parent;
                //if ( tabControl1.SelectedTab.Controls.ContainsKey("video"))
                //{
                //    Video selectedRtb = (Video)tabControl1.SelectedTab.Controls["video"];
                //    video.Audio.Volume = vhScroller.Value - 6000;
                //}
                video.Audio.Volume = vhScroller.Value - 6000;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }


        }

        void vid_Scroll(object sender, EventArgs e, Video video, TrackBar vtbar)
        {
            video.CurrentPosition = vtbar.Value;

        }

        void vplay_Click(object sender, EventArgs e, Video video, Label vtim,Timer vt1)
        {
            if (video.State != StateFlags.Running)
                video.Play();
            TimeSpan vt = TimeSpan.FromSeconds(video.Duration);
            vtim.Text = "" + vt.Hours + ": " + vt.Minutes + ": " + vt.Seconds + "";
            vt1.Enabled = true;

        }

        void vid_Tick(object sender, EventArgs e, Video video, TrackBar vtbar)
        {
            vtbar.Value = (int)video.CurrentPosition;

        }

        void vpause_Click(object sender, EventArgs e, Video video, Timer vt1)
        {
            if (video.State != StateFlags.Stopped)
                video.Pause();
            vt1.Enabled = false;
        }

        void vstop_Click(object sender, EventArgs e, Video video, Timer vt1)
        {
            if (video.State != StateFlags.Stopped)
                video.Stop();
            vt1.Enabled = false;
           
        }

        void fullscreen(object sender, MouseEventArgs e, Video video,Panel pnl)
        {
            Form1 f = new Form1();

            //if (e.KeyChar == (char)Keys.Space)
            //{
                if (FullScreen)
                {
                    f.Hide();
                    video.Owner = pnl;
                    pnl.Size = new System.Drawing.Size(1330, 560);
                    video.Play();
                    FullScreen = false;
                }
                else
                {
                    f.WindowState = FormWindowState.Maximized;
                    video.Pause();
                    video.Owner = f;
                    f.Show();
                    video.Play();
                    FullScreen = true;
                }
            //}
        }

        void btnPlay_Click(object sender, EventArgs e, Audio audio, Label atim,Timer at1)
        {
            audio.Play();
            TimeSpan at = TimeSpan.FromSeconds(audio.Duration);
            atim.Text = "" + at.Hours + ": " + at.Minutes + ": " + at.Seconds + "";
            at1.Enabled = true;
            // mciSendString("open \"" + process.musicFile + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            // mciSendString("play MediaFile", null, 0, IntPtr.Zero);
        }

        void btnStop_Click(object sender, EventArgs e, Audio audio,Timer at1)
        {
            audio.Stop();
            at1.Enabled = false;
            // mciSendString("close MediaFile", null, 0, IntPtr.Zero);
        }

        void btnPause_Click(object sender, EventArgs e, Audio audio,Timer at1)
        {
            audio.Pause();
            at1.Enabled = false;
        }

        void aud_Scroll(object sender, EventArgs e, Audio audio,TrackBar atbar)
        {

            audio.CurrentPosition = atbar.Value;

        }

        void audvol_scroll(object sender, EventArgs e, Audio audio)
        {
            try
            {
                HScrollBar ahScroller = (HScrollBar)sender;
                audio.Volume = ahScroller.Value - 6000;


            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        void aud_Tick(object sender, EventArgs e, Audio audio,TrackBar atbar)
        {
            atbar.Value = (int)audio.CurrentPosition;


        }

        void abcToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            funcAddWebBrowser("");
           
        }


        #endregion


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (treeView1.SelectedNode.Text != "test")
            //{
            //    MessageBox.Show(treeView1.SelectedNode.FullPath);
            //    getFilesAndDir(treeView1.SelectedNode, new DirectoryInfo(Path.GetDirectoryName(treeView1.SelectedNode.FullPath)));
            //}
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Text == "..." && e.Node.Nodes[0].Tag == null)
                {
                    e.Node.Nodes.Clear();

                    //get the list of sub direcotires
                    string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());
                    string[] files = Directory.GetFiles(e.Node.Tag.ToString());
                    foreach (string dir in dirs)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 1);

                        try
                        {
                            node.Tag = dir;  //keep the directory's full path in the tag for use later

                            //if the directory has any sub directories add the place holder
                            //  if (di.GetDirectories().Count() > 0)
                            node.Nodes.Add(null, "...", 0, 0);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //if an unauthorized access exception occured display a locked folder
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "DirectoryLister", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }
                    }
                    foreach (string dir in files)
                    {
                        DirectoryInfo di = new DirectoryInfo(dir);
                        TreeNode node = new TreeNode(di.Name, 0, 7);

                        try
                        {
                            node.Tag = dir;  //keep the directory's full path in the tag for use later

                            //if the directory has any sub directories add the place holder
                            //  if (di.GetDirectories().Count() > 0)
                            //  node.Nodes.Add(null, "...", 0, 7);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //if an unauthorized access exception occured display a locked folder
                            node.ImageIndex = 12;
                            node.SelectedImageIndex = 12;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "DirectoryLister", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                        finally
                        {
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
            }
            else
            {
               // MessageBox.Show(e.Node.Tag.ToString());
                SwitchLogic(e.Node.Tag.ToString());

            }
        }


        private void treeView1_MouseHover(object sender, EventArgs e)
        {
            if (treeView1.Visible == false)
            {
                treeView1.Visible = true;
            }
        }

        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            if (treeView1.Visible == true)
            {
                treeView1.Visible = false;
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // PictureBox pictureBox;
           // pictureBox = this.Controls["pictureBox0"] as PictureBox;
           // pictureBox = this.Controls.Find("pictureBox" + tabControl1.SelectedIndex.ToString(), true).FirstOrDefault() as PictureBox;
          //  MessageBox.Show(pictureBox.Name);
            zoom += 0.1F;
            if (pictureBox != null)
            {
                pictureBox.Refresh();
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
          //  PictureBox pictureBox;
           // pictureBox = this.Controls.Find("pictureBox" + tabControl1.SelectedIndex.ToString(), true).FirstOrDefault() as PictureBox;

            zoom -= 0.1F;
            if (pictureBox != null)
            {
                pictureBox.Refresh();
            }
        }


    }


}
