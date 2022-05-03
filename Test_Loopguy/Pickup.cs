using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    public class Pickup : LevelObject
    {
        public bool pickedUp = false;
        private Rectangle pickupBox;
        private float wave = 0;
        private float waveadjust = 0;
        protected SoundEffect pickupSound;
        public Pickup(Vector2 position) : base(position)
        {
            this.position = position;
            pickupBox = new Rectangle((int)position.X - 4, (int)position.Y - 4, 24, 24);
        }

        public void Update()
        {
            PickUp();
            Wave();
        }

        private void Wave()
        {
            wave += 0.15F;
            waveadjust = (float)(Math.Sin(wave)) / 2;
            position.Y += waveadjust;
        }


        private void PickUp()
        {
            if(pickupBox.Contains(EntityManager.player.centerPosition) && !pickedUp )
            {
                ParticleManager.NewParticle(ParticleSelection.SparkSmall, position);
                position.X = -10000;
                position.Y = -10000;
                pickedUp = true;
                Effect();
                if(pickupSound != null)
                {
                    Audio.PlaySound(pickupSound);
                }
                
                
            }
        }

        protected virtual void Effect()
        {
            
        }
    }

    public class KeyPickup : Pickup
    {
        int id;
        bool permanent;

        public string GetStringOfParams()
        {
            return id.ToString() + "," + permanent.ToString();
        }
        public KeyPickup(Vector2 position, int id, bool permanent) : base(position)
        {
            this.position = position;
            this.id = id;
            this.permanent = permanent;
            texture = TextureManager.keycard;
            this.hitBox = new Rectangle(0, 0, 0, 0);
            pickupSound = Audio.keypickup;
        }
        protected override void Effect()
        {
            GiveKey();
            
        }

        private void GiveKey()
        {
            EntityManager.player.keys.Add(id);
            if(permanent)
            {
                SaveKeyToFile();
            }
        }

        private void SaveKeyToFile()
        {
            string path = @"saves\keys.txt";

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine();
                sw.WriteLine(id.ToString());
            }

            //add to save it to the savefile
        }
    }

    public class AmmoPickup : Pickup
    {
        protected int ammoAmount;
        public AmmoPickup(Vector2 position) : base(position)
        {

        }
        protected override void Effect()
        {
            EntityManager.player.AddAmmo(ammoAmount);
        }
    }

    public class SmallAmmoPickup : AmmoPickup
    {
        public SmallAmmoPickup(Vector2 position) : base(position)
        {
            ammoAmount = 1;
            hitBox = Rectangle.Empty;
            texture = TextureManager.medkit;
            sourceRectangle = new Rectangle(0, 0, 16, 16);
        }

    }

    public class HealthPickup : Pickup
    {
        protected int healAmount;
        public HealthPickup(Vector2 position) : base(position)
        {

        }

        protected override void Effect()
        {
            if (EntityManager.player.HealDamage(healAmount))
            {

            }
            else
            {
                Audio.PlaySound(Audio.meepmerp);
                LevelManager.QueueAddObject(new SmallHealthPickup(EntityManager.player.centerPosition));
            }
        }
    }

    public class SmallHealthPickup : HealthPickup
    {
        public SmallHealthPickup(Vector2 position) : base(position)
        {
            this.position = position;
            this.healAmount = 1;
            this.hitBox = Rectangle.Empty;
            texture = TextureManager.medkit;
            sourceRectangle = new Rectangle(0, 0, 16, 16);
        }
        
        
    }
}
