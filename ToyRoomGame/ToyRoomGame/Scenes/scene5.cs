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
            colorList = GameTools.elementColors(colorList);
            gameStorage = new GameData("lobby");
           
          
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

          
            spriteBatch = new SpriteBatch(gameMS.GraphicsDevice);
            mannRect = new Rectangle(345, 10, 185, 370);
            mannRect2 = new Rectangle(353, 10, 175, 370);
            mannRect3 = new Rectangle(315, 10, 215, 370);
            indicatorRct = new Rectangle(270, 850, 200, 200);
            guiBubbleRct = new Rectangle(120, 45, 300, 200);
         
            roomRect = new Rectangle(-170, -80, 1200, 620);

            //default toy størrelse
            defaultRect = new Rectangle(310, 200, 100, 90);


            rectangles[0] = new Rectangle(310, 170, 120, 90); //sportscar
            rectangles[1] = new Rectangle(305, 170, 130, 120); //plane
            rectangles[2] = new Rectangle(310, 165, 130, 90);//train
            rectangles[3] = new Rectangle(297, 192, 120, 90); //tractor
            rectangles[4] = new Rectangle(280, 150, 120, 120); //digger
            rectangles[5] = new Rectangle(310, 140, 120, 120); //dino
            rectangles[6] = new Rectangle(295, 160, 120, 120); //baby
            rectangles[7] = new Rectangle(296, 147, 120, 120); //soldier
            rectangles[8] = new Rectangle(295, 180, 100, 90);//actionfigure
            rectangles[9] = new Rectangle(297, 160, 120, 120);//teddy
            rectangles[10] = new Rectangle(310, 195, 70, 70); //block
            rectangles[11] = new Rectangle(310, 165, 100, 100);//ball
            rectangles[12] = new Rectangle(320, 110, 100, 150); //balloon
            rectangles[13] = new Rectangle(305, 155, 110, 100); //pad
            rectangles[14] = new Rectangle(300, 145, 120, 120); //horse
            rectangles[15] = new Rectangle(305, 190, 120, 90); //racecar
            rectangles[16] = new Rectangle(280, 140, 150, 120);//dumper
          



            rnd = new Random();

            numberOfTurns = rectangles.Length;




            basketLeftRect = new Rectangle(0, 340, 240, 140);
            basketRightRect = new Rectangle(550, 340, 240, 140);
            basketOpeningLRct = new Rectangle(15, 320, 245, 150);
            basketOpeningRRct = new Rectangle(565, 320, 245, 150);
            bskHitL = new Rectangle(35, 410, 180, 80);
            bskHitR = new Rectangle(585, 410, 180, 80); 
          

          

            base.Initialize();
        }
      
      


        protected override void LoadContent()
        {
           
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
            loadScreenDraw(loadScreen, new Vector2(-90, -20));
            loadPauseScreen();

            yeah = Content.Load<SoundEffect>("yeah2");
            ohno = Content.Load<SoundEffect>("ohno");
            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");
            indicatorTxt = Content.Load<Texture2D>("Images\\indicator");
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            boyDissapointedTxt = Content.Load<Texture2D>("Images\\boytdown");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\talkingBubbleLeft");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");

            textures[0] = Content.Load<Texture2D>("Images\\sportscar");
            textures[1] = Content.Load<Texture2D>("Images\\theplane");
            textures[2] = Content.Load<Texture2D>("Images\\train");
            textures[3] = Content.Load<Texture2D>("Images\\tractor");
            textures[4] = Content.Load<Texture2D>("Images\\digger");
            textures[5] = Content.Load<Texture2D>("Images\\dino");
            textures[6] = Content.Load<Texture2D>("Images\\baby");
            textures[7] = Content.Load<Texture2D>("Images\\soldier");
            textures[8] = Content.Load<Texture2D>("Images\\actionFigure");
            textures[9] = Content.Load<Texture2D>("Images\\teddy");
            textures[10] = Content.Load<Texture2D>("Images\\block");
            textures[11] = Content.Load<Texture2D>("Images\\ball");
            textures[12] = Content.Load<Texture2D>("Images\\balloon");
            textures[13] = Content.Load<Texture2D>("Images\\colored\\pad");
            textures[14] = Content.Load<Texture2D>("Images\\horse");
            textures[15] = Content.Load<Texture2D>("Images\\racecar");
            textures[16] = Content.Load<Texture2D>("Images\\dumper");

            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
          
            spriteFont = Content.Load<SpriteFont>("sf20");

            spriteFont = Content.Load<SpriteFont>("sf20");
            spriteFont.LineSpacing = 25;
            spriteFont.Spacing = 1;
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(150, 80), "Please help ! \n Be quick! \n Or more toys \n will come", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
            talkingBubble.GuiBackgroundRect = guiBubbleRct;
            talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
            talkingBubble.GuiVisible = true;

            talkingBubble.DrawOrder = 10000;
          

            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
            basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect, 0.0f);
            basketOpeningLeft = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningLRct, 0.0f);
            basketOpeningRight = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRRct, 0.0f);
            
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.Orange, 0.0f);
         

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
            indicator = new BasicComponent(this.Game, indicatorTxt, indicatorRct, Color.White, 0.0f);
            indicator.DrawOrder = 12122;
            indicator.ItemDraw = false;
            Components.Add(indicator);
            Components.Add(roomBackground);


            //for (int i = 0; i < textures.Length; i++)
            //{
            //    addFloorToy(i, i);
            //}

            addToys(420, 450, 0, 700, 300, 330, 0, 700, true, true, 222, 150, true);

            Components.Add(boy);
          
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

                if ((toysBackground.graphicElements + toysForeground.graphicElements) >= 0 && GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed && restartButtonPushed == false && menuButtonPushed == false && allCleaned == false)
                {

                    basketOpeningLeft.ComponentColor = basketLeft.ComponentColor;
                    basketOpeningRight.ComponentColor = basketRight.ComponentColor;
                    if (toy.CompRectX < 360)
                        basketCollisionHandling(basketLeft, true);
                    else
                        basketCollisionHandling(basketRight, false);

                    timer++;

                    if ((toysForeground.graphicElements + toysBackground.graphicElements) > 0)
                    {

                    if (firstRun == false && timer % 200 == 0)
                    {
                        
                        newRand = rnd.Next(0, textures.Length - 1);
                        foreOrBack = rnd.Next(0, 2);
                        if (foreOrBack == 0)
                        {
                            toysBackground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 330, 370, 0, 700);
                            toysCollected.Add(newRand);
                        }
                        else
                        {
                            toysForeground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 410, 450, 0, 700);
                            toysCollected.Add(newRand);
                        }
                            

                    }

                    if (timer == trigger)
                    {
                        if (talkingBubble.GuiVisible == true)
                        {
                            talkingBubble.GuiVisible = false;
                            Components.Remove(talkingBubble);
                            backgroundMusicInst.Play();
                        }
                        if (backgroundMusicInst.State.ToString() == musicStateStopped)
                            backgroundMusicInst.Play();
                        if (itemPlaced == true || firstRun == true)
                        {
                            toy.ItemTouched = false;
                            toy.CompRectX = defaultRect.X;
                            toy.CompRectY = defaultRect.Y;
                            firstRun = false;
                            toy.ItemDraw = true;
                            nextTexture = rnd.Next(0, textures.Length - 1);
                            nextColor = rnd.Next(0, 8);
                            nextWrongColor = nextColor;
                            while (nextWrongColor == nextColor)
                                nextWrongColor = rnd.Next(0, 8);
                            chooseBasket = rnd.Next(1, 3);

                            nextTexture = rnd.Next(0, toysCollected.Count - 1);
                            toyName = toyNamePrefix + toysCollected[nextTexture].ToString();
                            if (toysBackground.removeSprite(toyName) == false)
                            {
                                toysForeground.removeSprite(toyName);
                                rightColor = toysForeground.PickedToyColorNumber;
                            }
                            else
                            {
                                rightColor = toysBackground.PickedToyColorNumber;
                            }
                             GameTools.randomColor(ref wrongColor, 255, rnd);
                            while (rightColor == wrongColor)
                                 GameTools.randomColor(ref wrongColor, 255, rnd);

                            switch (chooseBasket)
                            {
                                case 1:
                                    basketLeft.ComponentColor = wrongColor;  
                                    basketRight.ComponentColor = rightColor;
                                    toy.ComponentColor = rightColor; 


                                    break;
                                case 2:
                                    basketLeft.ComponentColor = rightColor;
                                    basketRight.ComponentColor = wrongColor; 
                                    toy.ComponentColor = rightColor;


                                    break;
                                default:

                                    break;
                            }



                      

                            toy.ComponentTexture = textures[toysCollected[nextTexture]];
                            toy.ComponentRectangle = rectangles[toysCollected[nextTexture]];
                            toysCollected.RemoveAt(nextTexture);

                           

                            boy.ComponentTexture = mannTexture2;
                            boy.ComponentRectangle = mannRect2;
                        }                   

                       
                    }

                    }
                    if (timerNow == 0)
                    {
                        timerNow = timer;
                    }
                    if (timer - timerNow == 200 && toy.ItemTaken == false)
                    {
                        indicator.ItemDraw = true;
                        timerNow = 0;
                    }
                    if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3 && boy.ComponentTexture != boyDissapointedTxt)
                    {
                        if (indicator.ItemDraw == true)
                            indicator.ItemDraw = false;
                        boy.ComponentTexture = mannTexture;
                        boy.ComponentRectangle = mannRect;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHitL))
                    {
                        toysCased++;
                       
                        if (toy.ComponentColor == basketLeft.ComponentColor)
                        {
                            yeahinst.Play();
                           

                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                        }
                        else
                        {
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            ohnoinst.Play();
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;

                          newRand = rnd.Next(0, textures.Length - 1);
                            
                            //Components.Add(addFloorToy(newRand, floorToys.Count));
                        }

                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;
                        if ((toysBackground.graphicElements + toysForeground.graphicElements) == 0)
                            allCleaned = true;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHitR))
                    {
                        toysCased++;
                        if (toy.ComponentColor == basketRight.ComponentColor)
                        {
                            yeahinst.Play();

                          
                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                        }
                        else
                        {
                            ohnoinst.Play();
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;
                            newRand = rnd.Next(0, textures.Length - 1);
                            int foreOrBack = rnd.Next(0, 2);
                            if (foreOrBack == 0)
                            {
                                toysBackground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 330, 370, 0, 700);
                                toysCollected.Add(newRand);
                            }
                            else
                            {
                                toysForeground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 410, 450, 0, 700);
                                toysCollected.Add(newRand);
                            }
                            
                        }
                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;
                        if ((toysBackground.graphicElements + toysForeground.graphicElements) == 0)
                            allCleaned = true;

                    }
                }
                else
                {
                    if ((toysBackground.graphicElements + toysForeground.graphicElements) == 0)
                    {
                        if (gameStorage.getProgression("lobby") < 5)
                            gameStorage.saveProgression(5);
                    }
                    this.UnloadContent();
                    sceneCompleted = true;
                }


                base.Update(gameTime);
            }
        }




    }
}
