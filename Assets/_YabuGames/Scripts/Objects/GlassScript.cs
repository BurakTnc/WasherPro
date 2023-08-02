using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    [RequireComponent(typeof(BoxCollider))]
    public class GlassScript : MonoBehaviour
    {
        [SerializeField] private float fillingSpeed = 0.01f;
        [SerializeField] private Transform liquid;

        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public void Fill()
        {
            if (liquid.localScale.y<1)
            {
                liquid.localScale += Vector3.up * fillingSpeed;
            }
            else
            {
                _collider.enabled = false;
                CollectTheGlass();
            }
        }

        private void CollectTheGlass()
        {
            transform.DOShakeRotation(.2f, Vector3.forward * 7, 5, 100, true).SetLoops(3, LoopType.Yoyo)
                .OnComplete(() => transform.DOJump(Vector3.back * .5f, .5f, 1, 1));
        }
    }
}