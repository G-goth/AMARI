using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts.Extensions;

namespace AMARI.Assets.Scripts
{
    public class MouseBehaviour : MonoBehaviour
    {
        private static readonly int ONE = 1;
        private static readonly int TEN = 10;
        private CalcBehaviour ansReset;
        private TimerBehaviour addTime;
        public int CubeListElementCountProp{ get; set;}
        
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            // TimerBehaviourを取得
            addTime = GameObject.FindGameObjectWithTag("Canvas").GetComponent<TimerBehaviour>();
            // 選択したキューブのオブジェクトをスタックするList<T>
            List<GameObject> cubeObjectList = new List<GameObject>();
            // CalcBehaviourを取得
            ansReset = GetComponent<CalcBehaviour>();

            // マウスホールド時の挙動
            var mouseHold = this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Select(_ => GetObjectByRayCastHit())
                .Where(cube => GetObjectByRayCastHit() != null && cubeObjectList.IsAddTriming(cube))
                .Subscribe(_ => {
                    PostMessageToOnRecievedOneShotMaterialChange();
                    PostMessageToOnRecievedOneShotGetCubeNumbers();
                });
            // マウスホールドでキューブオブジェクトの個数を取得
            var mouseHoldOnGetCubeCount = this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButton(0))
                .Select(_ => CubeListElementCountProp = cubeObjectList.Count)
                .DistinctUntilChanged()
                .Subscribe(index => CubeListElementCountProp = index);

            // マウスボタンリリース時の挙動
            var mouseRelease = this.UpdateAsObservable()
                .Where(_ => cubeObjectList.Count >= ONE)
                .Where(_ => Input.GetMouseButtonUp(0))
                .Subscribe(_ => {
                    // すべてのマテリアルを白に戻す
                    PostMessageToOnRecievedMaterialAllChange();

                    // 合計値などを0にする前にタイマーに反映
                    addTime.AddTime();
                    DataReset(cubeObjectList);
                });
        }
        // 各種データのリセット
        private void DataReset(List<GameObject> cubeObjectList)
        {
            ansReset.AnswerProp = 0;
            ansReset.CoefficientProp = 1;
            cubeObjectList.Clear();
        }

        // 特定のメソッドへメッセージを飛ばす
        private void PostMessageToOnRecievedOneShotMaterialChange()
        {
            ExecuteEvents.Execute<IMessageProvider>(
                target: gameObject,
                eventData: null,
                functor: (reciever, evenData) => {
                    reciever.OnRecievedOneShotMaterialChange(GetObjectByRayCastHit());
                }
            );
        }
        private void PostMessageToOnRecievedOneShotGetCubeNumbers()
        {
            ExecuteEvents.Execute<IMessageProvider>(
                target: gameObject,
                eventData: null,
                functor: (reciever, eventData) => {
                    reciever.OnRecievedOneShotGetCubeNumbers(GetObjectByRayCastHit());
                }
            );
        }
        private void PostMessageToOnRecievedMaterialAllChange()
        {
            ExecuteEvents.Execute<IMessageProvider>(
                target: gameObject,
                eventData: null,
                functor: (reciever, eventData) => {
                    reciever.OnRecievedMaterialAllChange();
            });
        }

        private GameObject GetObjectByRayCastHit()
        {
            // ここに10以上の数値になったときにレイキャストを飛ばさないような処理を書く
            if(ansReset.AnswerProp >= TEN) return null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray.origin, ray.direction, out hit, 100.0f))
            {
                return hit.collider.gameObject;
            }
            return null;
        }   
    }
}