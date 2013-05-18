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
using System.Diagnostics;


namespace WindowsPhoneGame1.Components
{
    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MoveAbleComponent : RLGameComponent
    {
        private TouchCollection touchCollection;
        private bool itemTouched = false, itemDraw = false, itemTaken = false, itemMoving = false;
        private int mvSpeed = 20 , startX, startY; // sette i kontstruktør? 
        private bool movingAllowed = true;

        #region Public properties
        public bool MovingAllowed
        {
            set
            {
                movingAllowed = value;
            }
        }
        public bool ItemMoving {
            get
            {
                return itemMoving;
            }
            set
            {
                itemMoving = value;
            }
    }

        public bool ItemTouched {
            get
            {
                return itemTouched;
            }
            set
            {
                itemTouched = value;
            }
    }
        public bool ItemDraw
        {
            get
            {
                return itemDraw;
            }
            set
            {
                itemDraw = value;
            }
        }
        public bool ItemTaken
        {
            get
            {
                return itemTaken;
            }
            set
            {
                itemTaken = value;
            }
        }
       
        #endregion

        public MoveAbleComponent(Game game, Texture2D texture, Rectangle rectangle, Color color, float rot) : base(game, texture, rectangle, color,rot)
           
        {
           ComponentColor = color;
           startX = rectangle.X;
           startY = rectangle.Y;         
           
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            touchCollection = TouchPanel.GetState();
            
            
        }

        protected override void LoadContent()
        {

          
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            itemTaken = moveComponent();
            if (itemTaken == false)
                itemMoving = false;

        }

        public bool moveComponent()
        {
            if (movingAllowed == true)
            {
                touchCollection = TouchPanel.GetState();
                foreach (TouchLocation tl in touchCollection)
                {
                    if ((tl.State == TouchLocationState.Pressed)
                            || (tl.State == TouchLocationState.Moved))
                    {


                        if (componentRectangle.Intersects(new Rectangle((int)tl.Position.X, (int)tl.Position.Y, componentRectangle.Width, componentRectangle.Height)))
                        {
                            itemTouched = true;
                            ItemMoving = true;
                        }


                        if (itemTouched == true)
                        {
                            componentRectangle.Y = (int)tl.Position.Y - 150;
                            // boy.ComponentTexture = mannTexture;
                            if ((int)tl.Position.X < componentRectangle.X)
                                componentRectangle.X -= mvSpeed;
                            if ((int)tl.Position.X > componentRectangle.X)
                                componentRectangle.X += mvSpeed;
                            if ((int)tl.Position.Y < componentRectangle.Y)
                                componentRectangle.Y -= mvSpeed;
                            if ((int)tl.Position.Y > componentRectangle.Y)
                                componentRectangle.Y += mvSpeed;


                            return true;

                        }
                    }

                }
            }
            return false;
          
        }

        public override void Draw(GameTime gameTime)
        {
          
            spriteBatch.Begin();
            if(itemDraw)
            spriteBatch.Draw(componentTexture, componentRectangle, componentColor);
            spriteBatch.End();
            
        }

        public void resetToStart()
        {         
                if(this.CompRectX>startX+20)
                    this.CompRectX-=10;
                 if (this.CompRectX>startX) 
                    this.CompRectX-=1;
     
                if (this.CompRectX < startX - 20)
                    this.CompRectX += 10;
                 if (this.CompRectX < startX)
                    this.CompRectX += 1;

                if (this.CompRectY > startY +20)
                    this.CompRectY -= 10;
                 if (this.CompRectY > startY)
                    this.CompRectY -= 1;
    
                if (this.CompRectY < startY - 20)
                    this.CompRectY += 10;
                 if (this.CompRectY < startY)
                    this.CompRectY += 1;
       
        }
    }
}
