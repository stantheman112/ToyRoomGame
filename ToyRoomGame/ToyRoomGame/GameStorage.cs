using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;


namespace Toyroom.GameStorage
{
   public class GameData        
    {
        private string scene;
        public string FileName
        {
            get
            {
                return scene;
            }
        }
        public GameData(string sceneName)
        {

            scene = sceneName;
        }

        public bool fileExists(string fileName)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return file.FileExists(fileName);
            }
            
        }
        public void saveScore(int hscore, int mscore)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryWriter writer = new BinaryWriter(file.CreateFile(scene)))
                {
                    writer.Write(hscore + "_" + mscore);
                }

            }
        }
        public void saveProgression(int level)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryWriter writer = new BinaryWriter(file.CreateFile("lobby")))
                {
                    writer.Write(level);
                }

            }

        }
        public int getProgression(string fileName)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryReader reader = new BinaryReader(file.OpenFile(fileName, FileMode.OpenOrCreate)))
                {
                    if (reader.BaseStream.Length >0)
                        return reader.ReadInt16();
                    else return 0;
                }
            }
          
        }
        public string getHighScore(string fileName)
        {
            string highScore = string.Empty;
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryReader reader = new BinaryReader(file.OpenFile(fileName, FileMode.OpenOrCreate)))
                {
                 
                    highScore = reader.ReadString();
                }
            }
            return highScore;

        }



    }
}
