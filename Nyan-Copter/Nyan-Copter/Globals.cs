/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichoer contenant les variables et ressources
 *              globales devant �tre utilisables depuis
 *              n'importe quelle classe
 * **********************************************************
 *                      Modifications
 * 
 * Auteur:
 * Date:
 * But:
 * 
 * *********************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Nyan_Copter
{
    public static class Globals
    {
        public static List<string> ScoreList {get;set;}
        
        public static GraphicsDeviceManager GDM { get; set; }

        public static GameStateStack GameStateStack { get; set; }

        public static SpriteFont ArialFont { get; set; }

        public static SpriteBatch SpriteBatch { get; set; }
        
        public static ContentManager Content { get; set; } 

        public static Color NyanColor = new Color(15, 77, 143);

        public static Game NyanGame { get; set; }

        public static int intStarCount = 0;

        public static int intLastScore = 0;

        public static string strLastName = "";
    }
}