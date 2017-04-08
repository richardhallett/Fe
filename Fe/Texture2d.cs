using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    public class Texture2d<T> : TextureBase
    {
        public Texture2d(T[] data, int width, int height, TextureFormat format)
        {
            _data = data;
            Width = width;
            Height = height;
            TextureFormat = format;
            TextureType = TextureType.Texture2D;
        }

        public override int Width { get; set; }
        public override int Height { get; set; }
        public override TextureFormat TextureFormat { get; set; }
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
