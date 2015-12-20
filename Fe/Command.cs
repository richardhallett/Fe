﻿using System;
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

        internal ShaderProgram ShaderProgram;

        internal VertexBufferBase VertexBuffer;

        internal IndexBuffer IndexBuffer;

        internal UniformBuffer SharedUniforms;

        internal Nml.Matrix4x4 Transform;

        private byte viewId;
    }
}
