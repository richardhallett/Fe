using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// The types of uniform constants.
    /// </summary>
    public enum UniformType
    {
        /// <summary>
        /// A 4x4 floating point matrix
        /// </summary>
        Matrix4x4f,
        /// <summary>
        /// A single floating point.
        /// </summary>
        Uniform1f,
        /// <summary>
        /// A 2 dimensional floating point vector
        /// </summary>
        Uniform2f,
        /// <summary>
        /// A 3 dimensional floating point vector
        /// </summary>
        Uniform3f,
        /// <summary>
        /// A 3 dimensional floating point vector
        /// </summary>
        Uniform4f
    }
}
