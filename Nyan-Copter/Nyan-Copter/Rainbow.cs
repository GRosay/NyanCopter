/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant le code permettant d'afficher
 *              un arc-en-ciel derri�re Nyan Cat
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
    public class Rainbow
    {
        private Texture2D _tex_Rainbow;
        private string str_RainbowTex;
        private Rectangle _recDestination;
        private Vector2 _vec2RainbowPosition;
        public int intPubPosY = 0;

        public Rainbow(int intPosX, int intPosY)
        {
            str_RainbowTex = "arcenciel";
            _tex_Rainbow = Globals.Content.Load<Texture2D>(@str_RainbowTex);

            // On cr��e les bouts d'arc-en-ciel aux coordonn�es donn�es
            _vec2RainbowPosition.X = intPosX;
            _vec2RainbowPosition.Y = intPosY;
            _recDestination = new Rectangle((int)_vec2RainbowPosition.X, (int)_vec2RainbowPosition.Y, _tex_Rainbow.Width, _tex_Rainbow.Height);
            intPubPosY = intPosY;

        }

        public void Update(float deltatime, int intNyanPosY)
        {
            // On d�place le bout d'arc-en-ciel � la nouvelle coordonn�es Y
            _vec2RainbowPosition.Y = intNyanPosY;
            _recDestination = new Rectangle((int)_vec2RainbowPosition.X, (int)_vec2RainbowPosition.Y, _tex_Rainbow.Width, _tex_Rainbow.Height);
            intPubPosY = (int)_vec2RainbowPosition.Y;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_tex_Rainbow, _recDestination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}