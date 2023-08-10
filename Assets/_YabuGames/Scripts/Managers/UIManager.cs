using System;
using _YabuGames.Scripts.Controllers;
using _YabuGames.Scripts.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _YabuGames.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private GameObject mainPanel, gamePanel, winPanel, losePanel, storePanel;
        [SerializeField] private TextMeshProUGUI[] moneyText;
        [SerializeField] private TextMeshProUGUI buyButtonText, earnedText;
        [SerializeField] private Button buyButton, goButton;

        private float _buyPrice;


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

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Start()
        {
            SetMoneyTexts();
            UpdateButtonConditions();
            buyButton.interactable = false;
            goButton.interactable = false;
        }

        #region Subscribtions
        private void Subscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin += LevelWin;
                    CoreGameSignals.Instance.OnLevelFail += LevelLose;
                    CoreGameSignals.Instance.OnGameStart += OnGameStart;
                    CoreGameSignals.Instance.OnSave += Save;
                    LevelSignals.Instance.OnInit += Save;
                }
        
                private void UnSubscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin -= LevelWin;
                    CoreGameSignals.Instance.OnLevelFail -= LevelLose;
                    CoreGameSignals.Instance.OnGameStart -= OnGameStart;
                    CoreGameSignals.Instance.OnSave -= Save;
                    LevelSignals.Instance.OnInit -= Save;
                }

        #endregion

        private void GetValues()
        {
            _buyPrice = PlayerPrefs.GetFloat("buyPrice", 75);
        }
        
        private void Save()
        {
            PlayerPrefs.SetFloat("buyPrice",_buyPrice);
            UpdateButtonConditions();
            SetMoneyTexts();
            goButton.interactable = true;
        }

        private void UpdateButtonConditions()
        {
            var money = GameManager.Instance.GetMoney();
            var washerCount = GameManager.Instance.GetWasherCount();
            
            buyButtonText.text = "$" + (int)_buyPrice;
            buyButton.interactable = money >= _buyPrice && washerCount < 10;
        }
        
        private void OnGameStart()
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
        private void SetMoneyTexts()
        {
            if (moneyText.Length <= 0) return;

            foreach (var t in moneyText)
            {
                if (t)
                {
                    t.text = "$" + GameManager.Instance.GetMoney();
                }
            }
        }
        private void LevelWin()
        {
            earnedText.text = "$" + GameManager.Instance.earnedValue;
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
            HapticManager.Instance.PlaySuccessHaptic();
        }

        private void LevelLose()
        {
            gamePanel.SetActive(false);
            losePanel.SetActive(true);
            HapticManager.Instance.PlayFailureHaptic();
        }
        
        public void DrillButton()
        {
            CoreGameSignals.Instance.OnSave?.Invoke();
            LevelSignals.Instance.OnDrillStart?.Invoke();
            gamePanel.SetActive(false);
        }

        public void BuyItemButton()
        {
            var pickedGrid = GridManager.Instance.PickAGrid();
    
            GameManager.Instance.money -= (int)_buyPrice;
            _buyPrice *= 1.5f;
            LevelSignals.Instance.OnSpawnNewItem?.Invoke(pickedGrid);
            UpdateButtonConditions();
        }
        public void PlayButton()
        {
            CoreGameSignals.Instance.OnGameStart?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void MenuButton()
        {
            mainPanel.SetActive(true);
            HapticManager.Instance.PlayLightHaptic();
        }

        public void NextButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void RetryButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }
    }
}
