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
        [SerializeField] private GameObject splashEffect;

        private bool _isCollected;
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public void Fill()
        {
            if(_isCollected)
                return;
            if (liquid.localScale.y<1)
            {
                liquid.localScale += Vector3.up * fillingSpeed;
            }
            else
            {
                _isCollected = true;
                CollectTheGlass();
            }
        }

        public void EnableSplashEffect()
        {
            splashEffect.SetActive(true);
        }

        public void DisableSplashEffect()
        {
            splashEffect.SetActive(false);
        }

        public void FixSplashPosition(Vector3 splashPosition)
        {
            var desiredPos = new Vector3(splashPosition.x, splashPosition.y, transform.position.z);

            splashEffect.transform.position = desiredPos;
        }

        private void CollectTheGlass()
        {
            transform.DOShakeRotation(.2f, Vector3.forward * 7, 5, 100, true).SetLoops(3, LoopType.Yoyo)
                .OnComplete(() => transform.DOJump(Vector3.back * .5f, .5f, 1, 1));
        }
    }
}