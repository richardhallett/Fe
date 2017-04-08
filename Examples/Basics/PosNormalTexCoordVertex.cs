
namespace Fe.Examples.Basics
{
    struct PosNormalTexCoordVertex : Fe.IVertex
    {
        public float x;
        public float y;
        public float z;
        public float normx;
        public float normy;
        public float normz;
        public float texcoord0;
        public float texcoord1;
        public uint abgr;

        public PosNormalTexCoordVertex(float x, float y, float z, float nx, float ny, float nz, float u, float v, uint abgr)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.normx = nx;
            this.normy = ny;
            this.normz = nz;
            this.texcoord0 = u;
            this.texcoord1 = v;

            this.abgr = abgr;
        }

        private static Fe.VertexDeclaration vertexDeclaration = new Fe.VertexDeclaration
        (
            new Fe.VertexAttribute[] {
                        new Fe.VertexAttribute("Position", 3, Fe.VertexAttributeType.Float),
                        new Fe.VertexAttribute("Normal", 3, Fe.VertexAttributeType.Float),
                        new Fe.VertexAttribute("UV", 2, Fe.VertexAttributeType.Float),
                        new Fe.VertexAttribute("Colour", 4, Fe.VertexAttributeType.Byte, true)
                }
        );

        Fe.VertexDeclaration Fe.IVertex.VertexDeclaration { get { return vertexDeclaration; } }

    };

}
