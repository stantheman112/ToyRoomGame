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
    public class Scene6 : MasterScene
    {
        #region private variables
        GameData gameStorage;
        SpriteBatch spriteBatch;
        Texture2D mannTexture, mannTexture2, mannTexture3, boyDissapointedTxt, loadScreen, basketTxt, roomTexture, crayonTxt, guiBubbleTxt, basketOpeningTxt;
        Rectangle mannRect, mannRect2, mannRect3, bskHit, basketLeftRect, basketOpeningRct, roomRect, crayonRect, guiBubbleRct, defaultRect;
        GameGUI gameGUI;
        List<Color> colorList = new List<Color>();
        List<Texture2D> textures = new List<Texture2D>();
        Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat, rightColor, wrongColor, tmpColor, floorToyColor;
        Random rnd;
        bool itemPlaced = false, firstRun = true, sceneCompleted=false;
        int trigger = 90, timer = 0, toysCased = 0, numberOfTurns, maxScore = 20, newRand, rollDirection;
        MoveAbleComponent toy;
        SpriteFont spriteFont;
        BasicComponent boy, basketLeft, basketOpening,  roomBackground;
        ContentManager Content;
        GameComponentCollection Components;
        List<Rectangle> rectangles = new List<Rectangle>();
        List<MoveAbleComponent> crayons = new List<MoveAbleComponent>();
        List<BasicComponent> floorToysB, floorToysI, floorToys;
        private bool somethingMoving = false;
        GameGUI talkingBubble;
        Accelerometer accelSensor;
        Vector3 accelReading = new Vector3();
        List<Texture2D> newTextures;
        List<Rectangle> newRectangles;
        float rotationDirection = -1f;

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




        public Scene6(Game game, ContentManager content, GameComponentCollection components)
            : base(game, content, components)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene6");
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
            accelSensor = new Accelerometer();
            accelSensor.Start();
            accelSensor.ReadingChanged += accelSensor_ReadingChanged;
            bskHit = new Rectangle(35, 390, 180, 80); 
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();
           
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(545, 80, 185, 370);
            mannRect2 = new Rectangle(550, 80, 180, 370);
            mannRect3 = new Rectangle(515, 80, 215, 370);
            crayonRect = new Rectangle(0, 430, 200, 70);
            basketLeftRect = new Rectangle(0, 320, 250, 150);
            basketOpeningRct = new Rectangle(25, 305, 250, 150);
        
            roomRect = new Rectangle(-30, -20, 900, 600);
            guiBubbleRct = new Rectangle(4, 31, 590, 410);


            //default toy størrelse
            defaultRect = new Rectangle(510, 200, 100, 90);
          
            rightColor = new Color();
            wrongColor = new Color();

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
            rectangles.Add(new Rectangle(510, 150, 120, 120)); //horse
            rectangles.Add(new Rectangle(510, 200, 120, 90)); //racecar
            newRectangles = new List<Rectangle>();
            newRectangles.AddRange(rectangles);
            newTextures = new List<Texture2D>();
          

            numberOfTurns = rectangles.Count;
          
            rnd = new Random();

            base.Initialize();
        }

          private void loadScreenDraw(Texture2D text2d, Vector2 vect2)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

          
            spriteBatch.Begin();

            spriteBatch.Draw(text2d, vect2, Color.White);

            spriteBatch.End();



            GraphicsDevice.Present();


        }


      
            

        protected override void LoadContent()
        {
            loadScreen = Content.Load<Texture2D>("Images\\gui\\loadscreen");
            loadScreenDraw(loadScreen, new Vector2(-90, -70));
      
            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            roomTexture = Content.Load<Texture2D>("Images\\theroom");
            crayonTxt = Content.Load<Texture2D>("Images\\bigcrayonside");

            guiBubbleTxt = Content.Load<Texture2D>("Images\\talkingBubbleLeft");


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
            newTextures.AddRange(textures);
            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket1");
            basketOpeningTxt = Content.Load<Texture2D>("Images\\wheelBasket2");

            spriteFont = Content.Load<SpriteFont>("sf20");
            talkingBubble = new GameGUI(this.Game, spriteFont, new Vector2(70, 100), "Kasper! \n the toycolors have gone \n crazy again! " +
               "they will \n only go  into  the basket\n  if the basket has the \n same color as themselves! \n You must color the basket \n in  the right color! \n Hurry or more toys will come!", Color.Black, 0f, new Vector2(0, 0), 1f, 0f);
            talkingBubble.GuiBackgroundRect = guiBubbleRct;
            talkingBubble.GuiBackgroundTxt = guiBubbleTxt;
            talkingBubble.GuiVisible = true;
            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0.0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
           //basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect, 0.0f);
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], rectangles[0], Color.Orange, 0.0f);
            gameGUI = new GameGUI(this.Game, spriteFont, new Vector2(10,50), "0/"+numberOfTurns.ToString(), Color.Black);
            gameGUI.MaxScore = rectangles.Count;
            Components.Add(roomBackground);

            for (int i = 0; i < 4; i++)
            {
                MoveAbleComponent crayonTemp = new MoveAbleComponent(this.Game, crayonTxt, crayonRect, colorList[i], 0.0f);
                crayonTemp.ItemDraw = true;
                crayonRect.X += 200;
                crayons.Add(crayonTemp);
                Components.Add(crayonTemp);
            }

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

            Components.Add(basketOpening);   
            Components.Add(toy);
            Components.Add(basketLeft); 
            Components.Add(talkingBubble);

            base.LoadContent();
        }
        protected override void UnloadContent()
        {

            Components.Remove(roomBackground);
            Components.Remove(gameGUI);
            Components.Remove(boy);
            Components.Remove(basketLeft);
          //  Components.Remove(basketRight);
            Components.Remove(toy);
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
        private BasicComponent addFloorToy(int compNumber, int toyNumber)
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
            if (somethingMoving == true)
            {
                bool chkMoving = false;
                if (toy.ItemMoving == true)
                    chkMoving = true;

                for (int u = 0; u < crayons.Count; u++)
                {
                    if (crayons[u].ItemMoving == true)
                        chkMoving = true;


                }
                if (chkMoving == false)
                {
                    for (int i = 0; i < crayons.Count; i++)
                    {
                        crayons[i].MovingAllowed = true;
                    }
                    toy.MovingAllowed = true;


                }
            }

            for (int i = 0; i < crayons.Count; i++)
            {
                if (crayons[i].ItemMoving == true)
                {
                    somethingMoving = true;
                    toy.MovingAllowed = false;
                    toy.CompRectY = 250;
                    for (int y = 0; y < crayons.Count; y++)
                    {
                        if (y != i)
                            crayons[y].MovingAllowed = false;
                    }
                }
            }
          
            if (toy.ItemMoving)
            {
                 somethingMoving = true;
                for (int i = 0; i < crayons.Count; i++)
                {
                    crayons[i].MovingAllowed = false;
                }
            }
           
            arrangeCrayons();
            if (toysCased < numberOfTurns)
            {
            }
            else {
                if (gameStorage.getProgression("lobby") < 6)
                    gameStorage.saveProgression(6);
                sceneCompleted = true; 
            }
            timer++;
            if (firstRun == false && timer % 200 == 0)
            {
                newRand = rnd.Next(0, newTextures.Count - 1);
                Components.Add(addFloorToy(newRand, floorToys.Count));

            }


            if (timer == trigger)
            {
                if (talkingBubble.GuiVisible == true)
                {
                    talkingBubble.GuiVisible = false;
                    Components.Remove(talkingBubble);
                }


                if (itemPlaced == true || firstRun == true)
                {
                    toy.ItemTouched = false;
                    toy.CompRectX = defaultRect.X;
                    toy.CompRectY = defaultRect.Y;
                    firstRun = false;
                    toy.ItemDraw = true;

                    int nextTexture = rnd.Next(0, textures.Count);


                    int nextColor = rnd.Next(0, 8);
                    int nextWrongColor = nextColor;
                    while (nextWrongColor == nextColor)
                        nextWrongColor = rnd.Next(0, 8);
                    int i = rnd.Next(1, 3);
                     GameTools.randomColor(ref rightColor,255);                 
                      GameTools.randomColor(ref wrongColor,255);
                    while(rightColor==wrongColor)
                        GameTools.randomColor(ref wrongColor, 255);
              
                    int chooseCrayon = rnd.Next(0, crayons.Count-1);
                  
                  
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

                               //     textures.Remove(textures[nextTexture]);
                                //    rectangles.Remove(rectangles[nextTexture]);

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
            if (toy.ComponentRectangle.Intersects(basketLeft.ComponentRectangle))
            {
                toysCased++;
                if (toy.ComponentColor == basketLeft.ComponentColor)
                {
                    gameGUI.CurrentScore = gameGUI.CurrentScore+1;
                    gameGUI.InfoText = gameGUI.CurrentScore.ToString() + "/" + maxScore.ToString();


                    boy.ComponentTexture = mannTexture3;
                    boy.ComponentRectangle = mannRect3;
                }
                else
                {

                    newRand = rnd.Next(0, newTextures.Count - 1);
                    Components.Add(addFloorToy(newRand, floorToys.Count));
                    boy.ComponentTexture = boyDissapointedTxt;
                    boy.ComponentRectangle = mannRect3;
                    newRand = rnd.Next(0, newTextures.Count - 1);
                    Components.Add(addFloorToy(newRand, floorToys.Count));
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
