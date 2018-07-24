/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant le jeu lui m�me.        
 *                                          
 *      
 * **********************************************************
 *                      Modifications
 * 
 * Auteur:
 * Date:
 * But:
 * 
 * *********************************************************/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using System.IO.IsolatedStorage;
using System.IO;

namespace Nyan_Copter
{
    class PlayGame : GameState
    {

        private NyanCat Nyan;
        private List<DrawStar> lstStars;
        private List<Rainbow> lstRainbow;
        private List<DrawCourse> lstGround;
        private List<DrawCourse> lstSky;
        private SoundEffectInstance _soundNyan;
        private const int INT_NB_RAINBOW = 19;
        private const int INT_NB_GROUND = 32;
        private const int INT_NB_SKY = 32;
        private Random rand;
        private Vector2 vecGroundPosition;
        private Vector2 vecSkyPosition;
        private float fltTotalTime;
        private bool blnHasHit;
        private bool blnHasTouched;
        private int intScore;
        private string strScore;
        private Vector2 vecScorePosition;
        private Vector2 vecScoreSize;

        public override void Initialize()
        {
            Nyan = new NyanCat();
            rand = new Random();
            lstStars = new List<DrawStar>();
            lstRainbow = new List<Rainbow>();
            lstGround = new List<DrawCourse>();
            lstSky = new List<DrawCourse>();
            blnHasHit = false;
            blnHasTouched = false;
            intScore = 5;
            strScore = ""+intScore;
            vecScoreSize = Globals.ArialFont.MeasureString(strScore)*2;
            vecScorePosition.X = 750 - vecScoreSize.X;
            vecScorePosition.Y = 0;
            
            // Lancement du fond sonore en boucle
            _soundNyan = Globals.Content.Load<SoundEffect>(@"nyan").CreateInstance();
            _soundNyan.IsLooped = true;
            _soundNyan.Play();

            // On cr�e le nobmre d�sir� d'arc-en-ciels derri�re Nyan Cat
            for (int x = 0; x < INT_NB_RAINBOW; x++)
            {
                Rainbow tmpRainbow = new Rainbow(x * 10, (480 / 2) - (Nyan._tex_NyanCat.Height / 2));
                lstRainbow.Add(tmpRainbow);
            }

            for (int x = 0; x < INT_NB_GROUND; x++)
            {
                if (x == 0)
                {
                    vecGroundPosition.Y = rand.Next(330, 455);
                    vecGroundPosition.X = x;
                }
                else
                {
                    float fltTemp = rand.Next(-1, 1);
                    vecGroundPosition.X += 25;
                    if(fltTemp < 0 && vecGroundPosition.Y < 455)
                    {
                        vecGroundPosition.Y += 10;
                    }
                    else if (vecGroundPosition.Y > 350)
                    {
                        vecGroundPosition.Y -= 10;
                    }

                }
                DrawCourse tmpGround = new DrawCourse(Globals.Content.Load<Texture2D>(@"tmpEarth"), vecGroundPosition);
                lstGround.Add(tmpGround);
            }

            for (int x = 0; x < INT_NB_SKY; x++)
            {
                if (x == 0)
                {
                    vecSkyPosition.Y = rand.Next(-125, 0);
                    vecSkyPosition.X = x;
                }
                else
                {
                    float fltTemp = rand.Next(-1, 1);
                    vecSkyPosition.X += 25;
                    if (fltTemp < 0 && vecSkyPosition.Y < -10)
                    {
                        vecSkyPosition.Y += 10;
                    }
                    else if (vecSkyPosition.Y > -115)
                    {
                        vecSkyPosition.Y -= 10;
                    }

                }
                DrawCourse tmpSky = new DrawCourse(Globals.Content.Load<Texture2D>(@"tmpSky"), vecSkyPosition);
                lstSky.Add(tmpSky);
            }
        }

        public override void Update(float deltatime)
        {
            // Mettre le jeu en pause
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) 
            {
                Globals.intStarCount = 0;
                _soundNyan.Stop();
                Globals.GameStateStack.Set(new MainMenu());
            }

            // R�cup�ration de l'�tat de l'�cran tactile
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count >= 1)
            {
                blnHasTouched = true;
            }

            if (!blnHasHit && blnHasTouched)
            {
                fltTotalTime += deltatime;
                intScore += (int)fltTotalTime;
                strScore = "" + intScore;
                vecScoreSize = Globals.ArialFont.MeasureString(strScore)*2;
                vecScorePosition.X = 750 - vecScoreSize.X;
                vecScorePosition.Y = 50;

                // On test si l'�cran est touch� ou non, si oui le perso monte, sinon il descend
                if (touchCollection.Count >= 5)
                {
                    Nyan.ChangeSkin();
                }
                if (touchCollection.Count >= 1)
                {

                    if ((Nyan._vec2_NyanPos.Y - 10) >= 0)
                    {
                        Nyan._vec2_NyanPos.Y -= 10;
                        foreach (DrawCourse currentSky in lstSky)
                        {
                            blnHasHit = Nyan._rec_Destination.Intersects(currentSky._recDestination);
                            if (blnHasHit)
                            {
                                Nyan._vec2_NyanPos.Y = currentSky.fltPosY + 151;
                                break;
                            }
                        }
                    }
                }
                else if ((Nyan._vec2_NyanPos.Y + 8) < (480 - Nyan._tex_NyanCat.Height))
                {
                    Nyan._vec2_NyanPos.Y += 8;
                    foreach (DrawCourse currentGround in lstGround)
                    {
                        blnHasHit = Nyan._rec_Destination.Intersects(currentGround._recDestination);
                        if (blnHasHit)
                        {
                            Nyan._vec2_NyanPos.Y = currentGround.fltPosY - 81;
                            break;
                        }
                    }
                }

                Nyan.Update();

                // On test le nombre d'�toiles actuellement affich�es
                if (Globals.intStarCount < 4)
                {
                    DrawStar tmpDrawStar = new DrawStar(fltTotalTime);
                    lstStars.Add(tmpDrawStar);
                    Globals.intStarCount += 1;
                }


                // On actualise la position des bouts d'arc-en-ciels
                for (int x = 0; x < INT_NB_RAINBOW; x++)
                {
                    if (lstRainbow[x] != lstRainbow[INT_NB_RAINBOW - 1])
                    {
                        lstRainbow[x].Update(deltatime, lstRainbow[x + 1].intPubPosY);
                    }
                }

                lstRainbow[INT_NB_RAINBOW - 1].Update(deltatime, (int)Nyan._vec2_NyanPos.Y);

                // On actualise les �toiles
                foreach (DrawStar currentStar in lstStars)
                {
                    currentStar.Update(deltatime, fltTotalTime);
                }

                // On actualise les positions du sol
                for (int x = 0; x < INT_NB_GROUND; x++)
                {
                    if (lstGround[x] != lstGround[INT_NB_GROUND - 1])
                    {
                        lstGround[x].Update(deltatime, lstGround[x + 1].fltPosY);
                    }
                }

                float fltTemp = rand.Next(-1, 1);
                if (fltTemp < 0 && vecGroundPosition.Y < 455)
                {
                    vecGroundPosition.Y += 10;
                }
                else if (vecGroundPosition.Y > 350)
                {
                    vecGroundPosition.Y -= 10;
                }
                lstGround[INT_NB_GROUND - 1].Update(deltatime, vecGroundPosition.Y);

                // On actualise les positions du ciel
                for (int x = 0; x < INT_NB_SKY; x++)
                {
                    if (lstSky[x] != lstSky[INT_NB_SKY - 1])
                    {
                        lstSky[x].Update(deltatime, lstSky[x + 1].fltPosY);
                    }
                }

                float fltTempSky = rand.Next(-1, 1);
                if (fltTempSky < 0 && vecSkyPosition.Y < -10)
                {
                    vecSkyPosition.Y += 10;
                }
                else if (vecSkyPosition.Y > -115)
                {
                    vecSkyPosition.Y -= 10;
                }
                lstSky[INT_NB_SKY - 1].Update(deltatime, vecSkyPosition.Y);
            }
            else if (blnHasHit)
            {
                Globals.intStarCount = 0;
                _soundNyan.Stop();
                Globals.intLastScore = intScore;
                Globals.GameStateStack.Set(new GameOver());
            }
        }

        public override void Draw()
        {
            Globals.GDM.GraphicsDevice.Clear(Globals.NyanColor);

            Globals.SpriteBatch.Begin(SpriteSortMode.BackToFront, null);

            Nyan.Draw();

            foreach (Rainbow currentRainbow in lstRainbow)
            {
                currentRainbow.Draw();
            }

            foreach (DrawCourse currentGround in lstGround)
            {
                currentGround.Draw();
            }

            foreach (DrawCourse currentSky in lstSky)
            {
                currentSky.Draw();
            }

            foreach (DrawStar currentStar in lstStars)
            {
                currentStar.Draw();
            }

            Globals.SpriteBatch.DrawString(Globals.ArialFont, strScore, vecScorePosition, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            Globals.SpriteBatch.End();
        }
    }
}