using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AMARI.Assets.Scripts
{
    interface ICalculateProvider : IEventSystemHandler
    {
        void CalculateLimited(List<int> numList);
    }
    
    interface IMessageProvider : IEventSystemHandler
    {
        void OnRecievedOneShotMaterialChange(GameObject obj);
        void OnRecievedMaterialAllChange();
        void OnRecievedOneShotGetCubeNumbers(GameObject obj);
    }
}