using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A view represents the area that is drawn to by the renderer.
    /// If scissoring is defined then nothing will be drawn outside the scissored rectangle.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="scissorEntireView">if set to <c>true</c> [scissor entire view].</param>
        public View(int x, int y, int width, int height, bool scissorEntireView = false)
        {
            ClearDepth = 1;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            if (scissorEntireView)
            {
                ScissorRect = new ScissorRect(x, y, width, height);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="scissorRect">A scissor rect to define clipping for pixels.</param>
        public View(int x, int y, int width, int height, ScissorRect scissorRect)
        {
            ClearDepth = 1;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            ScissorRect = scissorRect;
        }

        /// <summary>
        /// The colour the view is cleared with.
        /// </summary>
        public Colour4 ClearColour { get; set; }

        /// <summary>
        /// The value used when clearning the depth buffer. 
        /// The value is clamped between 0 and 1.
        /// </summary>
        public float ClearDepth { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public int Y { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the scissor rect. Used for defining clipping of pixels.
        /// </summary>
        /// <value>
        /// The scissor rect.
        /// </value>
        public ScissorRect? ScissorRect { get; set; }
    }
}
