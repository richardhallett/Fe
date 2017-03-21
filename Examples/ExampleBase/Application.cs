using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBase
{
    public class Application
    {        
        public static void Run(GraphicsCanvas canvas, Action updateFunc)
        {          
            // Main loop go!
            while (canvas.Exists)
            {
                canvas.ProcessEvents();

                updateFunc();                
            }
        }
        
    }
}
