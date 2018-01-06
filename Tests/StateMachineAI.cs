using UnityEngine;
using UnityStandardAssets._2D;
using System;
using System.Linq;
using UnityEngine.Assertions;
using Assets.Skrypty.Files;
using Assets.Skrypty.Background.FinalScene;

/// <summary>
/// Klasa testowa. Testuje działanie niektórych klas pełniąc rolę maszyny stanowej.
/// //TODO: przeksztalcic klase w bazowa i klasy z algorytmami dziedzicza po niej.
/// </summary>
namespace Assets.Skrypty.Tests
{
    [RequireComponent(typeof(PlatformerCharacter2D)), TestClass()]   
    public class StateMachineAI : MonoBehaviour
    {

        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        Vector3 botPosition;
        Vector3 distanceToObstacle;

        private float startPositionX;
        private float startPositionY;
        private float positionX;
        private float positionY;
        private float distanceToObstacleX;
        private float distanceToObstacleY;
        private bool isHigh = false;
        float h = 0;
        const bool crouch = false;
        ExtendedDictionary<State, float[]> dic;
        float[] nic = { 0.0f, 0.1f, 1.1f };
        string filePath = "data.txt";

        int state = 0;
        private void Start()
        {
            startPositionX = transform.position.x;
            startPositionY = transform.position.y;
            //Do in period of time
            //InvokeRepeating("DebugLogs", 0.0f, 2.0f);
        }

        // Use this for initialization
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            dic = new ExtendedDictionary<State, float[]>(new StateEqualityComparer());          
        }

        private void Update()
        {
            SetParameters();
            dic.AddOrUpdate(new State(DistanceToObstacleX, DistanceToObstacleY, PositionX), nic);
        }

        private void OnApplicationQuit()
        {
            Assert.IsNotNull(dic, "slownik jest null");;
        }

        private void DebugLogs()
        {
            Debug.Log("X:" + PositionX.ToString() + " " + "Y:" + PositionY.ToString());
            Debug.Log("Stan: " + state);
            Debug.Log("Czas: " + Math.Round(Time.time, 2) + "s");
            Debug.Log("Odleglosc do celu: " + DistanceToObstacleX + "," + DistanceToObstacleY);
        }

        private void FixedUpdate()
        {
            SetParameters();
            //DebugLogs();
            //StateMachine();   
            GoToReward();
        }

        private void StateMachine()
        {
            switch (state)
            {
                case 0:
                    h = 1;
                    if (PositionX - startPositionX > 5)
                    {
                        state = 1;
                    }
                    break;
                case 1:
                    h = -1;
                    if (PositionX - startPositionX < -5)
                    {
                        state = 2;
                    }
                    break;
                case 2:
                    if (!isHigh)
                    {
                        isHigh = true;
                    }
                    state = 0;
                    break;
            }
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, isHigh);
            isHigh = false;
        }

        private void GoToReward()
        {
            switch (state)
            {
                case 0:
                    h = 1;
                    if (PositionX - startPositionX > 5)
                    {
                        state = 1;
                    }
                    break;
                case 1:
                    if (m_Character.Grounded)
                    {
                        isHigh = true;
                    }
                    if (PositionX - startPositionX > 8)
                        state = 2;
                    break;
                case 2:
                    h = -1;
                    h = 1;
                    break;
            }
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, isHigh);
            isHigh = false;
        }

        Vector3 FindClosestTarget(string tag)
        {
            return GameObject.FindGameObjectsWithTag(tag)
            .OrderBy(go => Vector3.Distance(go.transform.position, transform.position))
            .FirstOrDefault().transform.position;
        }

        Vector3 DistanceToClosestTarget(string tag)
        {
            var closestTargetPosition = FindClosestTarget(tag);
            return new Vector3(closestTargetPosition.x - transform.position.x,
                closestTargetPosition.y - transform.position.y,
                closestTargetPosition.z - transform.position.z);
        }

        void SetParameters()
        {
            botPosition = transform.position;
            PositionX = botPosition.x;
            PositionY = botPosition.y;
            distanceToObstacle = DistanceToClosestTarget("Enemy");
            DistanceToObstacleX = distanceToObstacle.x;
            DistanceToObstacleY = distanceToObstacle.y;
        }

        float DistanceToObstacleX
        {
            get
            {
                return distanceToObstacleX;
            }
            set
            {
                distanceToObstacleX = (float)Math.Round(value, 1);
            }
        }
        float DistanceToObstacleY
        {
            get
            {
                return distanceToObstacleY;
            }
            set
            {
                distanceToObstacleY = (float)Math.Round(value, 1);
            }
        }

        float PositionX
        {
            get
            {
                return positionX;
            }
            set
            {
                positionX = (float)Math.Round(value, 1);
            }
        }
        float PositionY
        {
            get
            {
                return positionY;
            }
            set
            {
                positionY = (float)Math.Round(value, 1);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Reward")
            {
                EndGame final = new EndGame();
                Debug.Log("Przeszkoda");
                final.LoadFinalScene("Przegrana");                
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Reward")
            {
                Debug.Log("Zboczono z celu");
            }
        }

    }
}

