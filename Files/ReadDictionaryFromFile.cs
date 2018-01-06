using UnityEngine;
using System.Collections;
using Assets.Skrypty;
using System.IO;
using System;
using UnityEngine.Assertions;
using System.Globalization;

namespace Assets.Skrypty.Files
{
    public abstract class ReadDictionaryFromFile : IReadDictionaryFromFile
    {
        string filePath = "data.txt";
        protected string data = "";

        public ReadDictionaryFromFile(string filePath)
        {
            FilePath = filePath;            
        }

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
            }
        }

        public abstract void Read();
    }
}
