using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Assets.Skrypty.Files
{
    class QLambdaDictionaryToFile : SaveDictionaryToFile
    {

        ExtendedDictionary<State, EQValues> qLambdaDict;
        string filePath = "data.txt";

        public QLambdaDictionaryToFile(string filePath, ExtendedDictionary<State, EQValues> qLambdaDictionary) : base(filePath)
        {
            FilePath = filePath;
            QLambdaDict = qLambdaDictionary;
            DictionaryToFile();
        }

        public ExtendedDictionary<State, EQValues> QLambdaDict
        {
            get
            {
                return qLambdaDict;
            }

            set
            {
                qLambdaDict = value;
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
            Assert.IsNotNull(QLambdaDict, "slownik jest null");
            try
            {
                StreamWriter sw = File.CreateText(FilePath);
                string line = "";
                foreach (var d in QLambdaDict)
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

                    var value = d.Value.QValues;
                    for (int i = 0; i < value.Length; i++)
                    {
                        var v = value[i];
                        line += v;
                        if (i == value.Length - 1)
                            break;
                        line += ",";
                    }

                    line += ":";

                    var val = d.Value.EValues;
                    for (int i = 0; i < val.Length; i++)
                    {
                        var z = val[i];
                        line += z;
                        if (i == val.Length - 1)
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
            catch (IOException e)
            {
                var message = e.Message;
            }
        }
    }
}

