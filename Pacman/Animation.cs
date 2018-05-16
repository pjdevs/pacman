using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pacman
{
    public class Animation
    {
        private int currentFrame;
        
        public string Name { get; private set; }
        public int Row { get; private set; }
        public int Lenght { get; private set; }
        public int Dimension { get; private set; }

        public Rectangle SourceRectangle { get; private set; }

        public Animation(string name, int row, int lenght, int dimension)
        {
            currentFrame = 0;

            Name = name;
            Row = row;
            Lenght = lenght;
            Dimension = dimension;

            SourceRectangle = new Rectangle(0, row * Dimension, Dimension, Dimension);
        }

        public void Animate()
        {
            if (currentFrame == Lenght)
                currentFrame = 0;
            else currentFrame++;

            SourceRectangle = new Rectangle(currentFrame * Dimension, Row * Dimension, Dimension, Dimension);
        }
    }

    public class Animator
    {
        private List<Animation> animations;
        private int counter;
        private int speed;

        public Animation CurrentAnimation { get; private set; }

        public Animator()
        {
            counter = 0;
            speed = 4;

            animations = new List<Animation>();
        }

        public void Animate()
        {
            if (counter == speed)
            {
                CurrentAnimation.Animate();          
                counter = 0;
            }

            counter++;
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        public void SetAnimation(string name)
        {
            foreach (Animation a in animations)
                if (a.Name == name)
                    CurrentAnimation = a;          
        }
    }
}
