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



namespace Toyroom.Components
{
    
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MoveAbleComponent : RLGameComponent
    {
        private TouchCollection touchCollection;
        private bool itemTouched = false, itemTaken = false, itemMoving = false;
        private int mvSpeed = 20 , startX, startY, offSetPos = 135; // sette i kontstruktÝr? 
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
        //public MoveAbleComponent(Game game, Texture2D texture, Rectangle rectangle, Color color, int moveX, float rot)
        //    : base(game, texture, rectangle, color, rot)
        //{
        //    ComponentColor = color;
        //    startX = rectangle.X;
        //    startY = rectangle.Y;
        //    moveAllowedX = moveX; //moveable component must be higher than this to be allowed in x dir

        //}

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        //public override void Initialize()
        //{


        //    touchCollection = TouchPanel.GetState();
            
            
        //}


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
                            itemMoving = true;
                        }


                        if (itemTouched == true) //&& ((int)tl.Position.X >= componentRectangle.X - 100 &&(int)tl.Position.X <= componentRectangle.X + 100))
                        {
                            componentRectangle.Y = (int)tl.Position.Y - offSetPos;
                            // boy.ComponentTexture = mannTexture;


                            if ((int)tl.Position.X < componentRectangle.X)
                            {
                                itemMoving = true;
                                componentRectangle.X -= mvSpeed;
                            }
                            if ((int)tl.Position.X > componentRectangle.X)
                            {
                                itemMoving = true;
                                componentRectangle.X += mvSpeed;
                            }

                            if ((int)tl.Position.Y < componentRectangle.Y)
                            {
                                itemMoving = true;
                                componentRectangle.Y -= mvSpeed;
                            }
                            if ((int)tl.Position.Y > componentRectangle.Y)
                            {
                                itemMoving = true;
                                componentRectangle.Y += mvSpeed;
                            }
                            

                            return true;

                        }
                    }

                }
            }
            return false;
          
        }

        //public override void Draw(GameTime gameTime)
        //{
          
        //    spriteBatch.Begin();
        //    if(itemDraw)
        //    spriteBatch.Draw(componentTexture, componentRectangle, componentColor);
        //    spriteBatch.End();
            
        //}

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
