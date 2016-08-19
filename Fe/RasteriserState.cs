using System;

namespace Fe
{
    /// <summary>
    /// Describes how primitives are rasterised.
    /// </summary>
    public class RasteriserState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RasteriserState" /> class.
        /// </summary>
        /// <param name="cullMode">The cull mode.</param>
        /// <param name="enableMultisampling">if set to <c>true</c> [enable multisampling].</param>
        public RasteriserState(CullMode cullMode = CullMode.CounterClockwise, bool enableMultisampling = false)
        {
            CullMode = cullMode;
            EnableMultisampling = enableMultisampling;
        }

        /// <summary>
        /// Gets or sets the cull mode.
        /// </summary>
        /// <value>
        /// The cull mode.
        /// </value>
        public CullMode CullMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multisampling is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if multisampling enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableMultisampling { get; set; }

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

            return Equals((RasteriserState)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(RasteriserState other)
        {
            if (other == null)
                return false;

            return (CullMode == other.CullMode && EnableMultisampling == other.EnableMultisampling);
        }

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        /// <returns>
        /// A hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return CullMode.GetHashCode() ^ EnableMultisampling.GetHashCode();
        }
    }
}
