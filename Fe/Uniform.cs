using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A Uniform is a constant variable passed to shaders.
    /// </summary>
    public class Uniform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Uniform" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public Uniform(string name, UniformType type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the name of the uniform.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the uniform.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public UniformType Type { get; set; }
    }
}
