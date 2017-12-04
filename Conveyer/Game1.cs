using Conveyer.Constants;
using Conveyer.Screens;
using Conveyer.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Conveyer.Constants.Enums;

namespace Conveyer {

    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager; // So we can easily switch screens, and stack.

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            IsMouseVisible = true;

            ContentChest.Instance.Content = Content; // Singleton instance for easy access. Don't do this in most cases.
            screenManager = new ScreenManager(); // Initialise the screen manager here, but start it later, after asset loading.
            screenManager.game = this;
            Window.Title = "Warehouse Worker";
            graphics.PreferredBackBufferWidth = GameConstants.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.GAME_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentChest.Instance.Load(); // Loads all the assets for use.
            screenManager.StartScreen(ScreenTypes.MAIN_MENU); // We'll store them using ENUM, this way we can create a new instance if its not created yet.
        }

        protected override void UnloadContent() {

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            InputManager.Instance.Update();
            screenManager.Screen.Update(); // Update our screen.
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            screenManager.Screen.Draw(spriteBatch); // Call our screens draw method.

            base.Draw(gameTime);
        }
    }
}
