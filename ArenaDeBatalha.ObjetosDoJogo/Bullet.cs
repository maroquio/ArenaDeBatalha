using System.Drawing;

namespace ArenaDeBatalha.ObjetosDoJogo
{
    public class Bullet : GameObject
    {
        public Bullet(Size bounds, Graphics graphics, Point position) : base(bounds, graphics)
        {            
            this.Top = position.Y;
            this.Left = position.X;
            this.Speed = 20;
            this.Sound = Media.Missile;
            this.PlaySound();
        }

        public override Bitmap GetSprite()
        {
            return Media.Bullet;
        }
        
        public override void UpdateObject()
        {
            this.MoveUp();
            base.UpdateObject();
        }
    }
}
