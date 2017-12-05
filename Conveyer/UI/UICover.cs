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
        private Vector2 scoreWordPos = new Vector2(GameConstants.GAME_WIDTH - 10 - ContentChest.Instance.scoreFont.MeasureString("SCORE").X, GameConstants.GAME_HEIGHT - ContentChest.Instance.lowerUI.Height + 20);
        private int score;

        public UICover() {

        }

        public void Update(int score) {
            this.score = score;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(ContentChest.Instance.orderBar, orderBarPos, Color.White);
            spriteBatch.Draw(ContentChest.Instance.lowerUI, lowerUIPos, Color.White);

            Vector2 scorePos = new Vector2(GameConstants.GAME_WIDTH - 10 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).X, GameConstants.GAME_HEIGHT - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y + 5);

            spriteBatch.DrawString(ContentChest.Instance.scoreFont, "SCORE", scoreWordPos, Color.White);
            spriteBatch.DrawString(ContentChest.Instance.scoreFont, score.ToString(), scorePos, Color.White);
        }

    }

}
