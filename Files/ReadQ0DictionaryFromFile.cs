using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Assets.Skrypty.Files 
{
    public sealed class ReadQ0DictionaryFromFile : ReadDictionaryFromFile
    {
        ExtendedDictionary<State, float[]> q0Dictionary;

        public ReadQ0DictionaryFromFile(string filePath) : base(filePath)
        {
            FilePath = filePath;
            Q0Dictionary = new ExtendedDictionary<State, float[]>(new StateEqualityComparer());
            Read();
        }

        public ExtendedDictionary<State, float[]> Q0Dictionary
        {
            get
            {
                return q0Dictionary;
            }

            set
            {
                q0Dictionary = value;
            }
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
                    string[] kvPair = new string[2];
                    kvPair = pair.Split(new char[] { ':' }, StringSplitOptions.None);

                    string[] key = kvPair[0].Split(new char[] { ',' }, StringSplitOptions.None);
                    string[] value = kvPair[1].Split(new char[] { ',' }, StringSplitOptions.None);

                    float[] values = new float[3];
                    int[] keys = new int[3];

                    int i = 0;
                    foreach (var v in value)
                    {
                        try
                        {
                            values[i] = float.Parse(v);
                            i++;
                        }
                        catch (IndexOutOfRangeException x)
                        {
                            x.Message.ToString();
                            break;
                        }
                    }

                    i = 0;
                    foreach (var k in key)
                    {
                        try
                        {

                            keys[i] = int.Parse(k);
                            i++;

                        }
                        catch (IndexOutOfRangeException x)
                        {
                            x.Message.ToString();
                            break;
                        }
                    }

                    Q0Dictionary.AddOrUpdate(new State(keys[0], keys[1], keys[2]), values);
                    Assert.IsTrue(Q0Dictionary.ContainsKey(new State(keys[0], keys[1], keys[2])));
                }
                Assert.IsTrue(keyValuePairs.Length == Q0Dictionary.Count);
            }
            catch (FileNotFoundException)
            {
                Q0Dictionary.AddOrUpdate(new State(0, 0, 0), new float[1] { 0.0f });
            }
        }
    }
}
