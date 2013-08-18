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


namespace Toyroom.Components
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BasicComponent : RLGameComponent
    {
        
        
     
     

      

        public BasicComponent(Game game, Texture2D texture, Rectangle rectangle, Color color, float rot)
            : base(game, texture, rectangle, color, rot)
        {
            rotation = rot;
        }
        public BasicComponent(Game game, Texture2D texture, Rectangle rectangle, float rot) : base(game, texture, rectangle, rot) 
           
        {
            rotation = rot;
           ComponentColor = Color.White;
           
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
            // TODO: Add your update code here
          //  ComponentRectangle.X++;

        }

        //public override void Draw(GameTime gameTime)
        //{
        //    spriteBatch.Begin();
        //    if (rotation == 0)
        //        spriteBatch.Draw(componentTexture, componentRectangle, componentColor);
        //    else
        //        spriteBatch.Draw(componentTexture, componentRectangle, null, componentColor, rotation, new Vector2(0, 0), SpriteEffects.None, 0.0f);
        //    spriteBatch.End();
         

        //}
    }
}
