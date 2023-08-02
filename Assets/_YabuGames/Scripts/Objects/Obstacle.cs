using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private int obstacleStrength;
        [SerializeField] private Transform destroyParticle;


        public int GetStrength()
        {
            return obstacleStrength;
        }

        public void Explode()
        {
            destroyParticle.SetParent(null);
            destroyParticle.gameObject.SetActive(true);
            Destroy(destroyParticle.gameObject,3);
            Destroy(gameObject);
        }

        public void GetHit()
        {
            transform.DOShakeScale(.3f, Vector3.right*.3f, 5, 100, true);
        }
    }
}