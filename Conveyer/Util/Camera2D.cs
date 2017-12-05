using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Conveyer.Constants;

namespace Conveyer.Util
{

    public class Camera2D
    {

        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        private float startZoom;
        private float endZoom;
        protected float _rotation; // Camera Rotation
        private bool zoomIn = true, zoomOut, fastZooming;

        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        public void FastZoom() {
            fastZooming = true;
        }

        public float Zoom {
            get { return _zoom; }
            set { _zoom = value;
                if (_zoom < 0.1f) {
                    _zoom = 0.1f;
                }
                startZoom = _zoom;
                endZoom = _zoom + 0.03f;
            } // Negative zoom will flip image
        }

        public float Rotation {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(GameConstants.GAME_WIDTH / 2, GameConstants.GAME_HEIGHT / 2, 0));
            return _transform;
        }

        public void Update() {
            if(fastZooming) {
                if(zoomIn) {
                    if(_zoom < endZoom) {
                        _zoom += 0.001f;
                    } else {
                        zoomIn = false;
                        zoomOut = true;
                    }
                } else if(zoomOut) {
                    if(_zoom > startZoom) {
                        _zoom -= 0.001f;
                    } else {
                        zoomOut = false;
                        fastZooming = false;
                        zoomIn = true;
                        _zoom = startZoom;
                    }
                }
            }
            if (Keyboard.GetState().IsKeyDown(Constants.Keys.CAM_UP)) {
                _pos.Y -= GameConstants.CAM_SPEED;
            } else if (Keyboard.GetState().IsKeyDown(Constants.Keys.CAM_DOWN)) {
                _pos.Y += GameConstants.CAM_SPEED;
            }

            if (Keyboard.GetState().IsKeyDown(Constants.Keys.CAM_LEFT)) {
                _pos.X -= GameConstants.CAM_SPEED;
            } else if (Keyboard.GetState().IsKeyDown(Constants.Keys.CAM_RIGHT)) {
                _pos.X += GameConstants.CAM_SPEED;
            }
        }

        public bool Contains(Rectangle rectangle)
        {
            if (rectangle.X > Pos.X - GameConstants.GAME_WIDTH / 2 && rectangle.Y > Pos.Y - GameConstants.GAME_HEIGHT / 2)
            {
                if (rectangle.X < Pos.X + GameConstants.GAME_WIDTH / 2 && rectangle.Y < Pos.Y + GameConstants.GAME_HEIGHT / 2)
                {
                    return true;
                }
            }

            return false;
        }

    }

}