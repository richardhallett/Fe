﻿using System;
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
        public Command()
        {
            TextureStages = new TextureStage[16];
            for (int stage = 0; stage < 16; stage++)
            {
                TextureStages[stage] = new TextureStage();
            }
        }

        public byte ViewId { internal get { return viewId; } set { viewId = value; } }

        internal ShaderProgram ShaderProgram { get; set; }

        internal CommandInstructions Instructions { get; set; } = CommandInstructions.None;

        public void Reset()
        {
            Instructions = CommandInstructions.None;
            PrimitiveType = PrimitiveType.Triangles;
        }

        public void SetShader(Shader shader)
        {
            switch (shader.Type)
            {
                case ShaderType.Vertex:
                    VertexShader = shader;
                    Instructions |= CommandInstructions.SetVertexShader;
                    break;
                case ShaderType.Fragment:
                    FragmentShader = shader;
                    Instructions |= CommandInstructions.SetFragmentShader;
                    break;
            }
        }

        public void SetVertexBuffer(Buffer vb)
        {
            VertexBuffer = vb;
            Instructions |= CommandInstructions.SetVertexBuffer;
        }

        public void SetIndexBuffer(Buffer ib)
        {
            IndexBuffer = ib;
            Instructions |= CommandInstructions.SetIndexBuffer;
        }

        public void SetTransform(float[] value)
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

            Instructions |= CommandInstructions.SetTransform;
        }

        /// <summary>
        /// Sets the individual component values of the transform matrix.
        /// </summary>
        /// <remarks>This function can be slightly faster to set the transform components than directly using the property, depending on where the source data is coming from.</remarks>
        public void SetTransform(
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

            Instructions |= CommandInstructions.SetTransform;
        }

        public void SetBlendState(BlendState state)
        {
            BlendState = state;
        }

        public void SetDepthState(DepthState state)
        {
            DepthState = state;
        }

        public void SetRasteriserState(RasteriserState state)
        {
            RasteriserState = state;
        }

        public void SetSharedUniforms(UniformBuffer uniforms)
        {
            SharedUniforms = uniforms;
            Instructions |= CommandInstructions.SetSharedUniforms;
        }

        public void SetPrimitiveType(PrimitiveType type)
        {
            PrimitiveType = type;
            Instructions |= CommandInstructions.SetPrimitiveType;
        }

        public void SetTexture(uint stage, Texture texture, Uniform textureUniform, TextureSampler textureSampler)
        {           
            TextureStages[stage].Texture = texture;
            TextureStages[stage].TextureUniform = textureUniform;
            TextureStages[stage].TextureSampler = textureSampler;

            Instructions |= (CommandInstructions)Enum.Parse(typeof(CommandInstructions), $"SetTexture{stage}");
        }

        //public void SetTexture(uint stage, Texture texture, Uniform textureUniform, TextureSampler textureSampler)
        //{
        //    TextureStage textureStage;
        //    switch(stage)
        //    {
        //        case 0:
        //            textureStage = TextureStage0;
        //            break;
        //        case 1:
        //            textureStage = TextureStage1;
        //            break;
        //        case 2:
        //            textureStage = TextureStage2;
        //            break;
        //        case 3:
        //            textureStage = TextureStage3;
        //            break;
        //        case 4:
        //            textureStage = TextureStage4;
        //            break;
        //        case 5:
        //            textureStage = TextureStage5;
        //            break;
        //        case 6:
        //            textureStage = TextureStage6;
        //            break;
        //        case 7:
        //            textureStage = TextureStage7;
        //            break;
        //        case 8:
        //            textureStage = TextureStage8;
        //            break;
        //        case 9:
        //            textureStage = TextureStage9;
        //            break;
        //        case 10:
        //            textureStage = TextureStage10;
        //            break;
        //        case 11:
        //            textureStage = TextureStage11;
        //            break;
        //        case 12:
        //            textureStage = TextureStage12;
        //            break;
        //        case 13:
        //            textureStage = TextureStage13;
        //            break;
        //        case 14:
        //            textureStage = TextureStage14;
        //            break;
        //        case 15:
        //            textureStage = TextureStage15;
        //            break;
        //        default:
        //            return;
        //    }

        //    textureStage.Texture = texture;
        //    textureStage.TextureUniform = textureUniform;
        //    textureStage.TextureSampler = textureSampler;

        //    Instructions |= (CommandInstructions)Enum.Parse(typeof(CommandInstructions), $"Texture{stage}");
        //}

        internal Shader VertexShader { get; set; }

        internal Shader FragmentShader { get; set; }

        internal BlendState BlendState { get; set; }

        internal DepthState DepthState { get; set; }

        internal RasteriserState RasteriserState { get; set; }

        internal Buffer VertexBuffer { get; set; }

        internal Buffer IndexBuffer { get; set; }

        internal UniformBuffer SharedUniforms { get; set; }

        internal PrimitiveType PrimitiveType { get; set; } = PrimitiveType.Triangles;

        internal TextureStage[] TextureStages { get; set; }

        //internal TextureStage TextureStage0 { get; set; }
        //internal TextureStage TextureStage1 { get; set; }
        //internal TextureStage TextureStage2 { get; set; }
        //internal TextureStage TextureStage3 { get; set; }
        //internal TextureStage TextureStage4 { get; set; }
        //internal TextureStage TextureStage5 { get; set; }
        //internal TextureStage TextureStage6 { get; set; }
        //internal TextureStage TextureStage7 { get; set; }
        //internal TextureStage TextureStage8 { get; set; }
        //internal TextureStage TextureStage9 { get; set; }
        //internal TextureStage TextureStage10 { get; set; }
        //internal TextureStage TextureStage11 { get; set; }
        //internal TextureStage TextureStage12 { get; set; }
        //internal TextureStage TextureStage13 { get; set; }
        //internal TextureStage TextureStage14 { get; set; }
        //internal TextureStage TextureStage15 { get; set; }

        internal float[] Transform
        {
            get { return _transform; }
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

                Instructions |= CommandInstructions.SetTransform;
            }
        }

        //internal TextureStage GetTextureStage(uint stage)
        //{
        //    TextureStage textureStage;
        //    switch (stage)
        //    {
        //        case 0:
        //            textureStage = TextureStage0;
        //            break;
        //        case 1:
        //            textureStage = TextureStage1;
        //            break;
        //        case 2:
        //            textureStage = TextureStage2;
        //            break;
        //        case 3:
        //            textureStage = TextureStage3;
        //            break;
        //        case 4:
        //            textureStage = TextureStage4;
        //            break;
        //        case 5:
        //            textureStage = TextureStage5;
        //            break;
        //        case 6:
        //            textureStage = TextureStage6;
        //            break;
        //        case 7:
        //            textureStage = TextureStage7;
        //            break;
        //        case 8:
        //            textureStage = TextureStage8;
        //            break;
        //        case 9:
        //            textureStage = TextureStage9;
        //            break;
        //        case 10:
        //            textureStage = TextureStage10;
        //            break;
        //        case 11:
        //            textureStage = TextureStage11;
        //            break;
        //        case 12:
        //            textureStage = TextureStage12;
        //            break;
        //        case 13:
        //            textureStage = TextureStage13;
        //            break;
        //        case 14:
        //            textureStage = TextureStage14;
        //            break;
        //        case 15:
        //            textureStage = TextureStage15;
        //            break;
        //    }

        //    return textureStage;
        //}

        private float[] _transform = new float[16];

        private byte viewId;
    }
}
