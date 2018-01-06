using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Skrypty.Background
{
    public class Buttons : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("GraVsBot");
        }

        public void Test()
        {
            SceneManager.LoadScene("Test");
        }

        public void LearnQ0()
        {
            SceneManager.LoadScene("UczenieQ0");
        }

        public void LearnQLambda()
        {
            SceneManager.LoadScene("UczenieQLambda");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("MenuStart");
        }

        public void EndGame()
        {
            Application.Quit();
        }
    }
}
