/* Author: Ali Sher
 * File Name: Ninja Escape
 * Creation Date: Jan. 8th 2016
 * Modification Date: Jan. 18th 2016
 * Description: Game for computer science ISU
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISUGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ISUGame : Microsoft.Xna.Framework.Game
    {

        //background variables
        Texture2D bgImg;
        Rectangle bgBox;
        Rectangle bgBox2;
        Texture2D floorImg;
        Rectangle floorBox;

        //Song for game
        Song bgSong;

        //Sound effects
        SoundEffect starBreaks;
        SoundEffectInstance starBreaksInst;
        SoundEffect jumpSound;
        SoundEffectInstance jumpSoundInst;

        //hit sound effects
        SoundEffect[] hitSound;
        SoundEffectInstance[] hitSoundInst;

        //Level number tracker
        int levelNum;
        const int LEVEL_ONE = 1;
        const int LEVEL_TWO = 2;
        const int LEVEL_THREE = 3;

        //load next level
        bool nextLevel;

        //Draw a line
        Texture2D line;

        //fonts variables
        SpriteFont smallText;

        //platform textures
        Texture2D platformImg;
        Rectangle[] platformBoxes;

        //variables for screen size
        int screenWidth;
        int screenLength;

        //variables for player animation
        Rectangle playerBox;
        Texture2D playerImg;
        Animation playerCharacter;
        int pFramesWide = 9;
        int pFramesHigh = 2;
        int pNumFrames = 18;
        int pSmoothness = 5;

        //player hitbox
        Rectangle pHitbox;

        //effects for changing forms
        Texture2D ghostImg;
        Texture2D poofImg;
        Rectangle poofBox;
        Animation poofCharacter;

        //ghostMode variables
        Texture2D ghostPickupImg;
        Rectangle[] ghostPickupBoxes;
        int ghostPercent;
        int ghostTimer;

        //ghostmode counter loc
        Rectangle ghostCounterBox;
        Vector2 ghostCounterLoc;
  

        //variables for tribal animation
        Rectangle tribalBox;
        Texture2D tribalImg;
        Animation[] tribalCharacter;
        int tFramesWide = 9;
        int tFramesHigh = 2;
        int tNumFrames = 18;
        int tSmoothness = 1;

        //variables for robot animation
        Rectangle robotBox;
        Animation[] robotCharacter;
        Texture2D robotImg;
        int rFramesWide = 9;
        int rFramesHigh = 2;
        int rNumFrames = 18;
        int rSmoothness = 1;

        //stat tracking numbers
        int enemiesKilled;
        int starsThrown;
        int timeInGhost;

        //ninja star variables
        Texture2D starImg;
        Rectangle starBox;
        Texture2D starPickupImg;
        Rectangle[] starPickupBoxes;
        int starCount;

        //star score counter variables
        Vector2 starCounterLoc;
        Rectangle starCounterBox;

        //sign variable
        Texture2D signImg;
        Rectangle signBox;

        //set gamestates
        int gamestate;
        const int PREGAME = 0;
        const int INGAME = 1;
        const int DEATHSCREEN = 3;
        const int INSTRUCTIONS = 4;
        const int EXIT_SCREEN = 5;
        const int ENDGAME = 6;

        //user input variables
        MouseState mouse;
        MouseState prevMouse;
        KeyboardState kb;
        KeyboardState prevKb;

        //player physics variables
        float pAngle = 90;
        float pSpeed = 20;
        Vector2 pVelocity;
        Vector2 pInitialVelocity;
        float pAcceleration;
        Rectangle playerFeetBox;

        //gravity
        Vector2 gravity;
        Vector2 fallingVelocity;

        //acceleration limit
        int accLimit = 10;

        //star physics variables
        float sAngle = 45;
        float sSpeed = 35;
        Vector2 sVelocity;
        Vector2 sInitialVelocity;
        bool wasThrown;

        //jump variable
        bool pjumped = false;
        bool pFalling = false;

        //tribal movement variables
        int tDirection = T_RIGHT;
        const int T_RIGHT = 0;
        const int T_LEFT = 1;
        const int T_SPEED = 2;

        //robot movement variables
        int rDirection = R_RIGHT;
        const int R_RIGHT = 0;
        const int R_LEFT = 1;
        const int R_SPEED = 2;

        //floor location
        int floorLoc;

        //CollisionDetection variables
        const bool COLLISION = true;
        const bool NO_COLLISION = false;

        //blood splatter effect variables
        Texture2D bloodEffect1;
        Texture2D bloodEffect2;
        Texture2D bloodEffect3;
        Rectangle bloodEffectBox;

        //variables that show when to show different blood effects
        int hitTimer;
        int hitCount;
        const int NO_EFFECT = 0;
        const int BLOOD_EFFECT_1 = 1;
        const int BLOOD_EFFECT_2 = 2;
        const int BLOOD_EFFECT_3 = 3;
        
        //mouse box variable
        Rectangle mouseBox;

        //button variables
        SpriteFont buttonFont;
        Texture2D whiteimg;
        Rectangle[] whiteBox;

        //menu screen vectors
        Vector2 startLoc;
        Vector2 intructionsLoc;

        //title screen
        Texture2D titleImg;
        Rectangle titleBox;

        //instructions box
        Texture2D instructionsImg;
        Rectangle instructionsBox;

        //endscreen variables
        Texture2D endMsgImg;
        Rectangle endMsgBox = new Rectangle(50, 50, 360, 40);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public ISUGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //show mouse
            this.IsMouseVisible = true;

            //Change screen size and resolution
            graphics.PreferredBackBufferWidth = 1070;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // set screenwidth and screenlength to the screen
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenLength = graphics.GraphicsDevice.Viewport.Height;

            //game starts in pregame
            gamestate = PREGAME;

            gravity = new Vector2(0, 100f / 60);
            //floor is located at the y of the floorBox but plus 20 to create a more realistic effect
            floorLoc = 710;

            //initialize star boxes for pickup
            starPickupBoxes = new Rectangle[10];

            //initialize ghost boxes for pick up
            ghostPickupBoxes = new Rectangle[10];

            //initialize platform boxes 
            platformBoxes = new Rectangle[10];

            //initialize number of tribals and robots
            tribalCharacter = new Animation[10];
            robotCharacter = new Animation[10];

            //Load level one 
            levelNum = LEVEL_ONE;

            //initialize buttonbox array
            whiteBox = new Rectangle[10];

            //initilize hit sound effects
            //player hit
            hitSound = new SoundEffect[3];
            hitSoundInst = new SoundEffectInstance[3];

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            line = new Texture2D(GraphicsDevice, 1, 1);
            line.SetData<Color>(new Color[] { Color.White });

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Load all content here
            bgImg = Content.Load<Texture2D>("Images/Background/CaveBG2");
            bgSong = Content.Load<Song>("Sounds/30 Minutes of Dark Suspense Scary Creepy Horror Music (Instrumental Halloween Music)");
            jumpSound = Content.Load<SoundEffect>("Sounds/Grunt Sound Effect For Free");
            starBreaks = Content.Load<SoundEffect>("Sounds/Concrete break");
            floorImg = Content.Load<Texture2D>("Images/Background/CaveBG");
            starImg = Content.Load<Texture2D>("Sprites/Object Sprites/ninjaStar2");
            starPickupImg = Content.Load<Texture2D>("Sprites/Object Sprites/starPickup");
            ghostPickupImg = Content.Load<Texture2D>("Sprites/Object Sprites/ghostPickup");
            platformImg = Content.Load<Texture2D>("Sprites/Object Sprites/platform");
            signImg = Content.Load<Texture2D>("Sprites/Object Sprites/woodenSign");
            hitSound[0] = Content.Load<SoundEffect>("Sounds/pHit1");
            hitSound[1] = Content.Load<SoundEffect>("Sounds/pHit2");
            hitSound[2] = Content.Load<SoundEffect>("Sounds/pHit3");

            //blood effect content load
            bloodEffect1 = Content.Load<Texture2D>("Effects/bloodEffect1");
            bloodEffect2 = Content.Load<Texture2D>("Effects/bloodEffect2");
            bloodEffect3 = Content.Load<Texture2D>("Effects/bloodEffect3");
            bloodEffectBox = new Rectangle(0, 0, screenWidth, screenLength);

            //background rectangle load
            bgBox = new Rectangle(0, 0, screenWidth, screenLength);
            bgBox2 = new Rectangle(screenWidth, 0, screenWidth, screenLength);
            floorBox = new Rectangle(0, 690, screenWidth, 200);

            //object boxes
            starBox = new Rectangle(-50, 0, starImg.Width, starImg.Height);

            //platform boxes
            for (int i = 0; i <= 9; i++)
            {
                platformBoxes[i] = new Rectangle(-100, -100, 200, 50);
            }

            //load player content
            playerImg = Content.Load<Texture2D>("Sprites/Player Sprites/Ninja Walk Right");
            ghostImg = Content.Load<Texture2D>("Sprites/Player Sprites/Ninja Ghost Right");
            playerBox = new Rectangle(0, 400, 75, 100);
            playerCharacter = new Animation(playerImg, pFramesWide, pFramesHigh, pNumFrames, 0, 0, Animation.ANIMATE_FOREVER, pSmoothness, playerBox, true, false, ghostImg);
            playerFeetBox = new Rectangle(0, 0, (playerCharacter.destRec.Width - 40), 25);

            //changing forms content loaded here
            poofImg = Content.Load<Texture2D>("Sprites/Object Sprites/poof");
            poofBox = new Rectangle(0, 0, playerCharacter.destRec.Width + 50, playerCharacter.destRec.Height + 50);
            poofCharacter = new Animation(poofImg, 5, 5, 23, 0, Animation.NO_IDLE, Animation.ANIMATE_FOREVER, 5, poofBox, true, false, ghostImg);

            //load tribal content
            tribalImg = Content.Load<Texture2D>("Sprites/NPC Folder/tribeEnemy");
            tribalBox = new Rectangle(0, -150, 75, 100);

            for (int i = 0; i < 10; i++)
            {
                tribalCharacter[i] = new Animation(tribalImg, tFramesWide, tFramesHigh, tNumFrames, 0, 0, Animation.ANIMATE_FOREVER, tSmoothness, tribalBox, true, false, ghostImg);
            }

            //load robot content
            robotImg = Content.Load<Texture2D>("Sprites/NPC Folder/robotEnemy");
            robotBox = new Rectangle(0, -150, 75, 100);

            for (int i = 0; i < 10; i++)
            {
                robotCharacter[i] = new Animation(robotImg, rFramesWide, rFramesHigh, rNumFrames, 0, 0, Animation.ANIMATE_FOREVER, rSmoothness, robotBox, true, false, ghostImg);
            }

            for (int i = 0; i < 10; i++)
            {
                starPickupBoxes[i] = new Rectangle(-200, 0, 25, 25);
                ghostPickupBoxes[i] = new Rectangle(-200, 0, 25, 25);
            }

            //starPickupBox for the user to track number of throwing stars
            starCounterBox = new Rectangle(10, screenLength - (starPickupImg.Height + 10), starPickupImg.Width, starPickupImg.Height);
            starCounterLoc = new Vector2(starCounterBox.X + 26, starCounterBox.Y - 15);

            ghostCounterBox = new Rectangle(60, 665, starPickupImg.Width, starPickupImg.Height);
            ghostCounterLoc = new Vector2(ghostCounterBox.X + 26, ghostCounterBox.Y - 15);

            //load fonts here
            smallText = Content.Load<SpriteFont>("Texts/smallText");
            buttonFont = Content.Load<SpriteFont>("Texts/ButtonFont");

            //menu content
            whiteimg = Content.Load<Texture2D>("Effects/whiteBox");
            //game
            whiteBox[0] = new Rectangle(190, 380, 200, 100);
            //instructions
            whiteBox[1] = new Rectangle(680, 380, 200, 100);

            //set vector2 for menu screen
            startLoc = new Vector2(260, 420);
            intructionsLoc = new Vector2(710, 420);

            //instructions content
            instructionsImg = Content.Load<Texture2D>("Effects/instructions");
            instructionsBox = new Rectangle(50, 50, 628, 404);

            //endgame content
            endMsgImg = Content.Load<Texture2D>("Effects/escapeMsg");

            //load title content
            titleImg = Content.Load<Texture2D>("Effects/title");
            titleBox = new Rectangle(335, 50, 400, 110);

            //Play song
            MediaPlayer.Play(bgSong);
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;

            //Creat soundeffect instances
            jumpSoundInst = jumpSound.CreateInstance();
            starBreaksInst = starBreaks.CreateInstance();

            for (int i = 0; i < 3; i++)
            {
                hitSoundInst[i] = hitSound[i].CreateInstance();
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here

            //user input checks
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevKb = kb;
            kb = Keyboard.GetState();

            //|||||||||||||||||||||IN MENU||||||||||||||||||||||||||||\\

            if (gamestate == PREGAME)
            {

                //reset important values
                playerCharacter.destRec.X = 0;
                playerCharacter.destRec.Y = 400;
                starCount = 0;
                ghostPercent = 0;
                hitCount = 0;
                enemiesKilled = 0;
                starsThrown = 0;
                timeInGhost = 0;

                //assign mouseRec for sick collisions
                mouseBox = new Rectangle(mouse.X, mouse.Y, 1, 1);

                //sick effect with start btn
                if (CollisionDetection(mouseBox, whiteBox[0]))
                {
                    whiteBox[0] = new Rectangle(165, 355, 250, 150);

                    //start game if mouse is clicked
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        gamestate = INGAME;

                        //generate levels
                        LevelGenerator();
                    }
                }
                else
                {
                    whiteBox[0] = new Rectangle(190, 380, 200, 100);
                }

                //sick effect with instructions btn
                if (CollisionDetection(mouseBox, whiteBox[1]))
                {
                    whiteBox[1] = new Rectangle(655, 355, 250, 150);

                    //go to leaderboard
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        gamestate = INSTRUCTIONS;
                    }
                }
                else
                {
                    whiteBox[1] = new Rectangle(680, 380, 200, 100);
                }

            }

            if (gamestate == INSTRUCTIONS)
            {
                //allow player to return to main menu
                if (kb.IsKeyDown(Keys.Escape) && prevKb.IsKeyUp(Keys.Escape))
                {
                    gamestate = PREGAME;
                }
            }

            //!!!!!!!!!!!!!!!!!!!!!!! IN GAME !!!!!!!!!!!!!!!!!!!!!!!!\\
            if (gamestate == INGAME)
            {

                //allow player to return to main menu
                if (kb.IsKeyDown(Keys.Escape))
                {
                    gamestate = PREGAME;
                }

                //Update all animations in gameTime
                playerCharacter.Update(gameTime);
                poofCharacter.Update(gameTime);

                for (int i = 0; i < 10; i++)
                {
                    tribalCharacter[i].Update(gameTime);
                    robotCharacter[i].Update(gameTime);
                }


                //poof should occur at player loc
                poofCharacter.destRec.Y = playerCharacter.destRec.Y - 25;
                poofCharacter.destRec.X = playerCharacter.destRec.X - 25;

                //player hitbox is in update so that its coordinates are updated to stay with player
                pHitbox = new Rectangle(playerCharacter.destRec.X + 5, playerCharacter.destRec.Y, 65, 100);

                //left walk
                if (kb.IsKeyDown(Keys.A) && pFalling == false)
                {
                    playerCharacter.destRec.X -= (int)pAcceleration;

                    //set limit
                    if (pAcceleration < accLimit)
                    {
                        pAcceleration += 0.2f;
                    }
                }

                //right walk
                if (kb.IsKeyDown(Keys.D) && pFalling == false)
                {
                    playerCharacter.destRec.X += (int)pAcceleration;

                    //set limit
                    if (pAcceleration < accLimit)
                    {
                        pAcceleration += 0.2f;
                    }
                }

                //reset accleration
                if (kb.IsKeyDown(Keys.D) && (kb.IsKeyDown(Keys.A)))
                {
                    pAcceleration = 0;
                }
                if (kb.IsKeyUp(Keys.D) && (kb.IsKeyUp(Keys.A)))
                {
                    pAcceleration = 0;
                }


                //JUMP LOGIC AND OTHER PHYSICS CALCS

                //if character is in freefall
                if (!(CollisionDetection(playerCharacter.destRec, floorBox))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[0]))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[1]))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[2]))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[3]))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[4]))
                    && !(CollisionDetection(playerCharacter.destRec, platformBoxes[5]))
                    && pjumped == false)
                {

                    fallingVelocity = CalcVelocity(10, -85);
                    fallingVelocity = fallingVelocity + gravity;
                    playerCharacter.destRec.Offset((int)fallingVelocity.X, (int)fallingVelocity.Y);
                    pFalling = true;
                }
                else
                {
                    pFalling = false;
                }

                //calculate the speed based on how many stars are in the inventory and whether the player is in ghost mode
                if (playerCharacter.isGhost == false)
                {
                    pSpeed = 19 - (starCount);
                }
                else
                {
                    pSpeed = 19;
                }

                //Check if player jsut jumped
                if (!pjumped && pFalling == false)
                {
                    if (kb.IsKeyDown(Keys.Space))
                    {
                        pVelocity = CalcVelocity(pSpeed, pAngle);
                        pjumped = true;

                        //play sound only in human mode
                        if (playerCharacter.isGhost == false)
                        {
                            //play jump sound effect
                            jumpSoundInst.Play();
                        }
                    }
                }


                if (pjumped == true)
                {
                    //set accleration to 5 to reduce travel
                    pAcceleration = 5;

                    //assign calculations to velocity 
                    pVelocity = pVelocity + gravity;
                    pInitialVelocity = pVelocity;

                    playerCharacter.destRec.Offset((int)pVelocity.X, (int)pVelocity.Y);

                    //check to see when to reset jump
                    if (playerCharacter.destRec.Y > floorLoc - playerCharacter.destRec.Height)
                    {
                        playerCharacter.destRec.Y = floorLoc - playerCharacter.destRec.Height;
                        pjumped = false;
                        pVelocity = pInitialVelocity;
                    }
                }

                //Throw ninja star
                if (!wasThrown && playerCharacter.isGhost == false && starCount > 0)
                {
                    //calculate the trajectory here so player can input their own traj
                    sVelocity = CalcVelocity(sSpeed, sAngle);
                    sInitialVelocity = sVelocity;

                    if (kb.IsKeyDown(Keys.Enter) && prevKb.IsKeyUp(Keys.Enter))
                    {
                        //minus ninja stars when thrown
                        starCount--;

                        //star will appear at the start of the line used to measure the trajectory
                        starBox.X = playerCharacter.destRec.X + 25;
                        starBox.Y = playerCharacter.destRec.Y + 25;
                        wasThrown = true;

                        //add one to stars thrown
                        starsThrown++;
                    }
                }

                if (wasThrown == true)
                {
                    //physics on star
                    sVelocity = sVelocity + gravity;
                    starBox.Offset((int)sVelocity.X, (int)sVelocity.Y);

                    //stop star if it hits ground
                    if (starBox.Y > floorLoc - starBox.Height
                        || starBox.X > screenWidth //if star leaves the screen
                        || starBox.X < 0)          //it will reset
                    {
                        starBox.Y = floorLoc - starBox.Height;
                        wasThrown = false;
                        sVelocity = sInitialVelocity;
                    }
                }

                //allow user input to determine the angle of the ninja star toss
                if (kb.IsKeyDown(Keys.Up) && sAngle < 90)
                {
                    sAngle += 5f;
                }

                if (kb.IsKeyDown(Keys.Down) && sAngle > -90)
                {
                    sAngle -= 5f;
                }

                //Transform to ghost
                if (kb.IsKeyDown(Keys.E) && playerCharacter.isGhost == false && ghostPercent > 0)
                {
                    //set ghost timer to 0 everytime human transforms to ghost
                    ghostTimer = 0;
                    playerCharacter.isGhost = true;
                }

                //transform back to human
                if (kb.IsKeyDown(Keys.Q) && playerCharacter.isGhost == true)
                {
                    playerCharacter.isGhost = false;
                }

                //check for collision between star pickups and the player
                for (int i = 0; i <= 8; i++)
                {
                    if (CollisionDetection(playerCharacter.destRec, starPickupBoxes[i]))
                    {
                        starCount++;
                        starPickupBoxes[i].Y = -100;
                    }
                }

                //check for collision between ghost pickups and the player
                for (int i = 0; i <= 8; i++)
                {
                    if (CollisionDetection(playerCharacter.destRec, ghostPickupBoxes[i]) && ghostPercent < 100)
                    {
                        //only increase by 25 if at 75
                        if (ghostPercent <= 70)
                        {
                            ghostPercent += 30;
                        }
                        else
                        {
                            ghostPercent = 100;
                        }

                        ghostPickupBoxes[i].Y = -100;
                    }
                }

                //decrease ghost percent when in ghost mode
                if (playerCharacter.isGhost == true)
                {
                    ghostTimer++;

                    if (ghostTimer == 60)
                    {
                        //add one to seconds in ghost
                        timeInGhost += 1;

                        //decrease ghost percent
                        ghostPercent -= 10;
                        ghostTimer = 0;
                    }

                    //kick player out of ghostmode
                    if (ghostPercent == 0)
                    {
                        playerCharacter.isGhost = false;
                    }
                }

                //check collision between all platforms and star
                for (int i = 0; i < 10; i++)
                {
                    if (CollisionDetection(starBox, platformBoxes[i]))
                    {
                        //remove the star
                        starBox.X = -1000;
                        sVelocity = sInitialVelocity;

                        //play SoundEffect
                        starBreaksInst.Play();
                    }
                }

                //load player feet rectangle in update so it is always with the player
                playerFeetBox.X = playerCharacter.destRec.X + 20;
                playerFeetBox.Y = playerCharacter.destRec.Y + (playerCharacter.destRec.Height - (int)pSpeed);

                //Platform collision with all boxes, so if the playwer jumps on a platform, they can land on it
                PlatformDetection(playerCharacter.destRec, platformBoxes[0]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[1]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[2]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[3]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[4]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[5]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[6]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[7]);
                PlatformDetection(playerCharacter.destRec, platformBoxes[8]);


                //offset to next level if player reaches the sign
                if (CollisionDetection(playerCharacter.destRec, signBox) && levelNum == LEVEL_ONE)
                {
                    nextLevel = true;
                    levelNum = LEVEL_TWO;
                    LevelGenerator();
                }

                //go to level 3
                if (CollisionDetection(playerCharacter.destRec, signBox) && levelNum == LEVEL_TWO)
                {
                    nextLevel = true;
                    levelNum = LEVEL_THREE;
                    LevelGenerator();
                }

                //go to the end game screen
                if (CollisionDetection(playerCharacter.destRec, signBox) && levelNum == LEVEL_THREE)
                {
                    gamestate = ENDGAME;
                }

                //bool that when true moves everything from the screen
                if (nextLevel == true)
                {
                    //move background
                    bgBox.X -= 10;
                    bgBox2.X -= 10;

                    //move platforms and enemies
                    for (int i = 0; i < 10; i++)
                    {
                        platformBoxes[i].X -= 10;
                        tribalCharacter[i].destRec.X -= 10;
                        robotCharacter[i].destRec.X -= 10;
                        ghostPickupBoxes[i].X -= 10;
                        starPickupBoxes[i].X -= 10;
                    }

                    //move sign
                    signBox.X -= 10;

                    if (bgBox.X <= -screenWidth)
                    {
                        bgBox.X = screenWidth;
                    }

                    if (bgBox2.X <= -screenWidth)
                    {
                        bgBox2.X = screenWidth;
                        nextLevel = false;
                    }
                }


                //check for hits between player and tribal
                hitTimer++;

                //only check for hits 2 seconds apart
                if (hitTimer >= 120)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (CollisionDetection(pHitbox, tribalCharacter[i].destRec) && playerCharacter.isGhost == false)
                        {
                            //create a knockback effect
                            pVelocity = CalcVelocity(pSpeed, pAngle);
                            pjumped = true;

                            //add one to hitCount for visuals control
                            hitCount = hitCount + 1;

                            //reset hit timer
                            hitTimer = 0;

                            //play sound effect
                            HitSound();
                        }
                    }
                }

                //kill the player if they are hit 4 times
                if (hitCount == 4)
                {
                    gamestate = DEATHSCREEN;
                }

                //keep the player within the screen
                if (playerCharacter.destRec.X < 0)
                {
                    playerCharacter.destRec.X = 0;
                }

                if (playerCharacter.destRec.X > screenWidth - playerCharacter.destRec.Width)
                {
                    playerCharacter.destRec.X = screenWidth - playerCharacter.destRec.Width;
                }



                //TRIBAL MAN

                //move tribals
                //movement for level 1
                if (levelNum == LEVEL_ONE)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        //control the movement of tribals on the platforms
                        if (tribalCharacter[i].destRec.Right > platformBoxes[i].Right)
                        {
                            tDirection = T_LEFT;
                        }

                        if (tribalCharacter[i].destRec.Left < platformBoxes[i].Left)
                        {
                            tDirection = T_RIGHT;
                        }
                    }
                }

                //movement for level 2
                if (levelNum == LEVEL_TWO)
                {
                    for (int i = 3; i < 6; i++)
                    {
                        //control the movement of tribals on the platforms
                        if (tribalCharacter[i].destRec.Right > platformBoxes[i].Right)
                        {
                            tDirection = T_LEFT;
                        }

                        if (tribalCharacter[i].destRec.Left < platformBoxes[i].Left)
                        {
                            tDirection = T_RIGHT;
                        }
                    }
                }


                //ROBOT MAN
                //movement for level 3
                if (levelNum == LEVEL_THREE)
                {
                    for (int i = 6; i < 9; i++)
                    {
                        //control the movement of tribals on the platforms
                        if (robotCharacter[i].destRec.Right > platformBoxes[i].Right)
                        {
                            rDirection = R_LEFT;
                        }

                        if (robotCharacter[i].destRec.Left < platformBoxes[i].Left)
                        {
                            rDirection = R_RIGHT;
                        }
                    }
                }


                //CHEAT CODES
                if (kb.IsKeyDown(Keys.N) && prevKb.IsKeyUp(Keys.N))
                {
                    starCount++;
                    ghostPercent += 10;
                }

                if (kb.IsKeyDown(Keys.M) && prevKb.IsKeyUp(Keys.M))
                {
                    starCount--;
                    ghostPercent -= 10;
                }
            }


            if (gamestate == DEATHSCREEN)
            {
                //let player return to menu
                if (kb.IsKeyDown(Keys.Enter))
                {
                    gamestate = PREGAME;
                }
            }




            if (gamestate == ENDGAME)
            {

                // Allows the game to exit
                if (kb.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                //set player to middle, for effect

                playerCharacter.destRec.X = 500;
                playerCharacter.destRec.Y = 330;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Draw BG
            spriteBatch.Draw(bgImg, bgBox, Color.White);

            //Draw game if in menu
            if (gamestate == PREGAME)
            {
                //Draw buttons
                //game
                spriteBatch.Draw(whiteimg, whiteBox[0], Color.DarkBlue);
                spriteBatch.DrawString(buttonFont, "Start!", startLoc, Color.Red);
                //instructions
                spriteBatch.Draw(whiteimg, whiteBox[1], Color.DarkBlue);
                spriteBatch.DrawString(buttonFont, "Instructions", intructionsLoc, Color.Red);

                //Draw title
                spriteBatch.Draw(titleImg, titleBox, Color.White);
            }

            if (gamestate == INSTRUCTIONS)
            {
                spriteBatch.Draw(instructionsImg, instructionsBox, Color.White);
            }

            if (gamestate == INGAME)
            {
                //Draw background
                spriteBatch.Draw(bgImg, bgBox, Color.White);
                spriteBatch.Draw(bgImg, bgBox2, Color.White);
                spriteBatch.Draw(floorImg, floorBox, Color.White);
                //Draw ninja star
                spriteBatch.Draw(starImg, starBox, Color.White);
                //Draw ninja starPickups
                for (int i = 0; i <= 8; i++)
                {
                    spriteBatch.Draw(starPickupImg, starPickupBoxes[i], Color.White);
                }

                //sign symbol drawn here
                spriteBatch.Draw(signImg, signBox, Color.White);

                //Draw ghost pickups
                for (int i = 0; i <= 8; i++)
                {
                    spriteBatch.Draw(ghostPickupImg, ghostPickupBoxes[i], Color.White);
                }

                //Draw platforms
                for (int i = 0; i <= 9; i++)
                {
                    spriteBatch.Draw(platformImg, platformBoxes[i], Color.White);
                }

                //Draw a line for the ninja star
                if (playerCharacter.isGhost == false)
                {
                    DrawLine(spriteBatch, new Vector2(playerCharacter.destRec.X + 50, playerCharacter.destRec.Y + 50), new Vector2(playerCharacter.destRec.X - 15, playerCharacter.destRec.Y - 15));
                }

                //Draw runner
                //left walk
                if (kb.IsKeyDown(Keys.A) && kb.IsKeyUp(Keys.D))
                {
                        playerCharacter.Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                }

                //right walk
                if (kb.IsKeyDown(Keys.D) && kb.IsKeyUp(Keys.A))
                {
                        playerCharacter.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                }

                //idle stance
                if (kb.IsKeyUp(Keys.A) && kb.IsKeyUp(Keys.D) || (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.D)))
                {
                        playerCharacter.isAnimating = false;
                        playerCharacter.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                }
                else
                {
                        playerCharacter.isAnimating = true;
                }

                //Draw tribal man
                //walking to right
                if (tDirection == T_RIGHT)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        tribalCharacter[i].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                        tribalCharacter[i].destRec.X += T_SPEED;

                    }
                }

                //walking to left
                if (tDirection == T_LEFT)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        tribalCharacter[i].Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                        tribalCharacter[i].destRec.X -= T_SPEED;
                    }
                }

                //Draw robots
                //walking to right
                if (rDirection == R_RIGHT)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        robotCharacter[i].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                        robotCharacter[i].destRec.X += T_SPEED;

                    }
                }

                //walking to left
                if (rDirection == R_LEFT)
                {
                    for (int i = 0; i <= 9; i++)
                    {
                        robotCharacter[i].Draw(spriteBatch, Color.White, Animation.FLIP_HORIZONTAL);
                        robotCharacter[i].destRec.X -= T_SPEED;
                    }
                }

                //draw the poofAnimation when changing forms
                if (kb.IsKeyDown(Keys.Q) && ghostPercent > 0 || (kb.IsKeyDown(Keys.E)) && ghostPercent > 0)
                {
                    poofCharacter.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
                }

                //draw red cloud if there is collision
                for (int i = 0; i <= 9; i++)
                {
                    if (CollisionDetection(starBox, tribalCharacter[i].destRec))
                    {

                        //set X and Y to whichever character was hit
                        poofCharacter.destRec.Y = tribalCharacter[i].destRec.Y - 25;
                        poofCharacter.destRec.X = tribalCharacter[i].destRec.X - 25;

                        //move the tribal hit off screen
                        tribalCharacter[i].destRec.Y = -150;

                        poofCharacter.Draw(spriteBatch, Color.Red, Animation.FLIP_NONE);

                        //play sound effect
                        HitSound();

                        //add 1 to enemies killed counter and to level kills
                        enemiesKilled += 1;
                    }
                }

                //draw blue cloud if there is star collision robot
                for (int i = 0; i <= 9; i++)
                {
                    if (CollisionDetection(starBox, robotCharacter[i].destRec))
                    {

                        //set X and Y to whichever character was hit
                        poofCharacter.destRec.Y = robotCharacter[i].destRec.Y - 25;
                        poofCharacter.destRec.X = robotCharacter[i].destRec.X - 25;

                        //move the robot hit off screen
                        robotCharacter[i].destRec.Y = -150;

                        poofCharacter.Draw(spriteBatch, Color.Blue, Animation.FLIP_NONE);

                        //add 1 to enemies killed counter
                        enemiesKilled += 1;
                    }
                }

                //Draw HitEffects
                HitEffects();

                //ghostpickup symbol drawn here
                spriteBatch.Draw(ghostPickupImg, ghostCounterBox, Color.White);

                //starpickup symbol drawn here
                spriteBatch.Draw(starPickupImg, starCounterBox, Color.White);



                //DRAW FONTS HERE

                //star count font
                spriteBatch.DrawString(smallText, "" + starCount, starCounterLoc, Color.LightBlue);

                //ghostpercent font
                spriteBatch.DrawString(smallText, "" + ghostPercent + "%", ghostCounterLoc, Color.LightBlue);
            }

            if (gamestate == DEATHSCREEN)
            {
                spriteBatch.DrawString(buttonFont, "You DIED! \nPress enter to return to menu and retry! \n\n~~~STATS~~~ \nEnemies Killed: " + enemiesKilled + "\nStars Thrown: " + starsThrown + "\nTime as Ghost: " + timeInGhost + " seconds", new Vector2(50, 50), Color.LightBlue);
            }




            if (gamestate == ENDGAME)
            {

                //draw endgame msg
                spriteBatch.Draw(endMsgImg, endMsgBox, Color.White);

                //draw character for dramatic effect
                playerCharacter.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

       
        
        
        //||||||||||||||SUBPROGRAMS||||||||||||||\\


        //Pre: boxes/objects that we will be testing for collision
        //Post: We get a bool respoence as to whether or not there is collision
        //Description: Using two different rectangles, we use different criteria to know whether or not 
        //the boxes we are testing for ever make "contact"
        private bool CollisionDetection(Rectangle box1, Rectangle box2)
        {
            if (!(
                box1.Bottom < box2.Top ||
                box1.Right < box2.Left ||
                box1.Top > box2.Bottom ||
                box1.Left > box2.Right))
            {
                return COLLISION;
            }
            else
            {
                return NO_COLLISION;
            }
        }


        //Pre: speed, and angle that will be used to calculate velocity
        //Post: a vector 2 variable
        //Description: By using angles and trajectory calculations, we can find how an object would travel bbased on set angle and speed
        //to create a realistic falling effect for any object
        private Vector2 CalcVelocity(float speed, float angle)
        {
            float x = speed * (float)Math.Cos(MathHelper.ToRadians(angle));
            float y = speed * (float)(MathHelper.ToRadians(angle)) * -1;

            return new Vector2(x, y);
        }

        //Pre: spriteBatch, a vector 2 that points out the beggining and one that points out the end of the line
        //Post: A line will be drawn based on certain criteria
        //Description: A line is drawn with an angle that can be manipulated by the users input. Based on certain situations, 
        //the line will also be drawn in either green or red
        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle = MathHelper.ToRadians(-(sAngle));

            //draw a green line if there is a line
            if (starCount > 0)
            {
                spriteBatch.Draw(line, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, Color.GreenYellow, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            }

            //draw a red line if there are no stars
            else
            {
                spriteBatch.Draw(line, new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), 1), null, Color.Red, angle, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }

        //Pre: None
        //Post: Platforms, enemies, and pickups will be placed on the screen
        //Description: This subprogram generates predesigned levels based on the progression of the player
        private void LevelGenerator()
        {

            switch (levelNum)
            {
                case LEVEL_ONE:

                    //draw platforms and enemy
                    platformBoxes[0].X = 100;
                    platformBoxes[0].Y = 480;

                    tribalCharacter[0].destRec.X = platformBoxes[0].X;
                    tribalCharacter[0].destRec.Y = platformBoxes[0].Y - tribalCharacter[0].destRec.Height;


                    platformBoxes[1].X = 475;
                    platformBoxes[1].Y = 375;

                    tribalCharacter[1].destRec.X = platformBoxes[1].X;
                    tribalCharacter[1].destRec.Y = platformBoxes[1].Y - tribalCharacter[1].destRec.Height;


                    platformBoxes[2].X = 900;
                    platformBoxes[2].Y = 480;

                    tribalCharacter[2].destRec.X = platformBoxes[2].X;
                    tribalCharacter[2].destRec.Y = platformBoxes[2].Y - tribalCharacter[2].destRec.Height;

                    //place stars
                    starPickupBoxes[1].X = 300;
                    starPickupBoxes[1].Y = floorLoc - starPickupBoxes[0].Height;

                    starPickupBoxes[2].X = platformBoxes[0].X;
                    starPickupBoxes[2].Y = platformBoxes[0].Y - starPickupBoxes[2].Height;

                    //place ghost
                    ghostPickupBoxes[2].X = platformBoxes[2].X;;
                    ghostPickupBoxes[2].Y = platformBoxes[2].Y - ghostPickupBoxes[2].Height;

                    //draw sign
                    signBox = new Rectangle(platformBoxes[1].X + 100, platformBoxes[1].Y - 100, 75, 100);

                    break;

                case LEVEL_TWO:

                    //draw platforms and enemy
                    platformBoxes[3].X = 2170;
                    platformBoxes[3].Y = 480;

                    tribalCharacter[3].destRec.X = platformBoxes[3].X;
                    tribalCharacter[3].destRec.Y = platformBoxes[3].Y - tribalCharacter[3].destRec.Height;


                    platformBoxes[4].X = 2520;
                    platformBoxes[4].Y = 375;

                    tribalCharacter[4].destRec.X = platformBoxes[4].X;
                    tribalCharacter[4].destRec.Y = platformBoxes[4].Y - tribalCharacter[4].destRec.Height;


                    platformBoxes[5].X = 2870;
                    platformBoxes[5].Y = 275;

                    tribalCharacter[5].destRec.X = platformBoxes[5].X;
                    tribalCharacter[5].destRec.Y = platformBoxes[5].Y - tribalCharacter[5].destRec.Height;

                    //place stars
                    starPickupBoxes[3].X = 300;
                    starPickupBoxes[3].Y = floorLoc - starPickupBoxes[3].Height;

                    starPickupBoxes[5].X = platformBoxes[5].X;
                    starPickupBoxes[5].Y = platformBoxes[5].Y - starPickupBoxes[2].Height;

                    //place ghost
                    ghostPickupBoxes[3].X = platformBoxes[3].X;
                    ghostPickupBoxes[3].Y = floorLoc - ghostPickupBoxes[3].Height;

                    signBox = new Rectangle(platformBoxes[5].X + 100, platformBoxes[5].Y - 100, 75, 100);

                break;

                case LEVEL_THREE:

                //draw platforms and enemy
                    platformBoxes[6].X = 2250;
                    platformBoxes[6].Y = 380;

                    robotCharacter[6].destRec.X = platformBoxes[6].X;
                    robotCharacter[6].destRec.Y = platformBoxes[6].Y - tribalCharacter[3].destRec.Height;


                    platformBoxes[7].X = 2570;
                    platformBoxes[7].Y = 500;

                    robotCharacter[7].destRec.X = platformBoxes[7].X;
                    robotCharacter[7].destRec.Y = platformBoxes[7].Y - tribalCharacter[4].destRec.Height;


                    platformBoxes[8].X = 2870;
                    platformBoxes[8].Y = 500;

                    robotCharacter[8].destRec.X = platformBoxes[8].X;
                    robotCharacter[8].destRec.Y = platformBoxes[8].Y - tribalCharacter[5].destRec.Height;


                    starPickupBoxes[7].X = platformBoxes[6].X;
                    starPickupBoxes[7].Y = platformBoxes[6].Y - starPickupBoxes[2].Height;

                    //place ghost
                    ghostPickupBoxes[8].X = 2300;
                    ghostPickupBoxes[8].Y = floorLoc - ghostPickupBoxes[3].Height;

                    //draw sign
                    signBox = new Rectangle(platformBoxes[8].X + 100, platformBoxes[8].Y - 100, 75, 100);

                    break;
            }
        }

        //Pre: the players box, platform box
        //Post: the player will successfully land on a platform
        //Description: Allows the player to jump to and from a platform, resulting in proper collision
        private void PlatformDetection(Rectangle pBox, Rectangle platformBox)
        {
            if (CollisionDetection(pBox, platformBox))
            {
                if (CollisionDetection(playerFeetBox, platformBox) && pVelocity.Y > 0)
                {
                    playerCharacter.destRec.Y = platformBox.Y - pBox.Height;
                    pjumped = false;
                }
            }
        }

        //Pre: None
        //Post: Blood effect will be drawn on the screen
        //Description: Based on how many times the player is hit, the screen will be covered with a "bloody" effect to show how injured they are
        private void HitEffects()
        {
            switch (hitCount)
            {
                //if hit once
                case BLOOD_EFFECT_1:

                    {
                        //draw effect
                        spriteBatch.Draw(bloodEffect1, bloodEffectBox, Color.White);

                        break;
                    }

                //if hit twice
                case BLOOD_EFFECT_2:
                    {
                        //draw effect
                        spriteBatch.Draw(bloodEffect2, bloodEffectBox, Color.White);

                        break;
                    }

                //if hit thrice
                case BLOOD_EFFECT_3:
                    {
                        //draw effect
                        spriteBatch.Draw(bloodEffect3, bloodEffectBox, Color.White);

                        break;
                    }
            }
        }

        //Pre: None
        //Post: Create random sound effect from a selection of 3 for enemy or player
        //Description: Using an rng, create a hit sound effect for either the player or the enemy
        private void HitSound()
        {
            //creat rng
            Random rng = new Random();
            int rngHolder;
            rngHolder = rng.Next(1, 4);

            //generate sound effect based on whichever number
            switch (rngHolder)
            {
                //play sound 1
                case 1:
                    {
                        hitSoundInst[1].Play();
                        break;
                    }

                //play sound 2
                case 2:
                    {
                        hitSoundInst[2].Play();
                        break;
                    }

                //play sound 3
                case 3:
                    {
                        hitSoundInst[2].Play();
                        break;
                    }
            }
        }
    }
}
