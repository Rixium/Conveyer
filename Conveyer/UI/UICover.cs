using Conveyer.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.UI {

    class UICover {


        private Vector2 orderBarPos = new Vector2(0, 10);
        private Vector2 lowerUIPos = new Vector2(0, GameConstants.GAME_HEIGHT - ContentChest.Instance.lowerUI.Height);

        public UICover() {

        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(ContentChest.Instance.orderBar, orderBarPos, Color.White);
            spriteBatch.Draw(ContentChest.Instance.lowerUI, lowerUIPos, Color.White);
        }

    }

}
