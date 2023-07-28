using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class GridController : MonoBehaviour
    {
        private int _gridIndex;
        private bool _isOccupied;
        private BoxCollider _collider;
        private GridManager _gridManager;

        private void Awake()
        {
            _gridManager = GetComponentInParent<GridManager>();
            _collider = GetComponent<BoxCollider>();
            _gridIndex = transform.GetSiblingIndex();
        }

        public void SetGridConditions()
        {
            _isOccupied = !_isOccupied;
            //_gridManager.ChangeConditions(_gridIndex, _isOccupied);
            _collider.enabled = !_isOccupied;
            
        }
        
    }
}