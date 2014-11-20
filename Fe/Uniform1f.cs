using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A uniform for describing a single floating point number.
    /// </summary>
    public class Uniform1f : IUniform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Uniform1f"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Uniform1f(string name)
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
