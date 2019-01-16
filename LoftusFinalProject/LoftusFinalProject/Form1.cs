using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//Ryan Loftus
//December 11 2018
//ICS 3U
//Final Project
namespace LoftusFinalProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //declare refresh timer
        Timer refreshTimer = new Timer();
        //declare duck hit box array
        int[,] hitbox = new int[50, 50];
        //declare duck rectangle array
        List <Rectangle> duckRect = new List <Rectangle>();
        //declare duckDX list
        List<int> duckDX = new List<int>();
        //declare duckDY list
        List<int> duckDY = new List<int>();
        //declare random number generator
        Random ranNum = new Random();
        //declare start button
        Button startBtn = new Button();
        //declare how to play button
        Button howToPlayBtn = new Button();
        //declare sprite timer
        Timer spriteTimer = new Timer();
        //declare player score string
        int playerScore = 0;
        //declare ammo int
        int ammo = 0;
        //declare round counter int
        int round = 0;
        //declare background rect
        Rectangle backgroundRect;
        //declare image variable
        Image backgroundImage = Image.FromFile(Application.StartupPath + @"\backgroundImage.png");
        //create gravity constant integer
        int gravity = 15;
        //declare duck flying right image array
        Image[] duckSpriteRight = new Image[4];
        //declare duck flying left image array
        Image[] duckSpriteLeft = new Image[4];
        //declare duck falling image array
        Image[] duckSpriteFalling = new Image[2];
        //declare duck shot image variable
        Image duckShot = Image.FromFile(Application.StartupPath + @"\DuckShot.png");
        //declare ground variable
        double ground;
        //declare spawn point int
        int spawnPoint = 0;
        //duckspritecounter int
        int spriteCounter = 0;
        //declare hi score variable
        int hiScore;
        private void Form1_Load(object sender, EventArgs e)
        {
            //double buffered
            this.DoubleBuffered = true;
            //set sprite timer interval
            spriteTimer.Interval = 250;
            //set refresh timer interval
            refreshTimer.Interval = (1000 / 60);
            //start sprite timer
            spriteTimer.Start();
            //start refresh timer
            refreshTimer.Start();            
            //set form location
            this.Location = new Point(0, 0);
            //set form size
            this.MinimumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            //set form size
            this.MaximumSize = this.MinimumSize;
            //prevent minimizing window
            this.MinimizeBox = false;
            //prevent maximizing window
            this.MaximizeBox = false;
            //set horizontal location of start button
            startBtn.Left = this.ClientSize.Width / 2 - 100;
            //set vertical location of start button
            startBtn.Top = this.ClientSize.Height / 2 - 50;
            //make start button visible
            startBtn.Visible = true;
            //set start button size
            startBtn.Width = 200;
            //set start button size
            startBtn.Height = 100;
            //set start button text
            startBtn.Text = "Start";
            //set horizontal location of how to play button
            howToPlayBtn.Left = this.ClientSize.Width / 2 - 100;
            //set vertical location of how to play button
            howToPlayBtn.Top = this.ClientSize.Height / 3 - 50;
            //make start button visible
            howToPlayBtn.Visible = true;
            //set start button size
            howToPlayBtn.Width = 200;
            //set start button size
            howToPlayBtn.Height = 100;
            //set start button text
            howToPlayBtn.Text = "How To Play";
            //create how to play button method
            howToPlayBtn.Click += HowToPlayBtn_Click;
            //add how to play button to form
            this.Controls.Add(howToPlayBtn);
            //create form 1 paint
            this.Paint += Form1_Paint;
            //create mouse click mehtod
            MouseClick += Form1_MouseClick;
            //create refresh tick
            refreshTimer.Tick += RefreshTimer_Tick;
            //create sprite timer tick method
            spriteTimer.Tick += SpriteTimer_Tick;
            //add button to form
            this.Controls.Add(startBtn);
            //start button method
            startBtn.Click += StartBtn_Click;
            //declare values for backgroundrect
            backgroundRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            //assign value to ground
            ground = this.ClientSize.Height * 0.6;
            //add duck sprites to ducksprite right
            duckSpriteRight[0] = Image.FromFile(Application.StartupPath + @"\DuckRight0.png", true);
            duckSpriteRight[1] = Image.FromFile(Application.StartupPath + @"\DuckRight1.png", true);
            duckSpriteRight[2] = Image.FromFile(Application.StartupPath + @"\DuckRight2.png", true);
            duckSpriteRight[3] = Image.FromFile(Application.StartupPath + @"\DuckRight1.png", true);
            //add duck sprites to ducksprite left
            duckSpriteLeft[0] = Image.FromFile(Application.StartupPath + @"\DuckLeft0.png", true);
            duckSpriteLeft[1] = Image.FromFile(Application.StartupPath + @"\DuckLeft1.png", true);
            duckSpriteLeft[2] = Image.FromFile(Application.StartupPath + @"\DuckLeft2.png", true);
            duckSpriteLeft[3] = Image.FromFile(Application.StartupPath + @"\DuckLeft1.png", true);
            //add duck sprites to ducksprite falling
            duckSpriteFalling[0] = Image.FromFile(Application.StartupPath + @"\DuckDeadRight.png", true);
            duckSpriteFalling[1] = Image.FromFile(Application.StartupPath + @"\DuckDeadLeft.png", true);
        }

        private void HowToPlayBtn_Click(object sender, EventArgs e)
        {
            //remove start button
            this.Controls.Remove(startBtn);
            //remove how to play button from form
            this.Controls.Remove(howToPlayBtn);
            //show text explaining how to play
        }

        private void SpriteTimer_Tick(object sender, EventArgs e)
        {
            //counter increase
            spriteCounter++;
            //if to check if counter should reset
            if (spriteCounter == duckSpriteRight.Length)
            {
                //reset counter
                spriteCounter = 0;
            }
        }
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            //refresh screen
            this.Invalidate();
            //run through lists
            for (int i = 0; i < duckRect.Count; i++)
            {               
                //move ducks
                duckRect[i] = new Rectangle(duckRect[i].X + duckDX[i], duckRect[i].Y + duckDY[i], duckRect[i].Width, duckRect[i].Height);
                //if checks if ducks are off screen
                if (duckRect[i].Bottom <= -1 || duckRect[i].Top >= ground +1|| duckRect[i].Right <= -1 || duckRect[i].Left >= this.ClientSize.Width +1)
                {
                    //remove duck from each list
                    duckRect.RemoveAt(i);
                    duckDX.RemoveAt(i);
                    duckDY.RemoveAt(i);
                }

            }
            //if to run when game has started
            if (round > 0)
            {
                //if checks if ammo is empty
                if (ammo == 0)
                {
                    //game over
                    //MessageBox.Show("Game Over!");
                    //change round back to 0
                    round = 0;
                }
                //else if runs if round should increase
                else if (playerScore / round == 500 * round)
                {
                    //next round
                    round++;
                    //reload ammo
                    ammo = 3;
                }
            }
            //spawn duck method
            SpawnDuck();
        }
        private void StartBtn_Click(object sender, EventArgs e)
        {
            //remove start button
            this.Controls.Remove(startBtn);
            //remove how to play button from form
            this.Controls.Remove(howToPlayBtn);
            //set ammo
            ammo = 3;
            //set round to 1
            round = 1;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //if checks if player has ammo
            if (ammo > 0)
            {
                //subtract 1 from players ammo
                ammo--;
                //for loop to run through
                for (int i = 0; i < duckRect.Count; i++)
                {
                    //check for hit
                    if (duckRect[i].Contains(MousePosition))
                    {
                        //add to score
                        playerScore += 250;
                        //refund ammo
                        ammo++;
                        //stop horizontal movement
                        duckDX[i] = 0;
                        //start falling movement
                        duckDY[i] = gravity;
                    }
                }
            }
            //declare stream writer
            StreamWriter txtWriter = new StreamWriter(Application.StartupPath + @"\hi score.txt", false);
            //declare stream reader
            StreamReader txtReader;
            //check for hi score file
            if (File.Exists(Application.StartupPath + @"\hi score.txt"))
            {
                //declares reader value
                txtReader = new StreamReader(Application.StartupPath + @"\hi score.txt", true);
                //get value of hi score into hi score variable
                hiScore = Convert.ToInt32(txtReader.ReadLine());
            }
            //if to check if hi score has been beaten
            if (hiScore < playerScore)
            {
                //use stream writer
                txtWriter.WriteLine(playerScore);
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //paint background rect
            e.Graphics.DrawImage(backgroundImage, backgroundRect);
            //run through list
            for (int i = 0; i < duckRect.Count; i++)
            {
                //if statement checks if duck is flying upward
                if (duckDY[i] < 0)
                {
                    //if statement checks if duck is flying right
                    if (duckDX[i] > 0)
                    {
                        //paint duck
                        e.Graphics.DrawImage(duckSpriteRight[spriteCounter], duckRect[i]);
                    }
                    //else if checks if duck is flying left
                    else if (duckDX[i] < 0)
                    {
                        //paint duck
                        e.Graphics.DrawImage(duckSpriteLeft[spriteCounter], duckRect[i]);
                    }
                }
                //else
                else
                {
                    //if checks if duck is flying right
                    if (duckDX[i] > 0)
                    {
                        //paint duck
                        e.Graphics.DrawImage(duckSpriteRight[spriteCounter], duckRect[i]);
                    }
                    //else if checks if duck is flying left
                    else if (duckDX[i] < 0)
                    {
                        //paint duck
                        e.Graphics.DrawImage(duckSpriteLeft[spriteCounter], duckRect[i]);
                    }
                    //else
                    else
                    {
                        //paint duck
                        e.Graphics.DrawImage(duckSpriteFalling[spriteCounter % 2], duckRect[i]);
                    }
                }
            }
        }
        private void SpawnDuck()
        {
            //if runs when there are less ducks than the current round number
            if (duckRect.Count < round)
            {
                //generate spawn point
                spawnPoint = ranNum.Next(3);
                //if runs to check if duck will spawn at bottom (spawn point 0)
                if (spawnPoint == 0)
                {
                    //spawn duck and add duck to lists
                    duckRect.Add(new Rectangle(ranNum.Next(this.ClientSize.Width), Convert.ToInt32(ground), 100, 100));
                    duckDX.Add(ranNum.Next(2) == 1 ? 8 : -8);
                    duckDY.Add(-8);
                }
                //if runs to check if duck will spawn from right side (spawn point 1)
                if (spawnPoint == 1)
                {
                    //spawn duck and add to lists
                    duckRect.Add(new Rectangle(this.ClientSize.Width, ranNum.Next(Convert.ToInt32(ground)), 100, 100));
                    duckDX.Add(-10);
                    duckDY.Add(0);
                }
                //if runs to check if duck will spawn from left side (spawn point 2)
                if (spawnPoint == 2)
                {
                    //spawn duck and add to lists
                    duckRect.Add(new Rectangle(-100, ranNum.Next(Convert.ToInt32(ground)), 100, 100));
                    duckDX.Add(10);
                    duckDY.Add(0);
                }
            }
        }
    }
}
