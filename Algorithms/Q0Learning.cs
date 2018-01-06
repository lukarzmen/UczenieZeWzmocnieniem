using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Assets.Skrypty.Files;
using System;

namespace Assets.Skrypty.Algorithm
{
    public class Q0Learning : QLearning
    {
        ExtendedDictionary<State, float[]> qTable;
        public const string algorithmName = "Q0";
        string filePath;
        public string fileName = "qdata1.txt";
        public bool startLearning = false;
        const float EPSILON = 0.05f;

        static int actionAmount = 0;

        public override string FilePath
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

        public override void Awake()
        {
            base.Awake();
            FilePath = CombineFilePath(fileName);
            if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
            {
                qTable = new ExtendedDictionary<State, float[]>(new StateEqualityComparer());
            }
            else
            {
                qTable = new ReadQ0DictionaryFromFile(FilePath).Q0Dictionary;
            }

            if (startLearning)
                qTable = new ExtendedDictionary<State, float[]>(new StateEqualityComparer());

        }
        public override void Update()
        {
            base.Update();
            LearningAlgorithm();
            DontAllowFall();          
        }
        private void OnApplicationQuit()
        {
            new Q0DictionaryToFile(FilePath, qTable);
        }

        public override void LearningAlgorithm()
        {
            SetParameters();
            switch (state)
            {
                case (int)QLearningStateMachine.DoAction:
                    {
                        actualState = GetState();
                        if (!qTable.ContainsKey(actualState))
                        {
                            qTable.AddOrUpdate(actualState, ZeroActionValues());
                        }
                        selectedActionNumber = GetRandomAction(actualState);
                        ExecuteAction(selectedActionNumber);

                        state = (int)QLearningStateMachine.Observe;
                        break;
                    }
                case (int)QLearningStateMachine.Observe:
                    {
                        nextState = GetState();
                        DontAllowBlockBot();

                        if (actualState != nextState)
                        {                         
                            float maxQvalue;
                            if (qTable.ContainsKey(nextState))
                                maxQvalue = qTable[nextState].Max();
                            else
                            {
                                qTable.AddOrUpdate(nextState, ZeroActionValues());
                                maxQvalue = 0;
                            }

                            if (!rewardIsReached)
                                Reward = (nextState.State1 - actualState.State1) * Rewards.ForPosition;
                            if (enemyIsReached)
                            {
                                Reward = Rewards.EnemyTouched;
                            }
                            
                            float delta = Reward + GAMMA * maxQvalue - qTable[actualState][selectedActionNumber];
                            float qValue = qTable[actualState][selectedActionNumber] =
                                qTable[actualState][selectedActionNumber] + ALPHA * delta;                           
                            if(TerminationStateIsReached())
                                new Q0DictionaryToFile(FilePath, qTable);

                            state = (int)QLearningStateMachine.DoAction;
                        }
                        break;
                    }
            }
        }
        public override int GetRandomAction(State actualState)
        {
            int[] availableActions;
            actionAmount++;
            Debug.Log("Liczba akcji: " + actionAmount);
            if (bot.Grounded)
                availableActions = new int[3] { (int)Actions.Left, (int)Actions.Right, (int)Actions.Jump };
            else
                availableActions = new int[2] { (int)Actions.Left, (int)Actions.Right };

            System.Random random = new System.Random();
            float randomValue = (float)random.NextDouble();
            if (randomValue > EPSILON)
            {
                List<int> greedyActionsList = new List<int>();
                float[] qValues = qTable[actualState];
                float maxQvalue = qValues.Max();

                for (int i = 0; i < qValues.Length; i++)
                {
                    if (qValues[i] >= maxQvalue)
                    {
                        maxQvalue = qValues[i];
                        greedyActionsList.Add(i);
                    }
                }

                int[] greedyActions = greedyActionsList.ToArray();
                return greedyActions[random.Next(0, greedyActions.Length)];
            }
            else
            {
                return availableActions[random.Next(0, availableActions.Length)];
            }
        }
        public override string CombineFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}
