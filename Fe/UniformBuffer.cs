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
            _uniforms = new Dictionary<Uniform, object>();
        }

        /// <summary>
        /// Sets a uniform's value to 4x4 matrix
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="matrix">The matrix.</param>
        public void Set(Uniform uniform, Nml.Matrix4x4 matrix)
        {
            this._uniforms[uniform] = matrix;
        }

        /// <summary>
        /// Sets a uniform's value to a float.
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="value">The value.</param>
        public void Set(Uniform uniform, float value)
        {
            this._uniforms[uniform] = value;
        }
    
        /// <summary>
        /// Sets a uniform's value to a float2 (x,y).
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Set(Uniform uniform, float x, float y)
        {
            this._uniforms[uniform] = new float[] { x, y };
        }

        /// <summary>
        /// Sets a uniform's value to a float3 (x,y,z)
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public void Set(Uniform uniform, float x, float y, float z)
        {
            this._uniforms[uniform] = new float[] { x, y, z};
        }

        /// <summary>
        /// Sets a uniform's value to a float4 (x,y,z,w)
        /// </summary>
        /// <param name="uniform">The uniform.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        /// <param name="w">The w.</param>
        public void Set(Uniform uniform, float x, float y, float z, float w)
        {
            this._uniforms[uniform] = new float[] { x, y, z, w};
        }

        /// <summary>
        /// The _uniforms
        /// </summary>
        internal Dictionary<Uniform, object> _uniforms;
    }
}
