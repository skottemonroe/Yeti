using System;
using System.IO;
using System.Windows.Forms;

namespace Yeti {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            //textBox2.Text = @"C:\temp\SRC";
            //textBox3.Text = @"C:\temp\DEST";
        }



        /*
         * 
         * TAB ONE
         * 
         * In which we read a single fFile and upload it to The Cloud!!!
         * 
         */

        private void tabPage1_DragEnter(object sender, DragEventArgs e) {
            //This piece really just has to exist so we can DRAG a fFile OVER the box
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void tabPage1_DragDrop(object sender, DragEventArgs e) {

            //Once the fFile gets here, we need to read it.
            //This will toss all fFile names into an array.
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length != 1) {
                MessageBox.Show("Only 1 fFile at a time, please!!");

            } else if (Directory.Exists(files[0])) {
                MessageBox.Show("Sorry, No Directories allowed!!");

            } else {
                string strMes = "Do you want to upload this file?\n\n" + files[0];
                if (MessageBox.Show(strMes, "UPLOAD?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    //do some display stuff
                    label3.Visible = true;
                    this.Refresh();

                    //this sends it to our controller to do the actual upload.
                    UploadController uc = new UploadController();
                    string result = uc.PrepareToCopy(files[0]);

                    //Do some more display stuff
                    label3.Visible = false;
                    textBox1.Visible = true;
                    textBox1.Text = result;
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            //We probably want to paste the new fFilename in a
            //webbrowser or email or chatbox or something, so just copy it.
            Clipboard.SetText(textBox1.Text);
        }



        /*
         * 
         * TAB TWO
         * 
         * In which we perform a series of Backup fFile operations!
         * 
         */

        private void button1_Click(object sender, EventArgs e) {

            string strSource = textBox2.Text;
            string strDest = textBox3.Text;

            //validate

            if (!Directory.Exists(strSource)) {
                MessageBox.Show("Source must be an existing fFolder Path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(strDest)) {
                MessageBox.Show("Destination must be an existing fFolder Path!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            BackupController bc = new BackupController(strSource, strDest, listBox1);
            bc.BeginBackup();

            MessageBox.Show("BACKUP IS COMPLETED!!\nHOORAY!","Job Done",MessageBoxButtons.OK,MessageBoxIcon.Information);
        
        }
    }
}
