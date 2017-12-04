using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.UI {

    class InputManager {

        private Rectangle mouseRect = new Rectangle(0, 0, 1, 1);
        private bool mouseClicked = false;

        private KeyboardState lastKeyState;
        private KeyboardState keyState;

        private static InputManager instance;

        public static InputManager Instance {
            get {
                if (instance == null) {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public void Update() {
            MouseInput();
        }

        private void MouseInput() {
            mouseRect.X = Microsoft.Xna.Framework.Input.Mouse.GetState().X;
            mouseRect.Y = Microsoft.Xna.Framework.Input.Mouse.GetState().Y;

            if(Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == ButtonState.Pressed) {
                mouseClicked = true;
            } else {
                mouseClicked = false;
            }
        }

        public Rectangle Mouse {
            get {
                return mouseRect;
            }
        }

        public Boolean MouseClicked {
            get {
                return mouseClicked;
            }
        }

        public KeyboardState KeyboardState {
            get {
                keyState = Keyboard.GetState();
                return keyState;
            }
        }

        public KeyboardState LastKeyState {
            get {
                return lastKeyState;
            }
            set {
                this.lastKeyState = value;
            }
        }

        public bool KeyPressed(Keys key) {
            if(keyState.IsKeyDown(key)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
