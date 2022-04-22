
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Test_Loopguy
{
    public static class Audio
    {
        static float musicVolume = 0.3F;
        static float soundVolume = 0.5F;
        //music
        //bizarre error markings that could only be "fixed" like this, disregard these comments
        //he he he he he he he he he he he he he he he he he heeeeeeeeeeeeeeeeeeeeeeeeeeeeee he he//he he he he he he he he he he he he he he he he he heeeeeeeeeeeeeeeeeeeeeeeeeeeeee he he//he he he he he he he he he he he he he he he he he heeeeeeeeeeeeeeeeeeeeeeeeeeeeee he he
        //he he he he he he he he he he he he he he he he he heeeeeeeeeeeeeeeeeeeeeeeeeeeeee he he

        public static SoundEffect sus_low, sus_high, unatco_hq;
        
        private static SoundEffectInstance playingTrack;

        public static SoundCollection lasergun;
        //sound
        public static SoundEffect meepmerp, swing, open, deny, dash, door_hiss_sound, box_destroy, shrub_destroy, keypickup;

        public static SoundEffect EnergyGun_Shoot1A, EnergyGun_Shoot1B, EnergyGun_Shoot1C, EnergyGun_Shoot1D, EnergyGun_Shoot2A, EnergyGun_Shoot2B, EnergyGun_Shoot2C, EnergyGun_Shoot2D;
        static public void Load(ContentManager Content)
        {
            LoadMusic(Content);
            LoadSound(Content);
            CreateCollections();

        }
        static void CreateCollections()
        {
            lasergun = new SoundCollection();
            lasergun.AddSound(EnergyGun_Shoot1A);
            lasergun.AddSound(EnergyGun_Shoot1B);
            lasergun.AddSound(EnergyGun_Shoot1C);
            lasergun.AddSound(EnergyGun_Shoot1D);
            lasergun.AddSound(EnergyGun_Shoot2A);
            lasergun.AddSound(EnergyGun_Shoot2B);
            lasergun.AddSound(EnergyGun_Shoot2C);
            lasergun.AddSound(EnergyGun_Shoot2D);
        }
        static void LoadMusic(ContentManager c)
        {
            sus_high = c.Load<SoundEffect>("audio/music/sus_high");
            sus_low = c.Load<SoundEffect>("audio/music/sus_low");
            unatco_hq = c.Load<SoundEffect>("audio/music/unatco_hq");
        }

        

        static void LoadSound(ContentManager c)
        {
            meepmerp = c.Load<SoundEffect>("audio/sound/meepmerp");
            swing = c.Load<SoundEffect>("audio/sound/swing");
            open = c.Load<SoundEffect>("audio/sound/open");
            deny = c.Load<SoundEffect>("audio/sound/deny");
            dash = c.Load<SoundEffect>("audio/sound/dash");
            box_destroy = c.Load<SoundEffect>("audio/sound/box_destroy");
            shrub_destroy = c.Load<SoundEffect>("audio/sound/shrub_destroy");
            door_hiss_sound = c.Load<SoundEffect>("audio/sound/door_hiss_sound");
            keypickup = c.Load<SoundEffect>("audio/sound/keypickup");

            {
                EnergyGun_Shoot1A = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot1A");
                EnergyGun_Shoot1B = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot1B");
                EnergyGun_Shoot1C = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot1C");
                EnergyGun_Shoot1D = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot1D");
                EnergyGun_Shoot2A = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot2A");
                EnergyGun_Shoot2B = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot2B");
                EnergyGun_Shoot2C = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot2C");
                EnergyGun_Shoot2D = c.Load<SoundEffect>("audio/sound/gun/EnergyGun_Shoot2D");
            }
        }

        public static void PlayMusic(SoundEffect music)
        {
            if(music !=null)
            {
                SoundEffectInstance instance = music.CreateInstance();
                instance.Volume = musicVolume;
                instance.IsLooped = true;
                instance.Play();
                playingTrack = instance;
            }
        }

        public static void StopMusic()
        {
            playingTrack.Stop();
            playingTrack = null;
        }

        public static void PlaySound(SoundEffect sound)
        {
            if(sound != null)
            {
                SoundEffectInstance instance = sound.CreateInstance();
                instance.Volume = soundVolume;
                instance.Play();
            }
        }
    }
    public class SoundCollection
    {
        List<SoundEffect> sounds = new List<SoundEffect>();

        public SoundCollection()
        {

        }

        public void AddSound(SoundEffect sound)
        {
            sounds.Add(sound);
        }

        public void PlayRandomSound()
        {
            int randomizer = Game1.rnd.Next(sounds.Count);
            Audio.PlaySound(sounds[randomizer]);
        }
    }
}
