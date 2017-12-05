using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Conveyer.GameClasses;

namespace Conveyer {

    class ContentChest {

        // Instance of our contentchest, for singleton access.
        private static ContentChest instance;

        public ContentManager Content;

        public Texture2D bigBox;
        public Texture2D smallBox;
        public Texture2D fragileBox;
        public Texture2D bigBoxPackaged;
        public Texture2D smallBoxPackaged;
        public Texture2D fragileBoxPackaged;

        public Song theme;


        /* Singletons make it easy to access your public elements across all of your classes. It's a common thing to do, although not the best,
         * due to classes having access when you may not necessarily want them to. So preferably passsing the contentchest class to class is ideal. */

        public static ContentChest Instance { // Singleton use.
            get {
                if (instance == null) { // If the static instance on line 8 is no existent then we initialise it.
                    instance = new ContentChest(); 
                }
                return instance; // We return the instance.
            }
        }

        // Store all of our textures in an array. This should be faster than a dictionary, or a stacked list.
        public Texture2D[] conveyer = new Texture2D[4];
        public Texture2D[] floor = new Texture2D[1];
        public Texture2D[] boxSpawner = new Texture2D[1];

        // Main menu specific
        public Texture2D logo;
        public Texture2D[] buttons = new Texture2D[2];

        public Texture2D head;
        public Texture2D[] headWalk = new Texture2D[2];
        public Texture2D idleLegs;
        public Texture2D[] walkLegs = new Texture2D[7];
        public Texture2D[] bodyCarry = new Texture2D[2];
        public Texture2D body;

        public Texture2D pixel;

        public Texture2D lowerUI;

        public SoundEffect buttonSound;
        public SoundEffect wrong;
        public SoundEffect correct;
        public SoundEffect heartbeat;
        public SoundEffect zap;
        public SoundEffect fix;
        public SoundEffect machineBreak;


        public SoundEffectInstance heartBeatInstance;

        // UI Textures
        public Texture2D orderBar;
        public Texture2D orderHolder;
        public Texture2D orderSlip;

        public Texture2D tick;

        public Dictionary<string, Texture2D> smallItems = new Dictionary<string, Texture2D>();
        public Dictionary<string, Texture2D> fragileItems = new Dictionary<string, Texture2D>();
        public Dictionary<string,Texture2D> bigItems = new Dictionary<string, Texture2D>();

        public SoundEffect[] noOrderSounds;
        public SoundEffect[] droppedBoxSounds;
        public SoundEffect[] wrongBoxSounds;
        public SoundEffect[] fixBeltSounds;

        // Fonts

        public SpriteFont defaultFont;
        public SpriteFont scoreFont;
        public SpriteFont smallFont;
        public SpriteFont scoreFont2;

        public Texture2D[] sparks = new Texture2D[4];

        public ContentChest() {
        }

        public void Load() {
            // Loads all of our assets.

            for(int i = 0; i < conveyer.Length; i++) {
                conveyer[i] = Content.Load<Texture2D>("Conveyer/" + (i + 1));
            }

            floor[0] = Content.Load<Texture2D>("Floor/1");

            logo = Content.Load<Texture2D>("Logo/logo");
            buttons[0] = Content.Load<Texture2D>("Buttons/startButton");
            buttons[1] = Content.Load<Texture2D>("Buttons/quitButton");

            scoreFont2 = Content.Load<SpriteFont>("Fonts/scoreFont2");
            defaultFont = Content.Load<SpriteFont>("Fonts/defaultFont");
            smallFont = Content.Load<SpriteFont>("Fonts/smallFont");
            scoreFont = Content.Load<SpriteFont>("Fonts/scoreFont");

            buttonSound = Content.Load<SoundEffect>("Sounds/buttonSound");
            wrong = Content.Load<SoundEffect>("Sounds/wrong");
            correct = Content.Load<SoundEffect>("Sounds/correct");
            machineBreak = Content.Load<SoundEffect>("Sounds/break");
            zap = Content.Load<SoundEffect>("Sounds/zp");
            fix = Content.Load<SoundEffect>("Sounds/fix");
            heartbeat = Content.Load<SoundEffect>("Sounds/heartbeat");

            pixel = Content.Load<Texture2D>("UI/pixel");
            head = Content.Load<Texture2D>("Worker/Head/head1");
            body = Content.Load<Texture2D>("Worker/Body/body");

            
            for (int i = 1; i <= 2; i++) {
                headWalk[i - 1] = Content.Load<Texture2D>("Worker/Head/head" + (i));
            }

            for (int i = 0; i < 7; i++) {
                walkLegs[i] = Content.Load<Texture2D>("Worker/Legs/walk" + (i + 1));
            }

            idleLegs = Content.Load<Texture2D>("Worker/Legs/idleLegs");
            for (int i = 0; i < 2; i++) {
                bodyCarry[i] = Content.Load<Texture2D>("Worker/Body/bodyWalk" + (i + 1));
            }

            fragileItems.Add("Cups", Content.Load<Texture2D>("Items/Fragile/item1"));
            fragileItems.Add("Vase", Content.Load<Texture2D>("Items/Fragile/item2"));
            fragileItems.Add("Shades", Content.Load<Texture2D>("Items/Fragile/item3"));

            bigItems.Add("Teddy", Content.Load<Texture2D>("Items/Big/item1"));
            bigItems.Add("Spade", Content.Load<Texture2D>("Items/Big/item2"));
            bigItems.Add("Drill", Content.Load<Texture2D>("Items/Big/item3"));
            bigItems.Add("Television", Content.Load<Texture2D>("Items/Big/item4"));


            smallItems.Add("Book", Content.Load<Texture2D>("Items/Small/item1"));
            smallItems.Add("Oil", Content.Load<Texture2D>("Items/Small/item2"));
            smallItems.Add("Reader", Content.Load<Texture2D>("Items/Small/item3"));
            smallItems.Add("Jewellery", Content.Load<Texture2D>("Items/Small/item4"));

            orderBar = Content.Load<Texture2D>("UI/orderBar");
            orderHolder = Content.Load<Texture2D>("UI/order");
            orderSlip = Content.Load<Texture2D>("UI/orderSlip");

            bigBox = Content.Load<Texture2D>("Boxes/bigBox");
            smallBox = Content.Load<Texture2D>("Boxes/smallBox");
            fragileBox = Content.Load<Texture2D>("Boxes/fragileBox");

            bigBoxPackaged = Content.Load<Texture2D>("Boxes/bigBoxPackaged");
            smallBoxPackaged = Content.Load<Texture2D>("Boxes/smallBoxPackaged");
            fragileBoxPackaged = Content.Load<Texture2D>("Boxes/fragileBoxPackaged");

            theme = Content.Load<Song>("Music/theme");

            tick = Content.Load<Texture2D>("UI/tick");

            lowerUI = Content.Load<Texture2D>("UI/lowerUI");
            boxSpawner[0] = Content.Load<Texture2D>("Conveyer/boxSpawner1");

            for(int i = 0; i < sparks.Length; i++) {
                sparks[i] = Content.Load<Texture2D>("SparkAnimation/spark" + (i + 1));
            }
            heartBeatInstance = ContentChest.Instance.heartbeat.CreateInstance();

            // order Sounds

            wrongBoxSounds = new SoundEffect[2];
            noOrderSounds = new SoundEffect[4];
            droppedBoxSounds = new SoundEffect[1];
            fixBeltSounds = new SoundEffect[2];

            for(int i = 0; i < fixBeltSounds.Length; i++) {
                fixBeltSounds[i] = Content.Load<SoundEffect>("Sounds/fixBelt/" + (i + 1));
            }

            for(int i = 0; i < wrongBoxSounds.Length; i++) {
                wrongBoxSounds[i] = Content.Load<SoundEffect>("Sounds/wrongBox/" + (i + 1));
            }

            for (int i = 0; i < noOrderSounds.Length; i++) {
                noOrderSounds[i] = Content.Load<SoundEffect>("Sounds/noOrder/" + (i + 1));
            }

            for (int i = 0; i < droppedBoxSounds.Length; i++) {
                droppedBoxSounds[i] = Content.Load<SoundEffect>("Sounds/boxFloor/" + (i + 1));
            }
        }

    }

}
