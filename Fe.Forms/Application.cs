using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fe.Forms
{
    /// <summary>
    /// Platform helper for graphics based forms applications, provides a better main loop.
    /// </summary>
    public class Application
    {
#if PLATFORM_WINDOWS

        internal const string USER32LIB = "user32";

        [StructLayout(LayoutKind.Sequential)]
        internal struct Message
        {
            public IntPtr hWnd;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        [DllImport(USER32LIB, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

#endif

        /// <summary>
        /// An update function that you want to get called all the time.
        /// </summary>
        public delegate void Update();

        private static Update _updateFunc;

        /// <summary>
        /// Gets a value indicating whether [application still idle].
        /// </summary>
        /// <value>
        /// <c>true</c> if [application still idle]; otherwise, <c>false</c>.
        /// </value>
        static bool AppStillIdle
        {
            get
            {
#if PLATFORM_WINDOWS
                Message msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
#else
                return true;
#endif                
            }
        }

        /// <summary>
        /// Runs the specified form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="updateFunc">The update function.</param>
        public static void Run(Form form, Update updateFunc)
        {
            _updateFunc = updateFunc;
            // hook the application's idle event
            System.Windows.Forms.Application.Idle += new EventHandler(OnApplicationIdle);
            System.Windows.Forms.Application.Run(form);
        }

        /// <summary>
        /// Called when [application idle].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void OnApplicationIdle(object sender, EventArgs e)
        {
#if PLATFORM_WINDOWS
            while (AppStillIdle)
            {
                _updateFunc();
            }
#elif PLATFORM_LINUX
            _updateFunc();
#endif
        }
    }
}
