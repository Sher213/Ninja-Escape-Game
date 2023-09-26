/* Author: Ali Sher
 * File Name: Animation
 * Creation Date: Jan. 8th 2016
 * Modification Date: Jan. 18th 2016
 * Description: Animation class that allows animations of any sprites, and also allows ghost form to be drawn for the player, if isGhost = true
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISUGame
{
    class Animation
    {
        //variables for use in the main program, effects and idling
        public const int NO_IDLE = -1;
        public const int ANIMATE_FOREVER = -1;
        public const SpriteEffects FLIP_NONE = SpriteEffects.None;
        public const SpriteEffects FLIP_HORIZONTAL = SpriteEffects.FlipHorizontally;

        //variables to draw each img from spritesheet
        Texture2D img;          
        int framesWide;     
        int framesHigh;     
        int frameNum;   
        int idleFrame;       
        int smoothness;         
        int smoothCount;        
        int repeatCount;
        Rectangle srcBox;
        Texture2D ghostImg;
       
        //outside use, based on the data entered
        public int curFrame;      
        public int frameWidth;      
        public int frameHeight;     
        public bool isAnimating;
        public Rectangle destRec;
        public bool isGhost;


        //Pre: draw data, img for ghost
        //Post: a drawn image that will animate
        //Description: This is what actually gets the values from the main to animate
        public Animation(Texture2D newImg, int newNumFramesWide, int newNumFramesHigh, int newTotalFrameCount, int newStartingFrameNum, int newIdleFrameNum, int newRepeatCount, int newSmoothRate, Rectangle newDestRec, bool startNow, bool newIsGhost, Texture2D newGhostImg)
        {
           
            img = newImg;
            framesWide = newNumFramesWide;
            framesHigh = newNumFramesHigh;
            frameNum = newTotalFrameCount;
            curFrame = newStartingFrameNum;
            idleFrame = newIdleFrameNum;
            repeatCount = newRepeatCount;
            smoothness = newSmoothRate;
            destRec = newDestRec;
            isAnimating = startNow;
            frameWidth = newImg.Width / framesWide;
            frameHeight = newImg.Height / framesHigh;
            srcBox = GetSourceRectangle();
            isGhost = newIsGhost;
            ghostImg = newGhostImg;
        }

        //Pre: GameTime variable
        //Post: Updates the animation
        //Description: Required to keep drawing the next frame
        public void Update(GameTime gameTime)
        {
            if (isAnimating == true)
            {
                if (smoothCount == 0)
                {
                    srcBox = GetSourceRectangle();

                    SetNextFrame();
                }

                smoothCount = (smoothCount + 1) & smoothness;
            }
        }

        //Pre: spritebatch, color, and flip
        //Post: drawn image of the animation
        //Description: This actually draws the image, and if the user wants an idle position, does that, as well as any flipping of the img
        public void Draw(SpriteBatch spriteBatch, Color color, SpriteEffects flipType)
        {
            
            if (isAnimating == true || (isAnimating == false && idleFrame != NO_IDLE))
            {
                if (isGhost == false)
                {
                    spriteBatch.Draw(img, destRec, srcBox, color, 0f, Vector2.Zero, flipType, 0);
                }
                else
                {
                    spriteBatch.Draw(ghostImg, destRec, srcBox, color, 0f, Vector2.Zero, flipType, 0);
                }
                                                  
            }
        }

        //Pre: None
        //Post: Source rectangle
        //Description: Goes through with the "cookie cutter", getting each img after the other to create the animation
        public Rectangle GetSourceRectangle()
        {
            
            int srcX = (curFrame % framesWide) * frameWidth;
            int srcY = (curFrame / framesWide) * frameHeight;

           
            return new Rectangle(srcX, srcY, frameWidth, frameHeight);
        }


        private void SetNextFrame()
        {
            
            curFrame = curFrame + 1;

           
            if (curFrame == frameNum)
            {
               
                if (repeatCount == ANIMATE_FOREVER)
                {
                    curFrame = 0;
                }
                else
                {
                    
                    repeatCount--;
                    curFrame = 0;

                   
                    if (repeatCount == 0)
                    {
                        isAnimating = false;
                    }
                }
            }
        }
    }
}


