using System.Drawing;

namespace ArenaDeBatalha.ObjetosDoJogo
{
    public class Background : GameObject
    {
        public Background(Size bounds, Graphics g) : base(bounds, g)
        {            
            this.Left = 0;
            this.Top = 0;
            this.Speed = 0;
        }

        public override Bitmap GetSprite()
        {
            return Media.Background;
        }
    }
}
