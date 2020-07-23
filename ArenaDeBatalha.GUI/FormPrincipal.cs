using ArenaDeBatalha.ObjetosDoJogo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
//adicionar referência para os assemblies WindowsBase.dll e Presentation.Core

namespace ArenaDeBatalha.GUI
{
    public partial class FormPrincipal : Form
    {
        DispatcherTimer gameLoopTimer { get; set; }
        DispatcherTimer enemySpawnTimer { get; set; }
        bool gameIsOver { get; set; }
        bool canShoot { get; set; }
        Random random { get; set; }
        Graphics screen { get; set; }
        Bitmap screenBuffer { get; set; }
        Background background { get; set; }
        Player player { get; set; }
        GameOver gameOver { get; set; }
        List<GameObject> gameObjects { get; set; }

        public FormPrincipal()
        {            
            InitializeComponent();
            this.screenBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.screen = Graphics.FromImage(this.screenBuffer);
            this.background = new Background(this.ClientSize, this.screen);            
            this.player = new Player(this.ClientSize, this.screen, gameObjects);
            this.gameOver = new GameOver(this.ClientSize, this.screen);
            this.gameObjects = new List<GameObject>();
            this.gameObjects.Add(background);
            this.gameObjects.Add(player);
            StartGame();
        }

        public void StartGame()
        {
            random = new Random();
            gameIsOver = false;

            gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.6666);            
            gameLoopTimer.Tick += GameLoop;
            gameLoopTimer.Start();

            enemySpawnTimer = new DispatcherTimer();
            enemySpawnTimer.Interval = TimeSpan.FromMilliseconds(1000);
            enemySpawnTimer.Tick += SpawnEnemy;
            enemySpawnTimer.Start();
        }

        private void SpawnEnemy(object sender, EventArgs e)
        {
            Point enemyPosition = new Point(random.Next(2, this.ClientSize.Width - 66), -62);
            Enemy enemy = new Enemy(this.ClientSize, this.screen, enemyPosition);
            gameObjects.Add(enemy);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (gameIsOver)
            {
                this.EndGame();
                return;
            }

            gameObjects.RemoveAll(x => !x.Active);

            this.ProcessControls();            
                        
            foreach (GameObject goA in this.gameObjects)
            {
                goA.UpdateObject();

                if (goA.IsOutOfBounds())
                {
                    goA.Destroy();
                    continue;
                }

                if (goA is Enemy)
                {
                    if (gameIsOver) return;

                    if (goA.IsCollidingWith(player))
                    {                        
                        gameIsOver = true;                        
                        return;
                    }

                    foreach (GameObject goB in this.gameObjects.Where(x => x is Bullet))
                    {
                        if (goA.IsCollidingWith(goB))
                        {
                            goA.Destroy();
                            goB.Destroy();
                        }
                    }
                }                
            }

            this.Invalidate();
        }

        private void EndGame()
        {
            gameObjects.RemoveAll(x => !(x is Background));
            gameLoopTimer.Stop();
            enemySpawnTimer.Stop();
            background.UpdateObject();
            gameOver.UpdateObject();
            this.Invalidate();
        }

        private void ProcessControls()
        {
            if (Keyboard.IsKeyDown(Key.Left)) player.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right)) player.MoveRight();
            if (Keyboard.IsKeyDown(Key.Up)) player.MoveUp();
            if (Keyboard.IsKeyDown(Key.Down)) player.MoveDown();
            if (canShoot && Keyboard.IsKeyDown(Key.Space))
            {
                this.gameObjects.Add(player.Shoot());
                canShoot = false;
            }
            if (Keyboard.IsKeyUp(Key.Space))
            {
                canShoot = true;
            }
        }

        private void FormPrincipal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(screenBuffer, 0, 0);
        }
    }
}
