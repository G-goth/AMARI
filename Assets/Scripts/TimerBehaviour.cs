using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace AMARI.Assets.Scripts
{
    public class TimerBehaviour : MonoBehaviour, ITimerProvider
    {
        [SerializeField]
        private float speedCoeff = 1;
        private int answer;
        private (bool, bool, bool) threeCount;
        private static readonly int TEN = 10;
        private List<Toggle> isCheckObject = new List<Toggle>();
        private MouseBehaviour cubeCount;
        private CalcBehaviour calculatedRemainder;
        private Slider timerSlider;
        private List<Text> waitTextObject = new List<Text>();
        public int RemainderNumberProp{ get; private set; }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            // ゲームオーバーテキストの取得
            var gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            // 選択しているキューブの個数を取得
            cubeCount = GameObject.FindGameObjectWithTag("GameController").GetComponent<MouseBehaviour>();
            // 残機のチェックを外す
            isCheckObject = GameObject.FindGameObjectsWithTag("CounterSignal").Select(obj => obj.GetComponent<Toggle>()).ToList();
            // 残機の状態を表すテキストを取得
            waitTextObject = GameObject.FindGameObjectsWithTag("CounterSignal").Select(obj => obj.GetComponent<Text>()).ToList();
            threeCount.Item1 = true;
            threeCount.Item2 = true;
            threeCount.Item3 = true;

            // キューブの数値の計算後のあまりを取得
            calculatedRemainder = GameObject.FindGameObjectWithTag("GameController").GetComponent<CalcBehaviour>();
            // Sliderオブジェクトのセット
            timerSlider = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Slider>();
            // ゲージの初期値のセット
            timerSlider.maxValue = 30;
            // ゲージをカウントダウンする
            var timeSliderStream = this.FixedUpdateAsObservable()
                .Where(_ => timerSlider.value > 0)
                .Subscribe(_ => {
                    timerSlider.value -= Time.deltaTime * speedCoeff;
                });

            // // マウスリリースの時に現在のタイムを取得
            // var remainderTime = this.UpdateAsObservable()
            //     .Where(_ => cubeCount.CubeListElementCountProp > 0 & Input.GetMouseButtonUp(0))
            //     .Where(_ => timerSlider.value > 0)
            //     .Subscribe(_ => {
            //         // タイマーへ加算する処理
            //         if(calculatedRemainder.OverFlowAnsProp == 0)
            //         {
            //             timerSlider.value += TEN;
            //         }
            //         else
            //         {
            //             timerSlider.value += calculatedRemainder.OverFlowAnsProp;
            //         }
            //     });

            // タイマーが0になったときの挙動
            var timerZero = this.UpdateAsObservable()
                .Where(_ => timerSlider.value <= 0)
                .Where(_ => threeCount.Item1 == true | threeCount.Item2 == true | threeCount.Item3 == true)
                .Subscribe(_ => {
                    ThreeCount();
                    if(threeCount.Item3 != false)　timerSlider.value = 30;
                });
                
            // すべてのカウンターがfalseになったときの挙動
            var allCountFail = this.UpdateAsObservable()
                .Where(_ => threeCount.Item1 == false & threeCount.Item2 == false & threeCount.Item3 == false)
                .Distinct()
                .Subscribe(_ => {
                    gameOverText.enabled = true;
                    gameOverText.text = "GameOver...";
                    Debug.Log("GameOver...");
                });
        }
        

        // タイマーへの加算処理
        public void AddTime()
        {
            // 選んでいるキューブが0この場合は即リターン
            if(cubeCount.CubeListElementCountProp < 0) return;

            // 選んでいるキューブの数値が10未満の時は即リターン
            if(calculatedRemainder.AnswerProp < 10) return;

            // ピッタリ10の時とそうでない時でタイマーへの加算量を変える
            if(calculatedRemainder.OverFlowAnsProp == 0)
            {
                Debug.Log("TEN!!");
                timerSlider.value += TEN;
            }
            else
            {
                Debug.Log("Other");
                timerSlider.value += calculatedRemainder.OverFlowAnsProp;
            }
        }

        private void ThreeCount()
        {
            if(threeCount.Item1 != false)
            {
                threeCount.Item1 = false;
                isCheckObject[0].isOn = false;
                return;
            }
            if(threeCount.Item2 != false)
            {
                threeCount.Item2 = false;
                isCheckObject[1].isOn = false;
                return;
            }
            if(threeCount.Item3 != false)
            {
                threeCount.Item3 = false;
                isCheckObject[2].isOn = false;
                return;
            }
        }
    }
}