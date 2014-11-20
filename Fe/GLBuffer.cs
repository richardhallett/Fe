using System;
using System.Runtime.InteropServices;

namespace Fe
{ 
    internal class GLBuffer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GLBuffer"/> class.
        /// </summary>
        /// <param name="bufferTarget">GL Buffer Target e.g. GL_ARRAY_BUFFER</param>
        public GLBuffer(OpenTK.Graphics.OpenGL.BufferTarget bufferTarget)
        {
            this.BufferTarget = bufferTarget;
        }

        /// <summary>
        /// Create the GL buffer
        /// </summary>
        /// <param name="type">The type of the buffer</param>
        /// <param name="size">The size of the buffer</param>
        /// <param name="data">The data to go in the buffer</param>
        /// <param name="dynamic">if set to <c>true</c> [dynamic].</param>
        /// <exception cref="System.Exception">Buffer size must be greater than 0</exception>
        public void Create(Type type, int size, object data, bool dynamic = false)
        {
            if (size == 0)
            {
                throw new Exception("Buffer size must be greater than 0");
            }

            this.DataType = type;
            this.Size = size;

            // Create the index buffer
            var buffers = new uint[1];
            unsafe
            {
                fixed (uint* ptr = buffers)
                {
                    OpenTK.Graphics.OpenGL.GL.GenBuffers(1, ptr);
                }
            }

            this.BufferId = buffers[0];

            OpenTK.Graphics.OpenGL.GL.BindBuffer(this.BufferTarget, this.BufferId);

            GCHandle buffer_ptr;
            buffer_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                OpenTK.Graphics.OpenGL.GL.BufferData(this.BufferTarget, new IntPtr(this.Size * Marshal.SizeOf(this.DataType)), buffer_ptr.AddrOfPinnedObject(), dynamic ? OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw : OpenTK.Graphics.OpenGL.BufferUsageHint.StreamDraw);
            }
            finally
            {
                buffer_ptr.Free();
            }

            // Unbind the buffer
            OpenTK.Graphics.OpenGL.GL.BindBuffer(this.BufferTarget, 0);
        }

        /// <summary>
        /// Update the buffer with new data from specified offset
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        public void Update(object data, int offset)
        {
            OpenTK.Graphics.OpenGL.GL.BindBuffer(this.BufferTarget, this.BufferId);
            GCHandle buffer_ptr;
            buffer_ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                OpenTK.Graphics.OpenGL.GL.BufferSubData(this.BufferTarget, new IntPtr(offset), new IntPtr(this.Size * Marshal.SizeOf(this.DataType)), buffer_ptr.AddrOfPinnedObject());
            }
            finally
            {
                buffer_ptr.Free();
            }

            OpenTK.Graphics.OpenGL.GL.BindBuffer(this.BufferTarget, 0);
        }

        public OpenTK.Graphics.OpenGL.BufferTarget BufferTarget { get; protected set; }

        /// <summary>
        /// The underlying GL Buffer
        /// </summary>
        public uint BufferId { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the data in the buffer.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public Type DataType { get; protected set; }

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; protected set; }

        #region Cleanup

        /// <summary>
        /// Deletes the underlying opengl buffer
        /// </summary>
        public void Destroy()
        {
            unsafe
            {
                fixed (uint* ptr = new uint[] { this.BufferId })
                {
                    OpenTK.Graphics.OpenGL.GL.DeleteBuffers(1, ptr);
                }
            }
        }

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
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // Fire off the event if people are attached to the event
                if (_OnDispose != null)
                    _OnDispose(this, new EventArgs());

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

                Destroy();

                // Note disposing has been done.
                _disposed = true;
            }
        }

        private event EventHandler _OnDispose;
        /// <summary>
        /// Occurs when [on dispose].
        /// </summary>
        public event EventHandler OnDispose
        {
            add
            {
                _OnDispose += value;
            }
            remove
            {
                _OnDispose -= value;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="GLBuffer"/> is reclaimed by garbage collection.
        /// </summary>
        ~GLBuffer()
        {
            Dispose(false);
        }

        #endregion
    }
}
