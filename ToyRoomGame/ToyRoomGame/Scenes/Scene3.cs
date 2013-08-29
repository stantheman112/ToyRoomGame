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


namespace Toyroom.Scenes
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Scene3 : MasterScene
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




        public Scene3(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components )
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene3");
            if(gameStorage.fileExists(gameStorage.FileName)==false)
                 gameStorage.saveScore(0, 0);
           
            // TODO: Construct any child components here
        }

        private void accelSensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X;
            accelReading.Y = (float)e.Y;
            accelReading.Z = (float)e.Z;
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            initPauseScreen();
            bskHit = new Rectangle(35, 390, 180, 80); 
            accelSensor = new Accelerometer();
            accelSensor.Start();
            accelSensor.ReadingChanged += accelSensor_ReadingChanged;
        
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();

            mannRect = new Rectangle(545, 50, 185, 370);
            mannRect2 = new Rectangle(550, 50, 180, 370);
            mannRect3 = new Rectangle(515, 50, 215, 370);
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
         
            crayonRect = new Rectangle(0, 430, 200, 70);
            basketLeftRect = new Rectangle(0, 320, 250, 150);
            basketOpeningRct = new Rectangle(25, 305, 250, 150);
            roomRect = new Rectangle(-30, -20, 900, 600);
            guiBubbleRct = new Rectangle(4, 31, 590, 410);
            //default toy størrelse
            defaultRect = new Rectangle(320, 200, 100, 90);
            rightColor = new Color();
            wrongColor = new Color();

            rectangles.Add(new Rectangle(510, 180, 120, 90)); //sportscar
            rectangles.Add(new Rectangle(530, 150, 130, 120)); //plane
            rectangles.Add(new Rectangle(510, 170, 130, 90));//train
            rectangles.Add(new Rectangle(510, 190, 120, 90)); //tractor
            rectangles.Add(new Rectangle(510, 170, 120, 120)); //digger
            rectangles.Add(new Rectangle(510, 150, 130, 130)); //dino
            rectangles.Add(new Rectangle(505, 160, 120, 120)); //baby
            rectangles.Add(new Rectangle(505, 130, 120, 120)); //soldier
            rectangles.Add(new Rectangle(505, 180, 110, 140));//actionfigure
            rectangles.Add(new Rectangle(505, 180, 120, 120));//teddy
            rectangles.Add(new Rectangle(515, 195, 70, 70)); //block
         
            rectangles.Add(new Rectangle(505, 170, 100, 100));//ball
            rectangles.Add(new Rectangle(520, 130, 100, 150)); //balloon
            rectangles.Add(new Rectangle(500, 150, 130, 120)); //pad
            rectangles.Add(new Rectangle(505, 120, 120, 120)); //horse
            rectangles.Add(new Rectangle(505, 150, 120, 90)); //racecar
            rectangles.Add(new Rectangle(510, 190, 150, 120));//dumper
          

            numberOfTurns = rectangles.Count;
          
            rnd = new Random();

            base.Initialize();
        }

       
           
            
        
        protected override void LoadContent()
        {

            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");
            yeah = Content.Load<SoundEffect>("ugotit");
            ohno = Content.Load<SoundEffect>("ohno");
            loadPauseScreen();
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
            loadScreenDraw(loadScreen, new Vector2(-90, -50));
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            boyDissapointedTxt = Content.Load<Texture2D>("Images\\boytdown");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            crayonTxt = Content.Load<Texture2D>("Images\\bigcrayonside");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\talkingBubbleLeft");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");
            textures.Add(Content.Load<Texture2D>("Images\\sportscar"));
            textures.Add(Content.Load<Texture2D>("Images\\theplane"));
            textures.Add(Content.Load<Texture2D>("Images\\train"));
            textures.Add(Content.Load<Texture2D>("Images\\tractor"));
            textures.Add(Content.Load<Texture2D>("Images\\digger"));
            textures.Add(Content.Load<Texture2D>("Images\\dino"));
            textures.Add(Content.Load<Texture2D>("Images\\baby"));
            textures.Add(Content.Load<Texture2D>("Images\\soldier"));
            textures.Add(Content.Load<Texture2D>("Images\\actionFigure"));
            textures.Add(Content.Load<Texture2D>("Images\\teddy"));
            textures.Add(Content.Load<Texture2D>("Images\\block"));
            textures.Add(Content.Load<Texture2D>("Images\\ball"));
            textures.Add(Content.Load<Texture2D>("Images\\balloon"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\pad"));
            textures.Add(Content.Load<Texture2D>("Images\\horse"));
            textures.Add(Content.Load<Texture2D>("Images\\racecar"));
            
            textures.Add(Content.Load<Texture2D>("Images\\dumper"));

            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket1");



            yeahinst = yeah.CreateInstance();
            ohnoinst = ohno.CreateInstance();

            spriteFont = Content.Load<SpriteFont>("sf20");
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(70, 100), "Kasper! \n the toycolors have gone \n crazy again! " +
               "they will \n only go  into  the basket\n  if the basket has the \n same color as themselves! \nHelp me color the basket \n in  the right color!", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
            talkingBubble.GuiBackgroundRect = guiBubbleRct;
            talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
            talkingBubble.GuiVisible = true;
            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0.0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
           //basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect, 0.0f);
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.Orange, 0.0f);
            toy.CompRectX = -10000;
         //   gameGUI = new GameGUI(this.Game, spriteFont, new Vector2(10,50), "0/"+numberOfTurns.ToString(), Color.Black);
          //  gameGUI.MaxScore = rectangles.Count;
            Components.Add(roomBackground);
            basketOpening = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRct, Color.White, 0.0f);


            roomBackground.DrawOrder = -100;
            basketLeft.DrawOrder = 200;
            basketOpening.DrawOrder = 198;
            toy.DrawOrder = 199;
            boy.DrawOrder = 100;
            talkingBubble.DrawOrder = 10000;
            backgroundMusicInst = backgroundMusic.CreateInstance();
            yeahinst = yeah.CreateInstance();
            ohnoinst = ohno.CreateInstance();
           

            for (int i = 0; i < textures.Count; i++)
            {

                addFloorToy(i, i);
         

            }

          
        //    Components.Add(gameGUI);
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
            for (int i = 0; i < 4; i++)
            {
                BasicComponent crayonTemp = new BasicComponent(this.Game, crayonTxt, crayonRect, colorList[i], 0.0f);
                crayonTemp.ItemDraw = true;
                crayonRect.X += 200;
                crayons.Add(crayonTemp);
                Components.Add(crayonTemp);
            }
            Components.Add(talkingBubble);

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
                    arrangeCrayons();


                    timer++;

                    if (timer == trigger)
                    {
                        if (talkingBubble.GuiVisible == true)
                        {
                            talkingBubble.GuiVisible = false;
                            Components.Remove(talkingBubble);
                            backgroundMusicInst.Play();
                        }
                        if (backgroundMusicInst.State.ToString() == "Stopped")
                            backgroundMusicInst.Play();


                        if (itemPlaced == true || firstRun == true)
                        {
                            toy.ItemTouched = false;
                            toy.CompRectX = defaultRect.X;
                            toy.CompRectY = defaultRect.Y;
                            firstRun = false;
                            toy.ItemDraw = true;

                            int nextTexture = rnd.Next(0, floorToys.Count);


                            int nextColor = rnd.Next(0, 8);
                            int nextWrongColor = nextColor;
                            while (nextWrongColor == nextColor)
                                nextWrongColor = rnd.Next(0, 8);
                            int i = rnd.Next(1, 3);
                            GameTools.randomColor(ref rightColor, 255);
                            GameTools.randomColor(ref wrongColor, 255);
                            while (rightColor == wrongColor)
                                GameTools.randomColor(ref wrongColor, 255);

                            int chooseCrayon = rnd.Next(0, 4);


                            toy.ComponentColor = colorList[nextColor];
                            crayons[chooseCrayon].ComponentColor = colorList[nextColor];

                            toy.ComponentTexture = floorToys[nextTexture].ComponentTexture;
                            toy.ComponentRectangle = rectangles[floorToys[nextTexture].ComponentNumber];
                            boy.ComponentTexture = mannTexture2;
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

                    }
                    if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3 && boy.ComponentTexture != boyDissapointedTxt)
                    {
                        boy.ComponentTexture = mannTexture;
                        boy.ComponentRectangle = mannRect;
                        boy.ComponentTexture = mannTexture2;
                        boy.ComponentRectangle = mannRect2;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHit))
                    {
                        toysCased++;
                        if (toy.ComponentColor == basketLeft.ComponentColor)
                        {
                           
                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                            yeahinst.Play();
                            casedRight++;
                        }
                        else
                        {
                            ohnoinst.Play();
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;
                        }

                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;

                    }

                }
                else
                {
                    if (casedRight > toysCased - 3)
                    {
                        if (gameStorage.getProgression("lobby") < 3)
                            gameStorage.saveProgression(3);
                    }
                    this.UnloadContent();
                    sceneCompleted = true;
                }
                base.Update(gameTime);
            }

        }



    }
}
