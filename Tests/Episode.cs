using UnityEngine;
using System.Collections;

namespace Assets.Skrypty.Tests
{
    public class Episode
    {

        int moves;
        float elapsedTime;

        public int Moves
        {
            get;set;
        }
        public float ElapsedTime
        {
            get;set;
        }

        public Episode(int moves, float elapsedTime)
        {
            this.Moves = moves;
            this.ElapsedTime = elapsedTime;
        }
    }
}
