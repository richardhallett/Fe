using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Interface to describe a generic vertex buffer.
    /// </summary>
    public abstract class Buffer : GraphicsResource
    {       
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Buffer"/> is dynamic.
        /// </summary>
        /// <value>
        ///   <c>true</c> if dynamic; otherwise, <c>false</c>.
        /// </value>
        public abstract bool Dynamic { get; set; }

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public abstract int Size { get; set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        internal abstract Object Data { get; }

        /// <summary>
        /// Gets the type of the data in the vertex.
        /// </summary>
        /// <value>
        /// The type of the vertex.
        /// </value>
        internal abstract Type DataType { get; set; }
    }
}
