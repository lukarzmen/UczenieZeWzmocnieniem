using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Assets.Skrypty.Files
{
    class ReadQLambdaDictionaryFromFile : ReadDictionaryFromFile
    {
        ExtendedDictionary<State, EQValues> qLambdaDictionary;
        string filePath = "";
       
        public ReadQLambdaDictionaryFromFile(string filePath) : base(filePath)
        {
            FilePath = filePath;
            QLambdaDictionary = new ExtendedDictionary<State, EQValues>(new StateEqualityComparer());
            Read();
        }
        public override void Read()
        {
            try
            {
                data = File.ReadAllText(FilePath).Replace(Environment.NewLine, "");
                Assert.AreNotEqual(data, "");

                string[] keyValuePairs = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in keyValuePairs)
                {
                    string[] kvPair = new string[3];
                    kvPair = pair.Split(new char[] { ':' }, StringSplitOptions.None);

                    string[] key = kvPair[0].Split(new char[] { ',' }, StringSplitOptions.None);
                    string[] valueQ = kvPair[1].Split(new char[] { ',' }, StringSplitOptions.None);
                    string[] valueE = kvPair[2].Split(new char[] { ',' }, StringSplitOptions.None);

                    float[] valuesQ = new float[3];
                    float[] valuesE = new float[3];
                    int[] keys = new int[3];

                    int i = 0;
                    foreach (var v in valueQ)
                    {
                        valuesQ[i] = float.Parse(v);
                        i++;
                    }

                    i = 0;
                    foreach (var e in valueE)
                    {
                        valuesE[i] = float.Parse(e);
                        i++;
                    }

                    i = 0;
                    foreach (var k in key)
                    {
                        keys[i] = int.Parse(k);
                        i++;
                    }

                    QLambdaDictionary.AddOrUpdate(new State(keys[0], keys[1], keys[2]), new EQValues(valuesQ, valuesE));
                    Assert.IsTrue(QLambdaDictionary.ContainsKey(new State(keys[0], keys[1], keys[2])));
                }
                Assert.IsTrue(keyValuePairs.Length == QLambdaDictionary.Count);
            }
            catch (FileNotFoundException)
            {
                QLambdaDictionary.AddOrUpdate(new State(0, 0, 0), new EQValues());
            }
        }

        public ExtendedDictionary<State, EQValues> QLambdaDictionary
        {
            get
            {
                return qLambdaDictionary;
            }

            set
            {
                qLambdaDictionary = value;
            }
        }
    }
}
