using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conveyer.Constants.Enums;

namespace Conveyer.Screens {

    class ScreenManager {

        // Holds all the screens we've made.
        private Dictionary<ScreenTypes, IScreen> screens = new Dictionary<ScreenTypes, IScreen>();

        public Game1 game;

        // Holds the active screen.
        private IScreen activeScreen;

        // Pass in a screen type value, and either create the screen if it doesn't exist, or set it as active screen.
        public void StartScreen(ScreenTypes screenType) {
            if(screens.ContainsKey(screenType)) { // Checks if the screen has been made.
                activeScreen = screens[screenType]; // If so it sets it as active screen.
                activeScreen.Set();
                return;
            } else {
                screens.Add(screenType, CreateScreen(screenType)); // If not, it makes the screen.
                activeScreen = screens[screenType]; // Sets the active screen to the one just made.
                if(activeScreen != null) { // If we could make the screen, return.
                    return;
                } else {
                    Console.WriteLine("SCREEN_MANAGER: Screen type not found."); // If not, print an error.
                }
            }
        }

        public void Exit() {
            game.Exit();
        }

        // Just a get, we don't want to set this manually, we want our manager to do that.
        public IScreen Screen {
            get {
                return activeScreen;
            }
        }

        private IScreen CreateScreen(ScreenTypes screenType) {
            switch(screenType) {
                case ScreenTypes.MAIN_MENU:
                    return new MainMenu(this); // Returns new main menu.
                case ScreenTypes.GAME_SCREEN:
                    return new GameScreen(this); // Returns new game screen.
                default:
                    /* If the screentype doesnt exist, then return null, this shouldn't happen anyway with the use of a enum, 
                     * due to predetermined values. */
                    return null;
            }
        }
    }
}
