using System;
using System.Collections;
using _YabuGames.Scripts.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _YabuGames.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public GameObject loadingPanel;
        public Image loadingBar;

        private void Start()
        {
            LoadScene();
        }

        private void LoadScene()
        {
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            loadingPanel.SetActive(true);
            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / .9f);
                loadingBar.fillAmount = progress;
                yield return null;
            }
        }
    }
}