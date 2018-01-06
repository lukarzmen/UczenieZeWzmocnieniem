using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityStandardAssets._2D;
using System;
using Assets.Skrypty.Files;
using System.IO;

namespace Assets.Skrypty
{
    class EQValues : IEQValues
    {
        private float[] qValues;
        private float[] eValues;

        public float[] QValues
        {
            get
            {
                return qValues;
            }

            set
            {
                qValues = value;
            }
        }
        public float[] EValues
        {
            get
            {
                return eValues;
            }

            set
            {
                eValues = value;
            }
        }

        public EQValues()
        {
            QValues = new float[3];
            EValues = new float[3];
        }
        public EQValues(float[] qValues, float[] eValues)
        {
            QValues = qValues;
            EValues = eValues;
        }


        public void SetEQValues(int action, float e, float q)
        {
            QValues[action] = q;
            EValues[action] = e;
        }
        public void increaseEValue(int action)
        {
            EValues[action]++;
        }
        public void updateEValues(float lambda, float gamma)
        {
             for(int i = 0; i < EValues.Length; i++)
            {
                EValues[i] *= (lambda * gamma);
            }
        }
        public void zeroEValues()
        {
            for (int i = 0; i < EValues.Length; i++)
            {
                EValues[i] = 0;
            }
        }
        public void SetEValue(int action, float e)
        {
            EValues[action] = e;
        }
        public void SetQValue(int action, float q)
        {
            QValues[action] = q;
        }
        public float GetEValue(int action)
        {
            return EValues[action];
        }
        public float GetQValue(int action)
        {
            return QValues[action];
        }
    }
}
