using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Holds uniforms to be used for the active shader program.
    /// </summary>
    public class UniformBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniformBuffer"/> class.
        /// </summary>
        public UniformBuffer()
        {
            _uniforms = new Dictionary<IUniform, object>();
        }

        /// <summary>
        /// Sets the specified uniform.
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="matrix">The matrix.</param>
        public void Set(Uniform4x4f uniform, Nml.Matrix4x4 matrix)
        {
            this._uniforms[uniform] = matrix;
        }

        /// <summary>
        /// The _uniforms
        /// </summary>
        internal Dictionary<IUniform, object> _uniforms;
    }
}
