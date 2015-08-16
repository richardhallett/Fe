using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    public class Command
    {
        public void SetShaderProgram(ShaderProgram shaderProgram)
        {
            this.ShaderProgram = shaderProgram;
        }

        public void SetVertexBuffer(VertexBufferBase vertexBuffer) 
        {
            this.VertexBuffer = vertexBuffer;
        }

        public void SetIndexBuffer(IndexBuffer indexBuffer)
        {
            this.IndexBuffer = indexBuffer;
        }

        //TODO: Make this compile option for generic matrix type?
        public void SetTransform(Nml.Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public void SetUniformBuffer(UniformBuffer sharedUniforms)
        {
            this.SharedUniforms = sharedUniforms;
        }

        public byte ViewId { get { return viewId; } set { viewId = value; } }

        public ShaderProgram ShaderProgram;

        public VertexBufferBase VertexBuffer;

        public IndexBuffer IndexBuffer;        

        public UniformBuffer SharedUniforms;

        //TODO: This needs to be just a reference to the matrix data, stored in a cache or something
        public Nml.Matrix4x4 Transform;

        public int transformMatrixIndex;

        public byte viewId;

        private ulong sortKey;
    }
}
