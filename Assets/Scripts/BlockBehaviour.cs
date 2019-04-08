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
        List<int> blockNumberList = new List<int>();
        private List<(GameObject, Renderer)> cubeRendererTupleList = new List<(GameObject, Renderer)>();
        private List<(GameObject, TextMesh)> cubeTextTupleList = new List<(GameObject, TextMesh)>();
        private List<TextMesh> cubeTextMeshList = new List<TextMesh>();
        private static readonly int TEN = 10;
        private static readonly int CUBEINDEXNUMBER = 9;
        private static readonly int RANDMAX = 10;
        private CalcBehaviour ansReset;
        
        // Start is called before the first frame update
        void Start()
        {
            ansReset = GetComponent<CalcBehaviour>();
            // キューブに対して乱数を割り当てる
            var allocRandamNum = this.UpdateAsObservable()
                .Distinct()
                .Subscribe(_ => AllocateRandomNumbers());

            // オブジェクト名で昇順でソートしてRendererをListに代入する(ここの初期化処理を[SerializeField]を使って書き直す)
            List<GameObject> objectList = new List<GameObject>();
            objectList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
            foreach(var cube in objectList)
            {
                cubeRendererTupleList.Add((cube, cube.GetComponent<Renderer>()));
            }

            // オブジェクト名で昇順でソートしてTextMeshをListに代入する(ここの初期化処理を[SerializeField]を使って書き直す)
            List<GameObject> cubeTextMeshList = new List<GameObject>();
            cubeTextMeshList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
            foreach(var cube in cubeTextMeshList)
            {
                cubeTextTupleList.Add((cube, cube.GetComponentInChildren<TextMesh>()));
            }
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
            AssignRandomNumbersToSelectedCubes(ansReset.OverFlowAnsProp);
            blockNumberList.Clear();
        }

        private void AllocateRandomNumbers()
        {
            // キューブに1～9までの乱数を割り当てる
            for(int i = 0; i < CUBEINDEXNUMBER; ++i)
            {
                cubeTextTupleList[i].Item2.text = Random.Range(1, RANDMAX).ToString();
            }
        }
        private void AssignRandomNumbersToSelectedCubes()
        {
            // 選択したキューブに書かれている数値の合計値が10未満だったらList<TextMesh>をクリアして即リターン
            if(ansReset.AnswerProp < TEN)
            {
                cubeTextMeshList.Clear();
                return;
            }

            // 選択したキューブに1～9までの乱数を割り当てる
            foreach(var cube in cubeTextMeshList)
            {
                cube.text = Random.Range(1, RANDMAX).ToString();
            }
            cubeTextMeshList.Clear();
        }
        private void AssignRandomNumbersToSelectedCubes(int remainder)
        {
            // 選択したキューブに書かれている数値の合計値が10未満だったらList<TextMesh>をクリアして即リターン
            if(ansReset.AnswerProp < TEN)
            {
                cubeTextMeshList.Clear();
                return;
            }
            
            // 配列のインデックス
            var cubeIndex = cubeTextMeshList.Count - 1;
            // ランダムな値をList<T>に代入
            var cubeText = Enumerable.Range(1, cubeIndex)
                .Select(index => cubeTextMeshList[index])
                .Select(mesh => mesh.text)
                .Select(text => text = Random.Range(1, RANDMAX).ToString()).ToList();
            // 選んだキューブにランダムな数値を入れる
            for(int i = 0; i < cubeIndex; ++i)
            {
                cubeTextMeshList[i].text = cubeText[i];
            }
            // 最後に選択したあまりの数値が0のときはランダムな値を入れる
            if(remainder == 0)
            {
                cubeTextMeshList[cubeIndex].text = Random.Range(1, RANDMAX).ToString();
            }
            // あまりの数値を入れる
            else
            {
                cubeTextMeshList[cubeIndex].text = remainder.ToString();
            }
            cubeTextMeshList.Clear();
        }

        public void OnRecievedOneShotGetCubeNumbers(GameObject obj)
        {
            // 送られてきたGameObjectがcubeTextTupleListにある場合にはTextMeshのTextをintに変換してListに入れる
            if(cubeTextTupleList.TupleContains(obj, LRSwitch.LEFT))
            {
                cubeTextMeshList.Add(cubeTextTupleList.TupleContainsGetComponent(obj));
                blockNumberList.Add(int.Parse(cubeTextTupleList.TupleContainsGetComponent(obj).text));
                ExecuteEvents.Execute<ICalculateProvider>(
                    target: gameObject,
                    eventData: null,
                    functor: (reciever, eventData) => reciever.CalculateLimited(blockNumberList)
                );
            }
        }
    }
}