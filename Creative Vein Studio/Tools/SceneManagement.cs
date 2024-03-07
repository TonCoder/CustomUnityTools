using System;
using System.Collections;
using MAIN_PROJECT._Scripts.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MAIN_PROJECT._Scripts.Scene_Specific
{
    public class SceneManagement : MonoBehaviour
    {
        public static SceneManagement instance;

        public const string LobbyScene = "02_Lobby";
        public int lobbyIndex = 0;

        private void Awake()
        {
            Singleton();
            lobbyIndex = SceneManager.GetSceneByName(LobbyScene).buildIndex;
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadYourAsyncScene(sceneName));
        }

        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        IEnumerator LoadYourAsyncScene(string _sceneName)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void LoadLobby()
        {
            SceneManager.LoadScene(lobbyIndex);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void LoadFirstScene(int index = 0)
        {
            SceneManager.LoadScene(index);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void Singleton()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}