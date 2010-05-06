using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    partial class FileBrowserEditor
    {

        [ShortcutVisibility("File Browser", "Shows the File Browser tab", BasicEscopes.Global, Keys.T, Keys.Control)]
        public void show()
        {
            EditorManager.LoadEditor(this.DefaultFileName).Selected = true;
        }

        [ShortcutVisibility("1st Recent", "Opens the last recent document open.", BasicEscopes.Active, Keys.D1, Keys.Control)]
        public void Open1Recent()
        {
            open_nth_recent(1);
        }
        [ShortcutVisibility("2nd Recent", "Opens the second last recent document open.", BasicEscopes.Active, Keys.D2, Keys.Control)]
        public void Open2Recent()
        {
            open_nth_recent(2);
        }
        [ShortcutVisibility("3rd Recent", "Opens the third last recent document open.", BasicEscopes.Active, Keys.D3, Keys.Control)]
        public void Open3Recent()
        {
            open_nth_recent(3);
        }
        [ShortcutVisibility("4th Recent", "Opens the fourth last recent document open.", BasicEscopes.Active, Keys.D4, Keys.Control)]
        public void Open4Recent()
        {
            open_nth_recent(4);
        }
        [ShortcutVisibility("5th Recent", "Opens the fifth last recent document open.", BasicEscopes.Active, Keys.D5, Keys.Control)]
        public void Open5Recent()
        {
            open_nth_recent(5);
        }
        [ShortcutVisibility("6th Recent", "Opens the sixth last recent document open.", BasicEscopes.Active, Keys.D6, Keys.Control)]
        public void Open6Recent()
        {
            open_nth_recent(6);
        }
        [ShortcutVisibility("7th Recent", "Opens the seventh recent document open.", BasicEscopes.Active, Keys.D7, Keys.Control)]
        public void Open7Recent()
        {
            open_nth_recent(7);
        }
        [ShortcutVisibility("8th Recent", "Opens the second last recent document open.", BasicEscopes.Active, Keys.D8, Keys.Control)]
        public void Open8Recent()
        {
            open_nth_recent(8);
        }
        [ShortcutVisibility("9th Recent", "Opens the second last recent document open.", BasicEscopes.Active, Keys.D9, Keys.Control)]
        public void Open9Recent()
        {
            open_nth_recent(9);
        }
        private void open_nth_recent(int recentIndex)
        {
            if (dgRecentFiles.RowCount >= recentIndex)
                EditorManager.LoadEditor(dgRecentFiles["ColFileName", recentIndex - 1].Value.ToString()).Selected = true;
        }
    }
}
