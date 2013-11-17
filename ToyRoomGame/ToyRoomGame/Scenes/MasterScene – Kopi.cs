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
using System.Diagnostics;




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
      protected Texture2D mannTexture, mannTexture2, mannTexture3, basketTxt, boyDissapointedTxt, roomTexture, crayonTxt, guiBubbleTxt, basketLeftTxt, basketOpeningTxt, loadScreen, pauseScreenTxt, indicatorTxt,
                          resumeTxt, restartTxt, pauseButtonTxt, menuButtonTxt, mamaTxt, roomdooropen;
      protected Rectangle mannRect, bskHit, basketLeftRect, mannRect2, mannRect3, defaultRect, roomRect, indicatorRct,
                          crayonRect, guiBubbleRct, basketOpeningRct, basketRightRect, basketOpeningLRct,
                          basketOpeningRRct, bskHitL, bskHitR, pauseScreenRect, restartRect, resumeRect, pauseButtonRect, menuButtonRect,  mamaRct,  mannRect4;
      
      protected GameGUI gameGUI;
      protected List<Color> colorList = new List<Color>();
      protected List<Texture2D> textures = new List<Texture2D>();
      protected Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat, rightColor, wrongColor, tmpColor, floorToyColor;
      protected Random rnd;
      protected bool itemPlaced = false, firstRun = true, sceneCompleted = false, gamePaused = false, menuButtonPushed = false, restartButtonPushed = false;
      protected int trigger = 90, timer = 0, toysCased=0, casedRight = 0, numberOfTurns,  maxScore=20, rollDirection, newRand, nextTexture, timerNow = 0;
      protected MoveAbleComponent toy;
      protected SpriteFont spriteFont;
      protected BasicComponent boy, basketLeft, roomBackground, basketOpening, basketOpeningLeft, basketOpeningRight, basketRight, pauseScreen, resumeButton, restartButton, pauseButton, menuButton, indicator, mama;
      protected ContentManager Content;
      protected GameComponentCollection Components;
      protected List<Rectangle> rectangles = new List<Rectangle>();
      protected List<BasicComponent> crayons = new List<BasicComponent>();
      protected List<BasicComponent> floorToysB, floorToysI, floorToys;     
      protected GameGUI talkingBubble;
      protected float basketSpeed = 0.1f;
      protected int basketAccel, speed = 1;
      protected Accelerometer accelSensor;
      protected Vector3 accelReading = new Vector3();
      protected SoundEffect backgroundMusic, yeah, ohno, myBoy;
      protected List<int> toysCollected;
      protected SoundEffectInstance yeahinst, ohnoinst, backgroundMusicInst, myBoyInst;
      protected GraphicsDeviceManager graphics;    
      protected bool onTopOfBasket = false, leftOfBasket = false, rightOfBasket = false, allCleaned = false;
      protected TouchCollection touchCollection;
      protected VibrateController vibration; 
      protected float rotationDirection = -1f;
      protected Game gameMS;
      protected Graphics toysBackground, toysForeground;
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
       
            vibration  = VibrateController.Default;
            gameMS = game;
            // TODO: Construct any child components here
        }
        protected void addToys(int vertRangeMinFG, int vertRangeMaxFG, int horRangeMinFG, int horRangeMaxFG, int vertRangeMinBG,
            int vertRangeMaxBG, int horRangeMinBG, int horRangeMaxBG, bool randomRotation, bool randomColors, int foreGroundGraphicsDrawOrder, int backgroundGraphicsDrawOrder, bool randomMizeMoreThanOne)
        {

            toysCollected = new List<int>();
            for (int i = 0; i < textures.Count; i++)
            {
                toysCollected.Add(i);
            }
            List<float> rots = new List<float>();
            List<Color> colors = new List<Color>();
            
            List<Texture2D> graphicsBackground = new List<Texture2D>();
            List<Rectangle> rectanglesBackground = new List<Rectangle>();

            if (randomMizeMoreThanOne == true)
            {


                List<Texture2D> graphicsForeGround = new List<Texture2D>();
                List<Rectangle> rectanglesForeground = new List<Rectangle>();
                randomMizeGraphics(textures, rectangles, ref rectanglesBackground, ref rectanglesForeground, ref graphicsBackground, ref graphicsForeGround);
                toysBackground = new Graphics(this.Game, graphicsBackground, rectanglesBackground, randomRotation, randomColors, vertRangeMinBG, vertRangeMaxBG, horRangeMinBG, horRangeMaxBG);
                toysForeground = new Graphics(this.Game, graphicsForeGround, rectanglesForeground, randomRotation, randomColors, vertRangeMinFG, vertRangeMaxFG, horRangeMinFG, horRangeMaxFG);
                toysForeground.DrawOrder = foreGroundGraphicsDrawOrder;
                Components.Add(toysForeground);
            }
            else
            {
                addGraphics(textures, rectangles, ref rectanglesBackground, ref graphicsBackground);
            }
           
            toysBackground = new Graphics(this.Game, graphicsBackground, rectanglesBackground, randomRotation, randomColors, vertRangeMinBG, vertRangeMaxBG, horRangeMinBG, horRangeMaxBG);
            toysBackground.DrawOrder = backgroundGraphicsDrawOrder;
            Components.Add(toysBackground);
           
        }
        protected void accelSensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X;
            accelReading.Y = (float)e.Y;
            accelReading.Z = (float)e.Z;
        }
        //protected void setGraphicContent()
        //{
        //    graphics = new Graphics(
        //}
        protected void addGraphics(List<Texture2D> orgList, List<Rectangle> orgRectList, ref List<Rectangle> rectList1, ref List<Texture2D> textList1 )
        //   protected void randomMizeGraphics(int count)
        {
            for(int i = 0; i<orgList.Count; i++) {
              textList1.Add(orgList[i]);
                textList1[i].Name = "toy_" + i;
              rectList1.Add(orgRectList[i]);
            }
          

        }

       protected void randomMizeGraphics(List<Texture2D> orgList, List<Rectangle> orgRectList, ref List<Rectangle> rectList1,  ref List<Rectangle> rectList2, ref List<Texture2D> textList1, ref List<Texture2D> textList2)
     //   protected void randomMizeGraphics(int count)
      
        {
            List<int> tmp = new List<int>();
           
            int next;
        while(textList1.Count+textList2.Count<orgList.Count)
            {
                int wichList = rnd.Next(0, 2);
               switch (wichList) {
                   case 0 :
                        next = rnd.Next(0,orgList.Count);
                       if (tmp.Count == 0)
                       {
                           tmp.Add(next);
                           textList1.Add(orgList[next]);
                           textList1[textList1.Count - 1].Name = "toy_" + next;
                           rectList1.Add(orgRectList[next]);
                       }
                       else
                       {
                           bool inList = false;    
                           do
                           {
                               inList = false;                         
                              next = rnd.Next(0,orgList.Count);
                               
                               for (int t = 0; t < tmp.Count; t++)
                               {
                                   if (next == tmp[t])
                                   {
                                       inList = true;
                                       next = rnd.Next(0, orgList.Count);
                                       break;

                                   }
                              


                               }
                           } while (inList == true);
                           tmp.Add(next);
                           textList1.Add(orgList[next]);
                           textList1[textList1.Count - 1].Name = "toy_" + next;
                             rectList1.Add(orgRectList[next]);
                       }
                       break;
                   case 1: 
                         next = rnd.Next(0,orgList.Count);
                       if (tmp.Count == 0)
                       {
                           tmp.Add(next);
                           textList2.Add(orgList[next]);
                           textList2[textList2.Count - 1].Name = "toy_" + next;
                             rectList2.Add(orgRectList[next]);
                       }
                       else
                       {
                           bool inList = false;
                           do
                           {
                                inList = false;
                                next = rnd.Next(0,orgList.Count);
                               
                               for (int t = 0; t < tmp.Count; t++)
                               {
                                   if (next == tmp[t] && inList==false)
                                   {
                                       inList = true;
                                       next = rnd.Next(0, orgList.Count);
                                       break;

                                   }
                                 


                               }
                           } while (inList == true);
                           tmp.Add(next);
                           textList2.Add(orgList[next]);
                           textList2[textList2.Count - 1].Name = "toy_" + next;
                           rectList2.Add(orgRectList[next]);
                       }
                       break;
               }
            }
            
        }

        protected void basketRoll()
        {
           
          
            if (this.Game.Window.CurrentOrientation.ToString() == "LandscapeLeft")
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
            
            loadScreenDraw(loadScreen, new Vector2(-90, -20));
            accelSensor = null;
            indicator = null;           
            basketLeft = null;
            basketOpeningLeft = null; 
            basketOpeningLeft = null;
            basketRight = null;          
            mama = null;       
            boy = null;
            toy = null;
            talkingBubble = null;
            toysCollected = null;
            for (int r = 0; r < rectangles.Count; r++)
                rectangles[r] = Rectangle.Empty;          
            Content.Unload();           
            Components.Clear();
            rectangles.Clear();
            for (int txt = 0; txt < textures.Count; txt++)
            {            
                textures[txt] = null;
            }
            rectangles.Clear();
            textures.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();


            string test = GC.GetTotalMemory(true).ToString();
            Debug.WriteLine(test);
          
            base.UnloadContent();
        }

        protected void arrangeCrayons()
        {
            foreach (BasicComponent mc in crayons)
            {
            //    if (mc.ItemMoving == false)
            //    {
            //        mc.ItemTouched = false;
            //        mc.resetToStart();
            //    }
            //    else
            //    {
            //        toy.ItemTouched = false;
            //    }
                if (mc.ComponentRectangle.Intersects(bskHit))
                {
                    basketLeft.ComponentColor = mc.ComponentColor;
                    basketOpening.ComponentColor = mc.ComponentColor;
                }


            }
        }
        
        protected BasicComponent addFloorToy(int compNumber, int toyNumber)
        {
            int x = rnd.Next(0, 650);
            int y = rnd.Next(270, 420);

            //string rot = // Convert.ToString(rnd.Next(0, 2)) + "." + Convert.ToString(rnd.Next(0, 9));

            double rotation = rnd.NextDouble();
            int nextFloorToy = rnd.Next(0, textures.Count - 1);


            rotationDirection = rotationDirection * -1f;
            rotation = rotation * rotationDirection;
            Rectangle tmpRect = new Rectangle(x, y, rectangles[compNumber].Width, rectangles[compNumber].Height);


            GameTools.randomColor(ref floorToyColor, 255);
            tmpColor = floorToyColor;
            while (floorToyColor == tmpColor)
            {
                GameTools.randomColor(ref floorToyColor, 255);
            }
            GameTools.randomColor(ref tmpColor, 255);
            // rectangles.Add(rect);
            //textures.Add(text);
            BasicComponent tmp = new BasicComponent(this.Game, textures[compNumber], tmpRect, floorToyColor, (float)rotation);
            
            tmp.ComponentType = "toy" + Convert.ToString(toyNumber);
            tmp.ComponentNumber = compNumber;
            floorToys.Add(tmp);
            if ((y + tmpRect.Height)-60 < (mannRect.Y + mannRect.Height))
            {
                tmp.DrawOrder = 99;
                floorToysB.Add(tmp);
                tmp.Dispose();
                return floorToysB[floorToysB.Count - 1];
               
              
            }
            else
            {
                if (basketLeft.CompRectY + basketLeft.CompRectHeight < (tmpRect.Height + y)-60)
                {
                    tmp.DrawOrder = 205;
                    floorToysI.Add(tmp);
                   
                }
                else
                {
                    tmp.DrawOrder = 195;
                    floorToysI.Add(tmp);
                   
                }


                tmp.Dispose();
                return floorToysI[floorToysI.Count-1];
            }
           
        }

        protected BasicComponent addFloorToy(int compNumber, int toyNumber, Color originalColor)
        {
            int x = rnd.Next(0, 650);
            int y = rnd.Next(290, 400);

            //string rot = // Convert.ToString(rnd.Next(0, 2)) + "." + Convert.ToString(rnd.Next(0, 9));

            double rotation = rnd.NextDouble();
            int nextFloorToy = rnd.Next(0, textures.Count - 1);


            rotationDirection = rotationDirection * -1f;
            rotation = rotation * rotationDirection;
            Rectangle tmpRect = new Rectangle(x, y, rectangles[compNumber].Width, rectangles[compNumber].Height);
            BasicComponent tmp = new BasicComponent(this.Game, textures[0], rectangles[0], Color.Wheat, 0f);
      
           
            tmp.DrawOrder = -100;
            tmp.ComponentType = "toy" + Convert.ToString(toyNumber);
            tmp.ComponentNumber = compNumber;
            floorToys.Add(tmp);
            if ((y + tmpRect.Height)-60 < (mannRect.Y + mannRect.Height))
            {
                tmp.DrawOrder = 99;
                floorToysB.Add(tmp);
                tmp.Dispose();
              //  return tmp;
                return floorToysB[floorToysB.Count-1];
            }
            else
            {
                if (basketLeft.CompRectY + basketLeft.CompRectHeight < (tmpRect.Height + y) - 60)
                {
                    tmp.DrawOrder = 205;
                    floorToysI.Add(tmp);
                  
                }
                else
                {
                    tmp.DrawOrder = 195;
                    floorToysI.Add(tmp);
                  
                   
                }
                tmp.Dispose();
                return floorToysI[floorToysI.Count-1];
               // return tmp;
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




            //if (toy.CompRectX >= (basket.CompRectX) && (toy.CompRectY + toy.ComponentRectangle.Height) >= basket.CompRectY && onTopOfBasket == true)
            //{
            //    toy.CompRectX = basket.CompRectX;
            //}

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
