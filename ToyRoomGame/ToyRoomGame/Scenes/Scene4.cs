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
    public class Scene4 : MasterScene
    {
        #region private variables
       GameData gameStorage;
        //SpriteBatch spriteBatch;
        //Texture2D loadScreen;        
        //Texture2D mannTexture, mannTexture2, mannTexture3, basketLeftTxt, roomTexture, guiBubbleTxt;
        //Rectangle mannRect, mannRect2, mannRect3,  basketLeftRect, defaultRect, roomRect, guiBubbleRct;
        //List<BasicComponent> floorToysB, floorToysI, floorToys;
        //GameGUI talkingBubble;
        //List<Color> colorList = new List<Color>();
        //List<Texture2D> textures = new List<Texture2D>();
        //Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat;
        //Random rnd;
        //bool itemPlaced = false, firstRun = true, sceneCompleted=false;
        //int trigger = 90, timer = 0, toysCased=0, numberOfTurns,  nextTexture, newRand, rollDirection;
        //MoveAbleComponent toy;
        //SpriteFont spriteFont;
        //BasicComponent boy, basketLeft,  roomBackground;
           ContentManager Content;
       GameComponentCollection Components;
        //List<Rectangle> rectangles = new List<Rectangle>();
        //List<int> toyTidiedUp;
        //SoundEffect backgroundMusic;
        //float basketSpeed = 0.1f;
        //int basketAccel, speed = 1;
        //Accelerometer accelSensor;
        //Vector3 accelReading = new Vector3();
        //List<Texture2D> newTextures;
        //List<Rectangle> newRectangles;



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




        public Scene4(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene4");
            if(gameStorage.fileExists(gameStorage.FileName)==false)
                 gameStorage.saveScore(0, 0);
          //  toyTidiedUp = new List<int>();
            // TODO: Construct any child components here
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

            // TODO: Add your initialization code here
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(545, 80, 185, 370);
            mannRect2 = new Rectangle(550, 80, 180, 370);
            mannRect3 = new Rectangle(515, 80, 215, 370);
            guiBubbleRct = new Rectangle(210,-1, 400, 300);
           
            basketLeftRect = new Rectangle(0, 320, 250, 150);
        
            roomRect = new Rectangle(-170, -20, 1140, 620);
           
            
            //default toy størrelse
            defaultRect = new Rectangle(510, 200, 100, 90);

            rectangles.Add(new Rectangle(510, 230, 120, 90)); //sportscar
            rectangles.Add(new Rectangle(530, 200, 130, 120)); //plane
            rectangles.Add(new Rectangle(510, 220, 130, 90));//train
            rectangles.Add(new Rectangle(510, 240, 120, 90)); //tractor
            rectangles.Add(new Rectangle(510, 220, 120, 120)); //digger
            rectangles.Add(new Rectangle(510, 200, 130, 130)); //dino
            rectangles.Add(new Rectangle(505, 210, 120, 120)); //baby
            rectangles.Add(new Rectangle(505, 180, 120, 120)); //soldier
            rectangles.Add(new Rectangle(505, 230, 100, 90));//actionfigure
            rectangles.Add(new Rectangle(505, 230, 120, 120));//teddy
            rectangles.Add(new Rectangle(515, 245, 70, 70)); //block
            rectangles.Add(new Rectangle(505, 210, 100, 100));//ball
            rectangles.Add(new Rectangle(520, 180, 100, 150)); //balloon
            rectangles.Add(new Rectangle(530, 200, 130, 120)); //pad
            rectangles.Add(new Rectangle(310, 150, 120, 120)); //horse
            rectangles.Add(new Rectangle(310, 200, 120, 90)); //racecar

           
            rnd = new Random();

            numberOfTurns = rectangles.Count - 1;
            base.Initialize();
        }

      
        private BasicComponent addFloorToy(Rectangle rect, Texture2D text, int toyNumber)
        {
            int x = rnd.Next(150, 750);
            int y = rnd.Next(250, 400);

            //string rot = // Convert.ToString(rnd.Next(0, 2)) + "." + Convert.ToString(rnd.Next(0, 9));

            double rotation = rnd.NextDouble();
            Rectangle tmpRect = new Rectangle();
            tmpRect = rect;
            tmpRect.X = x;
            tmpRect.Y = y;

            BasicComponent tmp = new BasicComponent(this.Game, text, tmpRect, (float)rotation);
            tmp.ComponentType = "toy" + Convert.ToString(toyNumber);
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
      

        protected override void LoadContent()
        {


            backgroundMusic = Content.Load<SoundEffect>("ToyRoom");

            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            guiBubbleTxt = Content.Load<Texture2D>("Images\\tlkBubble");      
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
            textures.Add(Content.Load<Texture2D>("Images\\colored\\pad"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\horse"));
            textures.Add(Content.Load<Texture2D>("Images\\colored\\racecar"));
        
            basketLeftTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");
      
            spriteFont = Content.Load<SpriteFont>("sf20");
            spriteFont.LineSpacing = 25;
            spriteFont.Spacing = 1;

            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0f);
            basketLeft = new BasicComponent(this.Game, basketLeftTxt, basketLeftRect, Color.DarkOrange, 0.0f);
            basketOpening = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRct, Color.LightBlue, 0.0f);
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.White, 0.0f);
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(220, 120), "Hello friend \n will you help  me \n clean my room?", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
          talkingBubble.GuiBackgroundRect = guiBubbleRct;
          talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
          talkingBubble.GuiVisible = true;
           // gameGUI.MaxScore = 20;


            Components.Add(roomBackground);
           // Components.Add(gameGUI);
            for (int i = 0; i < textures.Count; i++)
            {
                addFloorToy(rectangles[i], textures[i], i);

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
          
          Components.Add(basketLeft);
           
            Components.Add(toy);
            Components.Add(talkingBubble);
            numberOfTurns = textures.Count;
            base.LoadContent();
        }
        protected override void UnloadContent()
        {

            Components.Remove(roomBackground);
           // Components.Remove(gameGUI);
           
            Components.Remove(boy);

            Components.Remove(basketLeft);
        
            Components.Remove(toy);
          
            base.UnloadContent();
        }

        private void basketRoll()
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



        }

        public override void Update(GameTime gameTime)
        {

           
            basketRoll();
            if (floorToys.Count==0)
            {
              //  gameStorage.saveScore(gameGUI.CurrentScore, gameGUI.MaxScore);
                if(gameStorage.getProgression("lobby")<4)
                   gameStorage.saveProgression(4);
                this.UnloadContent();
                sceneCompleted = true; 
            }
            timer++;
            if (firstRun == false && timer%200==0)
            {
                newRand = rnd.Next(0, textures.Count - 1);
                Components.Add(addFloorToy(newRand,  floorToys.Count));
               
            }

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


                    nextTexture = rnd.Next(0, textures.Count - 1);


                }
                //else
                //{
                //    if (firstRun == true)
                //    {
                //       Components.Add(addFloorToy(newRectangles[nextTexture], newTextures[nextTexture], floorToys.Count));
                //       nextTexture = rnd.Next(0, textures.Count - 1);
                //    }
                //}
                    

                    toy.ComponentTexture = textures[nextTexture];
                    toy.ComponentRectangle = rectangles[nextTexture];

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

                                    textures.Remove(textures[nextTexture]);
                                    rectangles.Remove(rectangles[nextTexture]);
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

                                        textures.Remove(textures[nextTexture]);
                                        rectangles.Remove(rectangles[nextTexture]);
                                       

                                    }

                                }
                            }
                    }


                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);

                    }
            

                    boy.ComponentTexture = mannTexture2;
                    boy.ComponentRectangle = mannRect2;


                

            }
            if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3)
            {
                boy.ComponentTexture = mannTexture;
                boy.ComponentRectangle = mannRect;
                
            }
            if (toy.ComponentRectangle.Intersects(basketLeft.ComponentRectangle))
            {
                toysCased++;
                boy.ComponentTexture = mannTexture3;
                boy.ComponentRectangle = mannRect3;
                if (nextTexture<6)
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
          

            base.Update(gameTime);
        }





    }
}
