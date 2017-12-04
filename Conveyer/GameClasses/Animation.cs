using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {

    class Animation {

        private Texture2D[] images;
        private int currIndex = 0;

        private int currTimer = 0;
        private int animationTimer = 5;
        private bool animateOnce = false;
        public bool finished = false;

        public bool AnimateOnce {
            set {
                animateOnce = value;
            }
        }

        public Animation(Texture2D[] images, int timer) {
            this.images = images;
            this.animationTimer = timer;
        }

        public void Update() {
            if (currTimer < animationTimer) {
                currTimer++;
            } else {
                currTimer = 0;
                if (currIndex < images.Length - 1) {
                    currIndex++;
                } else {
                    if (!animateOnce) {
                        currIndex = 0;
                    } else {
                        finished = true;
                    }
                }

            }
        }

        public void SetIndex(int index) {
            this.currIndex = index;
        }

        public Texture2D Frame {
            get {
                return images[currIndex];
            }
        }

    }
}
