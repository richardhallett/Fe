using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// For storing the state of a frame.
    /// </summary>
    internal class FrameState
    {
        public ShaderProgram ShaderProgram { get; set; }

        public UniformBuffer SharedUniforms { get; set; }

        public BlendState BlendState { get; set; }

        public byte? ViewId { get; set; }
    }
}
