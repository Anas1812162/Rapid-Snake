﻿ using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WMPLib;

namespace Snake
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();
            player.URL = "Stridin.mp3";

                        new Settings();

            //Set game speed and start timer
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
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);


            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle {X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos)};
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            
            if (Settings.GameOver)
            {
              
                if (Input.KeyPressed(Keys.Enter))  //Check if Enter is pressed
                {
                    StartGame();
                }
            }
            else
            {                            //controls
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //Set colour of snake

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Goldenrod;     //Draw head
                    else
                        snakeColour = Brushes.Green;    //Rest of body

                    
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    //Draw Food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
               
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    
                    if (Snake[i].X < 0 || Snake[i].Y < 0    //Detect collission with game borders.
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    
                    for (int j = 1; j < Snake.Count; j++)   //Detect collission with body
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)   //Detect collision with food piece
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {

            Circle circle = new Circle();
            
            Snake.Add(circle);

          
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is our first game we thought to remake the famous retro snake game" + "v 1.0" + "Enjoy it!");
        }

        private void CreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Remade by Anas Rana and Mahad Ahmed");
        }

        private void ExitGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.controls.play();
        }

        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.controls.pause();
        }
    }
}