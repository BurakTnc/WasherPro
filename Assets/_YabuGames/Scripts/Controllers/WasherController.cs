using System;
using System.Collections;
using _YabuGames.Scripts.Objects;
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
        [SerializeField] private float scaleErrorFix;

        private Vector3 _defaultWaterHoseScale;
        private BoxCollider _collider;
        private Transform _waterEffect;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _defaultWaterHoseScale = waterHose.transform.localScale;
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

        private void FixWaterHoseScale(Vector3 nozzlePosition)
        {
            var distance = (transform.position.z - nozzlePosition.z) / scaleErrorFix;
            var desiredScale = new Vector3(1, 1, Math.Abs(distance));
            
            desiredScale.z = Mathf.Clamp(desiredScale.z, 0, 1);
            waterHose.transform.localScale = desiredScale;

        }

        private void SetDefaultWaterScale()
        {
            waterHose.transform.localScale = _defaultWaterHoseScale;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gate"))
            {
                if (other.TryGetComponent(out GateController gate))
                {
                    gate.EnableSplashEffect();
                }
            }
            if (other.CompareTag("Glass"))
            {
                if (other.TryGetComponent(out GlassScript glass))
                {
                    glass.EnableSplashEffect();
                }
            }
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
                    gate.FixSplashPosition(transform.position);
                    gate.IncreaseGateStats();
                }
                FixWaterHoseScale(other.transform.position);
            }

            if (other.CompareTag("Glass"))
            {
                if (other.TryGetComponent(out GlassScript glass))
                {
                    glass.Fill();
                    glass.FixSplashPosition(transform.position);
                }
                FixWaterHoseScale(other.transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Gate"))
            {
                if (other.TryGetComponent(out GateController gate))
                {
                    gate.DisableSplashEffect();
                }
                SetDefaultWaterScale();
            }
            if (other.CompareTag("Glass"))
            {
                if (other.TryGetComponent(out GlassScript glass))
                {
                    glass.DisableSplashEffect();
                }
                SetDefaultWaterScale();
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