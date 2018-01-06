using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Skrypty.Background.FinalScene
{
    public class FinalScene : MonoBehaviour
    {
        public Text finalText;

        public GameObject persistantObject;
        public PersistObjectScript script;

        void Awake()
        {
            persistantObject = GameObject.Find("PersistantGameObject");
            //load persistantObjectScript attached to persistantObject
            script = persistantObject.GetComponent<PersistObjectScript>();
            finalText.text = script.Data;
        }
    }
}
