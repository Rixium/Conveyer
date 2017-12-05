using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Conveyer.GameClasses.Box;

namespace Conveyer.GameClasses {
    class Item : Entity {

        private Texture2D image;
        private Rectangle drawRect;
        private Vector2 startPos;
        public bool floored = false;
        public bool onConveyer = true;
        private bool movingUp = true, movingDown;
        private string name;
        public BoxType boxType;

        public Item(Texture2D itemImage, String name, BoxType boxType) {
            this.image = itemImage;
            this.name = name;
            this.collidable = false;
            this.boxType = boxType;
        }

        public Texture2D Image {
            get {
                return this.image;
            }
        }

        public override void Update() {
            if (GameConstants.ItemConveyerRunning && !floored) {
                position.X -= 0.5f;
                bounds.X = (int)position.X;
                drawRect.X = (int)position.X;
            }

            
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            if (floored || onConveyer) {
                spriteBatch.Draw(image, drawRect, Color.White);
            }
        }

        public override Vector2 Position {
            get {
                return position;
            }
            set {
                position = value;
                int width = GameConstants.TILE_SIZE / 2;
                float scale = (float)width / (float)image.Width;
                int height = (int)((float)image.Height * scale);
                bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
                drawRect = bounds;
                startPos = new Vector2(drawRect.X, drawRect.Y);
                bounds.Y += 10;
            }
        }
        
        public Rectangle DrawRect {
            get {
                return drawRect;
            }
        }
        public string Name {
            get {
                return name;
            }
        }
        public float X {
            get {
                return position.X;
            }
            set {
                position.X = value;
            }
        }

        public float Y {
            get {
                return position.Y;
            }
            set {
                position.Y = value;
            }
        }
    }
}
