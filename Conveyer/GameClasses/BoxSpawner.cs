using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {
    class BoxSpawner : Entity {

        private bool spawnedBox = false;
        private Rectangle drawRect;
        private int spawnTimer = 30;
        private int currTimer = 0;

        public BoxSpawner(int x, int y) {
            this.position = new Vector2(x, y);
            float scale = ContentChest.Instance.boxSpawner[0].Width / GameConstants.TILE_SIZE;
            int height = (int)((ContentChest.Instance.boxSpawner[0].Height) / scale);
            this.drawRect = new Rectangle(x, (int)position.Y + GameConstants.TILE_SIZE - height, GameConstants.TILE_SIZE, height);
            this.bounds = new Rectangle(x, (int)position.Y + (int)((float)GameConstants.TILE_SIZE / 1.3f), GameConstants.TILE_SIZE, GameConstants.TILE_SIZE - GameConstants.TILE_SIZE);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(ContentChest.Instance.boxSpawner[0], drawRect, Color.White);
        }

        public override void Update() {
            if(currTimer > 0) {
                currTimer--;
            } else {
                spawnedBox = false;
            }
        }

        public void Spawned() {
            spawnedBox = true;
            currTimer = spawnTimer;
        }

        public bool HasSpawned {
            get {
                return spawnedBox;
            }
        }

        public override Rectangle Bounds {
            get {
                return bounds;
            }
        }

    }
}
