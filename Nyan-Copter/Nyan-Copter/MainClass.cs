/************************************************************
 * 
 * Société:     -
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier d'entrée du jeu. Définit les    
 *              paramètres de base du jeu.  
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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;


namespace Nyan_Copter
{
    /// <summary>
    /// Type principal pour votre jeu
    /// </summary>
    public class MainClass : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<string> tmpScoreList = new List<string>();

        public MainClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Globals.Content = Content;

            // La fréquence d’image est de 30 i/s pour le Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Affichage du jeu en plein écran
            graphics.IsFullScreen = true;

            Globals.GDM = graphics;

            // On instancie le conteneur des états de jeu
            Globals.GameStateStack = new GameStateStack();

            // Augmenter la durée de la batterie sous verrouillage.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            Globals.NyanGame = this;

            Globals.ScoreList = new List<string>();

        }

        /// <summary>
        /// Permet au jeu d’effectuer l’initialisation nécessaire pour l’exécution.
        /// Il peut faire appel aux services et charger tout contenu
        /// non graphique. Calling base.Initialize énumère les composants
        /// et les initialise.
        /// </summary>
        protected override void Initialize()
        {
            LoadScore();
            Globals.ScoreList.AddRange(tmpScoreList);

            // Au lancement du jeu, on envoie directement sur le menu principal
            Globals.GameStateStack.Set(new MainMenu());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent est appelé une fois par partie. Emplacement de chargement
        /// de tout votre contenu.
        /// </summary>
        protected override void LoadContent()
        {
            // Créer un SpriteBatch, qui peut être utilisé pour dessiner des textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.SpriteBatch = spriteBatch;

            // On charge la police Arial dans les globals
            Globals.ArialFont = Content.Load<SpriteFont>("Arial");
        }

        /// <summary>
        /// UnloadContent est appelé une fois par partie. Emplacement de déchargement
        /// de tout votre contenu.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Déchargez le contenu non ContentManager ici
        }

        /// <summary>
        /// Permet au jeu d’exécuter la logique de mise à jour du monde,
        /// de vérifier les collisions, de gérer les entrées et de lire l’audio.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Update(GameTime gameTime)
        {
            // On récupère le temps écoulé depuis le dernier appel d'update
            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Globals.GameStateStack.Update(deltatime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Appelé quand le jeu doit se dessiner.
        /// </summary>
        /// <param name="gameTime">Fournit un aperçu des valeurs de temps.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // On affiche le state courant
            Globals.GameStateStack.Draw();

            base.Draw(gameTime);
        }

        private void LoadScore()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists("Scores/scores.xml"))
                {
                    using (IsolatedStorageFileStream isfs = isf.OpenFile("Scores/scores.xml", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                        List<string> score = serializer.Deserialize(isfs) as List<string>;
                        tmpScoreList.AddRange(score);                       
                    }
                }
            }
        }
    }
}