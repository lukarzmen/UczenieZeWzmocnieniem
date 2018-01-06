using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Assets.Skrypty.Files;
using UnityEngine.Assertions;
using Assets.Skrypty.Tests;

namespace Assets.Skrypty.Algorithm
{
    public class QLambdaLearning : QLearning
    {
        ExtendedDictionary<State, EQValues> qLambdaTable;
        string filePath;
        public string fileName = "qLambdaData.txt";
        public bool startLearning = false;

        public const string algorithmName = "QLambda";
        const float EPSILON = 0.05f;
        protected const float LAMBDA = 0.3f;

        public override void Awake()
        {
            base.Awake();
            FilePath = CombineFilePath(fileName);
            qLambdaTable = new ExtendedDictionary<State, EQValues>(new StateEqualityComparer());
            if (!File.Exists(FilePath) || new FileInfo(FilePath).Length == 0)
            {
                qLambdaTable = new ExtendedDictionary<State, EQValues>(new StateEqualityComparer());
            }
            else
            {
                qLambdaTable = new ReadQLambdaDictionaryFromFile(FilePath).QLambdaDictionary;
            }

            if (startLearning)
            {
                qLambdaTable = new ExtendedDictionary<State, EQValues>(new StateEqualityComparer());
                ResetETable();
            }
        }
        public override void Update()
        {
            base.Update();
            LearningAlgorithm();
        }
        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            SaveQLambdaDataToFile();
        }

        public override string CombineFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
        private void SaveQLambdaDataToFile()
        {
            new QLambdaDictionaryToFile(FilePath, qLambdaTable);
            Assert.IsTrue(File.Exists(FilePath));
        }
        public override void LearningAlgorithm()
        {
            SetParameters();
            switch (state)
            {
                case (int)QLearningStateMachine.DoAction:
                    {
                        actualState = GetState();
                        if (!qLambdaTable.ContainsKey(actualState))
                        {
                            qLambdaTable.AddOrUpdate(actualState, new EQValues());
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
                            if (qLambdaTable.ContainsKey(nextState))
                                maxQvalue = qLambdaTable[nextState].QValues.Max();
                            else
                            {
                                qLambdaTable.AddOrUpdate(nextState, new EQValues());
                                maxQvalue = 0;
                            }

                            if (!rewardIsReached)
                                Reward = (nextState.State1 - actualState.State1) * Rewards.ForPosition;
                            if (enemyIsReached)
                            {
                                Reward = Rewards.EnemyTouched;
                            }

                            float delta = Reward + GAMMA * maxQvalue - qLambdaTable[actualState].GetQValue(selectedActionNumber);
                            qLambdaTable[actualState].SetEValue(selectedActionNumber, 1);

                            if (!rewardIsReached)
                                foreach (var key in qLambdaTable.Keys)
                                {
                                    for (int action = 0; action < qLambdaTable[key].QValues.Length; action++)
                                        qLambdaTable[key].QValues[action] += (ALPHA * delta * qLambdaTable[key].EValues[action]);
                                    qLambdaTable[key].updateEValues(LAMBDA, GAMMA);
                                }

                            if (TerminationStateIsReached())
                            {
                                ResetETable();
                                SaveQLambdaDataToFile();
                            }
                            state = (int)QLearningStateMachine.DoAction;
                        }
                        break;
                    }
            }
        }
        private void ResetETable()
        {
            foreach (var key in qLambdaTable.Keys)
                qLambdaTable[key].zeroEValues();
        }
        public override int GetRandomAction(State actualState)
        {
            int[] availableActions;
            if (bot.Grounded)
                availableActions = new int[3] { (int)Actions.Left, (int)Actions.Right, (int)Actions.Jump };
            else
                availableActions = new int[2] { (int)Actions.Left, (int)Actions.Right };

            //wybór eksploaracja/ eksploatacja
            System.Random random = new System.Random();
            float randomValue = (float)random.NextDouble();
            if (randomValue > EPSILON)
            {
                List<int> greedyActionsList = new List<int>();
                //qvalues od 0 do 1 dla !bot.Grounded
                float[] qValues = qLambdaTable[actualState].QValues;
                float maxQvalue = qValues.Max();
                //jesli bot nie jest uziemiony to akcji mniej
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
        public override string FilePath { get; set; }

    }
}
