using System;
using System.Collections.Generic;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        
        public List<Transform> _gridList;
        private bool[] _occupiedGrids;

        private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }
            Instance = this;
            
            Init();
        }

        private void Init()
        {
            _occupiedGrids = new bool[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                _gridList.Add(transform.GetChild(i).transform);
            }
        }

        public Transform PickAGrid()
        {
            var pickedIndex = 0;
            
            for (var i = 0; i < _gridList.Count; i++)
            {
                if (_occupiedGrids[i]) 
                    continue;
                
                pickedIndex = i;
                break;
            }

            return _gridList[pickedIndex];
        }

        public int GetPickedGridIndex()
        {
            var pickedIndex = 0;
            
            for (var i = 0; i < _gridList.Count; i++)
            {
                if (_occupiedGrids[i]) 
                    continue;
                
                pickedIndex = i;
                break;
            }

            SetGridCondition(pickedIndex);
            return pickedIndex;
        }
        public void ChangeGridCondition(int gridIndex,bool isOccupied)
        {
            _occupiedGrids[gridIndex] = isOccupied;

        }

        private void SetGridCondition(int gridIndex)
        {
            _occupiedGrids[gridIndex] = true;
        }
    }
}
