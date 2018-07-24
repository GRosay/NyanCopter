/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant le menu principal du jeu
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

namespace Nyan_Copter
{
    class MainMenu : GameState
    {
        // Texture repr�sentant le texte du menu
        private Texture2D _tex_MainMenuText;
        // Attribution du nom de l'image � une variable string
        private string str_MainMenuText;
        // Rectangle dans le quel sera affich� la texture
        private Rectangle _MainMenuTextDestination = Rectangle.Empty;
        // Vecteur 2D pour positionner le rectangle
        private Vector2 _TextPosition = Vector2.Zero;

        // Coordonn�es d'interaction sur l'�cran
        private Vector2 _TouchPosition = Vector2.Zero;

        // Couleur temporaire pour afficher les coordonn�es du point de contact
        private Color tmpColor = Color.Red;

        private List<DrawStar> lstStars = new List<DrawStar>();

        private float fltTotalTime;

        private List<NyanCat> lstNyan = new List<NyanCat>();
        
        private Random rand = new Random();

        public override void Initialize()
        {
            // Attribution des valeurs de d�parts aux diff�rentes variables
            str_MainMenuText = "MainMenuText";
            _TextPosition.X = (800 / 2) - (513 / 2);
            _TextPosition.Y = (480 / 2) - (299 / 2);
            _tex_MainMenuText = Globals.Content.Load<Texture2D>(@str_MainMenuText);
            _MainMenuTextDestination = new Rectangle((int)_TextPosition.X, (int)_TextPosition.Y, 513, 299);
            fltTotalTime = 0;
        }

        public override void Update(float deltatime)
        {
            // Quitter le jeu
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Globals.NyanGame.Exit();

            // R�cup�ration de l'�tat de l'�cran tactile
            TouchCollection touchCollection = TouchPanel.GetState();

            // On test si l'�cran est touch� ou non, si oui on r�cup�re les coordonn�e du premier point de contact
            if (touchCollection.Count >= 1)
            {
                _TouchPosition = (touchCollection[0].Position);
            }

            // On ajoute le temps de l'Update au temps total d'affichage du menu
            fltTotalTime += deltatime;

            // On test si l'utilisateur appuie sur un des "boutons" (start game / score)

                // Test si dans la bonne zone en largeur
            if (_TouchPosition.X > 280 && _TouchPosition.X < 510)
            {
                // Test si dans la zone du bouton "Start Game"
                if (_TouchPosition.Y > 290 && _TouchPosition.Y < 340)
                {
                    Globals.intStarCount = 0;
                    Globals.GameStateStack.Set(new PlayGame());
                }
                // Sinon test si dans la zone du bouton Score
                else if (_TouchPosition.Y > 340 && _TouchPosition.Y < 390)
                {
                    Globals.intStarCount = 0;
                    Globals.GameStateStack.Set(new ShowScores());
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
            Globals.SpriteBatch.Draw(_tex_MainMenuText, _MainMenuTextDestination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            foreach (DrawStar currentStar in lstStars)
            {
                currentStar.Draw();
            }

            Globals.SpriteBatch.End();
        }
    }
}