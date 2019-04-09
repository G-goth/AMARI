using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts;

namespace AMARI.Assets.Scripts
{
    enum THREECOUNT
    {
        ONE,
        TWO,
        THREE
    }
    public class TimerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speedCoeff = 1;
        private (bool, bool, bool) threeCount;
        private static readonly int TEN = 10;
        private CalcBehaviour calculatedRemainder;
        private MouseBehaviour cubeCount;
        private List<Text> waitTextObject = new List<Text>();
        public int RemainderNumberProp{ get; private set; }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            // 選択しているキューブの個数を取得
            cubeCount = GameObject.FindGameObjectWithTag("GameController").GetComponent<MouseBehaviour>();

            // 残機の状態を表すテキストを取得
            waitTextObject = GameObject.FindGameObjectsWithTag("CounterSignal").Select(obj => obj.GetComponent<Text>()).ToList();
            threeCount.Item1 = true;
            threeCount.Item2 = true;
            threeCount.Item3 = true;

            // キューブの計算のあまりを取得
            calculatedRemainder = GameObject.FindGameObjectWithTag("GameController").GetComponent<CalcBehaviour>();
            // Sliderオブジェクトのセット
            var timerSlider = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Slider>();
            // ゲージの初期値のセット
            timerSlider.maxValue = 30;
            // ゲージをカウントダウンする
            var timeSliderStream = this.FixedUpdateAsObservable()
                .Subscribe(_ => {
                    timerSlider.value -= Time.deltaTime * speedCoeff;
                });

            // マウスリリースの時に現在のタイムを取得
            var remainderTime = this.UpdateAsObservable()
                .Where(_ => cubeCount.CubeListElementCountProp < 0 & Input.GetMouseButtonUp(0))
                .Where(_ => timerSlider.value > 0)
                .Subscribe(_ => {
                    // タイマーへ加算する処理
                    if(calculatedRemainder.AnswerProp <= 10)
                    {
                        timerSlider.value += TEN;
                    }
                    else
                    {
                        timerSlider.value += calculatedRemainder.OverFlowAnsProp;
                    }
                });

            // タイマーが0になったときの挙動
            var timerZero = this.UpdateAsObservable()
                .Where(_ => timerSlider.value <= 0)
                .Subscribe(_ => {
                });
        }

        private void ThreeCount()
        {
            if(threeCount.Item1 != false)
            {
                threeCount.Item1 = false;
                waitTextObject[0].text = "";
            }
        }
    }
}