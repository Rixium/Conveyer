using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Conveyer.GameClasses.Box;

namespace Conveyer.GameClasses {

    class Order {

        private Item item;
        private Vector2 position;
        private Vector2 slipPos;
        private Vector2 itemPos;

        private GameInstance game;
        private bool complete = false;

        public enum SmallItems {
            Book = 0,
            Oil = 1,
            Reader = 2,
            Jewellery = 3
        }

        public enum BigItems {
            Teddy = 0,
            Spade = 1,
            Drill = 2,
            Television = 3
        }

        public enum FragileItems {
            Cups = 0,
            Vase = 1,
            Shades = 2
        }

        public bool IsComplete {
            get {
                return complete;
            }
        }

        public bool Complete() {
            if(!complete) {
                complete = true;
                return true;
            }
            return false;
        }

        public Item Item {
            get {
                return item;
            }
        }

        public Order(GameInstance game) {
            this.game = game;
            position = new Vector2(GameConstants.GAME_WIDTH + ContentChest.Instance.orderHolder.Width, 5);
            slipPos = position;
            slipPos.X -= ContentChest.Instance.orderHolder.Width / 2;
            slipPos.Y += ContentChest.Instance.orderHolder.Height - 5;

            Random r = new Random();
            int random = r.Next(0, 3);

            switch(random) {
                case 0:
                    random = r.Next(0, ContentChest.Instance.smallItems.Count);
                    SmallItems itemName = (SmallItems)random;
                    string name = itemName.ToString();
                    item = new Item(ContentChest.Instance.smallItems[name], name, BoxType.SMALL);
                    break;
                case 1:
                    random = r.Next(0, ContentChest.Instance.bigItems.Count);
                    BigItems bigItemName = (BigItems)random;
                    name = bigItemName.ToString();
                    item = new Item(ContentChest.Instance.bigItems[name], name, BoxType.BIG);
                    break;
                case 2:
                    random = r.Next(0, ContentChest.Instance.fragileItems.Count);
                    FragileItems fragileItemName = (FragileItems)random;
                    name = fragileItemName.ToString();
                    item = new Item(ContentChest.Instance.fragileItems[name], name, BoxType.FRAGILE);
                    break;
            }

            game.AddItem(item);

            itemPos = new Vector2(slipPos.X + ContentChest.Instance.orderSlip.Width / 2 - item.Image.Width / 2,
               slipPos.Y + ContentChest.Instance.orderSlip.Height / 2 - item.Image.Height / 2);
        }

        public void Update() {
            if (position.X > 0 + ContentChest.Instance.orderHolder.Width || complete) {
                Vector2 newPos = position;
                newPos.X--;
                if (!game.CheckCollisionWithOrders(this, newPos)) {
                    position = newPos;
                    slipPos.X--;
                    itemPos.X--;
                }
            }
            if (position.X <= 0 - ContentChest.Instance.orderHolder.Width) {
                game.RemoveOrder(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(ContentChest.Instance.orderHolder, position, Color.White);
            spriteBatch.Draw(ContentChest.Instance.orderSlip, slipPos, Color.White);
            spriteBatch.Draw(item.Image, itemPos, Color.White);

            if(complete) {
                spriteBatch.Draw(ContentChest.Instance.tick, slipPos, Color.White);
            }
        }

        public bool Contains(Vector2 pos) {
            if(pos.X >= this.position.X && pos.Y >= position.Y && pos.X <= position.X + ContentChest.Instance.orderHolder.Width && pos.Y <= position.Y + ContentChest.Instance.orderHolder.Height) {
                return true;
            }
            return false;
        }
    }
}
