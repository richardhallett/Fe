
namespace Fe.Examples.Basics
{
    struct PosColorVertex : Fe.IVertex
    {
        public float x;
        public float y;
        public float z;
        public uint abgr;

        public PosColorVertex(float x, float y, float z, uint abgr = 0xFFFFFFFF)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.abgr = abgr;
        }

        private static Fe.VertexDeclaration vertexDeclaration = new Fe.VertexDeclaration
        (
            new Fe.VertexAttribute[] {
                        new Fe.VertexAttribute("Position", 3, Fe.VertexAttributeType.Float),
                        new Fe.VertexAttribute("Colour", 4, Fe.VertexAttributeType.Byte, true)
                }
        );

        Fe.VertexDeclaration Fe.IVertex.VertexDeclaration { get { return vertexDeclaration; } }

    };

}
