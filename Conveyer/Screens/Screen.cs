using Microsoft.Xna.Framework.Graphics;

namespace Conveyer.Screens {

    interface IScreen {

        void Update();

        void Draw(SpriteBatch spriteBatch);

        void CheckInput();

        void Set();
    }

}
