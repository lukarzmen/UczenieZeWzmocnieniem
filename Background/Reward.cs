using UnityEngine;
using System.Collections;
using Assets.Skrypty.Background.FinalScene;

namespace Assets.Skrypty.Background
{
    public class Reward : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D target)
        {
            if (target.tag == "Player")
            {
                new EndGame().LoadFinalScene("Wygrana");
                Debug.Log("Wygrana");
            }
            if (target.tag == "Bot")
            {
                new EndGame().LoadFinalScene("Przegrana");
                Debug.Log("Przegrana");
            }
        }
    }
}

