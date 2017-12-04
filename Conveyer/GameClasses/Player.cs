using Conveyer.Constants;
using Conveyer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Conveyer.GameClasses {

    class Player : Entity {

        GameInstance game;
        Vector2 headPos;
        public Vector2 bodyPos;
        Vector2 legPos;

        Animation legWalkAnimation;
        Animation bodyCarryAnimation;
        Animation headWalkAnimation;

        float speed = 2f;
        Boolean carrying = false;
        Item heldItem;
        private Rectangle itemPos;

        public Vector2 Center {
            get {
                return new Vector2(position.X + headPos.X + bodyPos.X, position.Y + headPos.Y + bodyPos.Y);
            }
        }
        private enum Directions {
            LEFT,
            RIGHT
        }

        private Directions Direction;

        enum LegState {
            Idle = 0,
            Walk = 1
        }

        enum BodyState {
            Idle = 0,
            Carry = 1
        }

        private LegState CurrentLegState = LegState.Idle;

        public Player(GameInstance game) {
            position = new Vector2(64, 64);
            headPos = new Vector2(-ContentChest.Instance.head.Width / 2, 0);
            bodyPos = new Vector2(headPos.X, ContentChest.Instance.head.Height);
            legPos = new Vector2(bodyPos.X, ContentChest.Instance.head.Height + ContentChest.Instance.body.Height - 5);

            this.game = game;
            legWalkAnimation = new Animation(ContentChest.Instance.walkLegs, 5);
            bodyCarryAnimation = new Animation(ContentChest.Instance.bodyCarry, 10);
            headWalkAnimation = new Animation(ContentChest.Instance.headWalk, 10);

           
            bounds = new Rectangle((int)position.X + (int)legPos.X, (int)position.Y + (int)legPos.Y, ContentChest.Instance.walkLegs[0].Height, ContentChest.Instance.body.Width);
        }

        public Item Item {
            get {
                if (this.heldItem != null) {
                    return this.heldItem;
                } else {
                    return null;
                }
            }
        }

        public void RemoveItem() {
            this.heldItem = null;
            carrying = false;
        }

        public override void Update() {
            
            CheckInput();
            if(CurrentLegState == LegState.Walk) {
                legWalkAnimation.Update();
                bodyCarryAnimation.Update();
                headWalkAnimation.Update();
            } else {
                legWalkAnimation.SetIndex(0);
                bodyCarryAnimation.SetIndex(0);
                headWalkAnimation.SetIndex(0);
            }

            if(carrying) {
                itemPos.X = (int)position.X + (int)bodyPos.X;
                itemPos.Y = (int)position.Y + (int)bodyPos.Y;
            }
        }

        public bool Carrying {
            get {
                return carrying;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if(Direction == Directions.RIGHT) {
                
                if (CurrentLegState == LegState.Idle) {
                    spriteBatch.Draw(ContentChest.Instance.head, position + headPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(ContentChest.Instance.body, position + bodyPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(ContentChest.Instance.idleLegs, position + legPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                } else {
                    spriteBatch.Draw(headWalkAnimation.Frame, position + headPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(bodyCarryAnimation.Frame, position + bodyPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(legWalkAnimation.Frame, position + legPos, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }

                if(carrying) {
                    spriteBatch.Draw(heldItem.Image, itemPos, new Rectangle(0, 0, Item.Image.Width, Item.Image.Height), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
            } else if (Direction == Directions.LEFT) {
               
                if (CurrentLegState == LegState.Idle) {

                    spriteBatch.Draw(ContentChest.Instance.head, position + headPos, Color.White);
                    spriteBatch.Draw(ContentChest.Instance.body, position + bodyPos, Color.White);
                    spriteBatch.Draw(ContentChest.Instance.idleLegs, position + legPos, Color.White);
                } else {
                    spriteBatch.Draw(headWalkAnimation.Frame, position + headPos, Color.White);
                    spriteBatch.Draw(bodyCarryAnimation.Frame, position + bodyPos, Color.White);
                    spriteBatch.Draw(legWalkAnimation.Frame, position + legPos, Color.White);

                }

                if (carrying) {
                    spriteBatch.Draw(heldItem.Image, itemPos, Color.White);
                }
            }
            
        }

        public bool SetItem(Item item) {
            if (!carrying) {
                carrying = true;
                this.heldItem = item;
                item.onConveyer = false;
                item.floored = false;

                float w = ContentChest.Instance.head.Width;
                float scale = (float)heldItem.Image.Width / w;
                float h = heldItem.Image.Height / scale;
                itemPos = new Rectangle((int)position.X + (int)bodyPos.X, (int)position.Y + (int)bodyPos.Y + 100, (int)w, (int)h);
                return true;
            } else {
                return false;
            }
        }

        private void CheckInput() {
            bool walking = false;
            bool hitX = false, hitY = false;

            Rectangle newBounds = bounds;
            Vector2 newPos = position;

            if(InputManager.Instance.KeyPressed(Keys.LEFT)) {
                newPos.X -= speed;
                if(newPos.X < 0) {
                    newPos.X = 0;
                }
                Direction = Directions.LEFT;
                walking = true;

            } else if (InputManager.Instance.KeyPressed(Keys.RIGHT)) {
                newPos.X += speed;
                if (newPos.X > GameConstants.MAP_SIZE_HORIZONTAL * GameConstants.TILE_SIZE - bounds.Width) {
                    newPos.X = GameConstants.MAP_SIZE_HORIZONTAL * GameConstants.TILE_SIZE - bounds.Width;
                }
                Direction = Directions.RIGHT;
                walking = true;
            }

            if (newPos.X != position.X || newPos.Y != position.Y) {
                newBounds.X = (int)newPos.X + (int)legPos.X;
                newBounds.Y = (int)newPos.Y + (int)legPos.Y;
                Rectangle oldBounds = bounds;
                bounds = newBounds;
                if (game.CheckCollision(this)) {
                    bounds = oldBounds;
                    walking = false;
                } else {
                    position = newPos;
                }
            }

            if (InputManager.Instance.KeyPressed(Keys.UP)) {
                newPos.Y -= speed;
                if (newPos.Y < 0) {
                    newPos.Y = 0;
                }
                walking = true;
            } else if (InputManager.Instance.KeyPressed(Keys.DOWN)) {
                newPos.Y += speed;
                if (newPos.Y > (GameConstants.MAP_SIZE_VERTICAL * GameConstants.TILE_SIZE)) {
                    newPos.Y = (GameConstants.MAP_SIZE_VERTICAL * GameConstants.TILE_SIZE);
                }
                walking = true;
            }

            if (newPos.X != position.X) {
                newBounds.X = (int)newPos.X + (int)legPos.X;
                Rectangle oldBounds = bounds;
                bounds = newBounds;

                if (game.CheckCollision(this)) {
                    bounds = oldBounds;
                    hitX = true;
                } else {
                    position = newPos;
                }
            }

            if (newPos.Y != position.Y) {
                newBounds.Y = (int)newPos.Y + (int)legPos.Y;
                Rectangle oldBounds = bounds;
                bounds = newBounds;
                
                if (game.CheckCollision(this)) {
                    bounds = oldBounds;
                    hitY = true;
                } else {
                    position = newPos;
                }
            }

            if(hitX && hitY) {
                walking = false;
            }

            if(walking) {
                CurrentLegState = LegState.Walk;
            } else {
                CurrentLegState = LegState.Idle;
            }
        }

    }


}
