using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {

    class Box : Interactable {

        Texture2D box;
        public BoxType boxType;
        private Rectangle drawRect;
        private bool packaged = false;
        private Vector2 startPos;
        private bool movingUp = true, movingDown;
        int startX, startY;
        private Spark spark;

        public enum BoxType {
            SMALL,
            BIG,
            FRAGILE,
            NONE
        }

        public Rectangle DrawRect {
            get {
                return drawRect;
            }
        }

        public bool Package() {
            if(!packaged) {
                spark = new Spark(new Vector2(drawRect.X, drawRect.Y));
                packaged = true;
                if (boxType == BoxType.BIG) {
                    box = ContentChest.Instance.smallBoxPackaged;
                    
                } else if (boxType == BoxType.SMALL) {
                    box = ContentChest.Instance.bigBoxPackaged;
                    
                } else if (boxType == BoxType.FRAGILE) {
                    box = ContentChest.Instance.fragileBoxPackaged;
                    
                }
                return true;
            }
            return false;
        }

        public Box(int x, int y, BoxType boxType) {
            startX = x;
            startY = y;
            collidable = false;
            if (boxType == BoxType.NONE) {
                Random r = new Random();
                int randomBox = r.Next(0, 3);
                if (randomBox == 0) {
                    box = ContentChest.Instance.smallBox;
                    boxType = BoxType.BIG;
                } else if (randomBox == 1) {
                    box = ContentChest.Instance.bigBox;
                    boxType = BoxType.SMALL;
                } else if (randomBox == 2) {
                    box = ContentChest.Instance.fragileBox;
                    boxType = BoxType.FRAGILE;
                }
            } else {
                this.boxType = boxType;
                if (boxType == BoxType.BIG) {
                    box = ContentChest.Instance.smallBox;
                } else if (boxType == BoxType.SMALL) {
                    box = ContentChest.Instance.bigBox;
                } else if (boxType == BoxType.FRAGILE) {
                    box = ContentChest.Instance.fragileBox;
                }
            }

            int width = GameConstants.TILE_SIZE;
            float scale = (float)width / (float)box.Width;
            int height = (int)(box.Height * scale);

            position = new Vector2(x, y - height / 2);
            drawRect = new Rectangle(x, y + 8 - height, width, height);
            startPos = new Vector2(drawRect.X, drawRect.Y);
            bounds = new Rectangle(x, y + GameConstants.TILE_SIZE - 5, box.Width, box.Height);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(box, drawRect, Color.White);

            if(spark != null) {
                spark.Draw(spriteBatch, new Vector2(drawRect.X + drawRect.Width / 2, drawRect.Y + drawRect.Height / 2));
            }
        }

        public override void Update() {
            if(spark != null) {
                spark.Update();
                if(spark.dead) {
                    spark = null;
                }
            }

            if (box == ContentChest.Instance.smallBox) {
                boxType = BoxType.BIG;
            } else if (box == ContentChest.Instance.bigBox) {
                boxType = BoxType.SMALL;
            } else if (box == ContentChest.Instance.fragileBox) {
                boxType = BoxType.FRAGILE;
            }

            if (GameConstants.BoxConveyerRunning) {
                position.X -= .5f;
                bounds.X = (int)position.X;
                drawRect.X = (int)position.X;
            }
        }

    }

}
