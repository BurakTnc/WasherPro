using System;
using UnityEngine;
using UnityEngine.Events;

namespace _YabuGames.Scripts.Signals
{
    public class LevelSignals : MonoBehaviour
    {
        public static LevelSignals Instance;

        public UnityAction OnDrillStart = delegate { };
        public UnityAction OnRunStart = delegate { };
        public UnityAction OnCleanDirt = delegate { };
        public UnityAction<Transform> OnSpawnNewItem = delegate { };
        public UnityAction OnInit =delegate { };


            private void Awake()
        {
            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}