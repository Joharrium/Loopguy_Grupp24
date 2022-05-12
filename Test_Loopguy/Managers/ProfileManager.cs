using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    public static class ProfileManager
    {
        static Profile currentProfile;

        public static void Init()
        {
            currentProfile = new Profile(1);
        }

        public static void AddKey(int key)
        {
            currentProfile.AddKey(key);
        }

        public static List<int> GetKeys()
        {
            return currentProfile.GetKeys();
        }
    }
}
