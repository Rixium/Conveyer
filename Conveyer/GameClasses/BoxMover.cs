using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {

    class BoxMover : Entity {

        private Boolean boxOn = true;
        public Animation movingBoxAnimation;
        private float life = 100;
        private Random r = new Random();
        private bool isBroke = false;
        public Type type;
        public bool breakable = true;
        private bool movingDown, movingUp;

        private Vector2 startPos;

        public enum Type {
            box = 0,
            item = 1
        }

        public BoxMover(int x, int y, Type type) {
            this.type = type;
            movingBoxAnimation = new Animation(ContentChest.Instance.conveyer, 1);
            position = new Vector2(x, y);
            startPos = position;
            movingUp = true;
            bounds = new Rectangle(x, y + (int)((float)GameConstants.TILE_SIZE / 1.5f), GameConstants.TILE_SIZE / 2, GameConstants.TILE_SIZE / 2);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(boxOn) {
                spriteBatch.Draw(movingBoxAnimation.Frame, new Rectangle((int)position.X, (int)position.Y, GameConstants.TILE_SIZE, GameConstants.TILE_SIZE), Color.White);
            } else {
                spriteBatch.Draw(ContentChest.Instance.conveyer[0], new Rectangle((int)position.X, (int)position.Y, GameConstants.TILE_SIZE, GameConstants.TILE_SIZE), Color.White);
            }
        }

        public bool Fix() {
            if(life < 100) {
                life = 100;
                isBroke = false;
                return true;
            }

            return false;
        }

        public override void Update() {
            if ((GameConstants.BoxConveyerRunning && type == Type.box) || (GameConstants.ItemConveyerRunning && type == Type.item)) {
                if (breakable) {
                    if (life > 0) {
                        int rNum = 0;
                        if (type == Type.box) {
                            rNum = r.Next(0, 32);
                        } else if (type == Type.item) {
                            rNum = r.Next(0, 13);
                        }
                        if (rNum == 1) {
                            life -= 1;
                            if (life <= 0 && !isBroke) {
                                ContentChest.Instance.machineBreak.Play();
                                isBroke = true;
                            }
                        }
                    }
                }

                if (boxOn) {
                    movingBoxAnimation.Update();
                } else {
                    movingBoxAnimation.SetIndex(0);
                }
            }
        }

        public bool Broke {
            get {
                return isBroke;
            }
        }

    }
}
