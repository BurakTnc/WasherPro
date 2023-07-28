using System;
using _YabuGames.Scripts.Objects;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CollisionController : MonoBehaviour
    {

        private GrabController _grabController;
        private DrillerItem _item;

        private void Awake()
        {
            _grabController = GetComponent<GrabController>();
            _item = GetComponent<DrillerItem>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Grid"))
            {
                _grabController.PlaceCube(other.transform.position,other.transform);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                var obstacle = other.GetComponent<Obstacle>();
                var obstacleStrength = obstacle.GetStrength();
                var itemStrength = _item.GetDrillForce();
                var canDrill = itemStrength >= obstacleStrength;


                if (canDrill)
                {
                    obstacle.Explode();
                }
                else
                {
                    _item.Explode();
                }

            }
        }
    }
}
