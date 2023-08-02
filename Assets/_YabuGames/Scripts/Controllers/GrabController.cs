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
        private bool _canMerge;
        private DrillerItem _drillerItem;

        private void Awake()
        {
            _camera = Camera.main;
            _drillerItem = GetComponent<DrillerItem>();
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
            var desiredPos = new Vector3(calculatedPos.x, -.3f, calculatedPos.z);
            transform.position = desiredPos;
        }

        private IEnumerator MergeAbilityLength()
        {
            _canMerge = true;
            yield return new WaitForSeconds(.02f);
            _canMerge = false;
        }

        public void LeaveGridByMerge()
        {
           // LevelSignals.Instance.OnGridLeave?.Invoke(_gridIndex);
        }
        
        public void PlaceCube(Vector3 placedPosition, Transform grid)
        {
            if(!_canMerge)
                return;
            
            if (grid.TryGetComponent( out GridController gridController))
            {
                _drillerItem.ChangeOldGridCondition(grid);
                gridController.SetGridConditions();
            }
            
            var index = grid.GetSiblingIndex();
            
            //LevelSignals.Instance.OnNewGrid?.Invoke(_gridIndex,index);
            _previousPos = new Vector3(placedPosition.x, -.3f, placedPosition.z);
            transform.DOMove(_previousPos, .5f).SetEase(Ease.OutBack)
                .OnComplete(() => _isMoving = false);

        }

        public void Merge(Transform selectedItem)
        {
            if (selectedItem.TryGetComponent(out DrillerItem item))
            {
                if(item.GetDrillForce() != _drillerItem.GetDrillForce())
                    return;
                if(!_canMerge)
                    return;
                
                item.Merge(transform.gameObject);
            }
        }
    }
}