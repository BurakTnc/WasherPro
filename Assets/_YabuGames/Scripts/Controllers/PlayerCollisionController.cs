using System;
using _YabuGames.Scripts.Objects;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class PlayerCollisionController : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = transform.root.GetComponent<PlayerController>();
        }


        private void GateSelection(GateController gate)
        {
            var washers = _playerController.GetWashers();

            foreach (var selected in washers)
            {
                var component = selected.GetComponent<DrillerItem>().GetWasherComponent();
                 gate.Selection(component);
                
                
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DirtyPoint"))
            {
                if (other.TryGetComponent(out DirtPoint dirtPoint))
                {
                    dirtPoint.InitNextPosition();
                }
                //Destroy(other.gameObject);
                LevelSignals.Instance.OnCleanDirt?.Invoke();
            }
            if (other.CompareTag("Gate"))
            {
                if (other.TryGetComponent(out GateController gate))
                {
                   GateSelection(gate);
                }
            }
        }
    }
}