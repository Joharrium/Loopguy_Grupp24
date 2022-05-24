using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test_Loopguy
{
    public class Profile
    {
        private int id;
        public bool hasRailgun;
        public bool colorBlind;
        
        public int Id
        {
        get { return id; }
        }

        
        List<int> keys = new List<int>();

        public Profile(int id)
        {
            this.id = id;
            LoadProfile();
        }

        public List<int> GetKeys()
        {

            return keys;
        }

        public void SaveToFile()
        {
            string path = String.Format(@"saves\profile{0}\settings.txt", id);
            List<string> toWrite = new List<string>();

            toWrite.Add(hasRailgun.ToString());
            toWrite.Add(colorBlind.ToString());

            File.WriteAllLines(path, toWrite);
        }

        public void ClearProfile()
        {
            keys.Clear();
            hasRailgun = false;
            colorBlind = false;
            SaveKeys();
            SaveToFile();
        }
        public void RailgunPickedUp()
        {
            hasRailgun = true;
            EntityManager.player.hasRailgun = true;
            SaveToFile();
            
        }
        private void LoadProfile()
        {
            if (File.Exists(String.Format(@"saves\profile{0}\keys.txt", id)))
            {
                List<string> lines = new List<string>();
                foreach (string line in System.IO.File.ReadLines(String.Format(@"saves\profile{0}\keys.txt", id)))
                {
                    lines.Add(line);
                }

                List<int> keysToAdd = new List<int>();
                foreach (string l in lines)
                {
                    keysToAdd.Add(Int32.Parse(l));
                }



                keys = keysToAdd;
            }
            if (File.Exists(String.Format(@"saves\profile{0}\keys.txt", id)))
            {
                List<string> lines = new List<string>();
                foreach (string line in System.IO.File.ReadLines(String.Format(@"saves\profile{0}\settings.txt", id)))
                {
                    lines.Add(line);
                }
                
                hasRailgun = Boolean.Parse(lines[0]);
                colorBlind = Boolean.Parse(lines[1]);
                
            }
            }

        public void AddKey(int i)
        {
            foreach(int k in keys)
            {
                if(k == i)
                {
                    return;
                }
            }
            keys.Add(i);
            SaveKeys();
        }

        private void SaveKeys()
        {
            string path = String.Format(@"saves\profile{0}\keys.txt", id);
            List<string> toWrite = new List<string>();

            foreach(int i in keys)
            {
                toWrite.Add(i.ToString());
            }

            File.WriteAllLines(path, toWrite);
        }
    }
}
