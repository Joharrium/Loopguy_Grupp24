using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy.Content
{
    public enum ParticleSelection
    {
        SparkSmall, ShotExplosion
    }
    public static class ParticleManager
    {
        static List<Particle> particles = new List<Particle>();

        public static void Update(GameTime gameTime)
        {
            foreach (Particle p in particles)
            {
                p.Update(gameTime);
            }
            Dispose();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(Particle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

        private static void Dispose()
        {
            Particle particleToRemove = new Particle(new Vector2(0,0));
            foreach (Particle p in particles)
            {
                if(p.donePlaying)
                {
                    particleToRemove = p;
                }
            }
            particles.Remove(particleToRemove);
        }

        public static void NewParticle(ParticleSelection particle, Vector2 position)
        {
            switch(particle)
            {
                case ParticleSelection.SparkSmall:
                    particles.Add(new SparkSmall(position));
                    break;
                case ParticleSelection.ShotExplosion:
                    particles.Add(new ShotExplosion(position));
                    break;
            }
        }
    }
}
