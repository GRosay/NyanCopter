/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Classe contenant les bases du personnage  
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
    public class NyanCat
    {
        public Texture2D _tex_NyanCat;
        private string strNyanTextureName;
        public Vector2 _vec2_NyanPos = Vector2.Zero;
        public Rectangle _rec_Destination = Rectangle.Empty;

        // Par d�faut, Nyan cat � 150px de la gauche et centr� verticalement
        public NyanCat()
        {
            strNyanTextureName = "nyan-cat";
            _tex_NyanCat = Globals.Content.Load<Texture2D>(@strNyanTextureName);

            // On lance avec Nyan Cat au centre de l'�cran (en hauteur)
            _vec2_NyanPos.Y = (480 / 2) - (_tex_NyanCat.Height / 2);
            _vec2_NyanPos.X = 150;
            _rec_Destination = new Rectangle((int)_vec2_NyanPos.X, (int)_vec2_NyanPos.Y, _tex_NyanCat.Width, _tex_NyanCat.Height);
        }

        // Surcharge permettant d'afficher le Nyan Cat aux coordonn�es donn�es
        public NyanCat(float fltNyanPosX, float fltNyanPosY)
        {
            _vec2_NyanPos.X = fltNyanPosX;
            _vec2_NyanPos.Y = fltNyanPosY;
            _rec_Destination = new Rectangle((int)_vec2_NyanPos.X, (int)_vec2_NyanPos.Y, _tex_NyanCat.Width, _tex_NyanCat.Height);
        }

        public void Update()
        {
            _rec_Destination = new Rectangle((int)_vec2_NyanPos.X, (int)_vec2_NyanPos.Y, _tex_NyanCat.Width, _tex_NyanCat.Height);
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_tex_NyanCat, _rec_Destination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void ChangeSkin()
        {
            _tex_NyanCat = Globals.Content.Load<Texture2D>(@"evil-cat");
        }
    }
}