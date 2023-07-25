using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class GrabController : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _offset;
        private Vector3 _previousPos;
        private bool _isMoving;
        private int _gridIndex;
        private UpgradeCube _cube;

        private void Awake()
        {
            _cube = GetComponent<UpgradeCube>();
            _camera = Camera.main;
        }
        

        private void OnMouseDown()
        {
            transform.DOKill();
            _offset = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
            if (_isMoving)
                return;
            _previousPos = transform.position;
        }
 
        private void OnMouseUp()
        {
            StartCoroutine(MergeAbilityLength());
            transform.DOMove(_previousPos, .5f).SetEase(Ease.OutBack).OnComplete(() => _isMoving = false);
        }

        private void OnMouseDrag()
        {
            
            var calculatedPos = _camera.ScreenToWorldPoint(Input.mousePosition - _offset);
            var desiredPos = new Vector3(calculatedPos.x, -.5f, calculatedPos.z);
            transform.position = desiredPos;
        }

        private IEnumerator MergeAbilityLength()
        {
            yield return new WaitForSeconds(.02f);
            _cube.canMerge = true;
        }

        public void LeaveGridByMerge()
        {
           // LevelSignals.Instance.OnGridLeave?.Invoke(_gridIndex);
        }

        public void InitFirstGrid(int gridIndex)
        {
            _gridIndex = gridIndex;
        }

        public void PlaceSoldier(Vector3 placedPosition, Transform grid)
        {
            var index = grid.GetSiblingIndex();

            grid.GetComponent<BoxCollider>().enabled = false;
            
            //LevelSignals.Instance.OnNewGrid?.Invoke(_gridIndex,index);
            _gridIndex = index;
            _previousPos = new Vector3(placedPosition.x, -.5f, placedPosition.z);
            transform.DOMove(_previousPos, .5f).SetEase(Ease.OutBack).OnComplete(() => _isMoving = false);
        
        }
    }
}