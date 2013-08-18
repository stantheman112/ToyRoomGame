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
    /// This is the master class for all gamecomponenttypes
    /// </summary>
    public abstract class RLGameComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
       protected Texture2D componentTexture;
      protected   Rectangle componentRectangle;
        List<Color> basketColor = new List<Color>();
        List<Texture2D> textures = new List<Texture2D>();
       protected Color componentColor = Color.White, leftBsktColor = Color.White, itemColor = Color.Wheat;
       protected TouchCollection touchCollection;
       protected float rotation;
        public int ComponentNumber = 0;
       public string ComponentType { get; set; }
        protected bool itemDraw = true;

       #region public properties
       public Texture2D ComponentTexture
        {
            set
            {
                componentTexture = value;
            }
            get
            {
                return componentTexture;
            }
        }
        public Color ComponentColor
        {
            set
            {
                componentColor = value;

            }
            get
            {
                return componentColor;
            }
        }
        public Rectangle ComponentRectangle
        {
            set
            {
                componentRectangle = value;

            }
            get
            {
                return componentRectangle;
            }
        }
        public int CompRectX
        {
            set
            {
                componentRectangle.X = value;
            }
            get
            {
                return componentRectangle.X;
            }
        }
        public int CompRectY
        {
            set
            {
                componentRectangle.Y = value;
            }
            get
            {
                return componentRectangle.Y;
            }
        }
        public int CompRectWidth
        {
            set
            {
                componentRectangle.Width = value;
            }
            get
            {
                return componentRectangle.Width;
            }
        }
        public int CompRectHeight
        {
            set
            {
                componentRectangle.Height = value;
            }
            get
            {
                return componentRectangle.Height;
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
       #endregion 
        public RLGameComponent(Game game, Texture2D texture, Rectangle rectangle, Color color, float rot)
            : base(game)
        {
            componentRectangle = rectangle;
            componentColor = color;
            componentTexture = texture;
            // TODO: Construct any child components here
        }
       
        public  RLGameComponent(Game game, Texture2D texture, Rectangle rectangle, float rot)
            : base(game)
        {
            componentRectangle = rectangle;            
            componentTexture = texture;
            rotation = rot;
         
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override  void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            touchCollection = TouchPanel.GetState();
            base.Initialize();
        }

        protected override  void LoadContent()
        {

            base.LoadContent();
        }

        public bool compPushed(TouchCollection touchCollection)
        {
           
            foreach (TouchLocation tl in touchCollection)
            {
                if ((tl.State == TouchLocationState.Pressed)
                        || (tl.State == TouchLocationState.Moved))
                {
                   if(this.componentRectangle.Contains( (int)tl.Position.X, (int)tl.Position.Y))                  
                    {
                        return true;
                    }
                }

            }
            
            
            return false; 
        }
      

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (itemDraw == true)
            {
                spriteBatch.Begin();

                if (rotation == 0.0f)
                    spriteBatch.Draw(componentTexture, componentRectangle, componentColor);
                else
                    spriteBatch.Draw(componentTexture, componentRectangle, null, componentColor, rotation, new Vector2(0, 0), SpriteEffects.None, 0.0f);
                spriteBatch.End();
                base.Draw(gameTime);
            }
        }
    }
}
