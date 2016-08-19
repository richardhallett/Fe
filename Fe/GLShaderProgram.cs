using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Represents an OpenGL shader program
    /// </summary>
    internal class GLShaderProgram : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GLShaderProgram"/> class.
        /// </summary>
        /// <param name="shaders">The shaders.</param>
        internal GLShaderProgram(IReadOnlyList<GLShader> shaders)
        {
            int program = OpenTK.Graphics.OpenGL.GL.CreateProgram();

            foreach (GLShader shader in shaders)
            {
                OpenTK.Graphics.OpenGL.GL.AttachShader(program, shader.ShaderRef);
            }

            OpenTK.Graphics.OpenGL.GL.LinkProgram(program);

            string infoLog = OpenTK.Graphics.OpenGL.GL.GetProgramInfoLog(program);
            Debug.WriteLine(infoLog);

            foreach (GLShader shader in shaders)
            {
                OpenTK.Graphics.OpenGL.GL.DetachShader(program, shader.ShaderRef);
            }

            this.ProgramRef = program;

            this.Uniforms = new Dictionary<string, int>();

            // Go work out what uniforms are available in this program.
            unsafe
            {
                int numActiveUniforms;
                
                OpenTK.Graphics.OpenGL.GL.GetProgram(this.ProgramRef, OpenTK.Graphics.OpenGL.GetProgramParameterName.ActiveUniforms, &numActiveUniforms);

                for(int i = 0; i < numActiveUniforms; i++)
                {
                    int unifLocation;
                    int size;
                    OpenTK.Graphics.OpenGL.ActiveUniformType type;
                    string name = OpenTK.Graphics.OpenGL.GL.GetActiveUniform(this.ProgramRef, i, out size, out type);
                    unifLocation = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.ProgramRef, name);
                    Uniforms.Add(name, unifLocation);
                }
            }
        }

        /// <summary>
        /// Gets or sets the gl program reference.
        /// </summary>
        /// <value>
        /// The gl program.
        /// </value>
        public int ProgramRef { get; protected set; }

        /// <summary>
        /// Gets or sets the uniforms available in this program.
        /// </summary>
        /// <value>
        /// Opengl uniforms.
        /// </value>
        public Dictionary<string, int> Uniforms { get; set; }       

        /// <summary>
        /// Links a program together.
        /// </summary>
        /// <param name="shaders">The shaders to link into the program</param>
        /// <returns>The reference to the shader</returns>
        private int LinkProgram(IReadOnlyList<int> shaders)
        {
            int program = OpenTK.Graphics.OpenGL.GL.CreateProgram();

            foreach (int shader in shaders)
            {
                OpenTK.Graphics.OpenGL.GL.AttachShader(program, shader);
            }

            OpenTK.Graphics.OpenGL.GL.LinkProgram(program);

            string infoLog = OpenTK.Graphics.OpenGL.GL.GetProgramInfoLog(program);
            Debug.WriteLine(infoLog);            

            foreach (int shader in shaders)
            {
                OpenTK.Graphics.OpenGL.GL.DetachShader(program, shader);
            }

            return program;
        }

        #region Cleanup

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                if (this.ProgramRef != 0)
                {
                    OpenTK.Graphics.OpenGL.GL.DeleteProgram(this.ProgramRef);
                }
                
                // Note disposing has been done.
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GLShaderProgram"/> class.
        /// </summary>
        ~GLShaderProgram()
        {
            Dispose(false);
        }

        #endregion
    }
}
