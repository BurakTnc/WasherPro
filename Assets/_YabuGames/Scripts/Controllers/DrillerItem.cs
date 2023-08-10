using System;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Controllers
{
    public class DrillerItem : MonoBehaviour
    {
        [SerializeField] private Vector3 cleaningRotation;
        [SerializeField] private int level = 0;
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject[] items;
        [SerializeField] private TextMeshPro levelText;

        private Vector3 _neutralRotation;
        private bool _isCleaning;
        private PlayerController _playerRoot;
        private Rigidbody _rb;
        private Vector3 _prevPosition;
        private Transform _currentGrid;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerRoot = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            SetLevelText();
            _neutralRotation = transform.rotation.eulerAngles;
        }

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
            LevelSignals.Instance.OnDrillStart += StartDrill;
            LevelSignals.Instance.OnCleanDirt += OnCleaning;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnDrillStart -= StartDrill;
            LevelSignals.Instance.OnCleanDirt -= OnCleaning;
        }

        private void SetLevelText()
        {
            levelText.text = (level + 1).ToString();
        }
        private void StartDrill()
        {
            _rb.velocity = Vector3.forward * (moveSpeed);
            levelText.gameObject.SetActive(false);
        }

        private void OnCleaning()
        {
            _isCleaning = !_isCleaning;
            if (_isCleaning)
                transform.DORotate(cleaningRotation, .3f).SetEase(Ease.InSine);
            else
            {
                transform.DORotate(_neutralRotation, .3f).SetEase(Ease.InSine);
            }
        }
        public void ChangeOldGridCondition(Transform newGrid)
        {
            if (_currentGrid)
            {
                var gridController = _currentGrid.GetComponent<GridController>();
                gridController.SetGridConditions();
            }
           
            if(!newGrid)
                return;
            _currentGrid = newGrid;
        }

        public void Init(int initLevel = 0)
        {
            level = initLevel;
            items[0].SetActive(false);
            items[level].SetActive(true);
            SetLevelText();
        }
        public int GetDrillForce()
        {
            return level;
        }

        public int GetLevel()
        {
            return level;
        }

        public void Explode()
        {
            Destroy(gameObject);
        }

        public void Merge(GameObject mergedObj)
        {
            LevelSignals.Instance.OnMerge?.Invoke();
            items[level].SetActive(false);
            mergedObj.SetActive(false);
            level++;
            SetLevelText();
            
            var newObj = items[level];
            var startScale = newObj.transform.localScale;
            
            newObj.transform.localScale = Vector3.zero;
            newObj.SetActive(true);
            newObj.transform.DOScale(startScale, .5f).SetEase(Ease.OutBack);
        }


        public void PrepareToRun()
        {
            if(!_rb)
                return;
            _rb.velocity = Vector3.zero;
            _playerRoot.SetActiveWasher(this.gameObject);
            LevelSignals.Instance.OnRunStart?.Invoke();
        }

        public WasherController GetWasherComponent()
        {
            return transform.GetChild(level).GetComponent<WasherController>();
        }
        
    }
}