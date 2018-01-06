using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Assets.Skrypty.Files
{
    class Q0DictionaryToFile : SaveDictionaryToFile
    {
        ExtendedDictionary<State, float[]> q0Dict;
        string filePath = "data0.txt";

        public Q0DictionaryToFile(string filePath, ExtendedDictionary<State, float[]> q0Dictionary) : base(filePath)
        {
            FilePath = filePath;
            Q0Dictionary = q0Dictionary;
            DictionaryToFile();
        }

        public ExtendedDictionary<State, float[]> Q0Dictionary
        {
            get
            {
                return q0Dict;
            }

            set
            {
                q0Dict = value;
            }
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

        public override void DictionaryToFile()
        {
            Assert.IsNotNull(Q0Dictionary, "slownik jest null");
            try
            {
                StreamWriter sw = File.CreateText(FilePath);
                string line = "";
                foreach (var d in Q0Dictionary)
                {
                    var key = d.Key.GetState();
                    for (int i = 0; i < key.Length; i++)
                    {
                        var k = key[i];
                        line += k;
                        if (i == key.Length - 1)
                            break;
                        line += ",";
                    }

                    line += ":";

                    var value = d.Value;
                    for (int i = 0; i < value.Length; i++)
                    {
                        var v = value[i];
                        line += v;
                        if (i == value.Length - 1)
                            break;
                        line += ",";
                    }
                    line += ";";

                    Assert.AreNotEqual(line, "", "wystapil blad tworzenia lancucha");
                    sw.WriteLine(line);
                    line = "";
                }
                sw.Close();
            }
            catch (IOException ex)
            {
                ex.Message.ToString();
            }
        }
    }
}
