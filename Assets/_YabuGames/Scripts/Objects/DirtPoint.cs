using UnityEngine;

namespace _YabuGames.Scripts.Objects
{
    public class DirtPoint : MonoBehaviour
    {
        [SerializeField] private Transform nextPosition;

        public void InitNextPosition()
        {
            Invoke(nameof(GoToNextPosition),1);
        }

        private void GoToNextPosition()
        {
            transform.root.position = nextPosition.position;
            Debug.Log("dirt");
        }
    }
}