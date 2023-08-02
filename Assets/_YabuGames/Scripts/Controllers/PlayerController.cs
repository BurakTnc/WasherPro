using System;
using System.Collections.Generic;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> washers = new List<GameObject>();
        [SerializeField] private float spaceBetweenWashers = .1f;


        #region Subscribtions

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
            LevelSignals.Instance.OnRunStart += AlignWashers;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnRunStart -= AlignWashers;
        }

        #endregion

        private void AlignWashers()
        {
            var leftSpace = .1f;
            var rightSpace = .1f;
            for (var i = 0; i < washers.Count; i++)
            {
                var takenWasher = washers[i].transform;
                var currentPosition = transform.position;
                if (i == 0)
                {
                    var desiredPosition = new Vector3(0, .5f, currentPosition.z);
                    takenWasher.DOMove(desiredPosition, .5f).SetEase(Ease.OutSine);
                    continue;
                }
                if (i % 2 != 0) 
                {
                    var desiredPosition = new Vector3(-leftSpace, .5f, currentPosition.z);
                    takenWasher.DOMove(desiredPosition, .5f).SetEase(Ease.OutSine);
                    leftSpace += .1f;
                }
                else
                {
                    var desiredPosition = new Vector3(rightSpace, .5f, currentPosition.z);
                    takenWasher.DOMove(desiredPosition, .5f).SetEase(Ease.OutSine);
                    rightSpace += .1f;
                }
                    
            }
        }
        public void SetActiveWasher(GameObject washer)
        {
            Destroy(washer.GetComponent<Rigidbody>());
            washer.transform.SetParent(transform);
            washers.Add(washer);
        }

        public List<GameObject> GetWashers()
        {
            return washers;
        }
    }
}