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
    /// <summary>
    /// A command represents instructions to be used by the renderer on next pass
    /// </summary>
    public class Command
    {
        public void SetVertexShader(Shader vertexShader)
        {
            this.VertexShader = vertexShader;
        }

        public void SetFragmentShader(Shader fragmentShader)
        {
            this.FragmentShader = fragmentShader;
        }

        public void SetBlendState(BlendState blendState)
        {
            BlendState = blendState;
        }

        public void SetDepthState(DepthState depthState)
        {
            DepthState = depthState;
        }

        public void SetRasteriserState(RasteriserState rasteriserState)
        {
            RasteriserState = rasteriserState;
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

        public void SetPrimitiveType(PrimitiveType primitiveType)
        {
            this.PrimitiveType = primitiveType;
        }

        public byte ViewId { get { return viewId; } set { viewId = value; } }

        internal ShaderProgram ShaderProgram;

        internal Shader VertexShader;

        internal Shader FragmentShader;

        internal BlendState BlendState;

        internal DepthState DepthState;

        internal RasteriserState RasteriserState;

        internal VertexBufferBase VertexBuffer;

        internal IndexBuffer IndexBuffer;

        internal UniformBuffer SharedUniforms;

        internal PrimitiveType PrimitiveType;

        internal Nml.Matrix4x4? Transform;

        private byte viewId;
    }
}
