using System.Collections.Generic;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Managers
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;
        
    [FormerlySerializedAs("firstParticle")]
    [Header("                               // Set Particles Stop Action To DISABLE //")]
    [Space(20)]
        [SerializeField] private List<GameObject> incomeParticle = new List<GameObject>();
        [SerializeField] private List<GameObject> secondParticle = new List<GameObject>();
        [SerializeField] private List<GameObject> thirdParticle = new List<GameObject>();
        [SerializeField] private List<GameObject> fourthParticle = new List<GameObject>();

        
        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            #endregion
            
        }

        public void GetIncomeParticle(Vector3 desiredPos, int value,bool isSmalled=false)
        {
            if (isSmalled)
            {
                HapticManager.Instance.PlayRigidHaptic();
            }
            else
            {
                HapticManager.Instance.PlaySelectionHaptic();
            }
            var temp = incomeParticle[0];
            incomeParticle.Remove(temp);
            temp.transform.position = desiredPos;
            temp.transform.localScale = isSmalled ? Vector3.one * .5f : Vector3.one;
            temp.GetComponent<TextMeshPro>().text = "$" + value;
            GameManager.Instance.money += value;
            GameManager.Instance.earnedValue += value;
            CoreGameSignals.Instance.OnSave?.Invoke();
            temp.SetActive(true);
            incomeParticle.Add(temp);
            temp.transform.DOMove(new Vector3(-1,4,3), 1).SetRelative(true).SetEase(Ease.InSine).OnComplete(() => temp.SetActive(false));
            temp.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InSine);

        }
        public void GetSecondParticle(Vector3 desiredPos)
        {
            var temp = secondParticle[0];
            secondParticle.Remove(temp);
            temp.transform.position = desiredPos;
            temp.SetActive(true);
            secondParticle.Add(temp);
        }
        public void GetThirdParticle(Vector3 desiredPos)
        {
            var temp = thirdParticle[0];
            thirdParticle.Remove(temp);
            temp.transform.position = desiredPos;
            temp.SetActive(true);
            thirdParticle.Add(temp);
        }
        public void GetFourthParticle(Vector3 desiredPos)
        {
            var temp = fourthParticle[0];
            fourthParticle.Remove(temp);
            temp.transform.position = desiredPos;
            temp.SetActive(true);
            fourthParticle.Add(temp);
        }
    }
}
