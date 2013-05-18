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
using WindowsPhoneGame1.Components;
using System.Diagnostics;

namespace WindowsPhoneGame1.Scenes
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Lobby : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region private variables

        SpriteBatch spriteBatch;
        Texture2D lobbyText, playBtnText;
        List<Texture2D> lvlTxts, lvlTxtsG;
        Rectangle lobbyRect, playBtnRect;
        GameGUI gameGUI;     
        SpriteFont sf50, sf32;
        BasicComponent lobby, playButton, box;
        List<BasicComponent> levels;
        ContentManager Content;
        GameComponentCollection Components;
       
        List<Rectangle> rectangles = new List<Rectangle>();
        private int wichLevel = 0, sceneProgression=1; //lobby = 0
        private TouchCollection touchCollection;
        GameState gameState;
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
            gameState = new GameState(this.Game);
            sceneProgression = gameData.getProgression("lobby");
           
            // TODO: Construct any child components here
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
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            levels = new List<BasicComponent>();
           
           

            lobbyRect = new Rectangle(0,0,800, 480);           
            playBtnRect = new Rectangle(380, 180, 100, 100);
           
         
            base.Initialize();
        }
       

        protected override void LoadContent()
        {
            for (int i = 1; i < 5; i++)
            {
                 Texture2D tmp, tmpg;
              
                        tmp = Content.Load<Texture2D>("Images\\gui\\"+i.ToString()+"lvl");              
                        tmpg = Content.Load<Texture2D>("Images\\gui\\"+i.ToString()+"lvlg");
                lvlTxts.Add(tmp);
                lvlTxtsG.Add(tmpg);
            }
            lobbyText = Content.Load<Texture2D>("Images\\lobby");
            playBtnText = Content.Load<Texture2D>("Images\\playButton");
           
            sf50 = Content.Load<SpriteFont>("sf50");

            sf32 = Content.Load<SpriteFont>("sf32");
            gameGUI = new GameGUI(this.Game, sf50, new Vector2(250, 10), "Toyroom", Color.Black);
            int lvlHorPos = 260;
            for (int i = 0; i < 4; i++)
            {
                if (i == sceneProgression)
                {
                    levels.Add(new BasicComponent(this.Game, lvlTxtsG[i], new Rectangle(lvlHorPos, 112,40, 48), Color.White, 0.0f));
                   //levels[i].ComponentType = "hei";// (i + 1).ToString();
                   
                }
                else
                {

                    levels.Add(new BasicComponent(this.Game, lvlTxts[i], new Rectangle(lvlHorPos, 112, 40, 48), Color.White, 0.0f));
                   // levels[i].ComponentType = (i + 1).ToString();
                }
               
                lvlHorPos = lvlHorPos + 100;
            }

         //   gameGUI = new GameGUI(this.Game, spriteFont, new Vector2(0, 0), "Dette er en test", Color.White);
            lobby = new BasicComponent(this.Game, lobbyText, lobbyRect, Color.White, 0.0f);
            playButton = new BasicComponent(this.Game, playBtnText, playBtnRect, Color.White, 0.0f);

          
            Components.Add(lobby);
         // Components.Add(gameGUI);
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].ComponentType = Convert.ToString(i + 1); ;
                Components.Add(levels[i]);
            }
           
            Components.Add(playButton);

            
          

            base.LoadContent();
        }
        protected override void UnloadContent()
        {

            Components.Remove(lobby);
            Components.Remove(gameGUI);
            for (int i = 0; i < levels.Count; i++)
                Components.Remove(levels[i]);
            Components.Remove(playButton);
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

           

            touchCollection = TouchPanel.GetState();

            if (playButton.compPushed(touchCollection))
            {
                wichLevel = gameData.getProgression("lobby")+1;
                this.UnloadContent();
           ;
            }
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels[i].compPushed(touchCollection))
                {
                    wichLevel = i + 1;
                    this.UnloadContent();
                }
            }
           
          
          

            base.Update(gameTime);
        }





    }
}
