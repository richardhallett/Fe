using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Stores index elements passed to the GPU that are used to index the vertices in a <see cref="VertexBuffer{T}" />
    /// </summary>
    public class IndexBuffer : Buffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="dynamic">if set to <c>true</c> [dynamic].</param>
        public IndexBuffer(int size, bool dynamic = false)
        {            
            this.Size = size;
            this.Dynamic = dynamic;
            this._indices = new List<uint>(this.Size);

            DataType = typeof(uint); // Index Buffers are always uint
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexBuffer"/> class.
        /// </summary>
        /// <param name="indices">The indices.</param>
        /// <param name="dynamic">if set to <c>true</c> [dynamic].</param>
        public IndexBuffer(IReadOnlyCollection<uint> indices = null, bool dynamic = false)
            : this(indices.Count, dynamic)
        {
            this.SetData(indices);
        }

        /// <summary>
        /// Set the data for the buffer
        /// </summary>
        /// <param name="indices">Vertices to fill the buffer with.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Too many indices for this buffer</exception>
        public void SetData(IReadOnlyCollection<uint> indices)
        {
            if (indices.Count > this.Size)
            {
                throw new ArgumentOutOfRangeException("Too many indices for this buffer");
            }

            this._indices = new List<uint>(indices);
            this.Changed = true;
        }

        /// <summary>
        /// Gets or sets the indice at the specified index.
        /// </summary>
        /// <value>
        /// The vertex.
        /// </value>
        /// <param name="index">The index into the buffer</param>
        /// <returns></returns>
        public uint this[int index]
        {
            get
            {
                var vertex = this._indices[index];
                return vertex;
            }
            set
            {
                this._indices[index] = value;
                this.Changed = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this IndexBUffer is dynamic.
        /// </summary>
        /// <value>
        ///   <c>true</c> if dynamic; otherwise, <c>false</c>.
        /// </value>
        public override bool Dynamic { get; set; }

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public override int Size { get; set; }

        /// <summary>
        /// Gets the type of the data is stored as.
        /// </summary>
        /// <value>
        /// The type of the vertex.
        /// </value>
        internal override Type DataType { get; set; }

        /// <summary>
        /// Gets the Indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        public IReadOnlyList<uint> Indices
        {
            get
            {
                return this._indices.AsReadOnly();
            }
        }

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
                return _indices.ToArray();
            }
        }

        private List<uint> _indices;
    }
}
