using System;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class DrillerItem : MonoBehaviour
    {
        [SerializeField] private int drillForce = 1;
        [SerializeField] private float moveSpeed;

        private Rigidbody _rb;
        private Vector3 _prevPosition;
        private Transform _currentGrid;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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
            return drillForce;
        }

        public void Explode()
        {
            Destroy(gameObject); //test
        }
        

    }
}