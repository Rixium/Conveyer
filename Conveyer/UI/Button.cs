using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Conveyer.Constants.Enums;

namespace Conveyer.UI {

    class Button {

        private Texture2D image;
        private Rectangle position;

        private Rectangle startPosition;
        private Rectangle hoverPosition;

        private Boolean hovering;
        private ButtonTag tag;

        private int hoverChange = 5;

        public Button(Texture2D image, Vector2 position, ButtonTag tag) {
            this.image = image;
            this.tag = tag;
            this.position = new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height);

            startPosition = this.position;
            hoverPosition = this.position;
            hoverPosition.Y -= hoverChange;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, position, Color.White);
        }

        public Rectangle Bounds {
            get {
                return startPosition;
            }
        }

        public void Hover(Boolean hovering) {
            if (!this.hovering && hovering) {
                this.hovering = true;
                ContentChest.Instance.buttonSound.Play();
            }

            if(hovering) {
                position = hoverPosition;
            } else {
                this.hovering = false;
                position = startPosition;
            }
        }

        public ButtonTag Tag {
            get {
                return tag;
            }
        }
    }
}
