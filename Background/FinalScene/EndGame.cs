using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Skrypty.Background.FinalScene
{
    public class EndGame : MonoBehaviour
    {
        private string text;            
        public PersistObjectScript script;
        GameObject persistantGameObject;

        public void LoadFinalScene(string text)
        {
            //load persistantObjectScript attached to persistantObject
            persistantGameObject = GameObject.Find("PersistantGameObject");
            script = persistantGameObject.GetComponent<PersistObjectScript>();
            script.Data = text;
            SceneManager.LoadScene("Koniec");
        }
    }
}
