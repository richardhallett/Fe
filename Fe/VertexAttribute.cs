using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// An attribute describes part of a vertex.
    /// </summary>
    public class VertexAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="size">The size.</param>
        /// <param name="type">The type.</param>
        /// <param name="normalised">if set to <c>true</c> [normalised].</param>
        public VertexAttribute(string name, int size, VertexAttributeType type, bool normalised = false)
        {
            this.Name = name;
            this.Size = size;
            this.Type = type;
            this.Normalised = normalised;
        }

        /// <summary>
        /// The name of the attribute.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; protected set; }

        /// <summary>
        /// The size of the attribute in bytes.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; protected set; }

        /// <summary>
        /// The type of the attribute.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public VertexAttributeType Type { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VertexAttribute"/> is normalised.
        /// </summary>
        /// <value>
        ///   <c>true</c> if normalised; otherwise, <c>false</c>.
        /// </value>
        public bool Normalised { get; protected set; }
    }   
}
