using System;
using System.Collections;
using _YabuGames.Scripts.Managers;
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
        [SerializeField] private float colliderScaleErrorFix;

        private float _gateIncreaseSpeed = .05f;
        private Vector3 _defaultWaterHoseScale;
        private Vector3 _currentColliderScale;
        private BoxCollider _collider;
        private Transform _waterEffect;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _currentColliderScale = _collider.size;
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
            var desiredScale = new Vector3(_defaultWaterHoseScale.x, _defaultWaterHoseScale.y, Math.Abs(distance));
            
            desiredScale.z = Mathf.Clamp(desiredScale.z, 0, .1f);
            waterHose.transform.localScale = desiredScale;

        }

        private void SetDefaultWaterScale()
        {
            waterHose.transform.localScale = _defaultWaterHoseScale;
            var localPosition = waterHose.transform.localPosition;
            localPosition = new Vector3(localPosition.x,
                localPosition.y, _defaultWaterHoseScale.z / 2);
            
            waterHose.transform.localPosition = localPosition;
        }
        

        private void CleanTheDirt(Transform dirt)
        {
            if (dirt.localScale.y <= 0)
            {
                dirt.GetComponent<BoxCollider>().enabled = false;
                return;
            }
            
            var desiredScale=dirt.transform.localScale - Vector3.up * washingForce;
            
            desiredScale.y = Mathf.Clamp(desiredScale.y, 0, 1);
            dirt.transform.localScale = desiredScale;
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
               // CleanTheDirt(other.transform);
               if (other.transform.root.TryGetComponent(out Clean clean))
               {
                   clean.CleanTheDirt(transform,washingForce);
                   //Debug.Log("dirt");
               }
            }

            if (other.CompareTag("Gate"))
            {
                if (other.TryGetComponent(out GateController gate))
                {
                    gate.FixSplashPosition(transform.position);
                    gate.IncreaseGateStats(_gateIncreaseSpeed);
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

        public void IncreasePower(float force)
        {
            washingForce += force / 1000;
            if (washingForce > 4)
                washingForce = 4;
        }

        public void IncreaseSpeed(float speed)
        {
            _gateIncreaseSpeed += speed/100;
        }

        public void IncreaseRange(float range)
        {
            var desiredSize = _currentColliderScale + Vector3.forward * (range / colliderScaleErrorFix);
            _collider.size = desiredSize;
            desiredSize.z /= 2;
            _collider.center = desiredSize;
            
            var desiredWaterScale = _defaultWaterHoseScale + Vector3.forward * (range / colliderScaleErrorFix / 22);
            _defaultWaterHoseScale = desiredWaterScale;
            
        }
    }
}