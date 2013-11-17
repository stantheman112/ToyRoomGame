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
            colorList = GameTools.elementColors(colorList);
            gameStorage = new GameData("lobby");
            if (gameStorage.fileExists(gameStorage.FileName) == false)
                gameStorage.saveProgression(0);
          
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
           
            spriteBatch = new SpriteBatch(gameMS.GraphicsDevice);
            mannRect = new Rectangle(555, 80, 185, 320);
            mannRect2 = new Rectangle(560, 80, 180, 320);
            mannRect3 = new Rectangle(525, 80, 215, 320);
            guiBubbleRct = new Rectangle(210,10, 400, 250);
           
            basketLeftRect = new Rectangle(0, 320, 250, 150);
            basketOpeningRct = new Rectangle(25, 305, 250, 150);
            bskHit = new Rectangle(35, 400, 180, 80);
            indicatorRct = new Rectangle(470, 145, 200, 200);
            roomRect = new Rectangle(-170, -20, 1140, 620);
         
            
            //default toy størrelse
            defaultRect = new Rectangle(510, 195, 120, 90);

            rectangles[0] = new Rectangle(510, 195, 120, 90); //sportscar
            rectangles[1] = new Rectangle(530, 205, 130, 120); //plane
            rectangles[2] = new Rectangle(510, 210, 130, 90);//train
            rectangles[3] = new Rectangle(510, 220, 120, 90); //tractor
            rectangles[4] = new Rectangle(515, 180, 120, 120); //digger
            rectangles[5] = new Rectangle(510, 180, 130, 130); //dino
            rectangles[6] = new Rectangle(505, 180, 120, 120); //baby
            rectangles[7] = new Rectangle(505, 160, 120, 120); //soldier
            rectangles[8] = new Rectangle(508, 190, 100, 90);//actionfigure
            rectangles[9] = new Rectangle(505, 195, 120, 120);//teddy
            rectangles[10] = new Rectangle(530, 210, 80, 80); //block
            rectangles[11] = new Rectangle(515, 210, 80, 80);//ball
            rectangles[12] = new Rectangle(525, 150, 105, 150); //balloon
            rectangles[13] = new Rectangle(510, 190, 130, 130);//horse
            rectangles[14] = new Rectangle(520, 185, 110, 110);//pad
            rectangles[15] = new Rectangle(510, 200, 130, 100);//racecar
            rectangles[16] = new Rectangle(510, 190, 150, 120);//dumper


           
            rnd = new Random();

            numberOfTurns = rectangles.Length - 1;
            base.Initialize();
        }

       

        protected override void LoadContent()
        {
           
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
           loadScreenDraw(loadScreen, new Vector2(-90, -20));

            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");
            yeah = Content.Load<SoundEffect>("yeah2");
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\tlkBubble");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");
            indicatorTxt = Content.Load<Texture2D>("Images\\indicator");

            textures[0] = Content.Load<Texture2D>("Images\\Colored\\sportscar");
            textures[1] = Content.Load<Texture2D>("Images\\Colored\\theplane");
            textures[2] = Content.Load<Texture2D>("Images\\Colored\\train");
            textures[3] = Content.Load<Texture2D>("Images\\Colored\\tractor");
            textures[4] = Content.Load<Texture2D>("Images\\Colored\\digger");
            textures[5] = Content.Load<Texture2D>("Images\\Colored\\dino");
            textures[6] = Content.Load<Texture2D>("Images\\Colored\\baby");
            textures[7] = Content.Load<Texture2D>("Images\\Colored\\soldier");
            textures[8] = Content.Load<Texture2D>("Images\\Colored\\actionFigure");
            textures[9] = Content.Load<Texture2D>("Images\\Colored\\teddy");
            textures[10] = Content.Load<Texture2D>("Images\\Colored\\block");
            textures[11] = Content.Load<Texture2D>("Images\\Colored\\ball");
            textures[12] = Content.Load<Texture2D>("Images\\Colored\\balloon");
            textures[13] = Content.Load<Texture2D>("Images\\Colored\\pad");
            textures[14] = Content.Load<Texture2D>("Images\\Colored\\horse");
            textures[15] = Content.Load<Texture2D>("Images\\Colored\\racecar");
            textures[16] = Content.Load<Texture2D>("Images\\Colored\\dumper");
             

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
            indicator = new BasicComponent(this.Game, indicatorTxt, indicatorRct, Color.White, 0.0f);
            indicator.DrawOrder = 12122;
            indicator.ItemDraw = false;
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.White, 0.0f);
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(220, 110), "Hello friend \n will you help  me \n clean my room?", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
          talkingBubble.GuiBackgroundRect = guiBubbleRct;
          talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
          talkingBubble.GuiVisible = true;
          basketLeft.DrawOrder = 200;
          basketOpening.DrawOrder = 198;
          toy.DrawOrder = 199;
          boy.DrawOrder = 100;
        
          toy.CompRectX = -10000;
          talkingBubble.DrawOrder = 10000;
            Components.Add(roomBackground);

         

            addToys(440, 460, 0, 700,360, 390, 0, 700, true, false, 222,150,true);
          
           
         
           Components.Add(boy); 
         

           Components.Add(basketOpening);
           
            Components.Add(toy);
            Components.Add(basketLeft);
            loadPauseScreen();     
            Components.Add(talkingBubble);
            Components.Add(indicator);
            numberOfTurns = textures.Length;


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


                          

                            nextTexture = rnd.Next(0, toysCollected.Count - 1);
                            toyName = toyNamePrefix+ toysCollected[nextTexture].ToString();
                           

                            if (toysBackground.removeSprite(toyName) == false)
                                toysForeground.removeSprite(toyName);
                               
                          
                        }

                        toy.ComponentTexture = textures[toysCollected[nextTexture]];
                        toy.ComponentRectangle = rectangles[toysCollected[nextTexture]];
                        toysCollected.RemoveAt(nextTexture);
              


                        boy.ComponentTexture = mannTexture2;
                        boy.ComponentRectangle = mannRect2;




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
                    if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3)
                    {
                        if (indicator.ItemDraw == true)
                            indicator.ItemDraw = false;
                        boy.ComponentTexture = mannTexture;
                        boy.ComponentRectangle = mannRect;

                    }
                    if (toy.ComponentRectangle.Intersects(bskHit))
                    {
                        toysCased++;
                        yeahinst.Play();
                        boy.ComponentTexture = mannTexture3;
                        boy.ComponentRectangle = mannRect3;
                      

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
