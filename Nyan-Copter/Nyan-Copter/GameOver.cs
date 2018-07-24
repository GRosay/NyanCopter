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
using Microsoft.Xna.Framework.GamerServices;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Nyan_Copter
{
    class GameOver : GameState
    {
        // Texture repr�sentant le texte "Game Over"
        private Texture2D _tex_GameOver;
        // Attribution du nom de l'image � une variable string
        private string str_GameOver;
        // Rectangle dans le quel sera affich� la texture
        private Rectangle _GameOverTextDestination = Rectangle.Empty;
        // Vecteur 2D pour positionner le rectangle
        private Vector2 _TextPosition = Vector2.Zero;

        // Coordonn�es d'interaction sur l'�cran
        private Vector2 _TouchPosition = Vector2.Zero;

        private List<DrawStar> lstStars = new List<DrawStar>();

        private float fltTotalTime;

        private Random rand = new Random();

        public string str_UserName;

        public string strScore;

        private Vector2 _scorePosition = Vector2.Zero;
        private Vector2 _vecScoreSize = Vector2.Zero;
        public override void Initialize()
        {
            // Attribution des valeurs de d�parts aux diff�rentes variables
            str_GameOver = "GameOver";
            _tex_GameOver = Globals.Content.Load<Texture2D>(str_GameOver);
            _GameOverTextDestination = new Rectangle(0, 0, 800, 480);
            fltTotalTime = 0;
            strScore = ""+Globals.intLastScore;
            _vecScoreSize = Globals.ArialFont.MeasureString(strScore)*2;
            _scorePosition.X = (800 / 2) - (_vecScoreSize.X / 2);
            _scorePosition.Y = 300;
            
            if (!Guide.IsVisible)
            {
                Guide.BeginShowKeyboardInput(PlayerIndex.One, "Perdu ! Entrez votre nom pour enregistrer votre score !", "", Globals.strLastName, ar => 
                {
                    if (Globals.strLastName != Guide.EndShowKeyboardInput(ar))
                    {
                        Globals.strLastName = Guide.EndShowKeyboardInput(ar);
                    }
                    str_UserName = Guide.EndShowKeyboardInput(ar);
                }, null);
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

            // On ajoute le temps de l'Update au temps total d'affichage du menu
            fltTotalTime += deltatime;

            if (_TouchPosition.Y > 425 && _TouchPosition.Y < 475)
            {
                // Test si dans la zone du bouton "Menu"
                if (_TouchPosition.X > 50 && _TouchPosition.X < 305)
                {
                    Globals.intStarCount = 0;
                    Globals.ScoreList.Add(str_UserName+": "+strScore);
                    SaveScore();
                    Globals.GameStateStack.Set(new MainMenu());
                }
                // Sinon test si dans la zone du bouton Score
                else if (_TouchPosition.X > 610 && _TouchPosition.X < 745)
                {
                    Globals.intStarCount = 0;
                    Globals.ScoreList.Add(str_UserName + " - " + strScore);
                    SaveScore();
                    Globals.GameStateStack.Set(new PlayGame());
                }
            }

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
            Globals.SpriteBatch.Draw(_tex_GameOver, _GameOverTextDestination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            Globals.SpriteBatch.DrawString(Globals.ArialFont, strScore, _scorePosition, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            foreach (DrawStar currentStar in lstStars)
            {
                currentStar.Draw();
            }

            Globals.SpriteBatch.End();
        }

        // Fonction d'enregistrement du score
        private void SaveScore()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists("Scores"))
                {
                    isf.CreateDirectory("Scores");
                }

                using (IsolatedStorageFileStream isfs = isf.OpenFile("Scores/scores.xml", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));

                    serializer.Serialize(isfs, Globals.ScoreList);
                }
            }
        }
    }
}