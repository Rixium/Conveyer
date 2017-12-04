using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Conveyer.GameClasses {

    abstract class Entity {

        protected Vector2 position;
        protected Rectangle bounds;
        protected bool collidable = true;
        protected bool showingText = false;

        public virtual void Update() {

        }

        public virtual void Draw(SpriteBatch spriteBatch) {

        }

        public virtual Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
            }
        }

        public virtual Rectangle Bounds {
            get {
                return bounds;
            }
        }

        public bool IsIn(Entity e) {
            if(e.bounds.X > this.bounds.X && e.bounds.X < this.bounds.X + this.bounds.Width) {
                if (e.bounds.Y > this.bounds.Y && e.bounds.Y < this.bounds.Y + this.bounds.Height) {
                    return true;
                }
            }
            return false;
        }

        public int Left {
            get {
                return (int)position.X;
            }
        }

        public bool Collidable {
            get {
                return collidable;
            }
        }

        
    }
}
