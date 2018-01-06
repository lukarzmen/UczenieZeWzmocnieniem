using UnityEngine;
using System.Collections;

namespace Assets.Skrypty.Background
{
    public class EnemyMove : MonoBehaviour
    {
        public float maxValue = 2; // or whatever you want the max value to be
        public float minValue = -2; // or whatever you want the min value to be
        float currentValue = 0; // or wherever you want to start
        int direction = -1;

        private float xPosition;
        private float yPosition;

        void Start()
        {
            xPosition = transform.position.x;
            yPosition = transform.position.y;
            currentValue = xPosition;
            //maxValue = currentValue - xPosition;
            //minValue = currentValue + xPosition;        
        }
        void Update()
        {
            Move();
        }

        private void Move()
        {
            currentValue += Time.deltaTime * direction; // or however you are incrementing the position
            if (currentValue >= maxValue)
            {
                direction *= -1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                currentValue = maxValue;
            }
            else if (currentValue <= minValue)
            {
                direction *= -1;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                currentValue = minValue;
            }
            transform.position = new Vector3(currentValue, yPosition, 0);
        }
    }
}
