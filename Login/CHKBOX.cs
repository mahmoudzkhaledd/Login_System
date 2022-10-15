﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    using System.Drawing;
    using System.Windows.Forms;

    public class TodoCheckBox : CheckBox
    {
        public override bool AutoSize
        {
            get => base.AutoSize;
            set => base.AutoSize = false;
        }

        public TodoCheckBox()
        {
            this.TextAlign = ContentAlignment.MiddleRight;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h = this.ClientSize.Height - 2;
            var rc = new Rectangle(new Point(-1, this.Height / 2 - h / 2), new Size(h, h));
            if (this.FlatStyle == FlatStyle.Flat)
            {
                ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Flat | ButtonState.Checked : ButtonState.Flat | ButtonState.Normal);
            }
            else
            {
                ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Checked : ButtonState.Normal);
            }
        }
    }
}
