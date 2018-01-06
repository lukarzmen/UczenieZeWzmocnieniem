using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Skrypty
{
    
    public class State
    {
        private float prescaler = 1.0f;
        int digits = 1;

        public State(float positionToObstacleX, float positionToObstacleY, float positionX)
        {
            State1 = DiscreteFloatToInteger(positionToObstacleX);
            State2 = DiscreteFloatToInteger(positionToObstacleY);
            State3 = DiscreteFloatToInteger(positionX);
        }

        public State(int state1, int state2, int state3)
        {
            State1 = state1;
            State2 = state2;
            State3 = state3;
        }

        public int[] GetState()
        {
            return new int[3] { State1, State2, State3 };
        }

        public int State1   {get; set;}
        public int State2   {get; set;}
        public int State3   {get; set;}
        public int State4   {get; set;}

        private float TranslatePosition(float position)
        {
            return (float)Math.Round(position * prescaler, 1);
        }

        private int DiscreteFloatToInteger(float transformPosition)
        {
            return Mathf.RoundToInt(TranslatePosition(transformPosition)); ;
        }

        public static bool operator ==(State x, State y)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(x, y))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            // Return true if the fields match:
            return x.State1 == y.State1 && x.State2 == y.State2 && x.State3 == y.State3 && x.State4 == y.State4;
        }
        public static bool operator !=(State x, State y)
        {
            return !(x == y);
        }
    }
}

