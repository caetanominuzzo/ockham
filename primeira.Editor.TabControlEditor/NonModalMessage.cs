using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace primeira.Editor
{
    public class NonModalMessage : Label
    {
        private static Timer _timer;
        private static Queue<NonModalMessage> _messages;
        private Control _parent;

        private NonModalMessage(Control parent)
        {
            _parent = parent;
            this.Dock = this.Dock & DockStyle.Right;
            this.Left = 0;
            this.Top = 0;
            this.Width = _parent.Width;
            this.Height = 30;
            this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            this.BackColor = Color.FromArgb(255, 240, 180);
            this.TextAlign = ContentAlignment.MiddleLeft;
        }

        public new void Dispose()
        {
            if (_messages.Count == 0)
                _timer.Dispose();

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        static void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            NonModalMessage m = _messages.Peek();

            Point p = m.PointToClient(MousePosition);

            if (!m.Bounds.Contains(p))
            {
                m = _messages.Dequeue();

                m._parent.Controls.Remove(m);

                m.Dispose();
            }

            if (_messages.Count > 0)
                _timer.Start();
        }

        public static NonModalMessage GetInstance(string message, Control parent)
        {
            if(_timer == null)
            {
                _timer = new Timer();
                _timer.Interval = 2000;
                _timer.Tick += new EventHandler(_timer_Tick);

                _messages = new Queue<NonModalMessage>();
            }

            _timer.Start();

            NonModalMessage m = new NonModalMessage(parent);
            m.Text = message;
            _messages.Enqueue(m);

            parent.Controls.Add(m);

            m.BringToFront();

            return m;
        }
    }
}
