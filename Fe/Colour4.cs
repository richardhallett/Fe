using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A four component (RGBA) colour, each component is a float in range 0.0f - 1.0f
    /// </summary>
    public struct Colour4 : IEquatable<Colour4>
    {
        /// <summary>
        /// Construct a new instance of Colour4 from packed integer.
        /// </summary>
        /// <param name="rgba">A packed integer for all four components in e.g. 0xFFFFFFFF</param>
        public Colour4(uint rgba)
        {
            this._red = (rgba >> 24 & 0xFF) / 255.0f;
            this._green = (rgba >> 16 & 0xFF) / 255.0f;
            this._blue = (rgba >> 8 & 0xFF) / 255.0f;
            this._alpha = (rgba & 0xFF) / 255.0f;
        }

        /// <summary>
        /// Construct a new instance of Colour4 from individual components.
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <param name="alpha">Alpha component (optional)</param>
        public Colour4(float red, float green, float blue, float alpha = 1.0f)
        {
            this._red = red;
            this._green = green;
            this._blue = blue;
            this._alpha = alpha;
        }        

        /// <summary>
        /// Red component of colour.
        /// </summary>
        public float Red { get { return this._red; } }

        /// <summary>
        /// Green component of colour.
        /// </summary>
        public float Green { get { return this._green; } }

        /// <summary>
        /// Blue component of colour.
        /// </summary>
        public float Blue { get { return this._blue; } }

        /// <summary>
        /// Alpha component of colour.
        /// </summary>
        public float Alpha { get { return this._alpha; } }

        private float _red;
        private float _green;
        private float _blue;
        private float _alpha;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Colour4 a, Colour4 b)
        {
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
        public static bool operator !=(Colour4 a, Colour4 b)
        {
            return a.Equals(b);
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

            return Equals((Colour4)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Colour4 other)
        {
            if (_red == other._red &&
                _green == other._green &&
                _blue == other.Blue &&
                _alpha == other.Alpha)
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
            return _red.GetHashCode() ^ _green.GetHashCode() ^ _blue.GetHashCode() ^ _alpha.GetHashCode();
        }
    }
}
