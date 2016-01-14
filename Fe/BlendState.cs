using System;

namespace Fe
{   
    /// <summary>
    /// Describe the blend state that can be used for a draw command.
    /// </summary>
    public class BlendState : IEquatable<BlendState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlendState" /> class.
        /// </summary>
        /// <param name="enableBlending">if set to <c>true</c> [enable blending].</param>
        /// <param name="sourceBlendColour">The source blend colour.</param>
        /// <param name="destinationBlendColour">The destination blend colour.</param>
        /// <param name="colourOperation">The colour operation.</param>
        /// <param name="sourceBlendAlpha">The source blend alpha.</param>
        /// <param name="destinationBlendAlpha">The destination blend alha.</param>
        /// <param name="alphaOperation">The alpha operation.</param>
        /// <param name="blendConstant">The blend constant.</param>
        public BlendState(
            bool enableBlending = false,
            BlendFactor sourceBlendColour = BlendFactor.One,
            BlendFactor destinationBlendColour = BlendFactor.Zero,
            BlendOperation colourOperation = BlendOperation.Add,
            BlendFactor sourceBlendAlpha = BlendFactor.One,
            BlendFactor destinationBlendAlpha = BlendFactor.Zero,
            BlendOperation alphaOperation = BlendOperation.Add,
            Colour4? blendConstant = null)
        {
            EnableBlending = enableBlending;
            SourceBlendColour = sourceBlendColour;
            DestinationBlendColour = destinationBlendColour;
            ColourOperation = colourOperation;
            SourceBlendAlpha = sourceBlendAlpha;
            DestinationBlendAlpha = destinationBlendAlpha;
            AlphaOperation = alphaOperation;
            BlendConstant = blendConstant;
        }

        /// <summary>
        /// Enable or disable blending.
        /// </summary>
        public bool EnableBlending { get; set; }

        /// <summary>
        /// Specifies how the RGB blend factors will be calculated.
        /// </summary>
        public BlendFactor SourceBlendColour { get; set; }

        /// <summary>
        /// Specifies how the RGB blend factors will be calculated for the destination
        /// </summary>
        public BlendFactor DestinationBlendColour { get; set; }

        /// <summary>
        /// How the SourceBlendColour and DestinationBlendColour will be combined.
        /// </summary>
        /// <value>
        /// The colour operation.
        /// </value>
        public BlendOperation ColourOperation { get; set; }

        /// <summary>
        /// Specifies how the alpha value will be calculated.
        /// </summary>
        public BlendFactor SourceBlendAlpha { get; set; }

        /// <summary>
        /// Specifies how the alpha value will be calculated for the destination
        /// </summary>
        public BlendFactor DestinationBlendAlpha { get; set; }

        /// <summary>
        /// How the SourceBlendAlpha and DestinationBlendAlpha will be combined.
        /// </summary>
        /// <value>
        /// The colour operation.
        /// </value>
        public BlendOperation AlphaOperation { get; set; }

        /// <summary>
        /// A RGBA value that to be used by BlendFactor.ConstantAlpha or BlendFactor.InvertConstantAlpha blend modes.
        /// </summary>
        /// <value>
        /// The blend constant.
        /// </value>
        public Colour4? BlendConstant { get; set; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(BlendState a, BlendState b)
        {
            if (((object)a) == null || ((object)b) == null)
                return Object.Equals(a, b);

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(BlendState a, BlendState b)
        {
            if (((object)a) == null || ((object)b) == null)
                return !Object.Equals(a, b);

            return !a.Equals(b);
        }

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
        public bool Equals(BlendState other)
        {
            if (other == null)
                return false;

            if (EnableBlending == other.EnableBlending &&
                SourceBlendColour == other.SourceBlendColour &&
                DestinationBlendColour == other.DestinationBlendColour &&
                ColourOperation == other.ColourOperation &&
                SourceBlendAlpha == other.SourceBlendAlpha &&
                DestinationBlendAlpha == other.DestinationBlendAlpha &&
                AlphaOperation == other.AlphaOperation &&
                BlendConstant.Equals(other.BlendConstant))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        /// <returns>
        /// A hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return EnableBlending.GetHashCode() ^ SourceBlendColour.GetHashCode() ^ DestinationBlendColour.GetHashCode() ^
                   ColourOperation.GetHashCode() ^ SourceBlendAlpha.GetHashCode() ^ DestinationBlendAlpha.GetHashCode() ^
                   AlphaOperation.GetHashCode() ^ BlendConstant.GetHashCode();
        }
    }
}
