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
using Microsoft.Phone;
using Microsoft.Phone.Shell;
using Microsoft.Devices;

namespace Toyroom.Scenes
{

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MasterScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        
        #region private variables
      protected GameData gameStorage;
      protected SpriteBatch spriteBatch;
      protected string floorToyPickedColor = string.Empty;
      protected int floorToypickedColorNumber = 0;
      protected Texture2D mannTexture, mannTexture2, mannTexture3, basketTxt, boyDissapointedTxt, roomTexture, crayonTxt, guiBubbleTxt, basketLeftTxt, basketOpeningTxt, loadScreen, pauseScreenTxt, indicatorTxt,
                          resumeTxt, restartTxt, pauseButtonTxt, menuButtonTxt, mamaTxt, roomdooropen;
      protected Rectangle mannRect, bskHit, basketLeftRect, mannRect2, mannRect3, defaultRect, roomRect, indicatorRct,
                          crayonRect, guiBubbleRct, basketOpeningRct, basketRightRect, basketOpeningLRct,
                          basketOpeningRRct, bskHitL, bskHitR, pauseScreenRect, restartRect, resumeRect, pauseButtonRect, menuButtonRect,  mamaRct,  mannRect4;
      
      protected GameGUI gameGUI;
     protected Color[] colorList = new Color[10];
     
      protected Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat, rightColor, wrongColor, tmpColor, floorToyColor;
      protected Random rnd;
      protected bool itemPlaced = false, firstRun = true, sceneCompleted = false, gamePaused = false, menuButtonPushed = false, restartButtonPushed = false;    
      protected MoveAbleComponent toy;
      protected SpriteFont spriteFont;
      protected BasicComponent boy, basketLeft, roomBackground, basketOpening, basketOpeningLeft, basketOpeningRight, basketRight, pauseScreen, resumeButton, restartButton, pauseButton, menuButton, indicator, mama;
      protected ContentManager Content;
      protected GameComponentCollection Components;
      
      protected BasicComponent[] crayons;
       
      protected GameGUI talkingBubble;
      protected float basketSpeed = 0.1f;
      protected int basketAccel, speed = 1, nextTexture, nextColor, nextWrongColor, trigger = 90, timer = 0, toysCased = 0, casedRight = 0, numberOfTurns, maxScore = 20, rollDirection, newRand, timerNow = 0,
          chooseCrayon, chooseBasket, foreOrBack;
      UInt16 toyCount = 17;
      protected Texture2D[] textures;
      protected Rectangle[] rectangles;
      protected Accelerometer accelSensor;
      protected Vector3 accelReading = new Vector3();
      protected SoundEffect backgroundMusic, yeah, ohno, myBoy;
      protected List<int> toysCollected;
      protected SoundEffectInstance yeahinst, ohnoinst, backgroundMusicInst, myBoyInst;
      protected GraphicsDeviceManager graphics;
      protected bool onTopOfBasket = false, leftOfBasket = false, rightOfBasket = false, allCleaned = false, removed;
      protected TouchCollection touchCollection;
      protected VibrateController vibration; 
      protected float rotationDirection = -1f;
      protected Game gameMS;
      protected Graphics toysBackground, toysForeground;
      protected string toyName, toyNamePrefix="toy_", musicStateStopped = "Stopped";
      string landScapeLeft = "LandscapeLeft";
        #endregion 
        #region public properties
    //  public ContentManager Content { get; set; }
        public bool SceneCompleted
        {
            get
            {
                return sceneCompleted;
            }
        }
        public bool Restart
        {
            get
            {
                return restartButtonPushed;
            }
        }
        #endregion




        public MasterScene(Game game, ContentManager content, GameComponentCollection components)
            : base(game)
        {
            Components = components;
            Content = content;
              textures = new Texture2D[toyCount];
        rectangles = new Rectangle[toyCount];
       
            vibration  = VibrateController.Default;
            gameMS = game;
         
        }
        protected void addToys(int vertRangeMinFG, int vertRangeMaxFG, int horRangeMinFG, int horRangeMaxFG, int vertRangeMinBG,
            int vertRangeMaxBG, int horRangeMinBG, int horRangeMaxBG, bool randomRotation, bool randomColors, int foreGroundGraphicsDrawOrder, 
            int backgroundGraphicsDrawOrder, bool randomMizeMoreThanOne)
        {

            toysCollected = new List<int>();
            for (int i = 0; i < textures.Length; i++)
            {
                toysCollected.Add(i);
            }
        

            if (randomMizeMoreThanOne == true)
            {
                int array1Length = rnd.Next(0, toyCount+1);
                int array2Length = toyCount - array1Length;

                Texture2D[] graphicsBackground = new Texture2D[array1Length];
                Rectangle[] rectanglesBackground = new Rectangle[array1Length];
                Texture2D[] graphicsForeGround = new Texture2D[array2Length];
                Rectangle[] rectanglesForeground = new Rectangle[array2Length];
                randomMizeGraphics(textures, rectangles, ref rectanglesBackground, ref rectanglesForeground, ref graphicsBackground, ref graphicsForeGround);
                toysBackground = new Graphics(this.Game, graphicsBackground, rectanglesBackground, randomRotation, randomColors, vertRangeMinBG, vertRangeMaxBG, horRangeMinBG, horRangeMaxBG);
                toysForeground = new Graphics(this.Game, graphicsForeGround, rectanglesForeground, randomRotation, randomColors, vertRangeMinFG, vertRangeMaxFG, horRangeMinFG, horRangeMaxFG);
                toysForeground.DrawOrder = foreGroundGraphicsDrawOrder;
              
                Components.Add(toysForeground);
            }
            else
            {

                Texture2D[] graphicsBackground = new Texture2D[toyCount];
                Rectangle[] rectanglesBackground = new Rectangle[toyCount];
                addGraphics(textures, rectangles, ref rectanglesBackground, ref graphicsBackground);
                toysBackground = new Graphics(this.Game, graphicsBackground, rectanglesBackground, randomRotation, randomColors, vertRangeMinBG, vertRangeMaxBG, horRangeMinBG, horRangeMaxBG);
            }
           
          
            toysBackground.DrawOrder = backgroundGraphicsDrawOrder;
            Components.Add(toysBackground);
           
        }
        protected void accelSensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X;
            accelReading.Y = (float)e.Y;
            accelReading.Z = (float)e.Z;
        }
       
        protected void addGraphics(Texture2D[] orgList, Rectangle[] orgRectList, ref Rectangle[] rectList1, ref Texture2D[] textList1 )       
        {
            for(int i = 0; i<orgList.Length; i++) {
              textList1[i] =orgList[i];
                textList1[i].Name = toyNamePrefix + i;
              rectList1[i] = orgRectList[i];
            }
          

        }
        private int[] randomMizeList (int[] numbers) {
            List<int> usedNumbers = new List<int>();
            bool used = true;
            int next = rnd.Next(0, numbers.Length);
            if (usedNumbers.Count == 0)
                usedNumbers.Add(next);
          if(usedNumbers.Count>0)
            {
                while (used == true || usedNumbers.Count<numbers.Length)
                {
                    used = false;
                    for (int u = 0; u < usedNumbers.Count; u++)
                    {
                        if (usedNumbers[u] == next)
                            used = true;

                    }
                    if(used == true)
                        next = rnd.Next(0, numbers.Length);
                    else
                        usedNumbers.Add(next);
                }
                
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = usedNumbers[i];
            }

            return numbers;

        }
       protected void randomMizeGraphics(Texture2D[] orgList, Rectangle[] orgRectList, ref Rectangle[] rectList1,  ref Rectangle[] rectList2, ref Texture2D[] textList1, ref Texture2D[] textList2)
        {
            List<int> tmp = new List<int>();

            int next = 0;
        bool checkNextValid = false;
          int[] textList1Teller = new int[toyCount];
          int theZeroPlace = rnd.Next(0, orgList.Length);
          textList1Teller = randomMizeList(textList1Teller);
         
        
       for(int l1 = 0; l1 <textList1.Length; l1++) {       

           rectList1[l1] = orgRectList[textList1Teller[l1]];
           textList1[l1] = orgList[textList1Teller[l1]];
           textList1[l1].Name = toyNamePrefix + textList1Teller[l1];

       }
                
           for (int l2 = 0; l2 < textList2.Length; l2++)
           {

              textList2[l2] = orgList[textList1Teller[textList1.Length + l2]];
              rectList2[l2] = orgRectList[textList1Teller[textList1.Length + l2]];
              textList2[l2].Name = toyNamePrefix + textList1Teller[textList1.Length + l2];              

           }         
            
        }

        protected void basketRoll()
        {         

           if (this.Game.Window.CurrentOrientation.ToString() == landScapeLeft)
            {
                rollDirection = -20;

                if (basketLeft.CompRectX >= -50 && accelReading.Y > 0.0f)
                {
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);                       

                }
                if (basketLeft.CompRectX < -50 && accelReading.Y < 0.0f)
                    basketLeft.CompRectX = -20;

                if (basketLeft.CompRectX <= 590 && accelReading.Y < 0.0f)
                {
                    
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
                }
                if (basketLeft.CompRectX > 590 && accelReading.Y > 0.0f)
                    basketLeft.CompRectX = 560;
            }
            else
            {
                rollDirection = 20;
             
                if (basketLeft.CompRectX <= 590 && accelReading.Y > 0.0f)
                {                    
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
                }
                if (basketLeft.CompRectX >= -50 && accelReading.Y < 0.0f)
                {
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
                }
                if (basketLeft.CompRectX < -50 && accelReading.Y > 0.0f)
                {
                    basketLeft.CompRectX = -20;
                }

                if (basketLeft.CompRectX > 590 && accelReading.Y < 0.0f)
                {
                    basketLeft.CompRectX = 560;
                }               
            }         
            basketOpening.CompRectX = basketLeft.CompRectX + 15;
            bskHit.X = basketLeft.CompRectX;
            basketOpening.ComponentColor = basketLeft.ComponentColor;

        }
     
        protected override void UnloadContent()
        {

            accelSensor = null;
            indicator = null;
            basketLeft = null;
            basketOpeningLeft = null;

            basketRight = null;
            mama = null;
            boy = null;
            toy = null;
            talkingBubble = null;
            gameStorage = null;
            toysBackground = null;
            toysForeground = null;
            toyName = null;
            gameStorage = null;
            spriteBatch = null;
            mannTexture = null;
            mannTexture2 = null;
            mannTexture3 = null;
            basketTxt = null;
            boyDissapointedTxt = null;
            roomTexture = null;
            crayonTxt = null;
            guiBubbleTxt = null;
            basketLeftTxt = null;
            basketOpeningTxt = null;
            loadScreen = null;
            pauseScreenTxt = null;
            indicatorTxt = null;

            resumeTxt = null;
            restartTxt = null;
            pauseButtonTxt = null;
            menuButtonTxt = null;
            mamaTxt = null;
            roomdooropen = null;
            mannRect = Rectangle.Empty;
            bskHit = Rectangle.Empty;
            basketLeftRect = Rectangle.Empty;
            mannRect2 = Rectangle.Empty;
            mannRect3 = Rectangle.Empty;
            defaultRect = Rectangle.Empty;
            roomRect = Rectangle.Empty;
            indicatorRct = Rectangle.Empty;
            crayonRect = Rectangle.Empty;
            guiBubbleRct = Rectangle.Empty;
            basketOpeningRct = Rectangle.Empty;
            basketRightRect = Rectangle.Empty;
            basketOpeningLRct = Rectangle.Empty;
            basketOpeningRRct = Rectangle.Empty;
            bskHitL = Rectangle.Empty;
            bskHitR = Rectangle.Empty;
            pauseScreenRect = Rectangle.Empty;
            restartRect = Rectangle.Empty;
            resumeRect = Rectangle.Empty;
            pauseButtonRect = Rectangle.Empty;
            menuButtonRect = Rectangle.Empty;
            mamaRct = Rectangle.Empty;
            mannRect4 = Rectangle.Empty;
            gameGUI = null;
            colorList = null;
            
            rnd = null;
            toy = null;
            spriteFont = null;
            boy = null;
            roomBackground = null;
            basketOpening = null;
            basketOpeningLeft = null;
            basketOpeningRight = null; basketRight = null;
            pauseScreen = null;
            resumeButton = null;
            restartButton = null;
            pauseButton = null;
            menuButton = null;
            indicator = null;
            mama = null;

            Components.Clear();
            for (int i = 0; i < rectangles.Length; i++)
            {
                rectangles[i] = Rectangle.Empty;
            }
            if (crayons != null)
            {
                for (int c = 0; c < crayons.Length; c++)
                {
                    crayons[c] = null;


                }
            }
            crayons = null;
            backgroundMusic = null;
            yeah = null;
            ohno = null;
            myBoy = null;
            toysCollected.Clear();
            toysCollected = null;
            yeahinst = null;
            ohnoinst = null;
            backgroundMusicInst = null;
            myBoyInst = null;
            graphics = null;
            vibration = null;
            gameMS = null;
            toysBackground = null;
            toysForeground = null;
            toyName = string.Empty;
            toyNamePrefix = string.Empty;
            musicStateStopped = string.Empty;
            for (int r = 0; r < rectangles.Length; r++)
                rectangles[r] = Rectangle.Empty;
            Content.Unload();
            while (Components.Count > 0)
                Components.RemoveAt(0);
          
            for (int txt = 0; txt < textures.Length; txt++)
            {
                textures[txt] = null;
            }

          
            textures = null;
            unLoadPauseScreen();
          
            base.UnloadContent();


            string test = GC.GetTotalMemory(true).ToString();
            
        }

        protected void arrangeCrayons()
        {
            foreach (BasicComponent mc in crayons)
            {
          
                if (mc.ComponentRectangle.Intersects(bskHit))
                {
                    basketLeft.ComponentColor = mc.ComponentColor;
                    basketOpening.ComponentColor = mc.ComponentColor;
                }

                
            }
        }
        
      

        //sikrer at man må dra leken rett oppi kurven
        protected void basketCollisionHandling(BasicComponent basket, bool leftside)
        {
            if (leftside == true)
            {
                if (onTopOfBasket == true)
                {
                    onTopOfBasket = true; leftOfBasket = false; rightOfBasket = false;
                }
                else
                {
                    onTopOfBasket = false; leftOfBasket = false; rightOfBasket = true;
                }
            }
            else
            {
                if (onTopOfBasket == true)
                {
                    onTopOfBasket = true; leftOfBasket = false; rightOfBasket = false;
                }
                else
                {
                    onTopOfBasket = false; leftOfBasket = true; rightOfBasket = false;
                }
            }

            if (toy.CompRectX <= (basket.CompRectX + basket.ComponentRectangle.Width) && toy.CompRectX >= basket.CompRectX && toy.CompRectY + toy.ComponentRectangle.Height < basket.CompRectY)
            {
                onTopOfBasket = true;
                leftOfBasket = false;
                rightOfBasket = false;
            }
            else
            {
                if (toy.ItemMoving == false)
                {
                    onTopOfBasket = false;
                }
            }


            if (toy.CompRectX > basket.CompRectX + basket.CompRectWidth)
            {
                rightOfBasket = true;
                leftOfBasket = false;
                onTopOfBasket = false;
            }
            else
            {
                if (toy.ItemMoving == false)
                    rightOfBasket = false;
            }

            if (toy.CompRectX + toy.CompRectWidth < basket.CompRectX)
            {
                leftOfBasket = true;
                rightOfBasket = false;
                onTopOfBasket = false;
            }
            else
            {
                if (toy.ItemMoving == false)
                    leftOfBasket = false;
            }

            if (toy.CompRectX - 10 <= (basket.CompRectX + basket.ComponentRectangle.Width) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basket.CompRectY && onTopOfBasket == false && leftOfBasket == false && rightOfBasket == true)
            {
                toy.CompRectX = basket.CompRectX + basket.ComponentRectangle.Width + 10;
            }

            if (leftOfBasket == true && rightOfBasket == false)
            {

                if (toy.CompRectX + toy.ComponentRectangle.Width >= (basket.CompRectX - 10) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basket.CompRectY && onTopOfBasket == false)
                {
                    toy.CompRectX = basket.CompRectX - (toy.ComponentRectangle.Width + 10);
                }

            }
            if (toy.ComponentRectangle.Intersects(basket.ComponentRectangle) && onTopOfBasket == true && leftOfBasket == false && rightOfBasket == false)
                toy.CompRectX = basket.CompRectX + 85;
        }


        //sikrer at man må dra leken rett oppi kurven
        protected void basketCollisionHandling(BasicComponent basket)
        {
            if (toy.CompRectX <= (basket.CompRectX + basket.ComponentRectangle.Width) && toy.CompRectX >= basket.CompRectX && toy.CompRectY + toy.ComponentRectangle.Height < basket.CompRectY)
            {
                onTopOfBasket = true;
                leftOfBasket = false;
                rightOfBasket = false;
            }
            else
            {

                onTopOfBasket = false;
            }


            if (toy.CompRectX > basket.CompRectX + basket.CompRectWidth)
            {
                rightOfBasket = true;
                leftOfBasket = false;
                onTopOfBasket = false;
            }
            else
            {
               
                if (toy.ItemMoving == false)
                    rightOfBasket = false;
            }

            if (toy.CompRectX + toy.CompRectWidth < basket.CompRectX)
            {
                leftOfBasket = true;
                onTopOfBasket = false;
            }
            else
            {
                if (toy.ItemMoving == false)
                    leftOfBasket = false;
            }

            if (toy.CompRectX - 10 <= (basket.CompRectX + basket.ComponentRectangle.Width) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basketLeft.CompRectY && onTopOfBasket == false && leftOfBasket == false && rightOfBasket == true)
            {
                toy.CompRectX = basket.CompRectX + basket.ComponentRectangle.Width + 10;
            }

            if (leftOfBasket == true && rightOfBasket == false)
            {

                if (toy.CompRectX + toy.ComponentRectangle.Width >= (basketLeft.CompRectX - 10) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basket.CompRectY && onTopOfBasket == false)
                {
                    toy.CompRectX = basket.CompRectX - (toy.ComponentRectangle.Width + 10);
                }

            }
            if (toy.ComponentRectangle.Intersects(basket.ComponentRectangle) && onTopOfBasket == false && leftOfBasket == false && rightOfBasket == false)
                toy.CompRectX = basket.CompRectX + 85;

            if (toy.CompRectX >= (basket.CompRectX) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basket.CompRectY && onTopOfBasket == true)
            {
                toy.CompRectX = basket.CompRectX;
            }

        }
        protected void loadScreenDraw(Texture2D text2d, Vector2 vect2)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            spriteBatch.Begin();

            spriteBatch.Draw(text2d, vect2, Color.White);

            spriteBatch.End();

            GraphicsDevice.Present();


        }
        protected void initPauseScreen()
        {
            pauseScreenRect = new Rectangle(0,0, 800, 500);
            restartRect = new Rectangle(50, 400, 130, 130);
            resumeRect = new Rectangle(50, 100, 130, 130);
            pauseButtonRect = new Rectangle(10, 10, 50, 50);
            menuButtonRect = new Rectangle(400, 400, 80, 80);

        }
        protected void loadPauseScreen()
        {
             pauseScreenTxt = Content.Load<Texture2D>("Images\\gui\\pausescreen");
             resumeTxt = Content.Load<Texture2D>("Images\\playButton");
             restartTxt = Content.Load<Texture2D>("Images\\gui\\restartButton");
             pauseButtonTxt = Content.Load<Texture2D>("Images\\gui\\pauseButton");
             menuButtonTxt = Content.Load<Texture2D>("Images\\gui\\menuButton");
             restartButton = new BasicComponent(this.Game, restartTxt, restartRect, Color.White, 0.0f);
             resumeButton = new BasicComponent(this.Game, resumeTxt, resumeRect, Color.White, 0.0f);
             pauseScreen = new BasicComponent(this.Game, pauseScreenTxt, pauseScreenRect, Color.White, 0.0f);
             pauseButton = new BasicComponent(this.Game, pauseButtonTxt, pauseButtonRect, Color.White, 0.0f);
             menuButton = new BasicComponent(this.Game, menuButtonTxt, menuButtonRect, Color.White, 0.0f);

             restartButton.ItemDraw = false;
             pauseScreen.ItemDraw = false;
             resumeButton.ItemDraw = false;
             menuButton.ItemDraw = false;
             pauseButton.DrawOrder = 1012;
             pauseScreen.DrawOrder = 1000;
             menuButton.DrawOrder = 1013;
            resumeButton.DrawOrder = 1014;
            restartButton.DrawOrder = 1015;
             Components.Add(pauseButton); 
             Components.Add(pauseScreen);
             Components.Add(restartButton);
             Components.Add(menuButton);
             Components.Add(resumeButton);             
             
        

        }
        protected void unLoadPauseScreen()
        {
           
            pauseScreenTxt = null;
            resumeTxt = null;
            restartTxt = null;
            pauseButtonTxt = null;
            menuButtonTxt = null;
            restartButton = null;
            resumeButton = null;
            pauseScreen = null;
            pauseButton = null;
            menuButton = null;

        }
        protected void GameDeactivated(object sender, EventArgs a)
        {
            gamePaused = true;

        }
        protected bool pauseGame()
        {             
            
              touchCollection = TouchPanel.GetState();
              if (gamePaused == true)
              {
                  if (menuButton.compPushed(touchCollection))
                  {
                      menuButtonPushed = true;                      
                      return false;
                  }

                  if (restartButton.compPushed(touchCollection))
                  {
                      restartButtonPushed = true;
                      return false;
                  }
                  if (resumeButton.compPushed(touchCollection))
                  {
                      pauseButton.ItemDraw = true;
                      gamePaused = false;
                      talkingBubble.Visible = false;
                      
                      restartButton.ItemDraw = false;
                      pauseScreen.ItemDraw = false;
                      resumeButton.ItemDraw = false;
                      menuButton.ItemDraw = false;
                      restartButtonPushed = false;
                      return gamePaused;
                  }
              }
              if (pauseButton.compPushed(touchCollection) || GamePad.GetState(PlayerIndex.One).Buttons.BigButton == ButtonState.Pressed || gamePaused == true)
              {
                  if (talkingBubble != null)
                      Components.Remove(talkingBubble);
                  pauseButton.ItemDraw = false;
                  indicator.ItemDraw = false;
                  restartButton.ItemDraw = true;
                  pauseScreen.ItemDraw = true;
                  resumeButton.ItemDraw = true;
                  menuButton.ItemDraw = true;
                  gamePaused = true;
                  return true;

              }
           
              if (gamePaused == false)
              {

                  return false;
              }
              else
              {
                  return true;
              }
            
        }
    }
}
