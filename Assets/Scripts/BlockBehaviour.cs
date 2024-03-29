using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts.Extensions;

namespace AMARI.Assets.Scripts
{
    public class BlockBehaviour : MonoBehaviour, IMessageProvider
    {
        [SerializeField]
        private Material _material = (default);
        [SerializeField]
        private Material _defMaterial = (default);
        // 定数
        private static readonly int TEN = 10;
        private static readonly int LOW_RANDMAX = 4;
        private static readonly int RANDMAX = 10;
        // レイキャストで取得したキューブの数字を一時的に入れる
        List<int> blockNumberList = new List<int>();
        // キューブとレンダラー
        private Dictionary<GameObject, Renderer> cubeRendererDict = new Dictionary<GameObject, Renderer>();
        // キューブとテキストメッシュ
        private Dictionary<GameObject, TextMesh> cubeTextDict = new Dictionary<GameObject, TextMesh>();
        private List<TextMesh> cubeTextMeshList = new List<TextMesh>();
        private CalcBehaviour calcProps;
        
        // Start is called before the first frame update
        void Start()
        {
            calcProps = GetComponent<CalcBehaviour>();
            // キューブに対して乱数を割り当てる
            var allocRandamNum = this.UpdateAsObservable()
                .Distinct()
                .Subscribe(_ => AllocateRandomNumbers());

            // オブジェクト名で昇順でソートしてRendererをListに代入する(ここの初期化処理を[SerializeField]を使って書き直す)
            List<GameObject> objectList = new List<GameObject>();
            objectList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
            foreach(var cube in objectList)
            {
                cubeRendererDict.Add(cube, cube.GetComponent<Renderer>());
            }

            // オブジェクト名で昇順でソートしてTextMeshをListに代入する(ここの初期化処理を[SerializeField]を使って書き直す)
            List<GameObject> cubeTextMeshList = new List<GameObject>();
            cubeTextMeshList.AddRange(GameObject.FindGameObjectsWithTag("Cube").OrderBy(go => go.name));
            foreach(var cube in cubeTextMeshList)
            {
                cubeTextDict.Add(cube, cube.GetComponentInChildren<TextMesh>());
            }
        }

        // 選択したブロックを赤く染める
        public void OnRecievedOneShotMaterialChange(GameObject obj)
        {
            obj.GetComponent<Renderer>().material = _material;
        }
        // 変更したマテリアルをデフォルトに戻す
        public void OnRecievedMaterialAllChange()
        {
            foreach(var rend in cubeRendererDict)
            {
                rend.Value.material = _defMaterial;
            }
            AssignRandomNumbersToSelectedCubes(calcProps.OverFlowAnsProp);
            blockNumberList.Clear();
        }

        private void AllocateRandomNumbers()
        {
            // キューブに1～9までの乱数を割り当てる
            foreach(var text in cubeTextDict)
            {
                var tempRand = (Random.Range(1, RANDMAX) + Random.Range(1, RANDMAX)) / 2;
                text.Value.text = tempRand.ToString();
            }
        }
        private void AssignRandomNumbersToSelectedCubes(int remainder)
        {
            // 選択したキューブに書かれている数値の合計値が10未満だったらList<TextMesh>をクリアして即リターン
            if(calcProps.AnswerProp < TEN)
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
            // 最後に選択したあまりの数値が0のときは低めなランダムな値を入れる
            if(remainder == 0)
            {
                cubeTextMeshList[cubeIndex].text = Random.Range(1, LOW_RANDMAX).ToString();
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
            if(cubeTextDict.ContainsKey(obj))
            {
                cubeTextMeshList.Add(cubeTextDict.GetMatchingComponent(obj));
                blockNumberList.Add(int.Parse(cubeTextDict.GetMatchingComponent(obj).text.ToString()));
                ExecuteEvents.Execute<ICalculateProvider>(
                    target: gameObject,
                    eventData: null,
                    functor: (reciever, evenData) => reciever.CalculateLimited(blockNumberList)
                );
            }
        }
    }
}