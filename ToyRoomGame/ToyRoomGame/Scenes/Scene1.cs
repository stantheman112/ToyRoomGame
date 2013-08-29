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
using Toyroom.GameStorage;
using System.Diagnostics;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using Microsoft.Phone;


namespace Toyroom.Scenes
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Scene1 : MasterScene
    {
        #region private variables

        #endregion 
        #region public properties
        public bool SceneCompleted
        {
            get
            {
                return sceneCompleted;
            }
        }
        #endregion




        public Scene1(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene1");
            if(gameStorage.fileExists(gameStorage.FileName)==false)
                 gameStorage.saveScore(0, 0);
         
            // TODO: Construct any child components here
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            PhoneApplicationService.Current.Deactivated += GameDeactivated; 
            //default toy pos and size settings
            initPauseScreen();
            accelSensor = new Accelerometer();
            accelSensor.Start();
            accelSensor.ReadingChanged += accelSensor_ReadingChanged;
          
            // TODO: Add your initialization code here
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(555, 80, 185, 320);
            mannRect2 = new Rectangle(560, 80, 180, 320);
            mannRect3 = new Rectangle(525, 80, 215, 320);
            guiBubbleRct = new Rectangle(210,-1, 400, 250);
           
            basketLeftRect = new Rectangle(0, 320, 250, 150);
            basketOpeningRct = new Rectangle(25, 305, 250, 150);
            bskHit = new Rectangle(35, 390, 180, 80); 
        
            roomRect = new Rectangle(-170, -20, 1140, 620);
         
            
            //default toy størrelse
            defaultRect = new Rectangle(510, 200, 100, 90);

            rectangles.Add(new Rectangle(510, 190, 120, 90)); //sportscar
            rectangles.Add(new Rectangle(530, 205, 130, 120)); //plane
            rectangles.Add(new Rectangle(510, 210, 130, 90));//train

            rectangles.Add(new Rectangle(510, 210, 120, 90)); //tractor
            rectangles.Add(new Rectangle(515, 180, 120, 120)); //digger
            rectangles.Add(new Rectangle(510, 190, 130, 130)); //dino

            rectangles.Add(new Rectangle(505, 180, 120, 120)); //baby
            rectangles.Add(new Rectangle(505, 160, 120, 120)); //soldier
            rectangles.Add(new Rectangle(505, 180, 100, 90));//actionfigure

            rectangles.Add(new Rectangle(505, 195, 120, 120));//teddy
            rectangles.Add(new Rectangle(520, 230, 60, 60)); //block
            rectangles.Add(new Rectangle(515, 210, 80, 80));//ball

            rectangles.Add(new Rectangle(520, 155, 105, 150)); //balloon
            rectangles.Add(new Rectangle(510, 200, 130, 130));//horse
            rectangles.Add(new Rectangle(510, 195, 130, 130));//pad
            rectangles.Add(new Rectangle(510, 190, 130, 100));//racecar
            rectangles.Add(new Rectangle(510, 190, 150, 120));//dumper


           
            rnd = new Random();

            numberOfTurns = rectangles.Count - 1;
            base.Initialize();
        }

       

        protected override void LoadContent()
        {
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
            loadScreenDraw(loadScreen, new Vector2(-90, -70));

            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");
            yeah = Content.Load<SoundEffect>("ugotit");
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\tlkBubble");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");

            textures.Add(Content.Load<Texture2D>("Images\\colored\\sportscar"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\theplane"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\train"));

            textures.Add(Content.Load<Texture2D>("Images\\colored\\tractor"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\digger"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\dino"));

            textures.Add(Content.Load<Texture2D>("Images\\colored\\baby"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\soldier"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\actionFigure"));

            textures.Add(Content.Load<Texture2D>("Images\\colored\\teddy"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\block"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\ball"));

            textures.Add(Content.Load<Texture2D>("Images\\colored\\balloon")); 
            textures.Add(Content.Load<Texture2D>("Images\\colored\\horse"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\pad"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\racecar"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\dumper"));


            basketLeftTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
            yeahinst = yeah.CreateInstance();
            spriteFont = Content.Load<SpriteFont>("sf20");
            spriteFont.LineSpacing = 25;
            spriteFont.Spacing = 1;
        //    baskethitvisual = new BasicComponent(this.Game, baskethitvisualTxt, bskHit, Color.Black, 0f);
            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0f);
            basketLeft = new BasicComponent(this.Game, basketLeftTxt, basketLeftRect, Color.LightBlue, 0.0f);
            basketOpening = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRct, Color.LightBlue, 0.0f);
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            roomBackground.DrawOrder = -100;
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.White, 0.0f);
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(220, 120), "Hello friend \n will you help  me \n clean my room?", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
          talkingBubble.GuiBackgroundRect = guiBubbleRct;
          talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
          talkingBubble.GuiVisible = true;
          basketLeft.DrawOrder = 200;
          basketOpening.DrawOrder = 198;
          toy.DrawOrder = 199;
          boy.DrawOrder = 100;
           // gameGUI.MaxScore = 20;
          toy.CompRectX = -10000;

            Components.Add(roomBackground);
           // Components.Add(gameGUI);
            for (int i = 0; i < textures.Count; i++)
            {
                addFloorToy(i, i, Color.White);
             

            }

            for (int o = 0; o < floorToysB.Count; o++)
            {
                Components.Add(floorToysB[o]);
            }
         
           Components.Add(boy);
           for (int b = 0; b < floorToysI.Count; b++)
           {

               Components.Add(floorToysI[b]);
           }

           Components.Add(basketOpening);
           
            Components.Add(toy);
            Components.Add(basketLeft);
            loadPauseScreen();     
            Components.Add(talkingBubble);
        
            numberOfTurns = textures.Count;
            base.LoadContent();
        }
      


    
        public override void Update(GameTime gameTime)
        {
            if (pauseGame() == false)
            {

                if (toysCased < numberOfTurns && GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed && restartButtonPushed == false && menuButtonPushed == false)
                {

                    basketRoll();
                    basketCollisionHandling(basketLeft);



                    timer++;

                    if (timer == trigger)
                    {
                        if (talkingBubble.GuiVisible == true)
                            backgroundMusic.Play();
                        talkingBubble.GuiVisible = false;
                        Components.Remove(talkingBubble);
                        if (itemPlaced == true || firstRun == true)
                        {
                            toy.ItemTouched = false;
                            toy.CompRectX = defaultRect.X;
                            toy.CompRectY = defaultRect.Y;
                            firstRun = false;
                            toy.ItemDraw = true;


                            nextTexture = rnd.Next(0, floorToys.Count - 1);


                        }

                        toy.ComponentTexture = floorToys[nextTexture].ComponentTexture;
                        toy.ComponentRectangle = rectangles[floorToys[nextTexture].ComponentNumber];

                        try
                        {
                            bool removed = false;

                            for (int p = 0; p < floorToysI.Count; p++)
                            {
                                if (floorToysI[p].ComponentType == floorToys[nextTexture].ComponentType)
                                {
                                    Components.Remove(floorToys[nextTexture]);
                                    floorToysI.Remove(floorToysI[p]);
                                    floorToys.Remove(floorToys[nextTexture]);

                                    //    textures.Remove(textures[nextTexture]);
                                    //     rectangles.Remove(rectangles[nextTexture]);
                                    removed = true;

                                }
                            }
                            if (removed == false)
                            {
                                for (int n = 0; n < floorToysB.Count; n++)
                                {
                                    if (floorToysB[n].ComponentType == floorToys[nextTexture].ComponentType)
                                    {

                                        Components.Remove(floorToys[nextTexture]);
                                        floorToysB.Remove(floorToysB[n]);
                                        floorToys.Remove(floorToys[nextTexture]);

                                        //   textures.Remove(textures[nextTexture]);
                                        //   rectangles.Remove(rectangles[nextTexture]);


                                    }

                                }
                            }
                        }


                        catch (Exception ex)
                        {


                        }


                        boy.ComponentTexture = mannTexture2;
                        boy.ComponentRectangle = mannRect2;




                    }
                    if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3)
                    {
                        boy.ComponentTexture = mannTexture;
                        boy.ComponentRectangle = mannRect;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHit))
                    {
                        toysCased++;
                        yeahinst.Play();
                        boy.ComponentTexture = mannTexture3;
                        boy.ComponentRectangle = mannRect3;
                        if (nextTexture < 6)
                        {
                            // gameGUI.CurrentScore = gameGUI.CurrentScore+1;
                            //   gameGUI.InfoText = gameGUI.CurrentScore.ToString() + "/" + maxScore.ToString();
                        }
                        else
                        {
                            //spill av animasjon på at gutten blir oppgitt? 
                        }

                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;

                    }
                }
                else
                {
                    if (toysCased == numberOfTurns)
                    {
                        if (gameStorage.getProgression("lobby") < 1)
                            gameStorage.saveProgression(1);
                    }
                    this.UnloadContent();
                    sceneCompleted = true;


                }


                base.Update(gameTime);
            }
        }

      



    }
}
