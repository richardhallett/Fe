using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A command represents instructions to be used by the renderer on next pass
    /// </summary>
    public class Command
    {
        private const int numStages = 16;

        public Command()
        {
            // Create the commands texture stage data            
            TextureStages = new TextureStage[numStages];
            for (int i = 0; i < numStages; i++ )
            {
                TextureStages[i] = new TextureStage();
            }
        }

        public byte ViewId { internal get { return viewId; } set { viewId = value; } }

        internal ShaderProgram ShaderProgram { get; set; }

        public Shader VertexShader { internal get; set; }

        public Shader FragmentShader { internal get; set; }

        public BlendState BlendState { internal get; set; }

        public DepthState DepthState { internal get; set; }

        public RasteriserState RasteriserState { internal get; set; }

        public VertexBufferBase VertexBuffer { internal get; set; }

        public IndexBuffer IndexBuffer { internal get; set; }

        public UniformBuffer SharedUniforms { internal get; set; }

        public PrimitiveType PrimitiveType { internal get; set; } = PrimitiveType.Triangles;

        public TextureStage[] TextureStages { get; set; }

        public float[] Transform
        {
            internal get { return _transform; }
            set
            {
                _transform[0] = value[0];
                _transform[1] = value[1];
                _transform[2] = value[2];
                _transform[3] = value[3];
                _transform[4] = value[4];
                _transform[5] = value[5];
                _transform[6] = value[6];
                _transform[7] = value[7];
                _transform[8] = value[8];
                _transform[9] = value[9];
                _transform[10] = value[10];
                _transform[11] = value[11];
                _transform[12] = value[12];
                _transform[13] = value[13];
                _transform[14] = value[14];
                _transform[15] = value[15];
                
                isMatrixSet = true;
            }
        }

        /// <summary>
        /// Sets the individual component values of the transform matrix.
        /// </summary>
        /// <remarks>This function can be slightly faster to set the transform components than directly using the property, depending on where the source data is coming from.</remarks>
        public void SetTransformComponents(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            _transform[0] = m11;
            _transform[1] = m12;
            _transform[2] = m13;
            _transform[3] = m14;
            _transform[4] = m21;
            _transform[5] = m22;
            _transform[6] = m23;
            _transform[7] = m24;
            _transform[8] = m31;
            _transform[9] = m32;
            _transform[10] = m33;
            _transform[11] = m34;
            _transform[12] = m41;
            _transform[13] = m42;
            _transform[14] = m43;
            _transform[15] = m44;

            isMatrixSet = true;
        }

        public void Reset()
        {
            ShaderProgram = null;
            VertexShader = null;
            FragmentShader = null;
            BlendState = null;
            RasteriserState = null;
            DepthState = null;
            VertexBuffer = null;
            IndexBuffer = null;
            SharedUniforms = null;
            PrimitiveType = PrimitiveType.Triangles;
            isMatrixSet = false;
            foreach (var ts in TextureStages)
            {
                ts.Reset();
            }
        }

        private float[] _transform = new float[16];
        internal bool isMatrixSet = false;

        private byte viewId;
    }
}
