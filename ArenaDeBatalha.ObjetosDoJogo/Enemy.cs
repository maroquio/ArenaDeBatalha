using System.Drawing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ArenaDeBatalha.ObjetosDoJogo
{
    public class Enemy : GameObject
    {
        public Enemy(Size bounds, Graphics graphics, Point position) : base(bounds, graphics)
        {            
            this.Left = position.X;
            this.Top = position.Y;
            this.Speed = 5;
            this.Sound = Media.ExplosionShort;
        }

        public override Bitmap GetSprite()
        {
            return Media.Enemy;
        }

        public override void UpdateObject()
        {
            this.MoveDown();
            base.UpdateObject();
        }
    }
}
