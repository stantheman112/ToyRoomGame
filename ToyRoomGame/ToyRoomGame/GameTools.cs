using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RLGames
{
   public static class GameTools  
    {            
        private static List<Color> colors = new List<Color>();
      

        public static List<Color> elementColors()
        {                      
           
            List<Color> colors = new List<Color>();
            colors.Add(Color.Red);
            colors.Add(Color.Blue);
            colors.Add(Color.Yellow);
            colors.Add(Color.Violet);
            colors.Add(Color.SpringGreen);
            colors.Add(Color.Brown);
            colors.Add(Color.White);
            colors.Add(Color.Orange);

            return colors;
        }
       public static bool randomColor(ref Color rcolor, byte alpha) {
           Random rand = new Random();
          
           rcolor.G = Convert.ToByte(rand.Next(256));
           rcolor.R = Convert.ToByte(rand.Next(256));
           rcolor.B = Convert.ToByte(rand.Next(256));
           rcolor.A = alpha;
           
           return true;
          
       }
       public static Color randomColor( )
       {
           Color rcolor = new Color();
           Random rand = new Random();

           rcolor.G = Convert.ToByte(rand.Next(256));
           rcolor.R = Convert.ToByte(rand.Next(256));
           rcolor.B = Convert.ToByte(rand.Next(256));
         

           return rcolor;

       }
   }
}