using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;

namespace ExampleBase
{
    public class GraphicsCanvas
    {
        public GraphicsCanvas(int width = 1280, int height = 720, string title = "Fe Example")
        {
            _nativeWindow = new NativeWindow(
               width,
               height,
               title,
                GameWindowFlags.Default,
                new GraphicsMode(32, 24, 0, 8),
                DisplayDevice.Default);

            _nativeWindow.Visible = true;
            
            _nativeWindow.Resize += OnResize;
            _nativeWindow.Closing += OnClosing;
            //_nativeWindow.KeyDown += OnKeyDown;
            //_nativeWindow.KeyUp += OnKeyUp;
            //_nativeWindow.KeyPress += OnKeyPress;
            _nativeWindow.MouseDown += OnMouseDown;
            _nativeWindow.MouseUp += OnMouseUp;
            //_nativeWindow.MouseWheel += OnMouseWheel;            
            //_nativeWindow.Closed += OnClosed;            
        }

        public event Action Resize;
        public event Action Closing;
        public event Action MouseDown;
        public event Action MouseUp;

        public int Width
        {
            get { return _nativeWindow.Width; }
            set { _nativeWindow.Width = value; }
        }

        public int Height
        {
            get { return _nativeWindow.Height; }
            set { _nativeWindow.Height = value; }
        }
        
        public string Title
        {
            get { return _nativeWindow.Title; }
            set { _nativeWindow.Title = value; }
        }

        public bool Exists => _nativeWindow.Exists;
        public IntPtr Handle => _nativeWindow.WindowInfo.Handle;

        public void ProcessEvents()
        {
            this._nativeWindow.ProcessEvents();
        }

        private void OnResize(object sender, EventArgs e)
        {
            Resize?.Invoke();
        }

        private void OnClosing(object sender, EventArgs e)
        {
            Closing?.Invoke();
        }

        private void OnMouseDown(object sender, EventArgs e)
        {
            MouseDown?.Invoke();
        }

        private void OnMouseUp(object sender, EventArgs e)
        {
            MouseUp?.Invoke();
        }

        private NativeWindow _nativeWindow;
    }
}
