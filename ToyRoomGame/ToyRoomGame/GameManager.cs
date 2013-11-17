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
using Toyroom.Scenes;



namespace Toyroom
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameManager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;            
        Lobby lobby;
        Scene1 scene1;
        Scene2 scene2;
        Scene3 scene3;
        Scene4 scene4;
        Scene5 scene5;
        Scene6 scene6;
        Scene7 scene7;
     
       

        private int activeScene = 0, wichLevel = -1;
     
        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";         
            TargetElapsedTime = TimeSpan.FromTicks(333333);
       
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            lobby = new Lobby(this, Content, Components);
        
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

            graphics.IsFullScreen = true;
            graphics.GraphicsDevice.Clear(Color.Lime);
            base.Initialize();
        }

      

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
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
            {
                //sjekk om spillet er i gang gå så tilbake til meny
                //hvis man er i meny exit gamet totalt
                if (activeScene == 0)
                {
                    this.Exit();
                }
            }

            if (lobby != null)
                wichLevel = lobby.WichLevel;
            else
                wichLevel = -1;
               
          
           
            switch (activeScene)
            {
                case 1:
                    if (scene1.SceneCompleted == true)
                    {
                        if (scene1.Restart == true)
                            wichLevel = 1;
                        else
                        {
                            Components.Remove(scene1);
                         Content.Unload();
                            scene1 = null;
                           
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;
                        }
                      

                    }
                    break;
                case 2:
                    if (scene2.SceneCompleted == true)
                    {
                        if (scene2.Restart == true)
                            wichLevel = 2;
                        else
                        {
                            Components.Remove(scene2);
                            Content.Unload();
                            scene2 = null;
                        
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;
                        }
                       

                    }
                    break;
                case 3:
                    if (scene3.SceneCompleted == true)
                    {
                        if (scene3.Restart == true)
                            wichLevel = 3;
                        else
                        {
                            Components.Remove(scene3);
                            Content.Unload();
                            scene3 = null;
                            
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;
                        }
                      

                    }
                    break;
                case 4:
                    if (scene4.SceneCompleted == true)
                    {
                        if (scene4.Restart == true)
                            wichLevel = 4;
                        else
                        {
                            Components.Remove(scene4);
                            Content.Unload();
                            scene4 = null;                           
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;
                        }

                    }
                    break;
                case 5:
                    if (scene5.SceneCompleted == true)
                    {
                        if (scene5.Restart == true)
                            wichLevel = 5;
                        else
                        {
                            Components.Remove(scene5);
                            Content.Unload();
                            scene5 = null;                           
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;
                        }

                    }
                    break;
                case 6:
                    if (scene6.SceneCompleted == true)
                    {
                        if (scene6.Restart == true)
                            wichLevel = 6;
                        else
                        {
                            Components.Remove(scene6);
                            Content.Unload();
                            scene6 = null;                          
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;

                        }

                    }
                    break;
                case 7:
                    if (scene7.SceneCompleted == true)
                    {
                        if (scene7.Restart == true)
                            wichLevel = 7;
                        else
                        {
                            Components.Remove(scene7);
                            Content.Unload();
                            scene7 = null;
                            Content.Unload();
                            lobby = new Lobby(this, Content, Components);
                            Components.Add(lobby);
                            activeScene = 0;
                            wichLevel = 0;

                        }

                    }
                    break;
                default:
                  
                    break;
            }
            
            
            if (wichLevel != 0)
            {
                switch (wichLevel)
                {
                    case 1:
                       
                        scene1 = new Scene1(this, Content, Components);
                        Components.Add(scene1);
                        Components.Remove(lobby);                     
                       lobby = null;                       
                        activeScene = 1;
                                         
                        break;
                    case 2:
                       
                        scene2 = new Scene2(this, Content, Components);
                        Components.Add(scene2);
                        Components.Remove(lobby);                    
                       lobby = null; 
                        activeScene = 2;
                        break;
                    case 3:
                        Content.Unload();
                        scene3 = new Scene3(this, Content, Components);
                        Components.Add(scene3);
                        Components.Remove(lobby);                     
                         lobby = null;
                        activeScene = 3;
                        wichLevel = -1;
                        break;
                    case 4:
                        Content.Unload();
                        scene4 = new Scene4(this, Content, Components);
                        Components.Add(scene4);
                        Components.Remove(lobby);                       
                        lobby = null;
                        activeScene = 4;
                        wichLevel = -1;
                        break;
                    case 5:
                      
                        scene5 = new Scene5(this, Content, Components);
                        Components.Add(scene5);
                        Components.Remove(lobby);                   
                        lobby = null; 
                        activeScene = 5;
                        wichLevel = -1;
                        break;
                    case 6:
                        Content.Unload();
                        scene6 = new Scene6(this, Content, Components);
                        Components.Add(scene6);
                        Components.Remove(lobby);                      
                       lobby = null; 
                        activeScene = 6;
                        wichLevel = -1;
                        break;
                    case 7 :
                        Content.Unload();
                        scene7 = new Scene7(this, Content, Components);
                        Components.Add(scene7);
                        Components.Remove(lobby);                     
                        lobby = null; 
                        activeScene = 7;
                        wichLevel = -1;
                        break;
                    default:
                      
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
            graphics.GraphicsDevice.Clear(Color.Lime);
            base.Draw(gameTime);
        }
    }
}
