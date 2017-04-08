using System;
using System.Collections.Generic;
using System.Text;

namespace Fe
{
    public class TextureStage
    {
        public TextureBase Texture { get; private set; }

        public Uniform TextureUniform { get; private set; }

        public bool IsSet { get; private set; }

        public void Set(TextureBase texture, Uniform textureUniform)
        {
            Texture = texture;
            TextureUniform = textureUniform;
            IsSet = true;
        }

        public void Reset()
        {
            Texture = null;
            TextureUniform = null;
            IsSet = false;
        }
    }
}
