using System.Linq;
using ImageSharp;
using System.IO;

namespace Fe.Examples.Basics
{
    class TexturesExample : IExample
    {
        Fe.Shader vs;
        Fe.Shader fs;
        Fe.Texture2d<Color> texture;
        Fe.TextureSampler textureSampler;
        Fe.Uniform colourMapUniform;
        Fe.RasteriserState rasteriserState;

        public TexturesExample(Renderer renderer)
        {
            // Create the shaders
            Fe.Shader vertexShader, fragmentShader;
            switch (renderer.GetRendererType())
            {
                case Fe.RendererType.OpenGL:
                    vertexShader = new Fe.Shader(Fe.ShaderType.Vertex, File.ReadAllText("withtexture.vert"));
                    fragmentShader = new Fe.Shader(Fe.ShaderType.Fragment, File.ReadAllText("withtexture.frag"));
                    break;
                default:
                    throw new System.Exception("Unknown backend renderer type");
            }

            vs = vertexShader;
            fs = fragmentShader;

            var cube = new Fe.Extra.Geometry.Cube();

            // Vertices that make up a cube.
            PosNormalTexCoordVertex[] vertices = new PosNormalTexCoordVertex[cube.Vertices.Length / 3];

            int i = 0;
            int vertOffset = 0;
            int texOffset = 0;
            for (i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = cube.Vertices[vertOffset];
                vertices[i].y = cube.Vertices[vertOffset + 1];
                vertices[i].z = cube.Vertices[vertOffset + 2];

                vertices[i].normx = cube.Normals[vertOffset];
                vertices[i].normy = cube.Normals[vertOffset + 1];
                vertices[i].normz = cube.Normals[vertOffset + 2];

                vertices[i].texcoord0 = cube.UVs[texOffset];
                vertices[i].texcoord1 = cube.UVs[texOffset + 1];

                vertices[i].abgr = 0xFF00FFFF;

                vertOffset += 3;
                texOffset += 2;
            }

            // For demo purposes lets up one side of of the cubes uv's to show sampler behaviour.
            vertices[1].texcoord0 = 5.0f;
            vertices[2].texcoord1 = 5.0f;
            vertices[3].texcoord0 = 5.0f;
            vertices[3].texcoord1 = 5.0f;

            // Create vertex buffer for our cube
            this._vb = new Fe.VertexBuffer<PosNormalTexCoordVertex>(vertices);

            // Create index buffer
            this._ib = new Fe.IndexBuffer(cube.Indices);

            rasteriserState = new RasteriserState(CullMode.Clockwise);

            colourMapUniform = new Fe.Uniform("colourMap", Fe.UniformType.Uniform1f);

            // Create a texture sampler state
            textureSampler = new TextureSampler();
            textureSampler.AddressU = TextureAddressMode.Repeat;
            textureSampler.AddressV = TextureAddressMode.Repeat;

            // Load example image we're going to apply
            using (Image image = Image.Load(@"..\assets\lilly.jpg"))
            {                
                texture = new Fe.Texture2d<Color>(image.Width, image.Height, SampleFormat.RGBA8, image.Pixels);
            }            

        }
        

        public void Update(CommandBucket commandBucket)
        {
            var cube = commandBucket.AddCommand(2);

            cube.SetShader(vs);
            cube.SetShader(fs);
            cube.SetVertexBuffer(_vb);
            cube.SetIndexBuffer(_ib);
            cube.SetSharedUniforms(_ub);
            cube.SetRasteriserState(rasteriserState);
            cube.SetTexture(0, texture, colourMapUniform, textureSampler);
            
            cube.SetTransform((Nml.Matrix4x4.Translate(new Nml.Vector3(z: -1.0f)) * Nml.Matrix4x4.RotateY(-0.5f)).ToArray());
        }

        private VertexBuffer<PosNormalTexCoordVertex> _vb;
        private IndexBuffer _ib;
        private UniformBuffer _ub;
    }
}
