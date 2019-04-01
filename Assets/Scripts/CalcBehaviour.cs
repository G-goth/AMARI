using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts;

namespace AMARI.Assets.Scripts
{
    public class CalcBehaviour : MonoBehaviour, ICalculateProvider
    {
        [SerializeField]
        private int LIMIT = 10;
        private int answer;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>        
        void Start()
        {
            var calculate = this.UpdateAsObservable().Subscribe(_ => {});
        }

        public void CalculateLimited(List<int> numList)
        {
            if(answer < LIMIT)
            {
                answer = numList.Sum();
                if(numList.Sum() >= LIMIT)
                {
                    var over = numList.Sum() - LIMIT;
                    Debug.Log("Over Flow!! Over is " + over);
                }
                else
                {
                    Debug.Log(answer);
                }
            }
        }
    }
}