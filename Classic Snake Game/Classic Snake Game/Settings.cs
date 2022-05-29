using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classic_Snake_Game
{
    class Settings
    {
        public static int Width { get; set; } // we don't need to make instance of this variables beacuse they are static. We can call them directly.
        public static int Height { get; set; } // we don't need to make instance of this variables beacuse they are static. We can call them directly.

        public static string directions;
        

        public Settings() //creating constructor
        {
            Width = 16;
            Height = 16;
            directions = "left";
            
        }


    }
}
