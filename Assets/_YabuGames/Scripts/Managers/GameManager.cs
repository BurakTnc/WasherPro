using System;
using System.Collections;
using System.Collections.Generic;
using _YabuGames.Scripts.Controllers;
using _YabuGames.Scripts.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int money;

        public int earnedValue;
        public int _washerCount;

        [SerializeField] private GameObject[] platforms;

        private bool _isStarted;
        private int _level;
        private int _mergedWashers;
        private readonly List<DrillerItem> _washerList = new List<DrillerItem>();

        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;

            #endregion
            GetValues();
            //platforms[_level].SetActive(true);
        }

        private IEnumerator Start()
        {
            var temp = _washerCount;
            _washerCount = 0;
            for (var i = 0; i < temp; i++)
            {
                yield return new WaitForSeconds(.15f);
                var grid = GridManager.Instance.PickAGrid();
                var washerLevel = PlayerPrefs.GetInt($"washerLevel{i}");
                InitWashers(grid,washerLevel);
            }

            yield return new WaitForSeconds(.1f);
            LevelSignals.Instance.OnInit?.Invoke();

        }

        #region Subscribtions
        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnSave += Save;
            CoreGameSignals.Instance.OnLevelWin += Win;
            LevelSignals.Instance.OnSpawnNewItem += SpawnNewItem;
            CoreGameSignals.Instance.OnLevelLoad += LoadScene;
            //LevelSignals.Instance.OnMerge += DecreaseWasherCount;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelWin -= Win;
            CoreGameSignals.Instance.OnLevelLoad -= LoadScene;
            LevelSignals.Instance.OnSpawnNewItem -= SpawnNewItem;
            //LevelSignals.Instance.OnMerge -= DecreaseWasherCount;
        }

        #endregion

        private void LoadScene()
        {
            SceneManager.LoadScene(0);
        }
        private void GetValues()
        {
            money = PlayerPrefs.GetInt("money", 0);
            _level = PlayerPrefs.GetInt("level", 0);
            _washerCount = PlayerPrefs.GetInt("washerCount", 1);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("money",money);
            PlayerPrefs.SetInt("level",_level);

            if(_isStarted)
                return;
            for (var i = 0; i < _washerList.Count; i++)
            {
                PlayerPrefs.SetInt($"washerLevel{i}",_washerList[i].GetLevel());
            }
            PlayerPrefs.SetInt("washerCount", _washerCount);
            _isStarted = true;
        }

        public void DecreaseWasherCount()
        {
            _washerCount--;
            if (_isStarted && _washerCount <= 0) 
                CoreGameSignals.Instance.OnLevelFail?.Invoke();
        }

        public void IncreaseWasherCount()
        {
            _washerCount++;
        }
        private void SpawnNewItem(Transform pickedGrid)
        {
            var item = Instantiate(Resources.Load<GameObject>("Spawnables/newhoes")).transform;
            var grabController = item.GetComponent<GrabController>();
            var washerComponent = item.GetComponent<DrillerItem>();
            
            _washerList.Add(washerComponent);
            grabController.PlaceSpawnedItem(pickedGrid.position, pickedGrid);
        }
        private void InitWashers(Transform pickedGrid,int washerLevel)
        {
            var item = Instantiate(Resources.Load<GameObject>("Spawnables/newhoes")).transform;
            var grabController = item.GetComponent<GrabController>();
            var washerComponent = item.GetComponent<DrillerItem>();
            
            washerComponent.Init(washerLevel);
            _washerList.Add(washerComponent);
            grabController.PlaceSpawnedItem(pickedGrid.position, pickedGrid);
        }

        private void Win()
        {
            _level++;
            money += 100;
            CoreGameSignals.Instance.OnSave?.Invoke();
        }
        public void ArrangeMoney(int value)
        {
            money += value;
        }

        public int GetWasherCount()
        {
            return _washerCount;
        }

        public int GetMoney()
        {
            return money < 0 ? 0 : money;
        }
        
    }
}