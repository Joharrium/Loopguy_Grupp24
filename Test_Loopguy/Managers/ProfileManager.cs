using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test_Loopguy
{
    public static class ProfileManager
    {
        static List<Profile> profiles = new List<Profile>();
        static Profile currentProfile;
        

        public static void Init()
        {
            profiles.Add(new Profile(1));
            profiles.Add(new Profile(2));
            profiles.Add(new Profile(3));
            currentProfile = profiles[0];
            
        }

        public static void AddKey(int key)
        {
            currentProfile.AddKey(key);
        }

        public static void TutorialFinished()
        {
            currentProfile.TutorialFinished();
        }

        public static List<int> GetKeys()
        {
            return currentProfile.GetKeys();
        }

        public static void ChangeProfile()
        {
            currentProfile = profiles[(CurrentProfileId() + 1) % profiles.Count];
            LevelManager.Reset();
            
        }

        public static bool HasPlayedTutorial()
        {
            return currentProfile.PlayedTutorial;
        }

        public static void SaveSettings(int music, int sound, int scale, bool fullscreen)
        {
            string path = @"saves\settings\settings.txt";
            List<string> toWrite = new List<string>();

            toWrite.Add(music.ToString());
            toWrite.Add(sound.ToString());
            toWrite.Add(scale.ToString());
            toWrite.Add(fullscreen.ToString());

            File.WriteAllLines(path, toWrite);
        }

        public static int CurrentProfileId()
        {
            return currentProfile.Id;
        }
    }
}
