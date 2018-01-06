using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityStandardAssets._2D;
using System;
using Assets.Skrypty.Files;
using System.IO;

namespace Assets.Skrypty.Algorithm
{
    /// <summary>
    ///  Jutro zrobić dziedziczenie po kontrolerze i sprawdzic czy działa bez unlock
    /// </summary>
    public abstract class QLearning : MonoBehaviour, IQLearning
    {
        protected PlatformerCharacter2D bot;
        protected int episode = 0;
        int actionsInEpisode = 0;
        Dictionary<int, int> episodeActionDictionary;

        const bool crouch = false;
        float velocity = 0.0f;
        bool jump = false;
        const float velocityValue = 1.0f;     

        protected State actualState;
        protected State nextState;
        protected State startState;

        protected int selectedActionNumber = 1;
        
        protected Vector3 distanceToObstacle;
        protected Vector3 botPosition;
        protected Vector3 startPosition;

        protected const float GAMMA = 0.9f;
        protected const float ALPHA = 0.9f;       

        protected string rewardTag = "Reward";
        protected string enemyTag = "Enemy";

        protected int state;
        protected int blocked;       

        protected int digit = 2;

        protected bool rewardIsReached = false;
        protected bool enemyIsReached = false;

        public abstract string FilePath {get; set; }
        public abstract string CombineFilePath(string fileName);

        protected static class Rewards
        {
            public const float IsNotReached = 0;
            public const float ForPosition = 0.05f;
            public const float EnemyTouched = -0.01f;
            public const float IsReached = 1;
            public const float IsBlocked = -0.5f;
        }

        protected enum Actions
        {
            Left,
            Right,
            Jump
        }
        protected enum QLearningStateMachine
        {
            DoAction,
            Observe
        }

        Tests.Tests test;
        float startEpisodeTime;
        float endEpisodeTime;
        float elapsedEpisodeTime;

        public virtual void Awake()
        {
            bot = GetComponent<PlatformerCharacter2D>();                        
        }
        void Start()
        {
            test = new Tests.Tests("test.txt");
            SetStartPoint();
            Debug.Log("Epizod: " + episode);
            episodeActionDictionary = new Dictionary<int, int>();
            //Debug.Log("Pozycja startowa: " + Round(startPosition.x) + "," + Round(startPosition.y));
            //InvokeRepeating("DebugLogs", 0.0f, 1.0f); //wywołuj logi co zadany czas
            SetPlayerOnStart();           
            Reward = 0;
        }
        public virtual void Update()
        {
            EndStatement();
        }
        public virtual void OnApplicationQuit()
        {
            
        }

        public abstract int GetRandomAction(State actualState);
        public abstract void LearningAlgorithm();
        protected void ExecuteAction(int actionNumber)
        {
            actionsInEpisode++;
            switch (actionNumber)
            {
                case (int)Actions.Left:
                    Left();
                    break;
                case (int)Actions.Right:
                    Right();
                    break;
                case (int)Actions.Jump:
                    High();
                    break;
            }
        }

        //do usunięcia
        protected void EndStatement()
        {
            if (episode == 35)
                Application.Quit();
        }
        protected bool DontAllowFall()
        {
            if (BotPositionY < -7.0f)
            {
                SetPlayerOnStart();
                Reward = (int)Rewards.IsBlocked;
                state = 0;
                return true;
            }
            return false;
        }
        protected bool DontAllowBlockBot()
        {
            blocked++;
            if (blocked == 25)
            {
                UnLock(0.0f, 1.5f);
                state = 0;
                blocked = 0;
                return true;
            }
            return false;
        }
        protected void DebugLogs()
        {
            Debug.Log("X:" + Round(BotPositionX) + " " + "Y:" + Round(BotPositionY));
            Debug.Log("Stan: " + state);
            Debug.Log("Czas: " + Math.Round(Time.time, 2) + "s");
            Debug.Log("Odleglosc do celu: " + Round(DistanceToObstacleX) + "," + Round(DistanceToObstacleY));
        }
        protected bool TerminationStateIsReached()
        {
            if (rewardIsReached)
            {
                Debug.Log("Epizod: " + episode);
                //przytrzymywanie w stanie. tablica do przeprowadzenia badania nad uczeniem
                if (actionsInEpisode > 5)
                {
                    episodeActionDictionary[episode] = actionsInEpisode;
                    endEpisodeTime = Time.realtimeSinceStartup;

                    ElapsedEpisodeTime = endEpisodeTime - startEpisodeTime;
                    test.Add(actionsInEpisode, ElapsedEpisodeTime);
                    test.WriteToFile();
                }
                SetPlayerOnStart();
                rewardIsReached = false;
                episode++;
                return true;
            }
            return false;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == rewardTag)
            {
                Reward = Rewards.IsReached;
            }
            if (other.tag == enemyTag)
            {
                enemyIsReached = true;
            }

            if (other.tag == "IsBlocked")
            {
                Reward = -10;
                bot.transform.position = startPosition;
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == enemyTag)
            {
                enemyIsReached = false;
            }
            if (other.tag == rewardTag)
            {
                rewardIsReached = true;
                Reward = (int)Rewards.IsNotReached; //TODO: zastanowic sie nad tym
            }
        }
        protected void UnLock(float xShift, float yShift)
        {
            Debug.LogError("Proba odblokowania");
            bot.transform.position = new Vector3(BotPositionX + xShift, BotPositionY + yShift, botPosition.z);
        }       
        protected float Reward { get; set; }
        /// <summary>
        /// using in Start();
        /// </summary>
        void SetStartPoint()
        {
            startPosition = transform.position;
            startState = new State(startPosition.x, DistanceToObstacleX, DistanceToObstacleY);
            Reward = 0;
        }
        public State GetState()
        {
            return new State(BotPositionX, DistanceToObstacleX, DistanceToObstacleY);
        }
        protected void SetPlayerOnStart()
        {
            bot.transform.position = new Vector3(startPosition.x, startPosition.y);
            actualState = nextState = startState;
            
            startEpisodeTime = Time.realtimeSinceStartup;
            actionsInEpisode = 0;
        }
        private void Left()
        {
            velocity = -velocityValue;
            bot.Move(velocity, crouch, jump);
        }
        private void Right()
        {
            velocity = velocityValue;
            bot.Move(velocity, crouch, jump);
        }
        private void High()
        {
            if (bot.Grounded)
            {
                jump = true;
                bot.Move(velocity, crouch, jump);
                jump = false;
            }
        }
        protected Vector3 FindClosestTarget(string tag)
        {
            return GameObject.FindGameObjectsWithTag(tag)
            .OrderBy(go => Vector3.Distance(go.transform.position, transform.position))
            .FirstOrDefault().transform.position;
        }
        protected Vector3 DistanceToClosestTarget(string tag)
        {
            var closestTargetPosition = FindClosestTarget(tag);
            return new Vector3(closestTargetPosition.x - transform.position.x,
                closestTargetPosition.y - transform.position.y,
                closestTargetPosition.z - transform.position.z);
        }
        protected static float[] ZeroActionValues()
        {
            return new float[3] { 0.0f, 0.0f, 0.0f };
        }
        protected void SetParameters()
        {
            distanceToObstacle = DistanceToClosestTarget(enemyTag);
            botPosition = transform.position;
            DistanceToObstacleX = distanceToObstacle.x;
            DistanceToObstacleY = distanceToObstacle.y;
            BotPositionX = botPosition.x;
        }
        /// <summary>
        /// do debugowania w konsoli unity
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        string Round(float position)
        {
            return Math.Round(position, digit).ToString();
        }
        float DistanceToObstacleX
        { get; set; }
        float DistanceToObstacleY
        { get; set; }
        float BotPositionX
        { get; set; }
        float BotPositionY
        { get; set; }
        public float ElapsedEpisodeTime {get;set;}
    }
}
