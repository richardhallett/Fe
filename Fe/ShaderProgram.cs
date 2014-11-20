using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A shader program combines multiple shader stages built from <see cref="Shader"/> objects into a single program that can then be bound to be used in the rendering pipeline.
    /// </summary>
    public class ShaderProgram : GraphicsResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderProgram"/> class.
        /// </summary>
        /// <param name="shaders">The shaders to link together.</param>
        public ShaderProgram(IReadOnlyList<Shader> shaders)
        {
            this.Shaders = shaders;
        }

        /// <summary>
        /// Gets or sets the shaders.
        /// </summary>
        /// <value>
        /// The shaders.
        /// </value>
        public IReadOnlyList<Shader> Shaders { get; private set; }
    }
}
