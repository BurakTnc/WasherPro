using System;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class PlayerCollisionController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DirtyPoint"))
            {
                Destroy(other.gameObject);
                LevelSignals.Instance.OnCleanDirt?.Invoke();
            }
        }
    }
}