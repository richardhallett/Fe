using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A uniform for describing 4x4 floating point matrix data.
    /// </summary>
    public class Uniform4x4f : IUniform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Uniform4x4f"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Uniform4x4f(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the uniform.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
