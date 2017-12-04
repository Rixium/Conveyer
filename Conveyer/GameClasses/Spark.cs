using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {

    class Spark {

        private Animation sparkAnimation;
        private Vector2 pos;
        public bool dead = false;

        public Spark(Vector2 pos) {
            this.pos = pos;
            sparkAnimation = new Animation(ContentChest.Instance.sparks, 5);
            sparkAnimation.AnimateOnce = true;
        }

        public void Update() {
            sparkAnimation.Update();
            if(sparkAnimation.finished) {
                dead = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos) {
            spriteBatch.Draw(sparkAnimation.Frame, new Vector2(pos.X - sparkAnimation.Frame.Width / 2, pos.Y - sparkAnimation.Frame.Height / 2), Color.White);
        }
    }
}
