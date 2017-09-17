using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Describes a shader that can be compiled as appropriate by the respective platform.
    /// </summary>
    public class Shader : GraphicsResource
    {
        /// <summary>
        /// Construct a new shader.
        /// </summary>        
        /// <param name="type">The type of the shader</param>
        /// <param name="data">The data that has the shader code in it.</param>
        public Shader(ShaderType type, string data)
        {
            this.Data = data;
            this.Type = type;
        }

        /// <summary>
        /// Gets the type of this shader.
        /// </summary>
        public ShaderType Type {get; internal set; }

        /// <summary>
        /// The shader.
        /// </summary>
        public String Data {get; internal set; }

    }
}
