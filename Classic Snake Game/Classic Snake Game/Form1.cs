using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; //can store jpg files !
using System.Media;
namespace Classic_Snake_Game
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        private Circle food1 = new Circle();
        private Circle food2 = new Circle();
        
        int maxWidth;
        int maxHeight;
        int score;
        int highScore;
        Random numGen = new Random();
        bool goLeft, goRight, goUp, goDown;
        
        public Form1()
        {
            InitializeComponent();
            new Settings();
            
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.A && Settings.directions != "right" && Settings.directions != "d")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D && Settings.directions != "left" && Settings.directions != "a")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W && Settings.directions != "down" && Settings.directions != "s")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S && Settings.directions != "up" && Settings.directions != "w")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored " + score + " and my Highest score is " + highScore + " on the Snake Game!";
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.LightBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width,height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height)); // point location of the image. Where the image to start from.
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            //setting the directions
            if(goLeft )
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            //end of directions.

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch(Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                    }
                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;

                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;

                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    if (Snake[i].X == food1.X && Snake[i].Y == food1.Y && score >= 40)
                    {
                        EatFood1();
                    }

                    if (Snake[i].X == food2.X && Snake[i].Y == food2.Y && score >= 75)
                    {
                        EatFood2();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }
                    }



                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
            picCanvas.Invalidate();
        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColor;
            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)//head of the snake is numbered as 0. Thats the number order when we use lists. That means snake's head color will be black
                {
                    snakeColor = Brushes.Black;
                }
                else
                {
                    snakeColor = Brushes.DarkGreen; //And here in this line of code everything else except the head will be dark green color which means snake's body.
                }
                canvas.FillEllipse(snakeColor, new Rectangle
                    (
                    Snake[i].X * Settings.Width,
                    Snake[i].Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));

                
            } //outside the loop
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
                    (
                    food.X * Settings.Width,
                    food.Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            if(score >= 40)
            { 
            canvas.FillEllipse(Brushes.Indigo, new Rectangle
                    (
                    food1.X * Settings.Width,
                    food1.Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
            }
            if (score >= 100)
            {

            
            canvas.FillEllipse(Brushes.Yellow, new Rectangle
                    (
                    food2.X * Settings.Width,
                    food2.Y * Settings.Height,
                    Settings.Width, Settings.Height
                    ));
        }
    }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear();
            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head); //adding the head part of the snake to the list.

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle {X = numGen.Next(2, maxWidth ), Y = numGen.Next(2, maxHeight)};
            food1 = new Circle { X = numGen.Next(2, maxWidth), Y = numGen.Next(2, maxHeight) };
            food2 = new Circle { X = numGen.Next(2, maxWidth), Y = numGen.Next(2, maxHeight) };
            gameTimer.Start();
        }

        private void EatFood()
        {
            score += 10;
            //SoundPlayer _music = new SoundPlayer(@"C:\music\Scoring.wav");
            //_music.Play();
            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            food = new Circle { X = numGen.Next(2, maxWidth), Y = numGen.Next(2, maxHeight) };
            
        }

        private void EatFood1()
        {
            score += 15;
            //SoundPlayer _music = new SoundPlayer(@"C:\music\Scoring.wav");
            //_music.Play();

            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            food1 = new Circle { X = numGen.Next(2, maxWidth), Y = numGen.Next(2, maxHeight) };

        }

        private void EatFood2()
        {
            score += 20;
            //SoundPlayer _music = new SoundPlayer(@"C:\music\Scoring.wav");
            //_music.Play();
            txtScore.Text = "Score: " + score;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(body);

            food2 = new Circle { X = numGen.Next(2, maxWidth), Y = numGen.Next(2, maxHeight) };
        }

            private void GameOver()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;

            if(score > highScore)
            {
                highScore = score;

                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
