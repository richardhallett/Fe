using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Stores vertices to be used on the GPU.
    /// </summary>
    public class VertexBuffer<T> : VertexBufferBase where T : IVertex
    {
        /// <summary>
        /// Init a VertexBuffer of a specific IVertex type.
        /// </summary>
        /// <param name="size">The number of vertices allowed in this buffer.</param>
        /// <param name="dynamic">Whether or not the data should be hinted as dynamic or not (potential performance gains if changing constantly)</param>
        public VertexBuffer(int size, bool dynamic = false)
        {
            this.Size = size;
            this.VertexType = typeof(T);
            this._vertices = new List<T>(size);

            this.Dynamic = dynamic;

            var type = Activator.CreateInstance(this.VertexType) as IVertex;
            if (type == null)
            {
                throw new Exception("Invalid Vertex Type specified, does not inherit from IVertex");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexBuffer{T}"/> class.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="dynamic">if set to <c>true</c> [dynamic].</param>
        public VertexBuffer(IReadOnlyCollection<T> vertices = null, bool dynamic = false)
            : this(vertices.Count, dynamic)
        {
            this.SetData(vertices);
        }

        /// <summary>
        /// Set the data for the buffer
        /// </summary>
        /// <param name="vertices">Vertices to fill the buffer with.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Too many vertices for this buffer</exception>
        public void SetData(IReadOnlyCollection<T> vertices)
        {
            if (vertices.Count > this.Size)
            {
                throw new ArgumentOutOfRangeException("Too many vertices for this buffer");
            }

            this._vertices = new List<T>(vertices);
            this.Changed = true;
        }

        /// <summary>
        /// Gets or sets the vertex at the specified index.
        /// </summary>
        /// <value>
        /// The vertex.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                var vertex = this._vertices[index];
                return vertex;
            }
            set
            {
                this._vertices[index] = value;
                this.Changed = true;
            }
        }
       
        /// <summary>
        /// Gets or sets a value indicating whether this VertexBuffer is dynamic.
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
        /// Gets the vertices.
        /// </summary>
        /// <value>
        /// The vertices.
        /// </value>
        public IReadOnlyList<T> Vertices
        {
            get
            {
                return this._vertices.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the type of the vertex.
        /// </summary>
        /// <value>
        /// The type of the vertex.
        /// </value>
        internal override Type VertexType { get; set; }

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
                return Vertices.ToArray();
            }
        }

        private List<T> _vertices;
    }
}
