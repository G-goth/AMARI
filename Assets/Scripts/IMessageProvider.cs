using UnityEngine;
using UnityEngine.EventSystems;

namespace AMARI.Assets.Scripts
{
    public interface IMessageProvider : IEventSystemHandler
    {
        void OnRecievedOneShotMaterialChange(GameObject obj);
        void OnRecievedMaterialAllChange();
    }
}