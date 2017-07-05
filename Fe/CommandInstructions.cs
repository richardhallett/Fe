using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    [Flags]
    internal enum CommandInstructions
    {
        None = 0,
        SetShaderProgram = 1 << 1,
        SetVertexShader = 1 << 2,
        SetFragmentShader = 1 << 3,
        SetBlendState = 1 << 4,
        SetRasteriserState = 1 << 5,
        SetDepthState = 1 << 6,
        SetVertexBuffer = 1 << 7,
        SetIndexBuffer = 1 << 8,
        SetSharedUniforms = 1 << 9,
        SetPrimitiveType = 1 << 10,
        SetTransform = 1 << 11,
        SetTexture0 = 1 << 12,
        SetTexture1 = 1 << 13,
        SetTexture2 = 1 << 14,
        SetTexture3 = 1 << 15,
        SetTexture4 = 1 << 16,
        SetTexture5 = 1 << 17,
        SetTexture6 = 1 << 18,
        SetTexture7 = 1 << 19,
        SetTexture8 = 1 << 20,
        SetTexture9 = 1 << 21,
        SetTexture10 = 1 << 22,
        SetTexture11 = 1 << 23,
        SetTexture12 = 1 << 24,
        SetTexture13 = 1 << 25,
        SetTexture14 = 1 << 26,
        SetTexture15 = 1 << 27,
        SetTextureAll = SetTexture0 | SetTexture1 | SetTexture2 | SetTexture3 | SetTexture4 | SetTexture5 | SetTexture6 | SetTexture7 | SetTexture8 | SetTexture9 | SetTexture10 | SetTexture11 | SetTexture12 | SetTexture13 | SetTexture14 | SetTexture15
    }
}
