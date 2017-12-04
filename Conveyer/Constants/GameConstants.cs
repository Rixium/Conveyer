using Conveyer.GameClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyer.Constants {

    class GameConstants {

        public static int GAME_WIDTH = 1280;
        public static int GAME_HEIGHT = 720;

        public static int MAP_SIZE_HORIZONTAL = 16;
        public static int MAP_SIZE_VERTICAL = 8;
        public static int TILE_SIZE = 16;

        public static int CAM_SPEED = 2;
        public static int CAM_ZOOM = 4;

        public static int TOP_SCORE = 0;

        public static Item itemToAdd = null;
        public static Box ACTIVE_BOX;
        public static bool ItemConveyerRunning = true;
        public static bool BoxConveyerRunning = true;
        public static Item item;

    }
}
