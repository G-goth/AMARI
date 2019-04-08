using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using AMARI.Assets.Scripts;

namespace AMARI.Assets.Scripts
{
    public class TimerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float speedCoeff = 1;
        private static readonly int TEN = 10;
        private CalcBehaviour calculatedRemainder;
        public int RemainderNumberProp{ get; private set; }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
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
                .Where(_ => Input.GetMouseButtonUp(0))
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

        }
    }
}