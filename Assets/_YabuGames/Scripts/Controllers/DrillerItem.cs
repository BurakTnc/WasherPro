using System;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Controllers
{
    public class DrillerItem : MonoBehaviour
    {
        [SerializeField] private int level = 1;
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject[] items;
        [SerializeField] private TextMeshPro levelText;

        private PlayerController _playerRoot;
        private Rigidbody _rb;
        private Vector3 _prevPosition;
        private Transform _currentGrid;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerRoot = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            SetLevelText();
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
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnDrillStart -= StartDrill;
        }

        private void SetLevelText()
        {
            levelText.text = (level + 1).ToString();
        }
        private void StartDrill()
        {
            _rb.velocity = Vector3.forward * (moveSpeed * Time.deltaTime);
        }
        public void ChangeOldGridCondition(Transform newGrid)
        {
            if (_currentGrid)
            {
                var gridController = _currentGrid.GetComponent<GridController>();
                gridController.SetGridConditions();
            }
           
            _currentGrid = newGrid;
        }

        public int GetDrillForce()
        {
            return level;
        }

        public void Explode()
        {
            Destroy(gameObject); //test
        }

        public void Merge(GameObject mergedObj)
        {
            items[level].SetActive(false);
            Destroy(mergedObj);
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
    }
}