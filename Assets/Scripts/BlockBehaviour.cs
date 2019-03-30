using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts.ScriptableObjectFolder;

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
        private BlockStatus blkNumberArray;
        private static readonly int CUBEINDEXNUMBER = 9;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            blkNumberArray = Resources.Load<BlockStatus>("BlockStatus");
            for(int i = 0; i < CUBEINDEXNUMBER; ++i)
            {
                blkNumberArray[i] = Random.Range(1, 10);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            var allocRandamNum = this.UpdateAsObservable()
                .Subscribe(_ => {});
            objectList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
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

        public void AllocateRandomNumbers()
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
    }
}