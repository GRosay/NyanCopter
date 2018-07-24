/************************************************************
 * 
 *
 * Auteur:      Gaspard Rosay
 * Date:        06.12.2011
 * But:         Fichier contenant les classes permettant
 *              la gestion des �tats du jeu.
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

namespace Nyan_Copter
{
    /************************************************************
     * 
     * Nom:     GameState
     * But:     D�finir l'�tat de base de �tats de jeux
     *      
     * *********************************************************/
    public abstract class GameState
    {
        // Initialisate l'�tat
        public abstract void Initialize();

        // D�truit l'�tat
        public virtual void Destroy() {}

        // Met � jour l'�tat
        public abstract void Update(float deltatime);

        // Affiche l'�tat
        public abstract void Draw();
    }


    /************************************************************
     * 
     * Nom:     GameStateStack
     * But:     D�finir l'�tat de base de �tats de jeux
     *      
     * *********************************************************/
    public class GameStateStack
    {
        // Stack (conteneur) des �tats
        public Stack<GameState> States = new Stack<GameState>();

        // Set: suprimmer tout les �tats et les remplacer par un autre
        public void Set(GameState state)
        {
            while (States.Count > 0)
            {
                Pop();
            }

            Add(state);
        }

        // Add: Initialise et ajoute l'�tat dans la stack
        public void Add(GameState state)
        {
            state.Initialize();
            States.Push(state);
        }

        // Supprimer l'�tat en cours du conteneur
        public void Pop()
        {
            if (States.Count > 0)
            {
                States.Peek().Destroy();
                States.Pop();
            }
        }

        // Met � jour l'�tat actuel
        public void Update(float deltatime)
        {
            if (States.Count > 0)
            {
                States.Peek().Update(deltatime);
            }
        }

        // Dessine l'�tat actuel
        public void Draw()
        {
            if (States.Count > 0)
            {
                States.Peek().Draw();
            }
        }
    }
}