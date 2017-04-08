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
        private const int numStages = 16;

        public FrameState()
        {
            // Create the commands texture stage data            
            TextureStages = new TextureStage[numStages];
            for (int i = 0; i < numStages; i++)
            {
                TextureStages[i] = new TextureStage();
            }
        }


        public ShaderProgram ShaderProgram { get; set; }

        public Shader VertexShader { get; set; }

        public Shader FragmentShader { get; set; }

        public UniformBuffer SharedUniforms { get; set; }

        public BlendState BlendState { get; set; }

        public DepthState DepthState { get; set; }

        public RasteriserState RasteriserState { get; set; }

        public VertexBufferBase VertexBuffer { get; set; }

        public IndexBuffer IndexBuffer { get; set; }

        public PrimitiveType PrimitiveType { get; set; }

        public TextureStage[] TextureStages { get; set; } = new TextureStage[16];

        public byte? ViewId { get; set; }
    }
}
