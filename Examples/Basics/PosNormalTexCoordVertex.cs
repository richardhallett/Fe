
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
