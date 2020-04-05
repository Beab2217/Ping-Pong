using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace PingPongGameSharp
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Timer timer = new Timer();
        private Timer timerTitre = new Timer();
        private Timer timerScore = new Timer();
        private int player1y, player2y;
        private int x, y;
        private int speedX = 1, speedY = 2;
        string[] titer = { "3..", "2..", "1..", "Start!", "" };
        private int timeLeft = 4;
        private int player1Score = 0, player2Score = 0;


        public Form1()
        {

            InitializeComponent();

            timerTitre.Interval = 1000;
            timerTitre.Enabled = true;
            timerTitre.Tick += new EventHandler(Form1_Load);
            timerTitre.Start();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = true;
            this.label1.Text = titer[4 - timeLeft];
            timeLeft--;
            if (timeLeft < 0)
            {
                timerTitre.Stop();
                label1.Visible = false;

                timer.Enabled = true;
                timer.Interval = 1;
                timer.Tick += new EventHandler(TimerCallback);
                this.KeyUp += new KeyEventHandler(MainFormKeyDown);
                this.Paint += new PaintEventHandler(MainFormPaint);
                timerTitre.Tick -= new EventHandler(Form1_Load);

                x = this.Width / 2;
                y = this.Height / 2;

                timeLeft = 4;
            }
        }

        private void PlayerScore(object sender, EventArgs e)
        {
            timeLeft -= 4;
            this.label1.Visible = true;
            this.label1.Text = player1Score.ToString() + ":" + player2Score.ToString();
            if (timeLeft < 0)
            {
                timerScore.Stop();
                this.label1.Visible = false;
                timerScore.Tick -= new EventHandler(PlayerScore);

                timeLeft = 4;
                timerTitre.Interval = 1000;
                timerTitre.Enabled = true;
                timerTitre.Tick += new EventHandler(Form1_Load);
                timerTitre.Start();
            }

        }

        private void MainFormPaint(object sender, PaintEventArgs e)
        {
            graphics = CreateGraphics();
            DrawRectangle(10, player1y, 20, 100, new SolidBrush(Color.Black));
            DrawRectangle(this.Width - 30, player2y, 20, 100, new SolidBrush(Color.Black));
            DrawRectangle(x, y, 20, 20, new SolidBrush(Color.Black));
            
        }

        private void DrawRectangle(int x, int y, int w, int h, SolidBrush color) 
        {
            graphics.FillRectangle(color, new Rectangle(x, y, w, h));
        }

        private void BallControl()
        {

            x += speedX;
            y += speedY;

            if (y <= 0 || y + 50 > this.Height)
            {
                speedX = Math.Sign(speedX)*Math.Abs(speedY);
                speedY = -Math.Sign(speedY)* Math.Abs(speedX);
            }

            if (IsCollided())
            {
                speedX = -speedX;
            }

            if (x < 0)
            {
                this.label1.Text = "1st player are failing!";
                this.label1.Visible = true;
                this.Paint -= new PaintEventHandler(MainFormPaint);
                timer.Tick -= new EventHandler(TimerCallback);
                this.KeyUp -= new KeyEventHandler(MainFormKeyDown);
                timer.Stop();
                player2Score++;
                timerScore.Interval = 3000;
                timerScore.Enabled = true;
                timerScore.Tick += new EventHandler(PlayerScore);
                timerScore.Start();
            }

            if (x + 20 > this.Width)
            {
                this.label1.Text = "2nd player are failing!";
                this.label1.Visible = true;
                this.Paint -= new PaintEventHandler(MainFormPaint);
                timer.Tick -= new EventHandler(TimerCallback);
                this.KeyUp -= new KeyEventHandler(MainFormKeyDown);
                timer.Stop();
                player1Score++;
                timerScore.Interval = 3000;
                timerScore.Enabled = true;
                timerScore.Tick += new EventHandler(PlayerScore);
                timerScore.Start();
            }

        }

        private bool IsCollided()
        {
            if ((x < 30 && y > player1y && y < player1y + 100) || 
                (x > this.Width - 50 && y > player2y && y < player2y + 100))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void TimerCallback(object sender, EventArgs e)
        {
            DrawRectangle(10, player1y, 20, 100, new SolidBrush(Color.Black));
            DrawRectangle(this.Width - 30, player2y, 20, 100, new SolidBrush(Color.Black));
            DrawRectangle(x, y, 20, 20, new SolidBrush(Color.Black));
            BallControl();

            this.Invalidate();
            return;
        }

        private void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            int key = e.KeyValue;
            
            if (key == 38 && player1y > 0) {
                for (int i = 0; i < 15; i++)
                {
                    player1y -= 1;
                }
            }

            if (key == 40 && player1y + 150 < this.Height)
            {
                for (int i = 0; i < 15; i++)
                {
                    player1y += 1;
                }
            }

            if (key == 87 && player2y > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    player2y -= 1;
                }
            }

            if (key == 83 && player2y + 150 < this.Height)
            {
                for (int i = 0; i < 15; i++)
                {
                    player2y += 1;
                }
            }



        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
