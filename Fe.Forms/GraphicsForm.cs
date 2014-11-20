using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace Fe.Forms
{
    /// <summary>
    /// A windows form specifically for working with graphics rendering, sets up some sane defaults.
    /// </summary>
    public class GraphicsForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsForm"/> class.
        /// </summary>
        public GraphicsForm()
            : base()
        {
            // Enable double buffering and tell the form we're in charge of the painting.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            ClientSize = new System.Drawing.Size(1280, 720);
        }

        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                Int32 CS_VREDRAW = 0x1;
                Int32 CS_HREDRAW = 0x2;
                Int32 CS_OWNDC = 0x20;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_VREDRAW | CS_HREDRAW | CS_OWNDC;
                return cp;
            }
        }
    }
}
