using Conveyer.Constants;
using Conveyer.GameClasses;
using Conveyer.Screens;
using Conveyer.UI;
using Conveyer.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Conveyer.GameClasses.Box;

namespace Conveyer.GameClasses {

    class GameInstance {

        private Player player;
        private List<Entity> entities = new List<Entity>();
        private Tile[,] tiles = new Tile[GameConstants.MAP_SIZE_HORIZONTAL, GameConstants.MAP_SIZE_VERTICAL];
        private BoxSpawner boxSpawner, boxKiller, itemSpawner, itemKiller;
        private UICover ui = new UICover();
        private List<Order> orders = new List<Order>();
        private int orderTimer = 100;
        private List<Item> items = new List<Item>();
        private int currOrderTimer = 0;
        bool heartbeating = false;
        private bool end = false;
        private bool start = true;
        private List<EpicText> texts = new List<EpicText>();
        private int score = 0;
        private int spawnTimer = 200;
        private int currentTimer;
        int forceSpawn = 0;

        int startCounter = 150;
        int clockDown = 3;
        private BoxMover itemBrokeBox, boxBrokeBox;

        private ScreenManager sm;
        Random random = new Random();

        private Camera2D cam = new Camera2D();

        public GameInstance(ScreenManager sm) {
            currentTimer = spawnTimer;
            this.sm = sm;
            int currMover = 0;
            for (int i = 0; i < GameConstants.MAP_SIZE_HORIZONTAL; i++) {
                for(int j = 0; j < GameConstants.MAP_SIZE_VERTICAL; j++) {
                    tiles[i, j] = new Tile(ContentChest.Instance.floor[0], i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE);

                    if(j == GameConstants.MAP_SIZE_VERTICAL - 1 || j == 0) {
                        if(i == GameConstants.MAP_SIZE_HORIZONTAL - 1 || i == 0) {
                            BoxSpawner mover = new BoxSpawner(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE);

                            if (j == GameConstants.MAP_SIZE_VERTICAL - 1 && i == 0) {
                                boxKiller = mover;
                            }
                            if (j == GameConstants.MAP_SIZE_VERTICAL - 1 && i == GameConstants.MAP_SIZE_HORIZONTAL - 1) {
                                boxSpawner = mover;
                            }

                            if (j == 0 && i == GameConstants.MAP_SIZE_HORIZONTAL - 1) {
                                itemSpawner = mover;
                            }

                            if (j == 0 && i == 0) {
                                itemKiller = mover;
                            }

                            entities.Add(mover);
                        } else {
                            BoxMover mover = null;
                            if (j == 0) {
                                mover = new BoxMover(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE, BoxMover.Type.item);
                            } else if (j == GameConstants.MAP_SIZE_VERTICAL - 1) {
                                mover = new BoxMover(i * GameConstants.TILE_SIZE, j * GameConstants.TILE_SIZE, BoxMover.Type.box);
                            }
                            entities.Add(mover);
                        }
                        currMover++;
                    }
                }
            }

            player = new Player(this);
            cam.Pos = new Microsoft.Xna.Framework.Vector2(player.Position.X, player.Position.Y);
            cam.Zoom = GameConstants.CAM_ZOOM;
            entities.Add(player);
            
        }

        public void AddItem(Item item) {
            if (GameConstants.ItemConveyerRunning) {
                item.Position = itemSpawner.Position;
                items.Add(item);
                entities.Add(item);
                /*
                if (GameConstants.ItemConveyerRunning) {
                    if (items.Count > 0) {
                        bool shouldSpawn = (random.Next(0, 5) == 1);
                        if (shouldSpawn) {
                            int randomItem = random.Next(0, items.Count);
                            entities.Add(items[randomItem]);
                        }
                    }
                }
                */
            }
        }

        public bool CheckCollisionWithOrders(Order order, Vector2 pos) {
            foreach(Order o in orders) {
                if (o != order) {
                    if (o.Contains(pos)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveOrder(Order o) {
            foreach(Order ord in new List<Order>(orders)) {
                if(ord == o) {
                    orders.Remove(o);
                    break;
                }
            }
        }

        public void Update() {
            if (!end && !start) {
                Item i = null;

                if (orders.Count > 0) {
                    if (currentTimer > 0) {
                        currentTimer--;
                    } else {
                        if (orders.Count > 5) {
                            if (!orders[0].IsComplete) {
                                currentTimer = spawnTimer;
                                Order o = orders[0];
                                GameConstants.itemToAdd = new Item(o.Item.Image, o.Item.Name, o.Item.boxType);
                            } else {
                                currentTimer = spawnTimer;
                            }
                        }
                    }
                }

                if (!boxSpawner.HasSpawned && GameConstants.itemToAdd != null && GameConstants.BoxConveyerRunning && GameConstants.ItemConveyerRunning) {   
                    i = new Item(GameConstants.itemToAdd.Image, GameConstants.itemToAdd.Name, GameConstants.itemToAdd.boxType);
                    Box b = new Box((int)boxSpawner.Position.X, (int)boxSpawner.Position.Y, i.boxType);
                    entities.Add(i);
                    entities.Add(b);
                    Console.Write(i.boxType.ToString() + " | " + b.boxType.ToString());
                    i.Position = itemSpawner.Position;
                    boxSpawner.Spawned();
                    forceSpawn = 150;
                    GameConstants.itemToAdd = null;
                    i = null;
                }

                if (forceSpawn > 0) {
                    forceSpawn--;
                }

                if (!boxSpawner.HasSpawned && GameConstants.itemToAdd == null) {
                    if (forceSpawn == 0) {
                        if (random.Next(0, 150) == 1) {
                            if (GameConstants.BoxConveyerRunning) {
                                entities.Add(new Box((int)boxSpawner.Position.X, (int)boxSpawner.Position.Y, BoxType.NONE));
                                boxSpawner.Spawned();
                            }
                        }
                    }
                    if (random.Next(0, 50) == 1) {
                        if (currOrderTimer <= 0) {
                            orders.Add(new Order(this));
                            currOrderTimer = orderTimer;
                        }
                    }
                }

                if (currOrderTimer > 0) {
                    currOrderTimer--;
                }
                entities = entities.OrderBy(o => o.Bounds.Y).ToList();

                bool showingText = false;

                bool itemBroke = false;
                bool boxBroke = false;
                bool hasFixed = false;
                bool showingBox = false;

                foreach (Entity e in new List<Entity>(entities)) {
                    e.Update();

                    if (e.GetType() == typeof(Box)) {
                        if (e.Left <= boxKiller.Left) {
                            entities.Remove(e);
                        }

                        if (e.GetType() == typeof(Box)) {
                            Box box = (Box)e;
                            if (GetDistance(new Vector2(box.DrawRect.X, box.DrawRect.Y), player.Position) <= 30) {
                                GameConstants.ACTIVE_BOX = box;
                                showingBox = true;
                            }
                        }
                    }

                    if (e.GetType() == typeof(BoxMover)) {
                        BoxMover bM = (BoxMover)e;
                        if (bM.Broke && bM.type == BoxMover.Type.item) {
                            itemBroke = true;
                            itemBrokeBox = bM;
                        }
                        if (bM.Broke && bM.type == BoxMover.Type.box) {
                            boxBroke = true;
                            boxBrokeBox = bM;
                        }

                        if (itemBroke) {
                            Vector2 pos = new Vector2(player.Position.X, itemSpawner.Position.Y);
                            if (GetDistance(player.Position, pos) <= 30) {
                                if (InputManager.Instance.KeyboardState.IsKeyDown(Keys.INTERACT) && InputManager.Instance.LastKeyState.IsKeyUp(Keys.INTERACT)) {
                                    itemBrokeBox.Fix();
                                    GameConstants.ItemConveyerRunning = true;
                                    itemBrokeBox = null;
                                    itemBroke = false;
                                    hasFixed = true;
                                    Celebrate();
                                    score += 5;
                                    texts.Add(new EpicText("+" + 5, player.Position));
                                    ContentChest.Instance.fix.Play();
                                    foreach (Entity ex in entities) {
                                        if (ex.GetType() == typeof(BoxMover)) {
                                            BoxMover boxMover2 = (BoxMover)ex;
                                            if (boxMover2.type == BoxMover.Type.item) {
                                                boxMover2.Fix();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (boxBroke) {
                            Vector2 pos = new Vector2(player.Position.X, boxSpawner.Position.Y);
                            if (GetDistance(player.Position, pos) <= 30) {
                                if (InputManager.Instance.KeyboardState.IsKeyDown(Keys.INTERACT) && InputManager.Instance.LastKeyState.IsKeyUp(Keys.INTERACT)) {
                                    boxBrokeBox.Fix();
                                    GameConstants.BoxConveyerRunning = true;
                                    boxBrokeBox = null;
                                    hasFixed = true;
                                    boxBroke = false;
                                    ContentChest.Instance.fix.Play();
                                    Celebrate();
                                    score += 5;
                                    texts.Add(new EpicText("+" + 5, player.Position));
                                    foreach (Entity ex in entities) {
                                        if (ex.GetType() == typeof(BoxMover)) {
                                            BoxMover boxMover2 = (BoxMover)ex;
                                            if (boxMover2.type == BoxMover.Type.box) {
                                                boxMover2.Fix();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (e.GetType() == typeof(Item)) {
                        if (!showingText) {
                            if (GetDistance(player.Center, e.Position) <= 20) {
                                GameConstants.item = (Item)e;
                                showingText = true;
                            }
                        }
                        if (e.Left <= itemKiller.Left) {
                            entities.Remove(e);
                        }
                    }
                }

                if (boxBroke) {
                    GameConstants.BoxConveyerRunning = false;
                } else {
                    GameConstants.BoxConveyerRunning = true;
                }

                if (itemBroke) {
                    GameConstants.ItemConveyerRunning = false;
                } else {
                    GameConstants.ItemConveyerRunning = true;
                }

                if (!showingText) {
                    GameConstants.item = null;
                }
                if (!showingBox) {
                    GameConstants.ACTIVE_BOX = null;
                }

                if (!hasFixed) {
                    if (InputManager.Instance.KeyboardState.IsKeyDown(Keys.INTERACT) && InputManager.Instance.LastKeyState.IsKeyUp(Keys.INTERACT)) {
                        if (GameConstants.item != null && player.Item == null) {
                            if (player.SetItem(GameConstants.item)) {
                                items.Remove(GameConstants.item);
                                entities.Remove(GameConstants.item);
                                GameConstants.item = null;
                            }
                        } else {
                            if (GetDistance(player.Position, new Vector2(player.Position.X, boxKiller.Position.Y)) >= 40) {
                                if (player.Item != null) {
                                    i = player.Item;
                                    player.RemoveItem();
                                    items.Add(i);
                                    score -= 10;
                                    texts.Add(new EpicText("-" + 10, player.Position));
                                    Celebrate();
                                    i.floored = true;
                                    i.Position = player.Position + player.bodyPos;
                                    entities.Add(i);
                                }
                            } else {
                                bool removePoints = true;
                                if(GameConstants.ACTIVE_BOX != null) {
                                    Box box = GameConstants.ACTIVE_BOX;
                                    if (player.Item != null) {
                                        if (box.boxType == player.Item.boxType) {
                                            if (box.Package()) {
                                                bool canComplete = false;
                                                foreach (Order o in new List<Order>(orders)) {
                                                    if (o.Item.Name.Equals(player.Item.Name)) {
                                                        if (o.Complete()) {
                                                            canComplete = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (canComplete) {
                                                    ContentChest.Instance.correct.Play();
                                                    items.Remove(player.Item);
                                                    score += 5;
                                                    texts.Add(new EpicText("+" + 5, player.Position));
                                                    Celebrate();
                                                    player.RemoveItem();
                                                    removePoints = false;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (removePoints) {
                                    ContentChest.Instance.wrong.Play();
                                    score--;
                                    texts.Add(new EpicText("-" + 1, player.Position));
                                    Celebrate();
                                }
                            }
                        }
                    }
                }

                int count = 0;
                foreach (Order o in new List<Order>(orders)) {
                    o.Update();
                    if (!o.IsComplete) {
                        count++;
                    }
                }

                if (count >= 28) {
                    if (!heartbeating) {
                        heartbeating = true;
                        ContentChest.Instance.heartBeatInstance.IsLooped = true;
                        ContentChest.Instance.heartBeatInstance.Play();
                        ContentChest.Instance.heartBeatInstance.Volume = 1f;
                    }
                } else {
                    if (heartbeating) {
                        ContentChest.Instance.heartBeatInstance.Stop();
                    }
                }

                if(orders.Count >= 40) {
                    end = true;
                    if (score > GameConstants.TOP_SCORE) {
                        GameConstants.TOP_SCORE = score;
                    }
                }

                if (heartbeating) {
                    Heartbeat();
                }

                cam._pos.X = player.Position.X;
                cam._pos.Y = player.Position.Y;

                foreach (EpicText t in new List<EpicText>(texts)) {
                    t.Update();
                    if (t.dead) {
                        texts.Remove(t);
                    }
                }

                cam.Update();
                InputManager.Instance.LastKeyState = InputManager.Instance.KeyboardState;
            } else {
                if(start) {
                    if (startCounter > 0) {
                        startCounter--;
                        if (startCounter % 50 == 0) {
                            clockDown--;
                            ContentChest.Instance.buttonSound.Play();
                        }
                    } else {
                        start = false;
                        startCounter = 0;
                        clockDown = 3;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(ContentChest.Instance.theme);
                    }
                }
                if(end) {
                    if(InputManager.Instance.KeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space)) {
                        sm.StartScreen(Enums.ScreenTypes.MAIN_MENU);
                    }
                }
            }
        }

        private static double GetDistance(Vector2 point1, Vector2 point2) {
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }

        public bool CheckCollision(Entity e) {
            foreach (Entity e2 in new List<Entity>(entities)) {
                if(!e2.Collidable) {
                    continue;
                }

                if (e != e2) {
                    if (e.Bounds.Intersects(e2.Bounds)) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Celebrate() {
            cam.FastZoom();
            ContentChest.Instance.zap.Play();
        }

        private void Heartbeat() {
            cam.FastZoom();
        }

        public bool Ended {
            get {
                return end;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, null, cam.get_transformation(spriteBatch.GraphicsDevice));

            foreach (Tile tile in tiles) {
                tile.Draw(spriteBatch);
            }

            foreach(Entity e in new List<Entity>(entities)) {
                e.Draw(spriteBatch);
            }

            foreach (EpicText t in new List<EpicText>(texts)) {
                t.Draw(spriteBatch);
            }

            spriteBatch.End();

            spriteBatch.Begin();
            ui.Draw(spriteBatch);

            foreach(Order o in new List<Order>(orders)) {
                o.Draw(spriteBatch);
            }

            Vector2 pos = Vector2.Zero;
            if (GameConstants.BoxConveyerRunning) {
                if (GameConstants.ACTIVE_BOX != null) {
                    if (player.Item != null) {
                        pos = new Vector2(10, GameConstants.GAME_HEIGHT - 30);
                        Color choice;
                        if(GameConstants.ACTIVE_BOX.boxType == player.Item.boxType) {
                            choice = Color.Green;
                        } else {
                            choice = Color.Red;
                        }

                        spriteBatch.DrawString(ContentChest.Instance.smallFont, "E - PUT " + player.Item.Name.ToUpper() +" IN " + GameConstants.ACTIVE_BOX.boxType.ToString() + " BOX", pos, choice);
                    }
                }
            } else {
                if (GetDistance(player.Position, new Vector2(player.Position.X, boxSpawner.Position.Y)) <= 40) {
                    pos = new Vector2(10, GameConstants.GAME_HEIGHT - 30);
                    spriteBatch.DrawString(ContentChest.Instance.smallFont, "E - FIX CONVEYER", pos, Color.White);
                }
            }

            if (GameConstants.ItemConveyerRunning) {
                if (GameConstants.item != null) {
                    Color choice;
                    if (!player.Carrying) {
                        choice = Color.Green;
                    } else {
                        choice = Color.Red;
                    }
                    pos = new Vector2(10, GameConstants.GAME_HEIGHT - 30);
                    spriteBatch.DrawString(ContentChest.Instance.smallFont, "E - PICK UP " + GameConstants.item.Name.ToString().ToUpper(), pos, choice); ;
                }
            } else {
                if (GetDistance(player.Position, new Vector2(player.Position.X, itemSpawner.Position.Y)) <= 40) {
                    pos = new Vector2(10, GameConstants.GAME_HEIGHT - 30);
                    spriteBatch.DrawString(ContentChest.Instance.smallFont, "E - FIX CONVEYER", pos, Color.White);
                }
            }
            
            pos = new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).X / 2, GameConstants.GAME_HEIGHT - 20 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y);
            spriteBatch.DrawString(ContentChest.Instance.scoreFont, score.ToString(), pos, Color.White);

            if (end || start) {
                if(start) {
                    spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle(0, 0, GameConstants.GAME_WIDTH, GameConstants.GAME_HEIGHT), Color.Black * 0.2f);
                    spriteBatch.DrawString(ContentChest.Instance.scoreFont, clockDown.ToString(), new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.scoreFont.MeasureString(clockDown.ToString()).X / 2, GameConstants.GAME_HEIGHT / 2 - ContentChest.Instance.scoreFont.MeasureString(clockDown.ToString()).Y / 2), Color.White);

                } else if (end) {
                    spriteBatch.Draw(ContentChest.Instance.pixel, new Rectangle(0, 0, GameConstants.GAME_WIDTH, GameConstants.GAME_HEIGHT), Color.Black * 0.8f);
                    spriteBatch.DrawString(ContentChest.Instance.scoreFont, "FINAL SCORE", new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.scoreFont.MeasureString("FINAL SCORE").X / 2, GameConstants.GAME_HEIGHT / 2 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y / 2 - 10 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y), Color.White);
                    spriteBatch.DrawString(ContentChest.Instance.scoreFont, score.ToString(), new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).X / 2, GameConstants.GAME_HEIGHT / 2 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y / 2), Color.White);
                    spriteBatch.DrawString(ContentChest.Instance.scoreFont, "Press SPACE to END", new Vector2(GameConstants.GAME_WIDTH / 2 - ContentChest.Instance.scoreFont.MeasureString("Press SPACE to END").X / 2, GameConstants.GAME_HEIGHT / 2 - ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y / 2 + 10 + ContentChest.Instance.scoreFont.MeasureString(score.ToString()).Y * 2), Color.White);
                }
                
            }

            spriteBatch.End();

            
        }

    }
}
