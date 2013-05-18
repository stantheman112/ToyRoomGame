using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using WindowsPhoneGame1.Scenes;


namespace WindowsPhoneGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameState : Microsoft.Xna.Framework.GameComponent
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch; Scene1 scene1; Lobby lobby;
        
        
        Scene1 scene2;
        Scene3 scene3;
        Scene1 scene4;
     
       
  
     
        public int ActiveScene {get; set;}

        public GameState(Game game) : base(game)
        {
           
         
        
           
            
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
       

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
       
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

   
    }
}
