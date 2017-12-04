using Conveyer.Constants;
using Conveyer.GameClasses;
using Conveyer.UI;
using Conveyer.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Conveyer.Constants.Enums;

namespace Conveyer.Screens {

    class MainMenu : IScreen {

        private Button[] buttons = new Button[2];
        private int buttonPadding = 20;

        private List<Entity> entities = new List<Entity>();
        private Tile[,] tiles = new Tile[GameConstants.MAP_SIZE_HORIZONTAL, GameConstants.MAP_SIZE_VERTICAL];

        SpriteFont font;
        int ludumStringX;
        int ludumStringY;
        Vector2 ludumStringPos;
        string ludumString = "Ludum Dare 40";

        ScreenManager manager;


        private Camera2D cam = new Camera2D();

        public void Set() {

        }

        public MainMenu(ScreenManager manager) {
            cam.Zoom = 5;
            cam.Pos = new Vector2(GameConstants.MAP_SIZE_HORIZONTAL * GameConstants.TILE_SIZE / 2, GameConstants.MAP_SIZE_VERTICAL * GameConstants.TILE_SIZE / 2);
            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
            for (int i = 0; i < GameConstants.MAP_SIZE_HORIZONTAL; i++) {
                for (int j = 0; j < GameConstants.MAP_SIZE_VERTICAL; j++) {
                    tiles[i, j] = new Tile(ContentChest.Instance.floor[0], i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE);

                    if (j == GameConstants.MAP_SIZE_VERTICAL - 1 || j == 0) {
                        if (i == GameConstants.MAP_SIZE_HORIZONTAL - 1 || i == 0) {
                            BoxSpawner mover = new BoxSpawner(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE);
                            entities.Add(mover);
                        } else {
                            BoxMover mover = null;
                            if (j == 0) {
                                mover = new BoxMover(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE, BoxMover.Type.item);
                            } else if (j == GameConstants.MAP_SIZE_VERTICAL - 1) {
                                mover = new BoxMover(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE, BoxMover.Type.box);
                            }
                            mover.breakable = false;
                            entities.Add(mover);
                        }
                    }
                }
            }

            // Font for the footer.
            font = ContentChest.Instance.defaultFont;

            // Saves big parameters in the button initialising.
            int middleScreen = GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.buttons[0].Width / 2;
            int buttonYStart = GameConstants.GAME_HEIGHT / 2;

            // Initialising all of the buttons.
            for(int i = 0; i < buttons.Length; i++) {
                buttons[i] = new Button(ContentChest.Instance.buttons[i], new Vector2(middleScreen, buttonYStart), (ButtonTag)i);
                buttonYStart += buttonPadding + ContentChest.Instance.buttons[i].Height;
            }

            // Positioning of the footer string.
            ludumStringX = GameConstants.GAME_WIDTH / 2 - (int)font.MeasureString(ludumString).X / 2;
            ludumStringY = GameConstants.GAME_HEIGHT - (int)font.MeasureString(ludumString).Y - 10;
            ludumStringPos = new Vector2(ludumStringX, ludumStringY);

            this.manager = manager; // For access to screen changing.
        }
        
        public void Draw(SpriteBatch spriteBatch) {

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, cam.get_transformation(spriteBatch.GraphicsDevice));
            foreach (Tile tile in tiles) {
                tile.Draw(spriteBatch);
            }

            foreach (Entity e in entities) {
                e.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            
            // Draw the logo.
            spriteBatch.Draw(ContentChest.Instance.logo, 
                new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.logo.Width / 2, 200), Color.White);

            // Draw all the buttons.
            for(int i = 0; i < buttons.Length; i++) {
                buttons[i].Draw(spriteBatch);
            }

            // Draw our footer.
            spriteBatch.DrawString(ContentChest.Instance.defaultFont, ludumString, ludumStringPos, Color.White);
            Vector2 scorePos = new Vector2(GameConstants.GAME_WIDTH - ContentChest.Instance.scoreFont.MeasureString("TOP SCORE: " + GameConstants.TOP_SCORE).X - 20, 20);
            spriteBatch.DrawString(ContentChest.Instance.scoreFont, "TOP SCORE: " + GameConstants.TOP_SCORE, scorePos, Color.White);
            spriteBatch.End();
        }

        public void Update() {
            foreach(Entity e in entities) {
                e.Update();
            }
            // Check the input.
            CheckInput();
        }

        public void CheckInput() {
            // Buttons can be interacated with so update them here, according to input.
            for (int i = 0; i < buttons.Length; i++) {
                if (InputManager.Instance.Mouse.Intersects(buttons[i].Bounds)) {
                    buttons[i].Hover(true);
                    if (InputManager.Instance.MouseClicked) {
                        switch (buttons[i].Tag) {
                            case ButtonTag.START:
                                Console.WriteLine("MAIN_MENU: Changing Screen to Game Screen.");
                                manager.StartScreen(ScreenTypes.GAME_SCREEN); // Setting the screen to the game if start is hit.
                                break;
                            case ButtonTag.QUIT:
                                manager.Exit();
                                break;
                            default:
                                break;
                        }
                    }

                } else {
                    buttons[i].Hover(false);
                }
            }
        }

    }

}
