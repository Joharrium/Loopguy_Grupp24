
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
        static float musicVolume = 0.0F;
        static float soundVolume = 0.8F;
        //music
        static SoundEffect musicTitle, musicBattle1, musicEnemyTurn, musicOverworld1, musicOverworld2, musicOverworld3, musicMystery;

        //sound
        public static SoundEffect meepmerp, swing, open, deny, dash, door_hiss_sound;
        static public void Load(ContentManager Content)
        {
            LoadMusic(Content);
            LoadSound(Content);

            /*
            foreach (SoundEffectInstance s in sound)
            {
                s.Volume = soundVolume;
            }


            foreach (SoundEffectInstance m in music)
            {
                m.IsLooped = true;
                m.Volume = musicVolume;
            }
            */
        }

        static void LoadMusic(ContentManager c)
        {

        }

        static void LoadSound(ContentManager c)
        {
            meepmerp = c.Load<SoundEffect>("audio/sound/meepmerp");
            swing = c.Load<SoundEffect>("audio/sound/swing");
            open = c.Load<SoundEffect>("audio/sound/open");
            deny = c.Load<SoundEffect>("audio/sound/deny");
            dash = c.Load<SoundEffect>("audio/sound/dash");
            door_hiss_sound = c.Load<SoundEffect>("audio/sound/door_hiss_sound");
        }

        public static void PlaySound(SoundEffect sound)
        {
            if(sound != null)
            {
                SoundEffectInstance instance = sound.CreateInstance();
                instance.Play();
            }
        }

        /*

        static void FadeOut(SoundEffectInstance sound)
        {
            if (sound.Volume > 0)
            {
                sound.Volume = Math.Clamp(sound.Volume - 0.01F, 0, 0.35F);
            }
            if (sound.Volume == 0)
            {
                sound.Pause();
            }
        }

        static void FadeIn(SoundEffectInstance sound)
        {
            if (sound.Volume < 0.35)
            {
                sound.Volume = Math.Clamp(sound.Volume + 0.01F, 0, 0.35F);
            }
        }
        */
        /*
        public static void Music(GameState gamestate, int level)
        {
            if (gamestate == GameState.Title)
            {
                musicTitleInst.Play();
                FadeIn(musicTitleInst);

            }
            else { FadeOut(musicTitleInst); }

            if (gamestate == GameState.Conversation)
            {
                musicMysteryInst.Play();
                FadeIn(musicMysteryInst);
            }

            else { FadeOut(musicMysteryInst); }


            int battleSong = -1;
            if (gamestate == GameState.Battle)
            {
                if (battleSong == -1)
                {
                    battleSong = rnd.Next(0, 2);
                }

                if (battleSong == 0)
                {
                    musicBattle1Inst.Play();
                    FadeIn(musicBattle1Inst);
                }
                //change so one is for enemy turn instead

            }
            else
            {
                battleSong = -1;
                FadeOut(musicBattle1Inst);
            }

            if (gamestate == GameState.Map)
            {
                if (TurnHandler.playerTurn)
                {
                    FadeOut(musicEnemyTurnInst);
                    if (level == 1)
                    {
                        musicOverworld1Inst.Play();
                        FadeIn(musicOverworld1Inst);
                    }
                    else { FadeOut(musicOverworld1Inst); }

                    if (level == 2)
                    {
                        musicOverworld2Inst.Play();
                        FadeIn(musicOverworld2Inst);
                    }
                    else { FadeOut(musicOverworld2Inst); }

                    if (level == 3)
                    {
                        musicOverworld3Inst.Play();
                        FadeIn(musicOverworld3Inst);
                    }
                    else { FadeOut(musicOverworld3Inst); }
                }
                else
                {
                    musicEnemyTurnInst.Play();
                    FadeIn(musicEnemyTurnInst);
                }
            }
            else
            {
                FadeOut(musicOverworld1Inst);
                FadeOut(musicOverworld2Inst);
                FadeOut(musicOverworld3Inst);
                FadeOut(musicEnemyTurnInst);
            }


        }

        /*
        public void Reset()
        {
            foreach (SoundEffectInstance m in music)
            {
                m.Volume = 0;
                m.Play();
                m.Stop();
                m.Volume = musicVolume;
            }
        }
        

        public void Update(Vector2 mousePos, bool mousePressed)
        {
            if(mousePressed)
            {
                musicSlider.Move(mousePos);
                soundSlider.Move(mousePos);
            }
            musicVolume = (float)musicSlider.value;
            soundVolume = (float)soundSlider.value;
            foreach (SoundEffectInstance s in sound)
            {
                s.Volume = soundVolume;
            }
            foreach (SoundEffectInstance m in music)
            {
                m.Volume = musicVolume;
            }
        }
        */
    }
}
