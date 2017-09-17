using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Combines multiple shader stages built from <see cref="Shader"/> objects into a single program that can then be bound to be used in the rendering pipeline.
    /// </summary>
    public class ShaderProgram : GraphicsResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderProgram" /> class.
        /// </summary>
        /// <param name="vertexShader">The vertex shader.</param>
        /// <param name="fragmentShader">The fragment shader.</param>
        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
        {
            this.VertexShader = vertexShader;
            this.FragmentShader = fragmentShader;
        }

        /// <summary>
        /// Gets or sets the vertex shader.
        /// </summary>
        /// <value>
        /// The vertex shader.
        /// </value>
        internal Shader VertexShader { get; set; }

        /// <summary>
        /// Gets or sets the fragment shader.
        /// </summary>
        /// <value>
        /// The fragment shader.
        /// </value>
        internal Shader FragmentShader { get; set; }
    }
}
