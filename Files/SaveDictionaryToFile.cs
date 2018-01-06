using System.IO;
using UnityEngine.Assertions;
using System;
using UnityEngine;

namespace Assets.Skrypty.Files
{
    public abstract class SaveDictionaryToFile : ISaveDictionaryToFile
    {
        string data = "";
        private string path;

        public SaveDictionaryToFile(string path)
        {
            Path = Application.persistentDataPath +  path;
        }

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                if (path != "" || path.Contains(".txt"))
                    path = value;
                else
                    path = "data.txt";
            }
        }

        public abstract void DictionaryToFile();
    }
}