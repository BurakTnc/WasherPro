using System.Collections;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        public static bool OnTrap;

        [SerializeField] private float xPosClamp, speedSideways, speed, cleaningSpeed;

        private float _currentSpeed;
        private bool _isCleaning;
        private Rigidbody _rb;
        private Vector3 _pos1, _pos2;
        private bool _holding;
        private bool _isGameRunning;
        
       private void Awake()
        {
            _rb = GetComponentInChildren<Rigidbody>();
            _isGameRunning = _holding = OnTrap = false;
            _currentSpeed = speed;
        }

       #region Subscribtions
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
           LevelSignals.Instance.OnRunStart += OnGameStart;
           CoreGameSignals.Instance.OnLevelWin += OnGameEnd;
           CoreGameSignals.Instance.OnLevelFail += OnGameEnd;
           LevelSignals.Instance.OnCleanDirt += OnCleaning;
       }

       private void UnSubscribe()
       {
           LevelSignals.Instance.OnRunStart -= OnGameStart;
           CoreGameSignals.Instance.OnLevelWin -= OnGameEnd;
           CoreGameSignals.Instance.OnLevelFail -= OnGameEnd;
           LevelSignals.Instance.OnCleanDirt -= OnCleaning;
       }

       #endregion
       
       private void Update()
       {
            Movement();
       }

       private IEnumerator StartDelay()
       {
           yield return new WaitForSeconds(1);
           Run();
       }

       private void OnCleaning()
       {
           _isCleaning = !_isCleaning;
           _currentSpeed = _isCleaning ? cleaningSpeed : speed;
       }

       private void Run()
       {
           _isGameRunning = true;
       }

       private void OnGameStart()
       {
           StartCoroutine(StartDelay());
       }

       private void OnGameEnd()
       {
           _isGameRunning = false;
       }
       
       private void Movement()
        {
            if(!_isGameRunning) return;
            
            if (!OnTrap)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _pos1 = GetMousePosition();
                    _holding = true;
                }

                if (Input.GetMouseButton(0) && _holding)
                {
                    _pos2 = GetMousePosition();
                    Vector3 delta = _pos1 - _pos2;
                    _pos1 = _pos2;
                    delta = new Vector3(Mathf.Clamp(delta.x, -0.05f, 0.05f), delta.y);
                    _rb.velocity = new Vector3(Mathf.Lerp(_rb.velocity.x, -delta.x * speedSideways, 0.2f), _rb.velocity.y, _currentSpeed);
                }
                else
                {
                    _rb.velocity = transform.forward * _currentSpeed;
                }

                var position = transform.position;
                
                position = new Vector3(Mathf.Clamp(position.x, -xPosClamp, xPosClamp),
                    position.y, position.z);
                transform.position = position;
            }
        }

        private Vector2 GetMousePosition()
        {
            var pos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

            return pos;
        }

    }
}
