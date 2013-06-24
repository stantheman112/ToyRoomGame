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
    public class Scene2 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region private variables
        GameData gameStorage;
        SpriteBatch spriteBatch;
        Texture2D mannTexture, mannTexture2, mannTexture3, basketTxt, roomTexture, guiBubbleTxt, basketOpeningTxt;
        Rectangle mannRect, mannRect2, mannRect3, basketLeftRect, basketRightRect, defaultRect, roomRect, guiBubbleRct, basketOpeningLRct, basketOpeningRRct,  bskHitL, bskHitR;
        List<BasicComponent> floorToysB, floorToysI, floorToys;
        GameGUI gameGUI;
        List<Color> colorList = new List<Color>();
        List<Texture2D> textures = new List<Texture2D>();
        Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat, rightColor, wrongColor, tmpColor, floorToyColor;
        Random rnd;
        bool itemPlaced = false, firstRun = true, sceneCompleted=false;
        int trigger = 90, timer = 0, toysCased=0, numberOfTurns=3,  maxScore=20;
        MoveAbleComponent toy;
        SpriteFont spriteFont;
        BasicComponent boy, basketLeft, basketRight, basketOpeningRight, basketOpeningLeft, roomBackground;
        ContentManager Content;
        GameComponentCollection Components;
        List<Rectangle> rectangles = new List<Rectangle>();
        GameGUI talkingBubble;
        Accelerometer accelSensor;
        Vector3 accelReading = new Vector3();


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




        public Scene2(Game game, ContentManager content, GameComponentCollection components)
            : base(game)
        {
            Components = components;
            Content = content;
            colorList = GameTools.elementColors();
            gameStorage = new GameData("scene2");
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
          

            // TODO: Add your initialization code here

            // TODO: Add your initialization code here
            floorToysB = new List<BasicComponent>();
            floorToysI = new List<BasicComponent>();
            floorToys = new List<BasicComponent>();
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(345, 10, 185, 370);
            mannRect2 = new Rectangle(350, 10, 180, 370);
            mannRect3 = new Rectangle(315, 10, 215, 370);
            guiBubbleRct = new Rectangle(10, 2, 400, 500);
         
            roomRect = new Rectangle(-170, -80, 1200, 620);

            //default toy størrelse
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
            rectangles.Add(new Rectangle(310, 213, 150, 150));//teddy
            rectangles.Add(new Rectangle(310, 225, 70, 70)); //block
            rectangles.Add(new Rectangle(310, 190, 100, 100));//ball
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

            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
            mannTexture3 = Content.Load<Texture2D>("Images\\boytup");
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

            boy = new BasicComponent(this.Game, mannTexture, mannRect, 0f);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect, 0.0f);
            basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect, 0.0f);
            basketOpeningLeft = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningLRct, 0.0f);
            basketOpeningRight = new BasicComponent(this.Game, basketOpeningTxt, basketOpeningRRct, 0.0f);
            roomBackground = new BasicComponent(this.Game, roomTexture, roomRect, Color.White, 0.0f);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.Orange, 0.0f);
            gameGUI = new GameGUI(this.Game, spriteFont, new Vector2(10,10), "0/"+textures.Count, Color.Black);
            gameGUI.MaxScore = textures.Count;
           

            for (int i = 0; i < textures.Count; i++)
            {
                int x = rnd.Next(150, 750);
                int y = rnd.Next(250, 400);
           
                double rotation = rnd.NextDouble();
                Rectangle tmpRect = new Rectangle();
                tmpRect = rectangles[i];
                tmpRect.X = x;
                tmpRect.Y = y;

               
                GameTools.randomColor(ref floorToyColor, 255);
                tmpColor = floorToyColor;
                while (floorToyColor == tmpColor)
                {
                    GameTools.randomColor(ref floorToyColor, 255);
                }
                GameTools.randomColor(ref tmpColor, 255);
                BasicComponent tmp = new BasicComponent(this.Game, textures[i], tmpRect, floorToyColor, (float)rotation);
                tmp.ComponentType = "toy" + Convert.ToString(i);
                floorToys.Add(tmp);
                if ((y + tmpRect.Height) < (mannRect.Y + mannRect.Height))
                    floorToysB.Add(tmp);
                else
                    floorToysI.Add(tmp);

            }

            Components.Add(roomBackground);
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
        protected override void UnloadContent()
        {

            Components.Remove(roomBackground);
            Components.Remove(gameGUI);
            Components.Remove(boy);
            
            Components.Remove(toy);
            Components.Remove(basketLeft);
            Components.Remove(basketRight);
          
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            basketOpeningLeft.ComponentColor = basketLeft.ComponentColor;
            basketOpeningRight.ComponentColor = basketRight.ComponentColor;
            if (toysCased < numberOfTurns)
            {
            }
            else {
                if (gameStorage.getProgression("lobby") < 2)
                    gameStorage.saveProgression(2);
                this.UnloadContent();
                sceneCompleted = true; 
            }
            timer++;         

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

                  
                    toy.ComponentTexture = textures[nextTexture];
                    toy.ComponentRectangle = rectangles[nextTexture];
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





                }

            }
            if (toy.ItemTaken == true && boy.ComponentTexture != mannTexture3)
            {
                boy.ComponentTexture = mannTexture;
                boy.ComponentRectangle = mannRect;

            }
            if (toy.ComponentRectangle.Intersects(bskHitL))
            {
                toysCased++;
                if (toy.ComponentColor == basketLeft.ComponentColor)
                {
                    gameGUI.CurrentScore = gameGUI.CurrentScore+1;
                    gameGUI.InfoText = gameGUI.CurrentScore.ToString() + " / " + gameGUI.MaxScore.ToString();

                    boy.ComponentTexture = mannTexture3;
                    boy.ComponentRectangle = mannRect3;
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
            if (toy.ComponentRectangle.Intersects(bskHitR))
            {
                toysCased++;
                if (toy.ComponentColor == basketRight.ComponentColor)
                {
                    gameGUI.CurrentScore = gameGUI.CurrentScore + 1;
                    gameGUI.InfoText = gameGUI.CurrentScore.ToString() + " / " + gameGUI.MaxScore.ToString();

                    boy.ComponentTexture = mannTexture3;
                    boy.ComponentRectangle = mannRect3;
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
