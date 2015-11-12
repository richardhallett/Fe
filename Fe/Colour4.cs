using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A four component (RGBA) colour, each component is a float in range 0.0f - 1.0f
    /// </summary>
    public struct Colour4
    {
        /// <summary>
        /// Construct a new instance of Colour4 from packed integer.
        /// </summary>
        /// <param name="rgba">A packed integer for all four components in e.g. 0xFFFFFFFF</param>
        public Colour4(uint rgba)
        {
            this._red = (rgba >> 24 & 0xFF) / 255.0f;
            this._green = (rgba >> 16 & 0xFF) / 255.0f;
            this._blue = (rgba >> 8 & 0xFF) / 255.0f;
            this._alpha = (rgba & 0xFF) / 255.0f;
        }

        /// <summary>
        /// Construct a new instance of Colour4 from individual components.
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <param name="alpha">Alpha component (optional)</param>
        public Colour4(float red, float green, float blue, float alpha = 1.0f)
        {
            this._red = red;
            this._green = green;
            this._blue = blue;
            this._alpha = alpha;
        }        

        /// <summary>
        /// Red component of colour.
        /// </summary>
        public float Red { get { return this._red; } }

        /// <summary>
        /// Green component of colour.
        /// </summary>
        public float Green { get { return this._green; } }

        /// <summary>
        /// Blue component of colour.
        /// </summary>
        public float Blue { get { return this._blue; } }

        /// <summary>
        /// Alpha component of colour.
        /// </summary>
        public float Alpha { get { return this._alpha; } }

        private float _red;
        private float _green;
        private float _blue;
        private float _alpha;
    }
}
