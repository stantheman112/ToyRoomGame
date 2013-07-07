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
using WindowsPhoneGame1.GameStorage;
using System.Diagnostics;
using Microsoft.Devices.Sensors;


namespace WindowsPhoneGame1.Scenes
{


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MasterScene : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region private variables
        GameData gameStorage;
        SpriteBatch spriteBatch;
      protected  Texture2D mannTexture, mannTexture2, mannTexture3, basketTxt, boyDissapointedTxt, roomTexture, crayonTxt, guiBubbleTxt, basketLeftTxt, basketOpeningTxt, loadScreen;
        protected Rectangle mannRect, bskHit, basketLeftRect, mannRect2, mannRect3, defaultRect, roomRect, crayonRect, guiBubbleRct, basketOpeningRct, basketRightRect, basketOpeningLRct, basketOpeningRRct, bskHitL, bskHitR ;


          
        GameGUI gameGUI;
        List<Color> colorList = new List<Color>();
      protected  List<Texture2D> textures = new List<Texture2D>();
        Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat, rightColor, wrongColor, tmpColor, floorToyColor;
      protected  Random rnd;
        bool itemPlaced = false, firstRun = true, sceneCompleted=false;
        int trigger = 90, timer = 0, toysCased=0, numberOfTurns,  maxScore=20, rollDirection;
      protected MoveAbleComponent toy;
        SpriteFont spriteFont;
       protected BasicComponent boy, basketLeft, roomBackground, basketOpening, basketOpeningLeft, basketOpeningRight,basketRight ;
        ContentManager Content;
        GameComponentCollection Components;
     protected   List<Rectangle> rectangles = new List<Rectangle>();
       protected List<MoveAbleComponent> crayons = new List<MoveAbleComponent>();
        protected List<BasicComponent> floorToysB, floorToysI, floorToys;
        private bool somethingMoving = false;
        GameGUI talkingBubble;
        float basketSpeed = 0.1f;
      protected  int basketAccel, speed = 1;
        protected Accelerometer accelSensor;
        protected Vector3 accelReading = new Vector3();
        protected SoundEffect backgroundMusic;

        GraphicsDeviceManager graphics;
        float rotationDirection;

       protected bool onTopOfBasket = false, leftOfBasket = false, rightOfBasket = false;

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




        public MasterScene(Game game, ContentManager content, GameComponentCollection components)
            : base(game)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene3");
            if(gameStorage.fileExists(gameStorage.FileName)==false)
                 gameStorage.saveScore(0, 0);
           
            // TODO: Construct any child components here
        }

        protected void accelSensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            accelReading.X = (float)e.X;
            accelReading.Y = (float)e.Y;
            accelReading.Z = (float)e.Z;
        }


        
        protected void basketRoll()
        {

            if (this.Game.Window.CurrentOrientation.ToString() == "LandscapeLeft")
            {
                rollDirection = -20;
                if (basketLeft.CompRectX >= -50 && accelReading.Y > 0.0f)
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
                if (basketLeft.CompRectX <= 590 && accelReading.Y < 0.0f)
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
            }
            else
            {
                rollDirection = 20;
                if (basketLeft.CompRectX <= 590 && accelReading.Y > 0.0f)
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
                if (basketLeft.CompRectX >= -50 && accelReading.Y < 0.0f)
                    basketLeft.CompRectX = basketLeft.CompRectX + (int)(accelReading.Y * rollDirection);
            }

            basketOpening.CompRectX = basketLeft.CompRectX + 15;
            bskHit.X = basketLeft.CompRectX;
            basketOpening.ComponentColor = basketLeft.ComponentColor;

        }
       
           
            
     
        protected override void UnloadContent()
        {

            Components.Remove(roomBackground);
            Components.Remove(gameGUI);
            Components.Remove(boy);
       
            Components.Add(basketOpening);
            Components.Remove(toy);
            Components.Remove(basketLeft);
            foreach(MoveAbleComponent mc in crayons)
                Components.Remove(mc);
            base.UnloadContent();
        }

        private void arrangeCrayons()
        {
            foreach (MoveAbleComponent mc in crayons)
            {
                if (mc.ItemMoving == false)
                {
                    mc.ItemTouched = false;
                    mc.resetToStart();
                }
                else
                {
                    toy.ItemTouched = false;
                }
                if (mc.ComponentRectangle.Intersects(bskHit))
                    basketLeft.ComponentColor = mc.ComponentColor;
                
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
            if ((y + tmpRect.Height) < (mannRect.Y + mannRect.Height))
            {
                floorToysB.Add(tmp);
                return tmp;
            }
            else
            {

                floorToysI.Add(tmp);
                return tmp;
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






    }
}
