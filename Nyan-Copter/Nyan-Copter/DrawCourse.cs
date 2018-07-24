/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Code d'affichage du sol et du plafond
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
    public class DrawCourse
    {
        private Texture2D _texCourse;
        public Rectangle _recDestination;
        private Vector2 _vecCoursePosition;
        public float fltPosY = 0;

        public DrawCourse(Texture2D _texToUse, Vector2 _vecPosition)
        {
            _texCourse = _texToUse;
            _vecCoursePosition = _vecPosition;
            _recDestination = new Rectangle((int)_vecCoursePosition.X, (int)_vecCoursePosition.Y, _texCourse.Width, _texCourse.Height);
            fltPosY = _vecCoursePosition.Y;
        }

        public void Update(float deltatime, float fltNewPosY)
        {
            _vecCoursePosition.Y = fltNewPosY;
            _recDestination = new Rectangle((int)_vecCoursePosition.X, (int)_vecCoursePosition.Y, _texCourse.Width, _texCourse.Height);
            fltPosY = _vecCoursePosition.Y;
        }

        public void Draw()
        {
            Globals.SpriteBatch.Draw(_texCourse, _recDestination, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }
}