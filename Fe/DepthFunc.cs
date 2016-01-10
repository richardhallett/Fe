using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// The function to use to compare each incoming pixel depth value with the depth value present in the depth buffer.
    /// </summary>
    public enum DepthFunc
    {
        /// <summary>
        /// Never passes
        /// </summary>
        Never = 1,
        /// <summary>
        /// Passes if the incoming depth value is less than the stored depth value.
        /// </summary>
        Less = 2,
        /// <summary>
        /// Passes if the incoming depth value is equal to the stored depth value.
        /// </summary>
        Equal = 3,
        /// <summary>
        /// Passes if the incoming depth value is less than or equal to the stored depth value.
        /// </summary>
        LessEqual = 4,
        /// <summary>
        /// Passes if the incoming depth value is greater than the stored depth value.
        /// </summary>
        Greater = 5,
        /// <summary>
        /// Passes if the incoming depth value is not equal to the stored depth value.
        /// </summary>
        NotEqual = 6,
        /// <summary>
        /// Passes if the incoming depth value is greater than or equal to the stored depth value.
        /// </summary>
        GreaterEqual = 7,
        /// <summary>
        /// Always passes.
        /// </summary>
        Always = 8
    }
}
