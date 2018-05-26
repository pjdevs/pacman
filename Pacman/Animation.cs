using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using static Pacman.Constants; // Pour avoir accès aux constantes

namespace Pacman
{
    public class Animation // Classe représentant une animation
    {
        //Variables membres de la classe 
        private int currentFrame; // L'image actuelle
        
        public string Name { get; private set; } // Le nom de l'animation
        public int Row { get; private set; } // La ligne de l'image où l'animation se situe 
        public int Lenght { get; private set; } // La longueur de l'animation
        public int Dimension { get; private set; } // La dimension du carré de chaque image

        public Rectangle SourceRectangle { get; private set; } // Le rectangle qui servira de rectangle source pour dessiner chaque image

        // Constructeur
        public Animation(string name, int row, int lenght, int dimension)
        {
            currentFrame = 0;

            Name = name;
            Row = row;
            Lenght = lenght;
            Dimension = dimension;

            SourceRectangle = new Rectangle(0, row * Dimension, Dimension, Dimension);
        }

        // Fonction qui met à jour l'animation
        public void Animate()
        {
            if (currentFrame == Lenght)
                currentFrame = 0;
            else currentFrame++;

            SourceRectangle = new Rectangle(currentFrame * Dimension, Row * Dimension, Dimension, Dimension);
        }
    }

    public class Animator // Classe qui gère les animations
    {
        // Variables membres de la classe
        private List<Animation> animations; // La liste des animations
        private int counter; // Le compteur à atteindre pour la vitesse des animations
        private int speed; // Le nombre à atteindre pour le compteur

        public Animation CurrentAnimation { get; private set; } // L'animation actuelle

        // Constructeur
        public Animator()
        {
            counter = 0;
            speed = ANIMATION_SPEED;

            animations = new List<Animation>();
        }

        // Fonction qui gère la mise à jour de l'animation actuelle en fonction de la vitesse
        public void Animate()
        {
            if (counter == speed)
            {
                CurrentAnimation.Animate();          
                counter = 0;
            }

            counter++;
        }

        // Fonction pour ajouter une animation à la liste
        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        // Fonction pour mettre une animation en tant qu'animation actuelle
        public void SetAnimation(string name)
        {
            foreach (Animation a in animations)
                if (a.Name == name)
                    CurrentAnimation = a;          
        }
    }
}
