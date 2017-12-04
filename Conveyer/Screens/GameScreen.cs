using Conveyer.GameClasses;
using Microsoft.Xna.Framework.Graphics;

namespace Conveyer.Screens {

    class GameScreen : IScreen {

        private ScreenManager manager;
        private GameInstance game;

        public GameScreen(ScreenManager manager) {
            this.manager = manager; // For access to screen changing.

            game = new GameInstance(manager);
        }

        public void CheckInput() {
            
        }

        public void Set() {
            if(game.Ended) {
                game = new GameInstance(manager);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {   
            game.Draw(spriteBatch);
        }

        public void Update() {
            game.Update();
        }


    }

}
