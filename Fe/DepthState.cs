namespace Fe
{
    /// <summary>
    /// The render state for z-buffering.
    /// </summary>
    public class DepthState
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
    }
}
