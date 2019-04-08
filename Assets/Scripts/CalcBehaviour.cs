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
        private int answer = 0;
        public int AnswerProp
        {
            get{ return this.answer; }
            set{ this.answer = value; }
        }
        public int OverFlowAnsProp{ get; private set; }
        public int ScoreProp{ get; set; }
        public int CoefficientProp{ get; set; }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>        
        void Start()
        {
            var calculate = this.UpdateAsObservable().Subscribe(_ => {});
        }

        //private int coefficient = 1;
        public void CalculateLimited(List<int> numList)
        {
            // 係数に足していく(2倍、3倍、4倍・・・)
            ++CoefficientProp;
            if(answer >= LIMIT) return;

            answer = numList.Sum();
            if(numList.Sum() >= LIMIT)
            {
                var sum = numList.Sum();
                var over = sum - LIMIT;
                OverFlowAnsProp = over;
                ScoreProp = CalculateScore(sum, CoefficientProp);
                CoefficientProp = 1;
                // Debug.Log("Score is " + ScoreProp);
                // Debug.Log("Over Flow!! Over is " + over);
            }
        }

        private int CalculateScore(int count, int coefficient) => count * coefficient;
    }
}