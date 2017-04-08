using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Describes a vertex layout.
    /// </summary>
    public class VertexDeclaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        /// <param name="vertexAttributes">The vertex attributes.</param>
        public VertexDeclaration(IReadOnlyCollection<VertexAttribute> vertexAttributes)
        {
            this.Attributes = new List<VertexAttribute>();
            this.Attributes.AddRange(vertexAttributes);

            this.Offsets = new Dictionary<string, int>();

            this.Stride = 0;
            var stride = 0;
            foreach (var attribute in this.Attributes)
            {
                Type attribType;
                attribMapping.TryGetValue(attribute.Type, out attribType);

                this.Offsets[attribute.Name] = this.Stride;
                stride = Marshal.SizeOf(attribType) * attribute.Size;

                this.Stride += stride;
            }
        }

        /// <summary>
        /// Gets or sets the attributes as defined in the shader
        /// </summary>
        /// <remarks>The vertex attributes must be laid out in the same order(i.e. same index) as they are in the shader.</remarks>
        /// <value>
        /// The attributes.
        /// </value>
        public List<VertexAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the stride between each attribute.
        /// </summary>
        /// <value>
        /// The stride.
        /// </value>
        public int Stride { get; protected set; }

        /// <summary>
        /// The offsets for each attribute.
        /// </summary>
        /// <value>
        /// The offsets.
        /// </value>
        public Dictionary<string, int> Offsets { get; protected set; }

        /// <summary>
        /// The attribute mapping
        /// </summary>
        private static Dictionary<VertexAttributeType, Type> attribMapping = new Dictionary<VertexAttributeType, Type>() 
        {
            {VertexAttributeType.Float, typeof(float)}, 
            {VertexAttributeType.Byte, typeof(byte)}, 
            {VertexAttributeType.Short, typeof(short)}
        };
    }
}
