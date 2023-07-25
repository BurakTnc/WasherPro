using System;
using System.Collections.Generic;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class GridManager : MonoBehaviour
    {
        private List<Transform> _gridList;
        private List<Transform> _activeGrids;
        private bool[] _occupiedGrids;

        private void Awake()
        {
            _occupiedGrids = new bool[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                _gridList.Add(transform.GetChild(i));
            }
        }

        public void ChangeConditions(int gridIndex, bool isOccupied)
        {
            _occupiedGrids[gridIndex] = isOccupied;
        }
    }
}
