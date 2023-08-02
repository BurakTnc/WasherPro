using _YabuGames.Scripts.Signals;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float followSpeed = 3f;
        [SerializeField] private Vector3 followRotation;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 onDirtRotation;
        [SerializeField] private Vector3 onDirtOffset;

        private bool _isCleaning;
        private Transform _player;
        private bool _isGameRunning;

        private void Awake()
        {
            _player = GameObject.Find("Player").transform;
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        void Update()
        {
            Follow();
        }

        #region Subscribtions

        private void Subscribe()
                {
                    LevelSignals.Instance.OnRunStart += OnGameStart;
                    CoreGameSignals.Instance.OnLevelFail += OnGameEnd;
                    CoreGameSignals.Instance.OnLevelWin += OnGameEnd;
                    LevelSignals.Instance.OnCleanDirt += OnCleaningLook;
                }
        
                private void UnSubscribe()
                {
                    LevelSignals.Instance.OnRunStart -= OnGameStart;
                    CoreGameSignals.Instance.OnLevelFail -= OnGameEnd;
                    CoreGameSignals.Instance.OnLevelWin -= OnGameEnd;
                    LevelSignals.Instance.OnCleanDirt -= OnCleaningLook;
                }

        #endregion
        
        private void OnGameStart()
        {
            _isGameRunning = true;
            transform.DORotate(followRotation, 1);
        }

        private void OnGameEnd()
        {
            _isGameRunning = false;
        }

        private void OnCleaningLook()
        {
            _isCleaning = !_isCleaning;
            if (_isCleaning)
            {
                transform.DORotate(onDirtRotation, .6f).SetEase(Ease.InSine);
            }
            else
            {
                transform.DORotate(followRotation, .4f).SetEase(Ease.InSine);
            }
        }

        private void Follow()
        {
            if (_isGameRunning)
            {
                var currentOffset = _isCleaning ? onDirtOffset : offset;
                
                transform.position = Vector3.Lerp(transform.position, _player.position + currentOffset,
                    followSpeed * Time.deltaTime);
            }

            // if (_isGameRunning) 
            // {
            //     transform.position = Vector3.SmoothDamp(transform.position, _player.position, ref _velocity, followSpeed);
            // }

                                     // For Limited Follow
            
            // if (_isGameRunning)
            // {
            //     Vector3 desiredPos = new Vector3(0, transform.position.y, _player.position.z) + offset;
            //     transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
            // }
        }
    }
}
