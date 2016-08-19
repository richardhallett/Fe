using System;
using System.Diagnostics;

namespace Fe
{
    class GLShader : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GLShader"/> class.
        /// </summary>
        /// <param name="shader">The shader.</param>
        internal GLShader(Shader shader)
        {
            // Build the GL shader            
            switch (shader.Type)
            {
                case ShaderType.Vertex:
                    ShaderRef = this.BuildShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader, shader.Data);
                    break;
                case ShaderType.Fragment:
                    ShaderRef = this.BuildShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader, shader.Data);
                    break;
            }            
        }

        /// <summary>
        /// Gets or sets the gl shader reference.
        /// </summary>
        /// <value>
        /// The gl shader.
        /// </value>
        public int ShaderRef { get; protected set; }
        
        /// <summary>
        /// Build a shader
        /// </summary>
        /// <param name="type">The gl type of shader</param>
        /// <param name="shader_source">The shader_source.</param>
        /// <returns>The reference to the shader</returns>
        private int BuildShader(OpenTK.Graphics.OpenGL.ShaderType type, string shader_source)
        {
            int shader = OpenTK.Graphics.OpenGL.GL.CreateShader(type);

            unsafe
            {
                int length = shader_source.Length;
                OpenTK.Graphics.OpenGL.GL.ShaderSource(shader, 1, new string[] { shader_source }, &length);

                OpenTK.Graphics.OpenGL.GL.CompileShader(shader);

                string infoLog = OpenTK.Graphics.OpenGL.GL.GetShaderInfoLog(shader);

                // TODO: Change this to a different logger
                Debug.WriteLine(infoLog);
            }

            return shader;
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

                if (this.ShaderRef != 0)
                {
                    OpenTK.Graphics.OpenGL.GL.DeleteShader(this.ShaderRef);
                }

                // Note disposing has been done.
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="GLShader"/> class.
        /// </summary>
        ~GLShader()
        {
            Dispose(false);
        }

        #endregion
    }
}
