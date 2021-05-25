using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlappyBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // alla variabler
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
        int gravity = 8;
        bool gameOver;
        Rect flappyBirdHitBox;
        Rect piperHitBox;
        double highScore;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // bestämmer hur snabbt spelet ska gå
            StartGame(); // kallar StarGame
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score; // visar hur långt du har kommit

            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 5, flappyBird.Height-5); // ger fågeln en hitbox

            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 458) // om man går till toppen eller botten så avslutas spelet
            {
                EndGame(); // kallar klassen EndGame
            }

            foreach (var item in MyCanvas.Children.OfType<Image>()) // skapar flera rör
            {
                if ((string)item.Tag == "obs1" || (string)item.Tag == "obs2" || (string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - 5);


                    if (Canvas.GetLeft(item) < -100)
                    {
                        Canvas.SetLeft(item, 800); // flyttar tillbaks rörer 800px framför spelaren

                        score += 0.5; // ökar poängen för varje rör man har passerat
                    }

                     piperHitBox = new Rect(Canvas.GetLeft(item), Canvas.GetTop(item), item.Width, item.Height); // ger rören en hitbpx

                    if (flappyBirdHitBox.IntersectsWith(piperHitBox)) // om fågeln träffar röret
                    {
                        EndGame(); // kallar EndGame
                    }
                }

                if ((string)item.Tag == "cloud")
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - 1); // gör så att molnen rör på sig

                    if (Canvas.GetLeft(item) < -250) // när molnet har försvunnit ur bild
                    {
                        Canvas.SetLeft(item, 550); // flytta molnet så det går att se igen
                    }
                }
            }
        }
        
        private void KeyIsDown(object sender, KeyEventArgs e) // gör att spelar åker upp och restartar spelet
        {
            if (e.Key == Key.Space) // när man trycker på space
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2); // roterar spelaren -20 grader alltså 20 gråader upp
                gravity = -8; // tar bort gravitationen så spelaren åker upp
            }

            if (e.Key == Key.R && gameOver == true) // om spelet är slut och man trycker på r
            {
                StartGame(); // kallar StartGame
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e) // gör att spelar åker ned
        {

            flappyBird.RenderTransform = new RotateTransform(15, flappyBird.Width / 2, flappyBird.Height / 2); // roterar spelaren 15 grader alltså roterarar spelar 15 grader nedåt
            gravity = 8; // gör att spelaren sjunker

        }

        private void StartGame() // startar spelet
        {
            MyCanvas.Focus(); // gör canvas focused

            int temp = 300;

            score = 0; // gör så att poängen blir 0

            gameOver = false;

            Canvas.SetTop(flappyBird, 190); // sätter tillbaks spelaren

            foreach (var item in MyCanvas.Children.OfType<Image>()) // sätter tillbaks allt
            {
                if ((string)item.Tag == "obs1") // bestämmer vilket rör som ska flyttas
                {
                    Canvas.SetLeft(item, 500); // sätter tillbaks första röret
                }

                if ((string)item.Tag == "obs2") //osv
                {
                    Canvas.SetLeft(item, 800);
                }

                if ((string)item.Tag == "obs3")
                {
                    Canvas.SetLeft(item, 1100);
                }

                if ((string)item.Tag == "cloud")
                {
                    Canvas.SetLeft(item, 300 + temp);
                    temp = 800;
                }
            }

            gameTimer.Start(); // startar spelet
        }

        private void EndGame() // avslutar spelet
        {
            gameTimer.Stop(); // avslutar spelet
            gameOver = true;
            txtScore.Content += " Game over!! Press R to try again";
        }
    }
}
