using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assets.Skrypty.Tests
{
    [TestClass()]
    public class Tests
    {
        List<Episode> analysis;
        string filePath;

        public Tests(string filePath)
        {
            analysis = new List<Episode>();
            this.filePath = filePath;
        }

        public void Add(int move, float elapsedTime)
        {
            analysis.Add(new Episode(move, elapsedTime));
        }

        public void WriteToFile()
        {
            StreamWriter sw = File.CreateText(filePath);
            int episode = 1;
            foreach(var an in analysis)
            {
                string line = "";                                             
                line = episode.ToString() + ":" +  an.Moves.ToString() + ":" + an.ElapsedTime.ToString();
                episode++;
                sw.WriteLine(line);
                line = "";                
            }
            sw.Close();
        }
    }
}
