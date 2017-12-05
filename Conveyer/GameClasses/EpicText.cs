using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {
    class EpicText {

        String txt;
        public bool dead;
        Vector2 pos;
        float opacity = 1;
        int waitTimer = 50;
        private Color color;

        public EpicText(String text, Vector2 pos, Color color) {
            txt = text;
            Random r = new Random();
            int ran = r.Next(0, 2);
            int ran2 = r.Next(0, 20);
            int ran3 = r.Next(0, 10);
            int ran4 = r.Next(-10, 10);
            this.pos = pos;
            this.color = color;

            switch(ran) {
                case 0:
                    this.pos.X -= ran2;
                    this.pos.Y += ran4;
                    break;
                case 1:
                    this.pos.X += ran3;
                    this.pos.Y += ran4;
                    break;
                default:
                    break;
            }
        }

        public void Update() {
            if (waitTimer <= 0) {
                if (opacity > 0) {
                    opacity -= 0.05f;
                } else {
                    dead = true;
                }
            } else {
                waitTimer--;
                
            }
            pos.Y -= 0.2f;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(ContentChest.Instance.scoreFont2, txt, pos, color * opacity);
        }
    }
}
