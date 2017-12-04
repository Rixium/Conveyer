using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.GameClasses {

    class Tile {

        private Texture2D texture;
        private Rectangle position;

        public Tile(Texture2D texture, int x, int y) {
            this.texture = texture;
            this.position = new Rectangle(x, y, GameConstants.TILE_SIZE, GameConstants.TILE_SIZE);
        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
