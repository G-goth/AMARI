using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using AMARI.Assets.Scripts;

namespace AMARI.Assets.Scripts
{
    public class BlockBehaviour : MonoBehaviour, IMessageProvider
    {
        [SerializeField]
        private Material _material = (default);
        [SerializeField]
        private Material _defMaterial = (default);
        private List<GameObject> objectList = new List<GameObject>();
        private List<Renderer> rendererList = new List<Renderer>();
        
        // Start is called before the first frame update
        void Start()
        {
            objectList = GameObject.FindGameObjectsWithTag("Cube").ToList();
            rendererList = objectList.Select(obj => obj.GetComponent<Renderer>()).ToList();
        }

        public void OnRecievedOneShotMaterialChange(GameObject obj)
        {
            obj.GetComponent<Renderer>().material = _material;
        }
        public void OnRecievedMaterialAllChange()
        {
            foreach(var rend in rendererList)
            {
                rend.material = _defMaterial;
            }
        }
    }
}