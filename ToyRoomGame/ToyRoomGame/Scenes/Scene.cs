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
using System.Diagnostics;

namespace WindowsPhoneGame1.Scenes
{

  
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Scene : Microsoft.Xna.Framework.DrawableGameComponent
    {
             
        SpriteBatch spriteBatch;
        Texture2D mannTexture, mannTexture2,  basketTxt, crayonTxt;
        Rectangle mannRect,  basketRightRect, basketLeftRect, defaultRect, planeRect, crayonRect;

        List<Color> colorList = new List<Color>();
        List<Texture2D> textures = new List<Texture2D>();
        Color rightBsktColor = Color.Red, leftBsktColor = Color.White, itemColor = Color.Wheat;
        Random rnd;
        bool itemPlaced = false, firstRun = true;
        int trigger = 120, timer = 0;
        MoveAbleComponent toy;
        List<MoveAbleComponent> crayons = new List<MoveAbleComponent>();
        BasicComponent boy, basketLeft, basketRight;
        ContentManager Content;
        GameComponentCollection Components;
       List<Rectangle> rectangles = new List<Rectangle>();
     

        

        public Scene(Game game, ContentManager content, GameComponentCollection components)
            : base(game)
        {
            Components = components;
            Content =  content;
            colorList = GameTools.elementColors();
            // TODO: Construct any child components here
        }
       

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //default toy pos and size settings
           

            // TODO: Add your initialization code here
            crayonRect = new Rectangle(0, 0, 50, 50);
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            mannRect = new Rectangle(330, 80, 200, 400);
            planeRect = new Rectangle(200, 90, 200, 100);
            basketLeftRect = new Rectangle(0, 350, 200, 100);
            basketRightRect = new Rectangle(600, 350, 200,100);
            defaultRect = new Rectangle(320,200,100,90);
           //default settings

            rectangles.Add(defaultRect);
            rectangles.Add(new Rectangle(320, 200, 400, 120));
            rectangles.Add(defaultRect);
            rectangles.Add(new Rectangle(320, 200, 400, 120));
            rectangles.Add(new Rectangle(320, 200, 400, 120));
            rectangles.Add(new Rectangle(320, 200, 400, 120));
            rectangles.Add(new Rectangle(320, 200, 400, 120));
            rectangles.Add(new Rectangle(320,200, 400,120));
          

            rnd = new Random();
           
            base.Initialize();
        }
        private void arrangeCrayons () {
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
                if (mc.ComponentRectangle.Intersects(basketLeftRect))
                    basketLeft.ComponentColor = mc.ComponentColor;
                if (mc.ComponentRectangle.Intersects(basketRightRect))
                    basketRight.ComponentColor = mc.ComponentColor;
            }
        }

        protected override void LoadContent()
        {

            mannTexture = Content.Load<Texture2D>("Images\\theBoy");
            mannTexture2 = Content.Load<Texture2D>("Images\\theBoyShowing");
          
            
            textures.Add(Content.Load<Texture2D>("Images\\sportscar"));
            textures.Add(Content.Load<Texture2D>("Images\\theplane"));
            textures.Add(Content.Load<Texture2D>("Images\\ball"));
            textures.Add(Content.Load<Texture2D>("Images\\train"));
            textures.Add(Content.Load<Texture2D>("Images\\block"));
            textures.Add(Content.Load<Texture2D>("Images\\tractor"));
            textures.Add(Content.Load<Texture2D>("Images\\balloon"));




           
            
            crayonTxt = Content.Load<Texture2D>("Images\\crayon");           
            basketTxt = Content.Load<Texture2D>("Images\\wheelBasket");

            for (int i = 0; i < 5; i++)
            {
                MoveAbleComponent crayonTemp = new MoveAbleComponent(this.Game, crayonTxt, crayonRect, colorList[i]);
                crayonTemp.ItemDraw = true;
                crayonRect.X += 55;
                crayons.Add(crayonTemp);
                Components.Add(crayonTemp);
            }
          
            boy = new BasicComponent(this.Game, mannTexture, mannRect);
            basketLeft = new BasicComponent(this.Game, basketTxt, basketLeftRect);
            basketRight = new BasicComponent(this.Game, basketTxt, basketRightRect);
            toy = new MoveAbleComponent(this.Game, textures[0], defaultRect, Color.Orange);
           
            
            Components.Add(boy);
            Components.Add(basketLeft);
            Components.Add(basketRight);
            Components.Add(toy);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timer++;
           
                arrangeCrayons();

          

            if (timer == trigger)
            {
                
              
                if (itemPlaced == true || firstRun == true)
                {
                    toy.ItemTouched = false;
                    toy.CompRectX = defaultRect.X ;
                    toy.CompRectY = defaultRect.Y;        
                    firstRun = false;
                    toy.ItemDraw = true;
                 
                    int nextTexture = rnd.Next(0, textures.Count);


                    int nextColor = rnd.Next(0, 8);
                    int nextWrongColor = nextColor;
                    while (nextWrongColor == nextColor)
                        nextWrongColor = rnd.Next(0, 8);
                    int i = rnd.Next(1, 3);
                    int chooseCrayon = rnd.Next(0, 5);
                    switch (i)
                    {
                        case 1:
                          //  basketLeft.ComponentColor = colorList[nextWrongColor];
                            //basketRight.ComponentColor = colorList[nextColor];
                            toy.ComponentColor = colorList[nextColor];
                            crayons[chooseCrayon].ComponentColor = colorList[nextColor];

                            break;
                        case 2:
                           // basketLeft.ComponentColor = colorList[nextColor];
                           // basketRight.ComponentColor = colorList[nextWrongColor];
                            toy.ComponentColor = colorList[nextColor];
                            crayons[chooseCrayon].ComponentColor = colorList[nextColor];

                            break;
                        default:

                            break;
                    }


                    toy.ComponentTexture = textures[nextTexture];
                    toy.ComponentRectangle = rectangles[nextTexture];
                    boy.ComponentTexture = mannTexture2;
                }

            }
            if (toy.ItemTaken == true)
                boy.ComponentTexture = mannTexture;
            if (toy.ComponentRectangle.Intersects(basketLeft.ComponentRectangle))
            {
                if (toy.ComponentColor == basketLeft.ComponentColor)
                    Debug.WriteLine("riktig");
                else
                    Debug.WriteLine("feil");
                itemPlaced = true;
                toy.ItemDraw = false;
                toy.CompRectX = -10000;
               
               
                timer = 0;
            }
            if (toy.ComponentRectangle.Intersects(basketRight.ComponentRectangle))
            {
                if (toy.ComponentColor == basketRight.ComponentColor)
                    Debug.WriteLine("riktig");
                else
                    Debug.WriteLine("feil");
                itemPlaced = true;
                toy.ItemDraw = false;
                toy.CompRectX = -10000;
                     
                timer = 0;
            }

         

            base.Update(gameTime);
        }


     

        
    }
}
