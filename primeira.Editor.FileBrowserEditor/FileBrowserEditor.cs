using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using primeira.Editor.Components;
using primeira.Editor;

[assembly: EditorHeader(
    "primeira.Editor.FileBrowserEditor",
    "AddonInitialize",
    AddonOptions.LastInitilizedAddon,
    Name = "FileBrowserEditor",
    Id = "{B44995A7-81F8-41B8-94D0-9234A024966A}")]

namespace primeira.Editor
{
    [EditorDocument(DocumentType = typeof(FileBrowserDocument))]
    public partial class FileBrowserEditor :  EditorBase, IRecentFileControl
    {
        #region Fields

        private Image m_file;

        #endregion

        #region Ctor

        public FileBrowserEditor()
        {
            InitializeComponent();
        }

        public FileBrowserEditor(string fileName)
            : base(fileName)
        {

            InitializeComponent();

            FileManager.SetRecentManager(this);

            this.OnSelected += new SelectedDelegate(FileBrowser_OnSelected);

            m_file = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        }

        private Image GetDraftImage(Image image)
        {
            try
            {
                Bitmap b = new Bitmap(image.Width, image.Height - 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Rectangle r = new Rectangle(0, 0, image.Width, image.Height - 1);

                LinearGradientBrush brush = new LinearGradientBrush(r, Color.FromArgb(10, 255, 255, 255), Color.FromArgb(230, 255, 255, 255), 90);

                Graphics g = Graphics.FromImage(b);

                Pen p = new Pen(Color.White);

                g.DrawImage(image, 0, 0);

                g.FillRectangle(brush, r);

                b.MakeTransparent(Color.White);

                return (Image)b;
            }
            catch
            {
                return new Bitmap(1, 1);
            }
        }

        private void createQuickLaunch()
        {
            dgQuickLauch.Rows.Clear();

             foreach (DocumentHeader doc in DocumentManager.Headers)
            {
                if (doc.Options.HasFlag(DocumentHeaderOptions.ShowInQuickLauchDraft))
                {
                    int i = dgQuickLauch.Rows.Add(
                            new object[] { GetDraftImage(doc.Icon),
                            string.Format("Draft {0} File ", doc.Name),
                            "draft", 0, "", doc });

                    dgQuickLauch.Rows[i].Selected = false;
                }
            }

            foreach (DocumentHeader doc in DocumentManager.Headers)
            {
                if ((doc.Options & DocumentHeaderOptions.ShowIQuickLauchnOpen) > 0)
                {
                    int i = dgQuickLauch.Rows.Add(
                            new object[] {  doc.Icon,
                            string.Format("Open or Create {0} File ", doc.Name),
                            "", 0, "", doc });

                    dgQuickLauch.Rows[i].Selected = false;
                }
            }


        }

        private void createRecent()
        {
            dgRecentFiles.Rows.Clear();

            string[] files = ((FileBrowserDocument)Document).Recent;

            Size s = new Size(dgRecentFiles.Columns[1].Width, dgRecentFiles.RowTemplate.Height);
            Font f = dgRecentFiles.DefaultCellStyle.Font;
            DateTime d = DateTime.Now;
            DocumentHeader doc;
            TimeSpan lastWrite;

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    doc = DocumentManager.GetDocumentHeader(file);

                    lastWrite = d.Subtract(File.GetLastWriteTime(file));

                    dgRecentFiles.Rows.Add(
                    new object[] { 
                        doc.Icon,
                        file, FileManager.LastWrite(lastWrite), (int)lastWrite.TotalSeconds, file, null });
                }
            }

            dgRecentFiles.Sort(ColOrder, ListSortDirection.Ascending);
        }

        #endregion

        public static void AddonInitialize()
        {
            DocumentHeader doc = DocumentManager.GetDocumentHeader(typeof(FileBrowserDocument));

            EditorManager.LoadEditor(doc);
        }   

        #region Event Handlers

        private void FileBrowser_OnSelected(IEditor sender)
        {
            createQuickLaunch();

            createRecent();
        }

        #endregion

        #region DataGrids Event Handlers

        private void dataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Selected = true;
        }

        private void dataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            ((DataGridView)sender).Rows[e.RowIndex].Selected = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {   
            if(sender == dgDirFiles)
            {
                EditorManager.LoadEditor(dgDirFiles.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
            else if (sender == dgRecentFiles)
               EditorManager.LoadEditor(dgRecentFiles.Rows[e.RowIndex].Cells[4].Value.ToString());
            else
            {
                if(dgQuickLauch.Rows[e.RowIndex].Cells[2].Value.ToString() == "draft")
                {
                    string s = FileManager.GetNewFile((DocumentHeader)dgQuickLauch.Rows[e.RowIndex].Cells[5].Value, DocumentManager.BaseDir);
                    s = Path.Combine(DocumentManager.BaseDir, s);
                    File.Create(s).Close();
                    EditorManager.LoadEditor(s);
                }
                else
                {
                    OpenOrCreateDocument(true, (DocumentHeader)dgQuickLauch.Rows[e.RowIndex].Cells[5].Value);
                }
            }
        }

        private void OpenOrCreateDocument(bool NewFile, DocumentHeader FileVersion)
        {
            OpenFileDialog s = new OpenFileDialog();

            s.CheckFileExists = false;

            if (NewFile)
                s.FileName = FileManager.GetNewFile(FileVersion, DocumentManager.BaseDir);

            s.Filter = DocumentManager.RenderDialogFilterString();

            s.DefaultExt = FileVersion.DefaultFileExtension;

            s.FilterIndex = DocumentManager.GetDialogFilterIndex(FileVersion);

            s.InitialDirectory = DocumentManager.BaseDir;

            if (s.ShowDialog() == DialogResult.OK)
            {
                DocumentManager.BaseDir = s.InitialDirectory;

                string ss = Path.Combine(DocumentManager.BaseDir, s.FileName);

                if (!File.Exists(ss))
                    File.Create(ss).Close();

                EditorManager.LoadEditor(ss);
            }
        }

        #endregion

        #region Methods

        public void AddRecent(string fileName)
        {
            ((FileBrowserDocument)Document).AddRecent(fileName);

            Changed();
        }

        public string[] GetRecent()
        {
            return ((FileBrowserDocument)Document).Recent;
        }

        #endregion

        private void folderBrowser1_OnDirectoryChange(string directoryPath)
        {
            dgDirFiles.Rows.Clear();

            string[] files;
            DateTime d = DateTime.Now;

            foreach(DocumentHeader doc in DocumentManager.Headers)
            {

                files = Directory.GetFiles(directoryPath,"*"+ doc.DefaultFileExtension);

                foreach (string file in files)
                {
                    TimeSpan t = d.Subtract(File.GetLastWriteTime(file));
                    dgDirFiles.Rows.Add(
                    new object[] { m_file, file, FileManager.LastWrite(t), (int)t.TotalSeconds, file, null });
                }

            }


        }

        private void FileBrowserEditor_Load(object sender, EventArgs e)
        {
            if (dgRecentFiles.SelectedRows.Count == 1)
                dgRecentFiles.SelectedRows[0].Selected = false;

            if (dgQuickLauch.SelectedRows.Count == 1)
                dgQuickLauch.SelectedRows[0].Selected = false;

        }





    }
}
