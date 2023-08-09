using _YabuGames.Scripts.Controllers;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private int _money;

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
            LevelSignals.Instance.OnSpawnNewItem += SpawnNewItem;
        }

        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            LevelSignals.Instance.OnSpawnNewItem -= SpawnNewItem;
        }

        #endregion

        private void GetValues()
        {
            _money = PlayerPrefs.GetInt("money", 0);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("money",_money);
        }

        private void SpawnNewItem(Transform pickedGrid,int pickedGridIndex)
        {
            var item = Instantiate(Resources.Load<GameObject>("Spawnables/Washers")).transform;
            var component = item.GetComponent<GrabController>();
            
            component.PlaceSpawnedItem(pickedGrid.position, pickedGrid);

        }
        public void ArrangeMoney(int value)
        {
            _money += value;
        }

        public int GetMoney()
        {
            return _money < 0 ? 0 : _money;
        }
        
    }
}