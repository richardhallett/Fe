using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    public class Texture2d<T> : TextureBase
    {
        public Texture2d(int width, int height, PixelFormat format, T[] data)
        {            
            Width = width;
            Height = height;
            PixelFormat = format;
            TextureType = TextureType.Texture2D;

            _data = data;
        }

        public override int Width { get; set; }
        public override int Height { get; set; }
        public override PixelFormat PixelFormat { get; set; }
        public override TextureType TextureType { get; set; }


        /// <summary>
        /// Gets the type of the texture data.
        /// </summary>
        /// <value>
        /// The type of the texture data.
        /// </value>
        internal override Type TextureDataType { get; set; }

        /// <summary>
        /// Gets a representation of the data for internal use.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        internal override Object Data
        {
            get
            {
                return _data;
            }
        }

        private List<T> _vertices;

        private T[] _data;
    }
}
