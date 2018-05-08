using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleRun
{
    public class UILogic : MonoBehaviour
    {
        public void GameStart()
        {
            GameController.GameStart();
        }

        public void ToggleGamePause()
        {
            GameController.TogglePause();
        }

        public void RestartGame()
        {
            Global.ClearScore();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
