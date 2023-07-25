using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class UpgradeCube : MonoBehaviour
    {

        [HideInInspector] public bool canMerge;
        
        private Vector3 _prevPosition;
        private Transform _currentGrid;
        

        public void SetGrid(Transform grid)
        {
            _currentGrid = grid;
            _prevPosition = grid.position;
            Move();
        }

        private void Move()
        {
            transform.DOMove(_prevPosition, .5f).SetEase(Ease.OutBack);
        }
    }
}