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
    public class Scene7 : MasterScene
    {
        public ContentManager content7;




        public Scene7(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors(colorList);
            gameStorage = new GameData("lobby");
            Content.Unload();
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            accelSensor = new Accelerometer();
            accelSensor.Start();
            accelSensor.ReadingChanged += accelSensor_ReadingChanged;
            
            //floorToysB = new List<BasicComponent>();
            //floorToysI = new List<BasicComponent>();
            //floorToys = new List<BasicComponent>();
            indicatorRct = new Rectangle(470, 145, 200, 200);
            spriteBatch = new SpriteBatch(gameMS.GraphicsDevice);
         
            mamaRct = new Rectangle(345, 10, 245, 470);
            mannRect = new Rectangle(545, 40, 185, 370);
            mannRect2 = new Rectangle(550, 40, 180, 370);
            mannRect3 = new Rectangle(515, 40, 215, 370);
            mannRect4 = new Rectangle(555, 100, 180, 320);
            crayonRect = new Rectangle(0, 430, 200, 70);
            basketLeftRect = new Rectangle(0, 320, 250, 150);
            basketOpeningRct = new Rectangle(25, 305, 250, 150);
            bskHit = new Rectangle(35, 400, 180, 80);
            roomRect = new Rectangle(-30, -20, 900, 600);
            guiBubbleRct = new Rectangle(170, 55, 440, 270);


            //default toy størrelse
            defaultRect = new Rectangle(510, 200, 100, 90);

            rightColor = new Color();
            wrongColor = new Color();

            rectangles[0] = new Rectangle(510, 190, 120, 90); //sportscar
            rectangles[1] = new Rectangle(530, 160, 130, 120); //plane
            rectangles[2] = new Rectangle(510, 180, 130, 90);//train
            rectangles[3] = new Rectangle(510, 200, 120, 90); //tractor
            rectangles[4] = new Rectangle(510, 180, 120, 120); //digger
            rectangles[5] = new Rectangle(510, 160, 130, 130); //dino
            rectangles[6] = new Rectangle(505, 170, 120, 120); //baby
            rectangles[7] = new Rectangle(505, 165, 120, 120); //soldier
            rectangles[8] = new Rectangle(505, 170, 120, 120);//actionfigure
            rectangles[9] = new Rectangle(505, 190, 120, 120);//teddy
            rectangles[10] = new Rectangle(515, 205, 70, 70); //block
            rectangles[11] = new Rectangle(505, 170, 100, 100);//ball
            rectangles[12] = new Rectangle(520, 140, 100, 150); //balloon
            rectangles[13] = new Rectangle(515, 160, 130, 120); //pad
            rectangles[14] = new Rectangle(510, 160, 120, 120); //horse
            rectangles[15] = new Rectangle(510, 190, 120, 90); //racecar
            rectangles[16] = new Rectangle(510, 190, 150, 120);//dumper

         

            initPauseScreen();

            numberOfTurns = rectangles.Length;

            rnd = new Random();

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
            myBoy = Content.Load<SoundEffect>("goodboy");
            myBoyInst = myBoy.CreateInstance();

            mamaTxt = Content.Load<Texture2D>("Images\\mama");
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            boyDissapointedTxt = Content.Load<Texture2D>("Images\\boytdown");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            crayonTxt = Content.Load<Texture2D>("Images\\bigcrayonside");
            indicatorTxt = Content.Load<Texture2D>("Images\\indicator");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\talkingBubbleLeft");
            roomdooropen = Content.Load<Texture2D>("Images\\roomdooropen");

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

            spriteFont = Content.Load<SpriteFont>("sf20");


            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");

            yeahinst = yeah.CreateInstance();
            ohnoinst = ohno.CreateInstance();
            backgroundMusicInst = backgroundMusic.CreateInstance();

         
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(185, 130), "Friend! Hurry help!! \n You must be  \n very quick now!!", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
            talkingBubble.GuiBackgroundRect = guiBubbleRct;
            talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
            talkingBubble.GuiVisible = true;

            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0.0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
            basketOpening = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRct, Color.White, 0.0f);

            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], rectangles[0], Color.Orange, 0.0f);
            mama = new BasicComponent(this.Game, mamaTxt, mamaRct, 0.0f);
            mama.ItemDraw = false;
            indicator = new BasicComponent(this.Game, indicatorTxt, indicatorRct, Color.White, 0.0f);
            indicator.DrawOrder = 12122;
            indicator.ItemDraw = false;
            Components.Add(indicator);
            roomBackground.DrawOrder = -100;
            basketLeft.DrawOrder = 213;
            basketOpening.DrawOrder = 198;
            toy.DrawOrder = 199;
            boy.DrawOrder = 100;
            talkingBubble.DrawOrder = 1112;
            mama.DrawOrder = 20000;
            Components.Add(mama);
            Components.Add(roomBackground);
            toy.CompRectX = 10000;
            addToys(450, 460, 0, 700, 330, 390, 0, 700, true, true, 150, 150, false);
            Components.Add(gameGUI);
            Components.Add(boy);
            Components.Add(basketOpening);
            Components.Add(toy);
            Components.Add(basketLeft);
            crayons = new BasicComponent[4];
            for (int i = 0; i <crayons.Length; i++)
            {
                BasicComponent crayonTemp = new BasicComponent(this.Game, crayonTxt, crayonRect, colorList[i], 0.0f);
                crayonTemp.ItemDraw = true;
                crayonRect.X += 200;
                crayonTemp.DrawOrder = 212;
                crayons[i] = crayonTemp;
                Components.Add(crayonTemp);
            }
            Components.Add(talkingBubble);

            for (int i = 0; i < 20; i++)
            {
                newRand = rnd.Next(0, textures.Length - 1);
                toysBackground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 330, 370, 0, 700);
                toysCollected.Add(newRand);
            }

            base.LoadContent();
        }




        public override void Update(GameTime gameTime)
        {

            if (pauseGame() == false)
            {

                if (toysBackground.graphicElements >= 0 && GamePad.GetState(PlayerIndex.One).Buttons.Back != ButtonState.Pressed && restartButtonPushed == false && menuButtonPushed == false && allCleaned == false)
                {
                    basketRoll();
                    basketCollisionHandling(basketLeft);

                    arrangeCrayons();
                    if (toysBackground.graphicElements > 0)
                    {


                        timer++;
                        if (firstRun == false && timer % 150 == 0)
                        {

                            newRand = rnd.Next(0, textures.Length - 1);
                            toysBackground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 330, 370, 0, 700);
                            toysCollected.Add(newRand);

                        }
                        GameTools.randomColor(ref rightColor, 255, rnd);

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
                                nextTexture = rnd.Next(0, toysCollected.Count - 1);


                                nextColor = rnd.Next(0, 8);
                                nextWrongColor = nextColor;
                                while (nextWrongColor == nextColor)
                                    nextWrongColor = rnd.Next(0, 8);

                                GameTools.randomColor(ref rightColor, 255, rnd);
                                GameTools.randomColor(ref wrongColor, 255, rnd);
                                while (rightColor == wrongColor)
                                    GameTools.randomColor(ref wrongColor, 255, rnd);

                                chooseCrayon = rnd.Next(0, crayons.Length - 1);
                                nextTexture = rnd.Next(0, toysCollected.Count - 1);
                                toyName = "toy_" + toysCollected[nextTexture].ToString();

                                toysBackground.removeSprite(toyName);
                                rightColor = toysBackground.PickedToyColorNumber;
                                toysBackground.removeSprite(toyName);
                                crayons[chooseCrayon].ComponentColor = rightColor;
                                toy.ComponentColor = rightColor;
                              
                              
                               


                                toy.ComponentTexture = textures[toysCollected[nextTexture]];
                                toy.ComponentRectangle = rectangles[toysCollected[nextTexture]];
                                toysCollected.RemoveAt(nextTexture);
                                boy.ComponentTexture = mannTexture2;                           


                                boy.ComponentTexture = mannTexture2;
                                boy.ComponentRectangle = mannRect2;

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
                    }
                    if (toy.ComponentRectangle.Intersects(bskHit))
                    {
                        toysCased++;
                        if (toy.ComponentColor == basketLeft.ComponentColor)
                        {

                            if (toysBackground.graphicElements != 0)
                                yeahinst.Play();
                            boy.ComponentTexture = mannTexture3;
                            boy.ComponentRectangle = mannRect3;
                            casedRight++;
                        }
                        else
                        {
                            vibration.Start(TimeSpan.FromMilliseconds(500));
                            ohnoinst.Play();
                            newRand = rnd.Next(0, textures.Length - 1);
                            toysBackground.addGraphicItem(newRand, textures[newRand], rectangles[newRand], true, true, 330, 370, 0, 700);
                            toysCollected.Add(newRand);
                            boy.ComponentTexture = boyDissapointedTxt;
                            boy.ComponentRectangle = mannRect3;

                        }

                        itemPlaced = true;
                        timer = 0;
                        toy.ItemDraw = false;
                        toy.CompRectX = -10000;
                        if (toysBackground.graphicElements == 0)
                            allCleaned = true;


                    }
                }
                else
                {


                    if (toysBackground.graphicElements == 0)
                    {
                        mama.ItemDraw = true;
                        roomBackground.ComponentTexture = roomdooropen;
                        boy.ComponentRectangle = mannRect4;

                        for (int c = 0; c < crayons.Length; c++)
                        {
                            Components.Remove(crayons[c]);
                        }
                        pauseButton.ItemDraw = false;
                        indicator.ItemDraw = false;
                        restartButton.ItemDraw = true;


                        menuButton.CompRectX = 20;
                        menuButton.CompRectY = 10;
                        restartButton.CompRectHeight = 80;
                        restartButton.CompRectWidth = 80;
                        restartButton.CompRectX = 20;
                        restartButton.CompRectY = 100;
                        if (menuButton.ItemDraw != true) //play sound only once
                            myBoyInst.Play();
                        basketLeft.CompRectX = 40;
                        basketOpening.CompRectX = 50;
                        menuButton.ItemDraw = true;
                        if (menuButton.compPushed(touchCollection))
                        {
                            menuButtonPushed = true;

                        }

                        if (restartButton.compPushed(touchCollection))
                        {
                            restartButtonPushed = true;

                        }

                    }
                    if (restartButtonPushed == true || menuButtonPushed == true)
                    {
                        this.UnloadContent();
                        sceneCompleted = true;
                    }
                }


                base.Update(gameTime);
            }
        }
    }



   
}
