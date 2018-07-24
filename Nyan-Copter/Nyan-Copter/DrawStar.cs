/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant la classe "DrawStar"    
 *              permettant l'affichage des �toiles
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Nyan_Copter
{
    public class DrawStar
    {
        private Texture2D _tex_Star;
        private string strTextureName;
        private Vector2 _randomPosition = Vector2.Zero;
        private Rectangle _rec_Destination;
        private Rectangle _rec_Source;
        private int _intCurrentIndex = 0;
        private int _intMaxIndex = 4;
        private Random rand = new Random();
        private float fltStartTime;
        private bool blnIsDestroyed = false;

        public DrawStar(float fltTotalTime)
        {
            // On choisit des coordonn�es sur l'�cran al�atoirement
            float flt_PositionX = rand.Next(0, 800);
            float flt_PositionY = rand.Next(0, 480);
            _randomPosition.X = (int)flt_PositionX;
            _randomPosition.Y = (int)flt_PositionY;
            strTextureName = "star";

            // On d�finit � quel nombre de seconde l'�toile a �t� cr��e
            fltStartTime = fltTotalTime;

            _tex_Star = Globals.Content.Load<Texture2D>(@strTextureName);
            _rec_Source = new Rectangle(0, 0, _tex_Star.Width / (_intMaxIndex), _tex_Star.Height);
            _rec_Destination = new Rectangle();
        }

        public void Update(float deltatime, float fltTotalTime)
        {
            // On test si il s'est pass� 0.45 secondes depuis la derni�re mise � jour et si l'�toile n'est pas d�truite
            if (fltTotalTime - fltStartTime >= 0.45 && !blnIsDestroyed)
            {
                // Si l'�toile a encore d'autre affichage � avoir
                if (_intCurrentIndex < _intMaxIndex)
                {
                    _rec_Source = new Rectangle(_intCurrentIndex * (_tex_Star.Width / (_intMaxIndex + 1)), 0, _tex_Star.Width / (_intMaxIndex + 1), _tex_Star.Height);
                    _randomPosition.X -= 20;
                    _rec_Destination = new Rectangle((int)_randomPosition.X, (int)_randomPosition.Y, _tex_Star.Width / (_intMaxIndex + 1), _tex_Star.Height);
                    _intCurrentIndex++;

                    fltStartTime = fltTotalTime;
                }
                else
                {
                    Destroy();
                }
            }


        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_tex_Star, _rec_Destination, _rec_Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void Destroy()
        {
            // On refixe le rectangle de "destination" � un rectangle nul
            _rec_Destination = new Rectangle();
            // On d�cr�mente le nombre d'�toiles affich�es
            Globals.intStarCount -= 1;
            blnIsDestroyed = true;
        }
    }
}