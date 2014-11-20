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
    public interface IUniform
    {
        /// <summary>
        /// Gets or sets the name of the uniform.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
    }
}
