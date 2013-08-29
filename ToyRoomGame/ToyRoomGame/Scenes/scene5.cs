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
    public class Scene5 : MasterScene
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




        public Scene5(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene5");
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
            accelSensor = new Accelerometer();
            accelSensor.Start();
            accelSensor.ReadingChanged += accelSensor_ReadingChanged;


            // TODO: Add your initialization code here

            // TODO: Add your initialization code here
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(345, 10, 185, 370);
            mannRect2 = new Rectangle(353, 10, 175, 370);
            mannRect3 = new Rectangle(315, 10, 215, 370);
            
            guiBubbleRct = new Rectangle(10, 2, 400, 500);
         
            roomRect = new Rectangle(-170, -80, 1200, 620);

            //default toy st�rrelse
            defaultRect = new Rectangle(310, 200, 100, 90);

          

            rectangles.Add(new Rectangle(310, 200, 120, 90)); //sportscar
            rectangles.Add(new Rectangle(310, 170, 130, 120)); //plane
            rectangles.Add(new Rectangle(310, 190, 130, 90));//train
            rectangles.Add(new Rectangle(310, 210, 120, 90)); //tractor



            rectangles.Add(new Rectangle(310, 190, 120, 120)); //digger
            rectangles.Add(new Rectangle(310, 150, 120, 120)); //dino
            rectangles.Add(new Rectangle(310, 190, 120, 120)); //baby
            rectangles.Add(new Rectangle(310, 160, 120, 120)); //soldier
            rectangles.Add(new Rectangle(310, 210, 100, 90));//actionfigure
            rectangles.Add(new Rectangle(310, 213, 120, 120));//teddy
            rectangles.Add(new Rectangle(310, 225, 70, 70)); //block
            rectangles.Add(new Rectangle(310, 190, 70, 100));//ball
            rectangles.Add(new Rectangle(330, 140, 100, 150)); //balloon
            rectangles.Add(new Rectangle(330, 200, 130, 120)); //pad
            rectangles.Add(new Rectangle(310, 150, 120, 120)); //horse
            rectangles.Add(new Rectangle(310, 200, 120, 90)); //racecar

          



            rnd = new Random();

            numberOfTurns = rectangles.Count;




            basketLeftRect = new Rectangle(0, 340, 240, 140);
            basketRightRect = new Rectangle(550, 340, 240, 140);
            basketOpeningLRct = new Rectangle(15, 320, 245, 150);
            basketOpeningRRct = new Rectangle(565, 320, 245, 150);
            bskHitL = new Rectangle(35, 390, 180, 80);
            bskHitR = new Rectangle(585, 390, 180, 80); 
          

          

            base.Initialize();
        }
      
      


        protected override void LoadContent()
        {
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
            loadScreenDraw(loadScreen, new Vector2(-90, -70));
            loadPauseScreen();

            yeah = Content.Load<SoundEffect>("ugotit");
            ohno = Content.Load<SoundEffect>("ohno");
            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");

            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            boyDissapointedTxt = Content.Load<Texture2D>("Images\\boytdown");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
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
            textures.Add(Content.Load<Texture2D>("Images\\pad"));
            textures.Add(Content.Load<Texture2D>("Images\\horse"));
            textures.Add(Content.Load<Texture2D>("Images\\racecar"));
          
            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
          
            spriteFont = Content.Load<SpriteFont>("sf20");

            spriteFont = Content.Load<SpriteFont>("sf20");
            spriteFont.LineSpacing = 25;
            spriteFont.Spacing = 1;
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(50, 120), "Please help ! \n the color of \n  the toys have \n  gone crazy \n " +
                "and they will \n only be put \n  in a case \n the same color\n  as them selves!", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
            talkingBubble.GuiBackgroundRect = guiBubbleRct;
            talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
            talkingBubble.GuiVisible = true;

           //baskethitvisualL = new BasicComponent(this.Game, baskethitvisualTxt, bskHitL, Color.Black, 0f);
           // baskethitvisualR = new BasicComponent(this.Game, baskethitvisualTxt, bskHitR, Color.Black, 0f);

            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
            basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect, 0.0f);
            basketOpeningLeft = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningLRct, 0.0f);
            basketOpeningRight = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRRct, 0.0f);
            
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.Orange, 0.0f);
            gameGUI = new GameGUI(this.Game, spriteFont, new Vector2(10,10), "0/"+textures.Count, Color.Black);
            gameGUI.MaxScore = textures.Count;

            roomBackground.DrawOrder = -100;
            basketLeft.DrawOrder = 200;
            basketRight.DrawOrder = 201;
            basketOpeningLeft.DrawOrder = 198;
            basketOpeningRight.DrawOrder = 197;
            toy.DrawOrder = 199;
            boy.DrawOrder = 100;

            yeahinst = yeah.CreateInstance();
            ohnoinst = ohno.CreateInstance();
            backgroundMusicInst = backgroundMusic.CreateInstance();

            Components.Add(roomBackground);


            for (int i = 0; i < textures.Count; i++)
            {
                addFloorToy(i, i);
            }

            Components.Add(gameGUI);

            for (int o = 0; o < floorToysB.Count; o++)
            {
                Components.Add(floorToysB[o]);
            }

            Components.Add(boy);
            for (int b = 0; b < floorToysI.Count; b++)
            {

                Components.Add(floorToysI[b]);
            }
            Components.Add(basketOpeningRight);
            Components.Add(basketOpeningLeft);
            Components.Add(toy);
            Components.Add(basketLeft);
            Components.Add(basketRight);
          
            Components.Add(talkingBubble);
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            if (pauseGame() == false)
            {

                if (toysCased < numberOfTurns && GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed && restartButtonPushed == false && menuButtonPushed == false && allCleaned == false)
                {

                    basketOpeningLeft.ComponentColor = basketLeft.ComponentColor;
                    basketOpeningRight.ComponentColor = basketRight.ComponentColor;
                    if (toy.CompRectX < 360)
                        basketCollisionHandling(basketLeft, true);
                    else
                        basketCollisionHandling(basketRight, false);

                    timer++;
                    if (firstRun == false && timer % 200 == 0 && floorToys.Count > 0)
                    {
                        newRand = rnd.Next(0, textures.Count - 1);
                        Components.Add(addFloorToy(newRand, floorToys.Count));

                    }

                    if (timer == trigger)
                    {
                        if (talkingBubble.GuiVisible == true)
                        {
                            talkingBubble.GuiVisible = false;
                            Components.Remove(talkingBubble);
                            backgroundMusicInst.Play();
                        }
                        if (itemPlaced == true || firstRun == true)
                        {
                            toy.ItemTouched = false;
                            toy.CompRectX = defaultRect.X;
                            toy.CompRectY = defaultRect.Y;
                            firstRun = false;
                            toy.ItemDraw = true;

                            int nextTexture = rnd.Next(0, floorToys.Count - 1);


                            int nextColor = rnd.Next(0, 8);
                            int nextWrongColor = nextColor;
                            while (nextWrongColor == nextColor)
                                nextWrongColor = rnd.Next(0, 8);
                            int i = rnd.Next(1, 3);
                            GameTools.randomColor(ref rightColor, 255);
                            GameTools.randomColor(ref wrongColor, 255);
                            while (rightColor == wrongColor)
                                GameTools.randomColor(ref wrongColor, 255);

                            switch (i)
                            {
                                case 1:
                                    basketLeft.ComponentColor = wrongColor;  //colorList[nextWrongColor];
                                    basketRight.ComponentColor = rightColor;// GameTools.randomColor(); // colorList[nextColor];
                                    toy.ComponentColor = rightColor; //colorList[nextColor];


                                    break;
                                case 2:
                                    basketLeft.ComponentColor = rightColor;// colorList[nextColor];
                                    basketRight.ComponentColor = wrongColor; // colorList[nextWrongColor];
                                    toy.ComponentColor = rightColor;//colorList[nextColor];


                                    break;
                                default:

                                    break;
                            }


                            toy.ComponentTexture = floorToys[nextTexture].ComponentTexture;// textures[nextTexture];
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





                        }

                    }
                    if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3 && boy.ComponentTexture != boyDissapointedTxt)
                    {
                        boy.ComponentTexture = mannTexture;
                        boy.ComponentRectangle = mannRect;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHitL))
                    {
                        toysCased++;
                       
                        if (toy.ComponentColor == basketLeft.ComponentColor)
                        {
                            yeahinst.Play();
                            gameGUI.CurrentScore = gameGUI.CurrentScore + 1;
                            gameGUI.InfoText = gameGUI.CurrentScore.ToString() + " / " + gameGUI.MaxScore.ToString();

                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                        }
                        else
                        {
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            ohnoinst.Play();
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;
                          
                            Components.Add(addFloorToy(newRand, floorToys.Count));
                        }

                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;
                        if (floorToys.Count == 0)
                            allCleaned = true;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHitR))
                    {
                        toysCased++;
                        if (toy.ComponentColor == basketRight.ComponentColor)
                        {
                            yeahinst.Play();

                            gameGUI.CurrentScore = gameGUI.CurrentScore + 1;
                            gameGUI.InfoText = gameGUI.CurrentScore.ToString() + " / " + gameGUI.MaxScore.ToString();

                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                        }
                        else
                        {
                            ohnoinst.Play();
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;
                         
                            Components.Add(addFloorToy(newRand, floorToys.Count));
                        }
                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;
                        if (floorToys.Count == 0)
                            allCleaned = true;

                    }
                }
                else
                {
                    if (gameStorage.getProgression("lobby") < 5)
                        gameStorage.saveProgression(5);
                    this.UnloadContent();
                    sceneCompleted = true;
                }


                base.Update(gameTime);
            }
        }




    }
}
