
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
        public static float MusicVolume
        {
            get { return musicVolume; }
        }
        static float soundVolume = 0.5F;

        public static float SoundVolume
        {
            get { return soundVolume; }
        }
        //music

        public static SoundEffect sus_low, sus_high, unatco_hq, march_of_the_white_knights, nyc_streets, oceanlab_action;
        
        private static Song playingTrack;
        private static List<Song> combatMusicCurrent = new List<Song>();
        private static List<Song> idleMusicCurrent = new List<Song>();

        public static List<Song> songs = new List<Song>();


        //sound
        public static SoundCollection lasergun, footstepsMetal, footstepsStone, footstepsGeneric, footstepsGrass, footstepsDirt, meleeOnMetal, meleeOnFlesh;
        public static SoundEffect meepmerp, swing, open, deny, dash, door_hiss_sound, 
            box_destroy, shrub_destroy, keypickup, healing, player_hit, reload, splash,
            ping, robotEnemyCharge1, robotEnemyCharge2, robotEnemyShot, hitByElectricity;

        //public static SoundEffect 

        public static SoundEffect EnergyGun_Shoot1A, EnergyGun_Shoot1B, EnergyGun_Shoot1C, EnergyGun_Shoot1D, EnergyGun_Shoot2A, EnergyGun_Shoot2B, EnergyGun_Shoot2C, EnergyGun_Shoot2D;
        public static SoundEffect grass_1, grass_2, dirt_1, dirt_2, generic_1, generic_2, metal_1, metal_2, stone_1, stone_2;
        public static SoundEffect meleeOnMetal1, meleeOnMetal2, meleeOnMetal3, meleeOnMetal4, meleeOnMetal5;
        public static SoundEffect meleeOnFlesh1, meleeOnFlesh2, meleeOnFlesh3;

        static public void Load(ContentManager Content)
        {
            LoadMusic(Content);
            LoadSound(Content);
            CreateCollections();
            CreateSongs();  
        }
        static void CreateSongs()
        {
            //should probably be called stuff like "battle_1" or "idle_1" or stuff
            songs.Add(new Song("combat_1", march_of_the_white_knights));
            songs.Add(new Song("idle_1", unatco_hq));
            songs.Add(new Song("idle_2", nyc_streets));
            songs.Add(new Song("combat_2", oceanlab_action));

            foreach (Song s in songs)
            {
                s.SetVolume(musicVolume);
            }
        }

        public static void SetMusicVolume(int i)
        {
            musicVolume = (float)(Math.Pow((float)i / 100, 2));
            foreach (Song s in songs)
            {
                s.SetVolume(musicVolume);
            }
        }
        public static void SetSoundVolume(float i)
        {
            soundVolume = (float)(Math.Pow(i / 100, 2));
            //soundVolume = (i / 100);
        }

        static void CreateCollections()
        {
            //laser gun collection
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
            //footsteps
            {
                footstepsDirt = new SoundCollection();
                footstepsGeneric = new SoundCollection();
                footstepsGrass = new SoundCollection();
                footstepsMetal = new SoundCollection();
                footstepsStone = new SoundCollection();

                footstepsDirt.AddSound(dirt_1);
                footstepsDirt.AddSound(dirt_2);

                footstepsGeneric.AddSound(generic_1);
                footstepsGeneric.AddSound(generic_2);

                footstepsGrass.AddSound(grass_1);
                footstepsGrass.AddSound(grass_2);

                footstepsMetal.AddSound(metal_1);
                footstepsMetal.AddSound(metal_2);

                footstepsStone.AddSound(stone_1);
                footstepsStone.AddSound(stone_2);
            }
            //meleeHitOnMetal
            {
                meleeOnMetal = new SoundCollection();
                meleeOnMetal.AddSound(meleeOnMetal1);
                meleeOnMetal.AddSound(meleeOnMetal2);
            }
            //meleeHitOnFlesh
            {
                meleeOnFlesh = new SoundCollection();
                meleeOnFlesh.AddSound(meleeOnFlesh1);
                meleeOnFlesh.AddSound(meleeOnFlesh2);
                meleeOnFlesh.AddSound(meleeOnFlesh3);
            }
        }
        static void LoadMusic(ContentManager c)
        {
            unatco_hq = c.Load<SoundEffect>("audio/music/unatco_hq");
            march_of_the_white_knights = c.Load<SoundEffect>("audio/music/march_of_the_white_knights");
            nyc_streets = c.Load<SoundEffect>("audio/music/nyc_streets");
            oceanlab_action = c.Load<SoundEffect>("audio/music/oceanlab_action");
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
            healing = c.Load<SoundEffect>("audio/sound/healing");
            player_hit = c.Load<SoundEffect>("audio/sound/player_hit");
            reload = c.Load<SoundEffect>("audio/sound/reload");
            splash = c.Load<SoundEffect>("audio/sound/splash");
            ping = c.Load<SoundEffect>("audio/sound/ping");
            robotEnemyCharge1 = c.Load<SoundEffect>("audio/sound/egon_windup2");
            robotEnemyCharge2 = c.Load<SoundEffect>("audio/sound/tauCannonEdited2");
            robotEnemyShot = c.Load<SoundEffect>("audio/sound/tauCannonShotEdited");
            hitByElectricity = c.Load<SoundEffect>("audio/sound/playerZapped");

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

            {
                dirt_1 = c.Load<SoundEffect>("audio/sound/footsteps/dirt_1");
                dirt_2 = c.Load<SoundEffect>("audio/sound/footsteps/dirt_2");
                generic_1 = c.Load<SoundEffect>("audio/sound/footsteps/generic_1");
                generic_2 = c.Load<SoundEffect>("audio/sound/footsteps/generic_2");
                grass_1 = c.Load<SoundEffect>("audio/sound/footsteps/grass_1");
                grass_2 = c.Load<SoundEffect>("audio/sound/footsteps/grass_2");
                metal_1 = c.Load<SoundEffect>("audio/sound/footsteps/metal_1");
                metal_2 = c.Load<SoundEffect>("audio/sound/footsteps/metal_2");
                stone_1 = c.Load<SoundEffect>("audio/sound/footsteps/stone_1");
                stone_2 = c.Load<SoundEffect>("audio/sound/footsteps/stone_2");
            }

            {
                meleeOnMetal1 = c.Load<SoundEffect>("audio/sound/cbar_hit1reduced");
                meleeOnMetal2 = c.Load<SoundEffect>("audio/sound/cbar_hit2reduced");
            }

            {
                meleeOnFlesh1 = c.Load<SoundEffect>("audio/sound/cbar_hitbod1");
                meleeOnFlesh2 = c.Load<SoundEffect>("audio/sound/cbar_hitbod2");
                meleeOnFlesh3 = c.Load<SoundEffect>("audio/sound/cbar_hitbod3");
            }
        }

        public static List<Song> LoadLevelMusic(List<string> names, bool combat)
        {
            combatMusicCurrent.Clear();
            idleMusicCurrent.Clear();
            foreach(Song s in songs)
            {
                foreach(string n in names)
                {
                    if(s.name.Equals(n))
                    {
                        if(combat)
                        {
                            combatMusicCurrent.Add(s);
                        }
                        else
                        {
                            idleMusicCurrent.Add(s);
                        }
                        
                    }
                }
            }
            if(combat)
            {
                return combatMusicCurrent;
            }
            else
            {
                return idleMusicCurrent;
            }
            //PlayMusic();
        }

        public static void UpdateLevelMusic()
        {
            combatMusicCurrent.Clear();
            idleMusicCurrent.Clear();
            combatMusicCurrent.AddRange(LevelManager.GetMusic(true));
            idleMusicCurrent.AddRange(LevelManager.GetMusic(false));
            PlayMusic();
        }

        public static void Update()
        {
            if(playingTrack != null)
            {
                if (!playingTrack.IsPlaying())
                {
                    StopMusic();
                    PlayMusic();
                }
            }
            
        }

        public static void PlayMusic()
        {
            bool combat = true;
            //for testing, remove this bool later and get combat status from somewhere else

            if (combatMusicCurrent.Contains(playingTrack) && combat /*|| idleMusicCurrent.Contains(playingTrack)*/)
            {
                playingTrack.Play();
            }
            else
            {
                if(playingTrack != null)
                {
                    playingTrack.Stop();
                }
                
                playingTrack = null;
            }
           
            if (playingTrack == null)
            {
                //if player in combat
                if (combat)
                {
                    int trackRandomizer = Game1.rnd.Next(combatMusicCurrent.Count);
                    playingTrack = combatMusicCurrent[trackRandomizer];
                }
                else
                {
                    int trackRandomizer = Game1.rnd.Next(idleMusicCurrent.Count);
                    playingTrack = idleMusicCurrent[trackRandomizer];
                }
            }
            if(playingTrack != null)
            {
                playingTrack.Play();
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

    public class Song
    {
        public string name;
        private SoundEffect sound;
        private SoundEffectInstance soundInstance;

        public Song(string name, SoundEffect sound)
        {
            this.name = name;
            soundInstance = sound.CreateInstance();
            soundInstance.IsLooped = false;
        }

        public bool IsPlaying()
        {
            if(soundInstance.State == SoundState.Stopped)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Stop()
        {
            soundInstance.Stop();
        }

        public void SetVolume(float volume)
        {
            soundInstance.Volume = volume;
        }
        public void Pause()
        {
            soundInstance.Pause();
        }

        public void Play()
        {
            soundInstance.Play();
        }

    }
}
