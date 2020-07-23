using System.Drawing;
using System.IO;
using System.Media;

namespace ArenaDeBatalha.ObjetosDoJogo
{
    public partial class GameObject
    {
        public bool Active { get; set; }
        public int Speed { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public Bitmap Sprite { get; set; }
        public Size Bounds { get; set; }
        public Graphics Screen { get; set; }
        public Rectangle Rectangle { get; set; }
        public int Width { get { return this.Sprite.Width; } }
        public int Height { get { return this.Sprite.Height; } }
        public Stream Sound { get; set; }
        
        private SoundPlayer soundPlayer { get; set; }

        public GameObject(Size bounds, Graphics graphics)
        {
            this.InitializeGraphics();
            this.Bounds = bounds;
            this.Screen = graphics;
            this.Active = true;
            this.soundPlayer = new SoundPlayer();
        }

        public void InitializeGraphics()
        {
            this.Sprite = this.GetSprite();
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
        }

        public virtual Bitmap GetSprite() { return null; }

        public virtual void UpdateObject() 
        {
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
            this.Screen.DrawImage(this.Sprite, this.Rectangle);
        }

        public virtual void MoveLeft()
        {
            if (this.Left > 0)
                this.Left -= this.Speed;
        }

        public virtual void MoveRight()
        {
            if (this.Left < this.Bounds.Width - this.Width)
                this.Left += this.Speed;
        }

        public virtual void MoveDown()
        {
            this.Top += this.Speed;
        }

        public virtual void MoveUp()
        {
            this.Top -= this.Speed;
        }

        public bool IsOutOfBounds()
        {
            return
                (this.Top > this.Bounds.Height + this.Height) ||
                (this.Top < -this.Height) ||
                (this.Left > this.Bounds.Width + this.Width) ||
                (this.Left < -this.Width);
        }

        public bool IsCollidingWith(GameObject gameObject)
        {
            if (this.Rectangle.IntersectsWith(gameObject.Rectangle))
            {
                this.PlaySound();
                return true;
            }
            else return false;
        }

        public void Destroy()
        {
            this.Active = false;
        }

        public void PlaySound() 
        {
            soundPlayer.Stream = this.Sound;
            soundPlayer.Play();
        }
    }
}
