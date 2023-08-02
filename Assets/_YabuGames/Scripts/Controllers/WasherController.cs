using System;
using System.Collections;
using _YabuGames.Scripts.Signals;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace _YabuGames.Scripts.Controllers
{
    public class WasherController : MonoBehaviour
    {
        [SerializeField] private GameObject waterHose;
        [SerializeField] private Transform rope;
        [SerializeField] private float washingForce;

        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

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
            LevelSignals.Instance.OnRunStart += Init;
        }

        private void UnSubscribe()
        {
            LevelSignals.Instance.OnRunStart -= Init;
        }
        private void Init()
        {
            StartCoroutine(ActivationDelay());
        }

        private void Activate()
        {
            waterHose.SetActive(true);
            _collider.enabled = true;
            rope.SetParent(null);
            rope.gameObject.SetActive(true);
        }

        private IEnumerator ActivationDelay()
        {
            yield return new WaitForSeconds(.5f);
            Activate();
        }

        private void CleanTheDirt(Transform dirt)
        {
            if (dirt.localScale.y < washingForce)
            {
                dirt.GetComponent<BoxCollider>().enabled = false;
                return;
            }
            
            dirt.transform.localScale -= Vector3.up * washingForce;
            dirt.transform.position += Vector3.up * (washingForce / 2);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Dirt"))
            {
                CleanTheDirt(other.transform);
            }

            if (other.CompareTag("Gate"))
            {
                if (other.TryGetComponent(out GateController gate))
                {
                    gate.IncreaseGateStats();
                }
            }
        }

        public void IncreasePower(float damage)
        {
            
        }

        public void IncreaseRange(float range)
        {
            
        }
    }
}