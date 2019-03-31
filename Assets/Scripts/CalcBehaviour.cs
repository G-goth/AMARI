using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AMARI.Assets.Scripts;

namespace AMARI.Assets.Scripts
{
    public class CalcBehaviour : MonoBehaviour, ICalculateProvider
    {
        [SerializeField]
        private int LIMIT = 10;
        public void CalculateLimited(List<int> numList)
        {
            if(numList.Sum() < 10)
            {
                Debug.Log(numList.Sum());
            }
            else
            {
                var over = numList.Sum() - LIMIT;
                Debug.Log("Over Flow!! Over is " + over);
            }
        }
    }
}