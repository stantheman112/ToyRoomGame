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
    public class GameManager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        
        Lobby lobby;
        Scene1 scene1;
        Scene2 scene2;
        Scene3 scene3;
        Scene4 scene4;
        Scene5 scene5;
        Scene6 scene6;
       

        private int activeScene = 0;
     
        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            lobby = new Lobby(this, Content, Components);
          //  scene1 = new Scene1(this, Content, Components);

            Components.Add(lobby);
           
            
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.SupportedOrientations = DisplayOrientation.LandscapeRight |

                              DisplayOrientation.LandscapeLeft;
                                 


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

           
         

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
           
            
           
            switch (activeScene)
            {
                case 1:
                    if (scene1.SceneCompleted == true)
                    {
                        Components.Remove(scene1);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                case 2:
                    if (scene2.SceneCompleted == true)
                    {
                        Components.Remove(scene2);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                case 3:
                    if (scene3.SceneCompleted == true)
                    {
                        Components.Remove(scene3);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                case 4:
                    if (scene4.SceneCompleted == true)
                    {
                        Components.Remove(scene4);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                case 5:
                    if (scene5.SceneCompleted == true)
                    {
                        Components.Remove(scene5);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                case 6:
                    if (scene6.SceneCompleted == true)
                    {
                        Components.Remove(scene6);
                        lobby = new Lobby(this, Content, Components);
                        Components.Add(lobby);
                        activeScene = 0;
                        lobby.WichLevel = 0;

                    }
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            if (lobby.WichLevel != 0)
            {
                switch (lobby.WichLevel)
                {
                    case 1:
                        scene1 = new Scene1(this, Content, Components);
                        Components.Add(scene1);
                        Components.Remove(lobby);
                        activeScene = 1;
                        lobby.WichLevel = -1;                        
                        break;
                    case 2:
                        scene2 = new Scene2(this, Content, Components);
                        Components.Add(scene2);
                        Components.Remove(lobby);
                        activeScene = 2;
                        lobby.WichLevel = -1;  
                        break;
                    case 3:
                        scene3 = new Scene3(this, Content, Components);
                        Components.Add(scene3);
                        Components.Remove(lobby);
                        activeScene = 3;
                        lobby.WichLevel = -1;
                        break;
                    case 4:
                        scene4 = new Scene4(this, Content, Components);
                        Components.Add(scene4);
                        Components.Remove(lobby);
                        activeScene = 4;
                        lobby.WichLevel = -1;
                        break;
                    case 5:
                        scene5 = new Scene5(this, Content, Components);
                        Components.Add(scene5);
                        Components.Remove(lobby);
                        activeScene = 5;
                        lobby.WichLevel = -1;
                        break;
                    case 6:
                        scene6 = new Scene6(this, Content, Components);
                        Components.Add(scene6);
                        Components.Remove(lobby);
                        activeScene = 6;
                        lobby.WichLevel = -1;
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

            }
            
            //
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
                                   base.Draw(gameTime);
        }
    }
}
