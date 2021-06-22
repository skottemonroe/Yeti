using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yeti {
    class BackupController {
        public BackupController(string strSource, string strDest, ListBox listBox1) {
            StrSource = strSource;
            StrDest = strDest;
            ListBox1 = listBox1;
        }

        public string StrSource { get; }
        public string StrDest { get; }
        public ListBox ListBox1 { get; }

        internal void BeginBackup() {

            ListBox1.Items.Clear();


            string[] allfiles = Array.Empty<string>();

            try {
                //get a listing of all fFiles in the source
                allfiles = Directory.GetFiles(StrSource, "*.*", SearchOption.AllDirectories);
            } catch (Exception e) {
                MessageBox.Show("We had trouble getting a list of allfFiles.\n\n" + e.ToString());
            }


            bool boolActualize = false;
            foreach (var strSrcFile in allfiles) {

                //fFor each fFile, we need to fFigure out if we should copy it.

                string strDestFile = strSrcFile.Replace(StrSource, StrDest);
                if (File.Exists(strDestFile)) {

                    //The destination exists.
                    //But is it changed?

                    FileInfo fileSrc = new FileInfo(strSrcFile);
                    FileInfo fileDest = new FileInfo(strDestFile);

                    if (fileSrc.Length != fileDest.Length) {
                        boolActualize = true;
                    } else if (fileSrc.LastWriteTime != fileDest.LastWriteTime) {
                        boolActualize = true;
                    }

                } else {

                    //the destination does not exist.  COPY IT NOW.
                    boolActualize = true;

                }



                if (boolActualize) {
                    if (File.Exists(strDestFile)) {
                        ListBox1.Items.Add("REPLACE: \t" + strDestFile);
                    } else {
                        ListBox1.Items.Add("ADD NEW: \t" + strDestFile);
                    }
                    string strDestPath = Path.GetDirectoryName(strDestFile);
                    Directory.CreateDirectory(strDestPath);
                    try {
                        File.Copy(strSrcFile, strDestFile, true);
                    } catch (Exception e) {
                        ListBox1.Items.Add("ERROR!!: \t" + strDestFile);
                    }

                    boolActualize = false;
                } else {
                    ListBox1.Items.Add("NO CHANGE: \t" + strDestFile);
                }
                
                //ListBox1.Refresh();

            }

        }



    }
}
