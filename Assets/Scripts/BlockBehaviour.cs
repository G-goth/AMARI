using System.Linq;
using System.Collections.Generic;
using UnityEngine;
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
        private List<GameObject> cubeTextMeshList = new List<GameObject>();
        private List<(GameObject, Renderer)> cubeRendererTupleList = new List<(GameObject, Renderer)>();
        private List<(GameObject, TextMesh)> cubeTextTupleList = new List<(GameObject, TextMesh)>();

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
            var allocRandamNum = this.UpdateAsObservable().Subscribe(_ => {});

            // オブジェクト名で昇順でソートしてRendererをListに代入する
            objectList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
            foreach(var cube in objectList)
            {
                cubeRendererTupleList.Add((cube, cube.GetComponent<Renderer>()));
            }

            // オブジェクト名で昇順でソートしてTextMeshをListに代入する
            cubeTextMeshList.AddRange(GameObject.FindGameObjectsWithTag("CubeText").OrderBy(go => go.name));
            foreach(var cube in cubeTextMeshList)
            {
                cubeTextTupleList.Add((cube, cube.GetComponent<TextMesh>()));
            }
            // for(int i = 0; i < CUBEINDEXNUMBER; ++i)
            // {
            //     textMeshList[i].text = blkNumberArray[i].ToString();
            // }
        }

        public void OnRecievedOneShotMaterialChange(GameObject obj)
        {
            obj.GetComponent<Renderer>().material = _material;
        }
        public void OnRecievedMaterialAllChange()
        {
            foreach(var rend in cubeRendererTupleList)
            {
                rend.Item2.material = _defMaterial;
            }
        }

        public void AllocateRandomNumbers()
        {
            Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        public void OnRecievedOneShotChangeNumbers(GameObject obj)
        {
            Debug.Log(obj.name);
        }
    }
}