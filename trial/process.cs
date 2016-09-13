using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using org.pdfbox.pdmodel;
using org.pdfbox.util;
using System.Xml;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Microsoft.DirectX.AudioVideoPlayback;
//using Microsoft.Office.Interop.PowerPoint;
//using Microsoft.Office.Interop.Graph;




namespace GlobalFileReader
{
    class process
    {
        static string tempInfo, str;
        static public string _command;
        static public string musicFile;
        private Video video;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        static frmMain objMain = new frmMain();
        public static string funcTxtFileLogic(string filePath)
        {
            tempInfo = "";
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(filePath))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);

                    tempInfo = tempInfo + line + "\n";
                  

                }
               
                return tempInfo;
            }
        }


        public static void funcWordFileLogic(string filepath)
        {
            Microsoft.Office.Interop.Word.ApplicationClass wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
            object file = filepath; // Specify path for word file
            object nullobj = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref file, ref nullobj, ref nullobj,
                                                                                ref nullobj, ref nullobj, ref nullobj,
                                                                                ref nullobj, ref nullobj, ref nullobj,
                                                                                ref nullobj, ref nullobj, ref nullobj,
                                                                                ref nullobj, ref nullobj, ref nullobj, ref nullobj);
            
            doc.ActiveWindow.Selection.WholeStory();
            doc.ActiveWindow.Selection.Copy();
            IDataObject data = Clipboard.GetDataObject();
           
            //string allText = data.GetData(DataFormats.Text).ToString();
            doc.Close(ref nullobj, ref nullobj, ref nullobj);
            wordApp.Quit(ref nullobj, ref nullobj, ref nullobj);
            //Console.WriteLine(allText);

           

           // return allText;

        }


        public static string funcXmlFileLogic(string filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath);
            tempInfo = "";
            try
            {
                while (reader.Read())
                {
                    tempInfo = reader.ReadOuterXml();

                }
                return tempInfo;
            }
            catch (Exception e)
            {
                MessageBox.Show("XML File is not in correct format. Few of the contents will be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return tempInfo;
            }

           
        }


        public static void funcExcelFileLogic(string filePath, out System.Data.DataTable dt)
        {

            try
            {
                OleDbConnection theConnection = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;\"");
                theConnection.Open();
                OleDbDataAdapter theDataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", theConnection);
                //DataSet theDS = new DataSet();
                dt = new System.Data.DataTable();
                theDataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dt = new System.Data.DataTable();
            }
            
        }


        public static string funcPdfFileLogic(string filePath)
        {
            PDDocument doc1 = PDDocument.load(filePath);
            PDFTextStripper pdfStripper = new PDFTextStripper();
            Console.Write(pdfStripper.getText(doc1));
            return pdfStripper.getText(doc1);
                     
        }

        public static void funcMP3FileLogic(string sFileName)
        {


            musicFile = sFileName;
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);

            
            mciSendString("open \"" + sFileName + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile", null, 0, IntPtr.Zero);


        }

        public static void funcVideoFileLogic(Video video,TrackBar tbar)
        {
             //while (video .Playing)
              //tbar.Value = (int)video.CurrentPosition;
           
        }
        
    }
}
