using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RLGames;
using System.Linq;


namespace Toyroom.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Graphics : RLGameComponent
    {
        private Texture2D[] gameGraphicsTextures;        
        private Rectangle[] gameGraphicsRectangles;      
        private Color[] Colors;
        private float[] Rots;
        private bool randomRotation, randomColor;
        private Rectangle tmpRect = new Rectangle(0, 0, 0, 0);
        private Color orgColor = Color.White;
        private double rotation = 0.0f;
        public Color PickedToyColorNumber { get; set; }
        public int graphicElements
        {
            get
            {
                return gameGraphicsTextures.Length;
            }
        }
        public Color[] toyColors
        {
            get
            {
                return Colors;
            }
        }
        public Graphics(Game game, Texture2D[] textures, Rectangle[] rectangles,  bool randRotation, bool randColor, int vertRangeMin, int vertRangeMax, int horRangeMin, int horRangeMax)
            : base(game, textures, rectangles)
           
        {
            randomRotation = randRotation;
            randomColor = randColor;
            rnd = new Random();
            if (randomRotation == true || randomColor == true)
            {
             
            
                if (randomRotation == true)
                {
                    Rots = new float[textures.Length];
                }
                if (randomColor == true)
                {
                    Colors = new Color[textures.Length];


                }
            }




            gameGraphicsRectangles = new Rectangle[textures.Length];
            gameGraphicsTextures = new Texture2D[textures.Length];
            gameGraphicsTextures = textures;


                gameGraphicsRectangles=rectangles;
               
              
               
                spriteBatch = new SpriteBatch(game.GraphicsDevice);
                orgin = new Vector2(0, 0);
              



           
           
            for (int i = 0; i < textures.Length; i++)
            {
                addGameGraphics(i, vertRangeMin,  vertRangeMax,  horRangeMin,  horRangeMax ,  ref gameGraphicsTextures, ref gameGraphicsRectangles, ref Colors, ref Rots);
            }
        }
        public bool removeSprite(string elementName)
        {
          

            for (int i = 0; i <gameGraphicsTextures.Length; i++)
            {
                if (gameGraphicsTextures[i].Name == elementName)
                {
                    gameGraphicsTextures = gameGraphicsTextures.Where((val, idx) => idx != i).ToArray();
                    gameGraphicsRectangles = gameGraphicsRectangles.Where((val, idx) => idx != i).ToArray();
                
                    if(Rots!=null)
                        Rots = Rots.Where((val, idx) => idx != i).ToArray();
                    if (Colors != null)
                    {
                        PickedToyColorNumber = Colors[i];
                        Colors = Colors.Where((val, idx) => idx != i).ToArray();
                    }
                 
                   return true;
                    
                }
                
            }
            return false;

        }
        public void addGraphicItem(int elementNumber, Texture2D texture, Rectangle rectangle, bool randomRotation , bool randomColor,  int vRandMin, int vRandMax, int hRandMin, int hRandMax)
        {
            Array.Resize<Texture2D>(ref gameGraphicsTextures, gameGraphicsTextures.Length+1);
            gameGraphicsTextures[gameGraphicsTextures.Length - 1] = texture;
            gameGraphicsTextures[gameGraphicsTextures.Length - 1].Name = "toy_"+ elementNumber.ToString();
         
            tmpRect = rectangle;
            tmpRect.X = rnd.Next(hRandMin, hRandMax);
            tmpRect.Y = rnd.Next(vRandMin, vRandMax);
            Array.Resize<Rectangle>(ref gameGraphicsRectangles, gameGraphicsRectangles.Length + 1);
           gameGraphicsRectangles[gameGraphicsRectangles.Length - 1] = tmpRect;


         
           if (randomRotation)
           {
               Array.Resize<float>(ref Rots, Rots.Length + 1);
               rotation = rnd.NextDouble();
               rotationDirection = rotationDirection * -1f;
               rotation = rotation * rotationDirection;
               Rots[Rots.Length-1] = (float)rotation;
           }

           if (randomColor)
           {
               Array.Resize<Color>(ref Colors, Colors.Length + 1);
               GameTools.randomColor(ref floorToyColor, 255, rnd);
               tmpColor = floorToyColor;
               while (floorToyColor == tmpColor)
               {
                   GameTools.randomColor(ref floorToyColor, 255, rnd);
               }
               GameTools.randomColor(ref tmpColor, 255, rnd);

               Colors[Colors.Length - 1] = tmpColor;
           }  

        }
        public void addGameGraphics(int elementNumber, int vertRangeMin, int vertRangeMax, int horRangeMin, int horRangeMax, ref Texture2D[] textures, ref Rectangle[] rectangles, 
            ref Color[] Colors, ref float[] Rots)
        {
            int x = rnd.Next(horRangeMin, horRangeMax);
            int y = rnd.Next(vertRangeMin, vertRangeMax);
            double rotation = rnd.NextDouble();
       
            if (Rots !=null)
            {
                rotationDirection = rotationDirection * -1f;
                rotation = rotation * rotationDirection;
                Rots[elementNumber] =(float)rotation;
            }

            if (Colors!=null)
            {

                GameTools.randomColor(ref floorToyColor, 255, rnd);
                tmpColor = floorToyColor;
                while (floorToyColor == tmpColor)
                {
                    GameTools.randomColor(ref floorToyColor, 255, rnd);
                }
                GameTools.randomColor(ref tmpColor, 255, rnd);

                Colors[elementNumber] =tmpColor;
            }
            tmpRect.X = x-(rectangles[elementNumber].Width/2);
            tmpRect.Y = y - (rectangles[elementNumber].Height/2);
            tmpRect.Height = rectangles[elementNumber].Height;
            tmpRect.Width = rectangles[elementNumber].Width;
            rectangles[elementNumber] = tmpRect;
            if (draworder > 1.0f)
                draworder = 0f;

        }

       
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            if (itemDraw)
            {
                if (gameGraphicsTextures != null)
                {
                    if (randomRotation == true && randomColor == true)
                    {
                        for (int i = 0; i <gameGraphicsTextures.Length; i++)
                        {
                            spriteBatch.Draw(gameGraphicsTextures[i], gameGraphicsRectangles[i], null, Colors[i], Rots[i], rotational, SpriteEffects.None, 0.0f);
                        }
                    }
                    if (randomRotation == true && randomColor == false)
                    {
                        for (int i = 0; i <gameGraphicsTextures.Length; i++)
                        {
                            spriteBatch.Draw(gameGraphicsTextures[i], gameGraphicsRectangles[i], null, orgColor, Rots[i], rotational, SpriteEffects.None, 0.0f);
                        }
                    }
                    if (randomRotation == false && randomColor == false)
                    {
                        for (int i = 0; i <gameGraphicsTextures.Length; i++)
                        {
                            spriteBatch.Draw(gameGraphicsTextures[i], gameGraphicsRectangles[i], null, orgColor, 0.0f, rotational, SpriteEffects.None, 0.0f);
                        }
                    }

                }
          
            }
            spriteBatch.End();

        }
    }
}
