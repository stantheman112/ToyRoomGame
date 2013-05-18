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
using Microsoft.Xna.Framework.Input.Touch;
using RLGames;
using System.Diagnostics;


namespace WindowsPhoneGame1.Components
{

   
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameGUI : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private string infoText = "0/20";
        private Vector2 textPosition, origin;
        private float textScale, textDepth, textRotation;
        private Color textColor;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private int maxScore, currentScore;
        private bool advancedSettings, elementClicked;
        private TouchCollection touchCollection;
        private Texture2D guiBackgroundTxt;
      


        private Rectangle guiSize, guiBackgroundRect;
        GameState gameState;
        
#region public properties
        public bool GuiVisible { get; set; }
        public Rectangle GuiRect {
            get
            {
                return guiSize;
            }  
        }
        public Vector2 TextPosition
        {
            get
            {
                return textPosition;
            }
            set
            {
                textPosition = value;
            }
        }
        public Rectangle GuiBackgroundRect
        {
            get
            {
                return guiBackgroundRect;
            }
            set
            {
                guiBackgroundRect = value;
            }
        }

        public string InfoText {
            get
            {
                return infoText;
            }
            set
            {
                infoText = value;
            }
    }
        public Texture2D GuiBackgroundTxt
        {
            set {
                guiBackgroundTxt = value;
            }
            get
            {
                return guiBackgroundTxt;
            }
        }
        public int CurrentScore
        {
            set
            {
                currentScore = value;
            }
            get
            {
                return currentScore;
            }
        }
        public int MaxScore
        {
            set
            {
                maxScore = value;
            }
            get
            {
                return maxScore;
            }
        }
        public bool ElementClicked
        {
            get
            {
                return elementClicked;
            }
        }
#endregion
        

        public GameGUI(Game game, SpriteFont sprtFnt, Vector2 pos, string info, Color color)
            : base(game)     
           
        {
            spriteFont = sprtFnt;
            textPosition = pos;
            infoText = info;
            textColor = color;

           
        }

        public GameGUI(Game game, SpriteFont sprtFnt, Vector2 pos, string info, Color color, float rot, Vector2 ori, float scale, float z )
            : base(game)
        {
            spriteFont = sprtFnt;
            textColor = color;
            infoText = info;
            textScale = scale;
            origin = ori;
            textDepth = z;
            textRotation = rot;
            textPosition = pos;
            advancedSettings = true;


        }

        public GameGUI(Game game, SpriteFont sprtFnt, Vector2 pos, string info, Color color, int guiSizeX, int guiSizeY, GameState gmstate)
            : base(game)
        {
            spriteFont = sprtFnt;
            textColor = color;
            infoText = info;
            //textScale = scale;
           // origin = ori;
            //textDepth = z;
            //textRotation = rot;
            textPosition = pos;
            advancedSettings = false;
            guiSize = new Rectangle((int)pos.X, (int)pos.Y, guiSizeX, guiSizeY);
            gameState = gmstate;
        }



        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            
            
        }

        protected override void LoadContent()
        {
           
          
        }
        public bool chkClicked(Vector2 guiElementPos)
        {
           
            touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
               // Debug.WriteLine(tl.Position);
                //Vector2 touchSpot = new Vector2((int)tl.Position.X, (int)tl.Position.Y);
               Rectangle tmp = new Rectangle(Convert.ToInt16(tl.Position.X), Convert.ToInt16(tl.Position.Y), 20, 40);
                //Rectangle element = new Rectangle(Convert.ToInt16(guiElementPos.X), Convert.ToInt16(guiElementPos.Y), 20, 40);
               if (guiSize.Intersects(tmp))
                   gameState.ActiveScene = Convert.ToInt16(infoText);
                
                //if ((tl.State == TouchLocationState.Pressed)
                //        || (tl.State == TouchLocationState.Moved))
                //{
                  

                //}

            }
            return false;
          
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
           
            //if (chkClicked(this.textPosition))
            //{

            //    GameTools.randomColor(ref textColor, 255);
            //    elementClicked = true;

            //}

          

        }
        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (advancedSettings == false)
                spriteBatch.DrawString(spriteFont, infoText, textPosition, textColor);
            else
            {
                if (GuiVisible == true)
                {

                    if (guiBackgroundTxt != null)
                    {
                       
                        spriteBatch.Draw(guiBackgroundTxt, guiBackgroundRect, Color.White);
                        spriteBatch.DrawString(spriteFont, infoText, textPosition, textColor, textRotation, origin, textScale, SpriteEffects.None, textDepth);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, infoText, textPosition, textColor, textRotation, origin, textScale, SpriteEffects.None, textDepth);
                    }
                }
            }

            spriteBatch.End();
            
        }
    }
}
