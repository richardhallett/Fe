using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Used to defines a vertex.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// The vertex declaration to describe the attributes and how each vertex is laid out in memory.
        /// </summary>
        /// <value>
        /// The vertex declaration.
        /// </value>
        VertexDeclaration VertexDeclaration { get; }
    }
}
