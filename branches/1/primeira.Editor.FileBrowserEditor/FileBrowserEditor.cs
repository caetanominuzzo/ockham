﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using primeira.Editor.Business;
using primeira.Editor.Components;

namespace primeira.Editor
{
    public partial class FileBrowserEditor :  EditorBase, IRecentFileControl
    {
        #region Fields

        private Image m_file;

        #endregion

        #region Ctor

        public FileBrowserEditor(string filename, DocumentBase data)
            : base(filename, data, typeof(FileBrowserDocument))
        {
            InitializeComponent();

            this.TabButton.Size = new Size(40, 40);

            this.TabButton.SetToolTip("File Tab");

            this.ShowCloseButton = false;

            this.OnSelected += new SelectedDelegate(FileBrowser_OnSelected);

            this.TabButton.SelectedImage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");
            this.TabButton.UnselectedImage = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

            m_file = Image.FromFile(@"D:\Desenv\Neural Network\Imgs.png");

        }

        private Image GetDraftImage(Image image)
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

        private void createQuickLaunch()
        {
            dgQuickLauch.Rows.Clear();

            DocumentDefinition[] defs = DocumentManager.GetKnowDocumentDefinition();

            foreach (DocumentDefinition def in defs)
            {
                if ((def.Options & DocumentDefinitionOptions.ShowInQuickLauchDraft) == DocumentDefinitionOptions.ShowInQuickLauchDraft)
                {
                    int i = dgQuickLauch.Rows.Add(
                            new object[] { GetDraftImage(def.Icon), 
                            string.Format("Draft {0} File ", def.Name),
                            "draft", 0, "", def });

                    dgQuickLauch.Rows[i].Selected = false;
                }
            }

            foreach (DocumentDefinition def in defs)
            {
                if ((def.Options & DocumentDefinitionOptions.ShowIQuickLauchnOpen) == DocumentDefinitionOptions.ShowIQuickLauchnOpen)
                {
                    int i = dgQuickLauch.Rows.Add(
                            new object[] { def.Icon, 
                            string.Format("Open or Create {0} File ", def.Name),
                            "", 0, "", def });

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
            DocumentDefinition docDef;
            TimeSpan lastWrite;

            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    docDef = DocumentManager.GetDocumentDefinitionByFilename(file);

                    lastWrite = d.Subtract(File.GetLastWriteTime(file));

                    dgRecentFiles.Rows.Add(
                    new object[] { 
                        docDef==null? m_file : docDef.Icon,
                        file, FileManager.LastWrite(lastWrite), (int)lastWrite.TotalSeconds, file, null });
                }
            }

            dgRecentFiles.Sort(ColOrder, ListSortDirection.Ascending);
        }

        #endregion

        #region Event Handlers

        private void FileBrowser_OnSelected(IEditorBase sender)
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
                    string s = FileManager.GetNewFile((DocumentDefinition)dgQuickLauch.Rows[e.RowIndex].Cells[5].Value, DocumentManager.BaseDir);
                    s = Path.Combine(DocumentManager.BaseDir, s);
                    File.Create(s).Close();
                    EditorManager.LoadEditor(s);
                }
                else
                {
                    DocumentManager.OpenOrCreateDocument(true, (DocumentDefinition)dgQuickLauch.Rows[e.RowIndex].Cells[5].Value);
                }
            }
        }

        #endregion

        #region Methods

        public void AddRecent(string filename)
        {
            ((FileBrowserDocument)Document).AddRecent(filename);

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

            DocumentDefinition[] defs = DocumentManager.GetKnowDocumentDefinition();
            string[] files;
            DateTime d = DateTime.Now;

            foreach(DocumentDefinition def in defs)
            {
                files = Directory.GetFiles(directoryPath,"*"+ def.Extension);

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