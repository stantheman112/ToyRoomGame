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
        //public override void Draw(GameTime gameTime)
        //{
        //    base.Draw(gameTime);
        //}
                

    }
}
