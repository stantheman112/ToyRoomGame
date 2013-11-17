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
using Toyroom.Components;
using Microsoft.Devices;


namespace Toyroom.Scenes
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Lobby : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region private variables

        SpriteBatch spriteBatch;
        Texture2D lobbyText, playBtnText, playBtnChngText;
        List<Texture2D> lvlTxts, lvlTxtsG, lvlTxtsCurr;
        Rectangle lobbyRect, playBtnRect, playBtnChngRct;
        
        SpriteFont sf50, sf32;
        BasicComponent lobby, playButton;
        List<BasicComponent> levels;
        public ContentManager Content { get; set;}
        GameComponentCollection Components;
        private VibrateController vibration; 
        List<Rectangle> rectangles = new List<Rectangle>();
        private int wichLevel = 0, sceneProgression=0, timer = 0; //lobby = 0
        private TouchCollection touchCollection;
       
        GameStorage.GameData gameData = new GameStorage.GameData("lobby");
      
        #endregion 
        #region public properties
        public int WichLevel {
            get
            {
                return wichLevel;
            }
            set
            {
                wichLevel = value;
            }
        }
       
        #endregion


       

        public Lobby(Game game, ContentManager content, GameComponentCollection components)
            : base(game)
        {
           
            Components = components;
            
            Content = content;
         
           
            sceneProgression = gameData.getProgression("lobby");
            vibration = VibrateController.Default;           
          
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //default toy pos and size settings


            // TODO: Add your initialization code here
           lvlTxts = new List<Texture2D>();
           lvlTxtsG = new List<Texture2D>();
           lvlTxtsCurr = new List<Texture2D>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            levels = new List<BasicComponent>();
            lobbyRect = new Rectangle(0,0,800, 480);
            playBtnRect = new Rectangle(350, 200, 150, 150);
            playBtnChngRct = new Rectangle(350, 200, 150, 150); 
            base.Initialize();
        }
       

        protected override void LoadContent()
        {
            
            for (int i = 1; i < 7; i++)
            {
                Texture2D tmp, tmpg, tmpcurr;
                tmp = Content.Load<Texture2D>("Images\\gui\\lvl" + i.ToString() + "clear");
                tmpg = Content.Load<Texture2D>("Images\\gui\\lvl" + i.ToString() + "nclear");
                tmpcurr = Content.Load<Texture2D>("Images\\gui\\lvl" + i.ToString() + "curr");
                lvlTxts.Add(tmp);
                lvlTxtsG.Add(tmpg);
                lvlTxtsCurr.Add(tmpcurr);
            }
            lobbyText = Content.Load<Texture2D>("Images\\lobby");
            playBtnText = Content.Load<Texture2D>("Images\\playButton");
            playBtnChngText = Content.Load<Texture2D>("Images\\gui\\playButtonChanged");
           
            sf50 = Content.Load<SpriteFont>("sf50");

            sf32 = Content.Load<SpriteFont>("sf32");
         
            int lvlHorPos = 200;
            for (int i = 0; i < 6; i++)
            {
                if (i < sceneProgression)
                {
                    BasicComponent tmplvl = new BasicComponent(this.Game, lvlTxts[i], new Rectangle(lvlHorPos, 112, 80, 88), Color.White, 0.0f);
                    levels.Add(tmplvl);
                    tmplvl.Dispose();
                   
                }
                else if(i > sceneProgression  && (sceneProgression!=0 || i>0))
                {
                    BasicComponent tmplvl = new BasicComponent(this.Game, lvlTxtsG[i], new Rectangle(lvlHorPos, 112, 80, 88), Color.White, 0.0f);
                    levels.Add(tmplvl);
                    tmplvl.Dispose();
                   
                }
                else if (i == sceneProgression || (sceneProgression == 0 && i == 0))
                {
                    BasicComponent tmplvl = new BasicComponent(this.Game, lvlTxtsCurr[i], new Rectangle(lvlHorPos, 112, 80, 88), Color.White, 0.0f);
                    levels.Add(tmplvl);
                    tmplvl.Dispose();
                }
               
                lvlHorPos = lvlHorPos + 85;
            }


            playButton = new BasicComponent(this.Game, playBtnText, playBtnRect, Color.White, 0.0f);
            lobby = new BasicComponent(this.Game, lobbyText, lobbyRect, 0f);
          
            Components.Add(lobby);
      
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].ComponentType = Convert.ToString(i + 1); ;
                Components.Add(levels[i]);
            }
           
            Components.Add(playButton);

            if (gameData.getProgression("lobby") == 6)
            {
                playButton.ComponentTexture = playBtnChngText;
                playButton.ComponentRectangle = playBtnChngRct;
            }

            base.LoadContent();
        }
        protected override void UnloadContent()
        {

          
            for (int l = 0; l < levels.Count; l++)
               levels[l] = null;
            levels.Clear();
            playButton = null;
            lobby = null;

            Content.Unload();          
            Components.Clear();
            gameData = null;
            GC.Collect();
           
            GC.WaitForPendingFinalizers();

            string test = GC.GetTotalMemory(true).ToString();
        
            base.UnloadContent();
           
        }

        public override void Update(GameTime gameTime)
        {

           

            touchCollection = TouchPanel.GetState();
           
            if (touchCollection.Count > 0)
            {
                if (playButton.compPushed(touchCollection))
                {


                    wichLevel = sceneProgression + 1;
                    this.UnloadContent();



                }
            }  

            if (touchCollection.Count > 0)
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    if (levels[i].compPushed(touchCollection))
                    {
                        if (i <= gameData.getProgression("lobby"))
                        {
                            wichLevel = i + 1;
                            this.UnloadContent();
                        }
                        else
                        {
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                        }
                    }
                }

            }

            base.Update(gameTime);
        }

    }
}