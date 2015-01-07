using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// A command represents data that instructs the renderer what to do in the next frame.
    /// </summary>   
    public class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
            Transform = Nml.Matrix4x4.Identity;
        }

        /// <summary>
        /// The shader program to use for this command.
        /// </summary>
        /// <value>
        /// The shader program.
        /// </value>
        public ShaderProgram ShaderProgram;

        /// <summary>
        /// Gets or sets a vertex buffer used for this command.
        /// </summary>
        /// <value>
        /// The vertex buffer.
        /// </value>
        public VertexBufferBase VertexBuffer;

        /// <summary>
        /// Gets or sets a index buffer used for this command.
        /// </summary>
        /// <value>
        /// The index buffer.
        /// </value>
        public IndexBuffer IndexBuffer;

        /// <summary>
        /// The transform is used to give a matrix that can be used in shaders for transformations that is per command.
        /// </summary>
        /// <value>
        /// A transform.
        /// </value>
        public Nml.Matrix4x4 Transform;

        /// <summary>
        /// A uniform buffer is shared across all commands that use it and passed through to the shader pipeline.
        /// </summary>
        /// <value>
        /// A shared uniform buffer.
        /// </value>
        public UniformBuffer SharedUniforms;
    }
}
