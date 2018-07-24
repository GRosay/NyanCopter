/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant la page de fin du jeu   
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
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace Nyan_Copter
{
    class ShowScores : GameState
    {
        // Texture repr�sentant le texte "Game Over"
        private Texture2D _tex_Scores;
        // Attribution du nom de l'image � une variable string
        private string str_Scores;
        // Rectangle dans le quel sera affich� la texture
        private Rectangle _ScoresTextDestination = Rectangle.Empty;
        // Coordonn�es d'interaction sur l'�cran
        private Vector2 _TouchPosition = Vector2.Zero;
        
        private List<DrawStar> lstStars = new List<DrawStar>();

        private float fltTotalTime;

        private Random rand = new Random();
        
        private int intCurrentScore;

        private int y;

        private int intNbScore;

        public override void Initialize()
        {
            // Attribution des valeurs de d�parts aux diff�rentes variables
            str_Scores = "scores";
            _tex_Scores = Globals.Content.Load<Texture2D>(str_Scores);
            _ScoresTextDestination = new Rectangle(0, 0, 800, 480);
            fltTotalTime = 0;
            intCurrentScore = 0;
            y = 150;
            intNbScore = 0;
            foreach (string temp in Globals.ScoreList)
            {
                intNbScore += 1;
            }
        }

        public override void Update(float deltatime)
        {
            // Retour au menu
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Globals.intStarCount = 0;
                Globals.GameStateStack.Set(new MainMenu());
            }

            // R�cup�ration de l'�tat de l'�cran tactile
            TouchCollection touchCollection = TouchPanel.GetState();

            // On test si l'�cran est touch� ou non, si oui on r�cup�re les coordonn�e du premier point de contact
            if (touchCollection.Count >= 1)
            {
                _TouchPosition = (touchCollection[0].Position);
            }

            if (_TouchPosition.X > 650 && _TouchPosition.X < 710)
            {
                if (_TouchPosition.Y > 160 && _TouchPosition.Y < 290)
                {
                    if (intCurrentScore >= 3)
                    {
                        intCurrentScore -= 3;
                    }
                }
                else if (_TouchPosition.Y > 290 && _TouchPosition.Y < 425)
                {
                    if (intCurrentScore < (intNbScore - 3))
                    {
                        intCurrentScore += 3;
                    }
                }
            }

            // On ajoute le temps de l'Update au temps total d'affichage du menu
            fltTotalTime += deltatime;

            // On compte le nombre d'�toile actuellement affich�e, si - de 4, on en rajoute
            if (Globals.intStarCount < 4)
            {
                DrawStar tmpDrawStar = new DrawStar(fltTotalTime);
                lstStars.Add(tmpDrawStar);
                Globals.intStarCount += 1;
            }

            // On met � jour l'affichage des �toiles
            foreach (DrawStar currentStar in lstStars)
            {
                currentStar.Update(deltatime, fltTotalTime);
            }

        }

        public override void Draw()
        {
            // Change la couleur du fond
            Globals.GDM.GraphicsDevice.Clear(Globals.NyanColor);

            Globals.SpriteBatch.Begin(SpriteSortMode.BackToFront, null);
            // Affiche le texte
            Globals.SpriteBatch.Draw(_tex_Scores, _ScoresTextDestination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);


            if (y > 375)
            {
                y = 175;
            }
            foreach(string strCurrentScore in Globals.ScoreList)
            {
                Vector2 tmpVec = Vector2.Zero;

                if (!(intCurrentScore + 1 >= intNbScore) && !(intCurrentScore + 2 >= intNbScore))
                {
                    if (strCurrentScore == Globals.ScoreList[intCurrentScore] || strCurrentScore == Globals.ScoreList[intCurrentScore + 1] || strCurrentScore == Globals.ScoreList[intCurrentScore + 2])
                    {
                        tmpVec.X = 800 / 2 - (Globals.ArialFont.MeasureString(strCurrentScore).X);
                        tmpVec.Y = y;
                        Globals.SpriteBatch.DrawString(Globals.ArialFont, strCurrentScore, tmpVec, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                        y += 100;
                    }
                }
                else if (!(intCurrentScore + 1 >= intNbScore))
                {
                    if (strCurrentScore == Globals.ScoreList[intCurrentScore] || strCurrentScore == Globals.ScoreList[intCurrentScore + 1])
                    {
                        tmpVec.X = 800 / 2 - (Globals.ArialFont.MeasureString(strCurrentScore).X);
                        tmpVec.Y = y;
                        Globals.SpriteBatch.DrawString(Globals.ArialFont, strCurrentScore, tmpVec, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                        y += 200;
                    }
                }
                else
                {
                    if (strCurrentScore == Globals.ScoreList[intCurrentScore])
                    {
                        tmpVec.X = 800 / 2 - (Globals.ArialFont.MeasureString(strCurrentScore).X);
                        tmpVec.Y = y;
                        Globals.SpriteBatch.DrawString(Globals.ArialFont, strCurrentScore, tmpVec, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
                    }
                }
            }

            foreach (DrawStar currentStar in lstStars)
            {
                currentStar.Draw();
            }

            Globals.SpriteBatch.End();
        }
    }
}