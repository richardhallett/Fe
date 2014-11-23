using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct CommandState
    {
        public ShaderProgram ShaderProgram;

        public VertexBufferBase VertexBuffer;

        public IndexBuffer IndexBuffer;

        public int TransformMatrixIndex;

        public UniformBuffer SharedUniforms;
    }
}
