using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MAIN_PROJECT._Scripts.Helpers
{
    [System.Serializable]
    public class UnityEventScene : UnityEvent<Scene>
    {
    }

    [System.Serializable]
    public class SceneLoadedInfo : MonoBehaviour
    {
        public static SceneLoadedInfo instance;

        [SerializeField] private string _currentSceneName;

        [SerializeField] private UnityEventScene _onSceneLoaded;

        void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _currentSceneName = scene.name;
            _onSceneLoaded?.Invoke(scene);
        }
    }
}