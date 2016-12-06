using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();
            new Settings();
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            
            StartGame();
        }
        private void StartGame()
        {
            lblGameOver.Visible = false;
            new Settings();
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random rand = new Random();
            food = new Circle();
            food.X = rand.Next(0, maxXPos);
            food.Y = rand.Next(0, maxYPos);
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver==true)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right)&&Settings.direction!=Direction.Left)
                {
                    Settings.direction = Direction.Right;
                }
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                {
                    Settings.direction = Direction.Left;
                }
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                {
                    Settings.direction = Direction.Up;
                }
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                {
                    Settings.direction = Direction.Down;
                }


                MovePlayer();

           }
            pbCanvas.Invalidate();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (Settings.GameOver==false)
            {
                Brush snakeColor;
                for (int i = 0; i < Snake.Count; i++)
                {
                    //if (i == 0)
                    //{
                    //    snakeColor = Brushes.Fuchsia;
                    //}
                    //else
                    //{
                    //    snakeColor = Brushes.Gainsboro;
                    //}
                    if (i == 0)
                    {
                        snakeColor = Brushes.DarkGreen;
                    }
                    else if (i % 2 == 0)
                    {
                        snakeColor = Brushes.DarkOliveGreen;
                    }
                    else if (i % 3 == 0)
                    {
                        snakeColor = Brushes.DarkKhaki;
                    }
                    else if (i % 5 == 0)
                    {
                        snakeColor = Brushes.Olive;
                    }
                    else if (i % 7 == 0)
                    {
                        snakeColor = Brushes.ForestGreen;
                    }
                    else
                    {
                        snakeColor = Brushes.OliveDrab;
                    }
                
              //  }
                    //Snake
                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].X * Settings.Width,
                                       Snake[i].Y * Settings.Height,
                                       Settings.Width, Settings.Height));
                    //Food
                    canvas.FillEllipse(Brushes.Aquamarine, 
                        new Rectangle(food.X * Settings.Width, 
                        food.Y * Settings.Height,
                        Settings.Width, Settings.Height));

                }
            }
            else
            {
                lblGameOver.Text = String.Format("Game Over \n Your final score is: {0} \nPress Enter to try again",Settings.Score);
                lblGameOver.Visible = true;
            }
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count-1; i >= 0; i--)
            {
                if (i==0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        //default:
                        //    break;
                    }

                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].X<0||Snake[i].Y<0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }
                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        } 


        private void Eat()
        {
            GenerateFood();
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(food);
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

       
    }
}
