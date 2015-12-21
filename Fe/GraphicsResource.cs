using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Provides base implemented methods for resources that need to be cached.
    /// </summary>
    public class GraphicsResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsResource"/> class.
        /// </summary>
        public GraphicsResource()
        {
            ResourceIndex = ushort.MaxValue;
            Created = false;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="GraphicsResource"/> has changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if changed; otherwise, <c>false</c>.
        /// </value>
        public bool Changed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GraphicsResource"/> is fully created, i.e. has an underlying resource attached in the backend.
        /// </summary>
        /// <value>
        ///   <c>true</c> if created; otherwise, <c>false</c>.
        /// </value>
        public bool Created { get; set; }

        /// <summary>
        /// An identifier into the backend resource pool.
        /// </summary>
        /// <value>
        /// The index of the resource.
        /// </value>
        internal ushort ResourceIndex { get; set; }
    }
}
