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
using System.Runtime.Serialization;

namespace Nyan_Copter
{
    [DataContract]
    public class Score
    {
        [DataMember]
        public List<string> ScoreList {get;set;}
    }
}
