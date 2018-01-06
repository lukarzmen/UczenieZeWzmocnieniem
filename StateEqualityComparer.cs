using UnityEngine;
using System.Collections;
using Assets.Skrypty;
using System.Collections.Generic;
using System;

namespace Assets.Skrypty
{
    public class StateEqualityComparer : IEqualityComparer<State>
    {
        public bool Equals(State x, State y)
        {
            return x.State1 == y.State1 && x.State2 == y.State2 && x.State3 == y.State3 && x.State4 == y.State4;
        }
        public int GetHashCode(State obj)
        {
            unchecked
            {
                return 17 * 23 * (obj.State1 + obj.State2 + obj.State3 + obj.State4).GetHashCode();
            }
        }
    }
}