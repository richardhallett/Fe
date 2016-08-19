using System;

namespace Fe
{
    /// <summary>
    /// The render state for z-buffering.
    /// </summary>
    public class DepthState : IEquatable<DepthState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepthState"/> class.
        /// </summary>
        /// <param name="enableDepthTest">if set to <c>true</c> [enable depth test].</param>
        /// <param name="enableDepthWrite">if set to <c>true</c> [enable depth write].</param>
        /// <param name="depthFunc">The depth function.</param>
        public DepthState(bool enableDepthTest = true, bool enableDepthWrite = true, DepthFunc depthFunc = DepthFunc.Less)
        {
            EnableDepthTest = enableDepthTest;
            EnableDepthWrite = enableDepthWrite;
            DepthFunc = depthFunc;
        }

        /// <summary>
        /// Enable or disable depth test.
        /// </summary>
        public bool EnableDepthTest { get; set; }

        /// <summary>
        /// Enable or disable writing into the depth buffer.
        /// </summary>
        public bool EnableDepthWrite { get; set; }

        /// <summary>
        /// Comparison to use for depth buffer test.
        /// </summary>
        /// <value>
        /// The depth function.
        /// </value>
        public DepthFunc DepthFunc { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((DepthState)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(DepthState other)
        {
            if (other == null)
                return false;

            return (EnableDepthTest == other.EnableDepthTest &&
                    EnableDepthWrite == other.EnableDepthWrite &&
                    DepthFunc == other.DepthFunc);
        }

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        /// <returns>
        /// A hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return EnableDepthTest.GetHashCode() ^ EnableDepthWrite.GetHashCode() ^ DepthFunc.GetHashCode();
        }
    }
}
