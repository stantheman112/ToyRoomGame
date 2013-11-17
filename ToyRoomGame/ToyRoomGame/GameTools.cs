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
      

        public static Color[] elementColors(Color[] colors)
        {


            colors[0] = Color.Red;
            colors[1] = Color.Blue;
            colors[2] = Color.Yellow;
            colors[3] = Color.Violet;
            colors[4] = Color.SpringGreen;
            colors[5] = Color.Brown;
            colors[6] = Color.White;
            colors[7] = Color.Orange;

            return colors;
        }
       public static bool randomColor(ref Color rcolor, byte alpha, Random rand) {
           
           rcolor.G = Convert.ToByte(rand.Next(256));
           rcolor.R = Convert.ToByte(rand.Next(256));
           rcolor.B = Convert.ToByte(rand.Next(256));
           rcolor.A = alpha;
           
           return true;
          
       }
       public static Color randomColor(Color rcolor, Random rand )
       {
           
          

           rcolor.G = Convert.ToByte(rand.Next(256));
           rcolor.R = Convert.ToByte(rand.Next(256));
           rcolor.B = Convert.ToByte(rand.Next(256));
         

           return rcolor;

       }
   }
}