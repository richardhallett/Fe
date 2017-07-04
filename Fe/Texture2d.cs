using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    /// <summary>
    /// Represents a 2 dimensional texture object.
    /// </summary>
    /// <typeparam name="T">The type for the data being passed in for the texture to use.</typeparam>
    /// <seealso cref="Fe.Texture" />
    public class Texture2d<T> : Texture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2d{T}"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format of the pixel data.</param>
        /// <param name="data">The pixel data.</param>
        public Texture2d(int width, int height, SampleFormat format, T[] data)
        {            
            Width = width;
            Height = height;
            SampleFormat = format;
            TextureType = TextureType.Texture2D;

            _data = data;
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public override int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public override int Height { get; set; }

        /// <summary>
        /// Gets or sets the sample format.
        /// </summary>
        /// <value>
        /// The sample format.
        /// </value>
        public override SampleFormat SampleFormat { get; set; }

        /// <summary>
        /// Gets or sets the type of the texture.
        /// </summary>
        /// <value>
        /// The type of the texture.
        /// </value>
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

        private T[] _data;
    }
}
