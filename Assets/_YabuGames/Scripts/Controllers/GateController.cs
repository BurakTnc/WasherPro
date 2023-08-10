using _YabuGames.Scripts.Managers;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Controllers
{
    public class GateController : MonoBehaviour
    {
        [SerializeField] private GateMode gateMode;
        [SerializeField] private TextMeshPro gateText;
        [SerializeField] private float goDownDuration;
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private float power;
        [SerializeField] private TextMeshPro headlineText;
        [SerializeField] private GameObject splashEffect;

        private bool _isLocked;
        private GateVisuals _gateVisuals;
        private BoxCollider _boxCollider;

        private void Awake()
        {
           // _gateVisuals = GetComponent<GateVisuals>();
            _boxCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
           // SetGateVisuals();
           UpdateInterface();
        }

        private void SetGateVisuals()
        {
            switch (gateMode)
            {
                case GateMode.Speed:
                    _gateVisuals.SetGateColor(speed>=0);
                    break;
                case GateMode.Power:
                    _gateVisuals.SetGateColor(power>=0);
                    break;
                case GateMode.Range:
                    _gateVisuals.SetGateColor(range>=0);
                    break;
                default:
                    break;
            }
            UpdateInterface();
        }
        public void IncreaseGateStats(float increaseValue)
        {
            if(_isLocked)
                return;
            switch (gateMode)
            {
                case GateMode.Speed:
                    speed += increaseValue;
                   // _gateVisuals.SetGateColor(speed>=0);
                    break;
                case GateMode.Power:
                    power += increaseValue;
                    //_gateVisuals.SetGateColor(power>=0);
                    break;
                case GateMode.Range:
                   // _gateVisuals.SetGateColor(range>=0);
                    range += increaseValue;
                    break;
                default:
                    break;
            }
            UpdateInterface();
        }

        public void Selection(WasherController washerController)
        {
            if(_isLocked)
                return;
            //_isLocked = true;
            HapticManager.Instance.PlayRigidHaptic();
            switch (gateMode)
            {
                case GateMode.Speed:
                    washerController.IncreaseSpeed((int)speed);
                    break;
                case GateMode.Power:
                    washerController.IncreasePower((int)power);
                    break;
                case GateMode.Range:
                    washerController.IncreaseRange((int)range);
                    break;
                default:
                    break;
            }

            transform.DOMoveY(-5, goDownDuration).SetEase(Ease.InBack);


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
        private void UpdateInterface()
        {
            switch (gateMode)
            {
                case GateMode.Speed:
                    var signFire = Mathf.Sign(speed) > 0 ? "+" : "";
                    headlineText.text = "SPEED";
                    gateText.text = signFire +(speed).ToString("0");
                    break; 
                
                case GateMode.Power:
                    var signDamage = Mathf.Sign(power) > 0 ? "+" : "";
                    headlineText.text = "POWER";
                    gateText.text =signDamage + (power).ToString("0");
                    break;
                
                case GateMode.Range:
                    var signRange = Mathf.Sign(range) > 0 ? "+" : "";
                    headlineText.text = "RANGE";
                    gateText.text = signRange + (range).ToString("0");
                    break;
                
                default:
                    break;
            }
            
        }
    }
}