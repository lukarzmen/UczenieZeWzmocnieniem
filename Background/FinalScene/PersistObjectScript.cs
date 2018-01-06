using UnityEngine;
using System.Collections;

namespace Assets.Skrypty.Background.FinalScene
{
    public class PersistObjectScript : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private string data;

        public string Data
        {
            get
            {
                if (data.Contains("Wygrana") || data.Contains("Przegrana"))
                    return data;
                else
                    return "Blad";
            }
            set
            {
                data = value;
            }
        }
    }
}