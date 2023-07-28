using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private int obstacleStrength;


        public int GetStrength()
        {
            return obstacleStrength;
        }

        public void Explode()
        {
            Destroy(gameObject); //test
        }
    }
}