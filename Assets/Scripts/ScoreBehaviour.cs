using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace AMARI.Assets.Scripts
{
    public class ScoreBehaviour : MonoBehaviour
    {
        // スコアのテキスト表示
        private Text scoreText;
        private CalcBehaviour calc;

        // Start is called before the first frame update
        void Start()
        {
            int sumScore = 0;
            calc = GetComponent<CalcBehaviour>();
            scoreText = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<Text>();
            var scoreChange = this.UpdateAsObservable()
                .Select(_ => calc.ScoreProp)
                .DistinctUntilChanged()
                .Subscribe(_ => {
                    sumScore += calc.ScoreProp;
                    scoreText.text = "Your Score is " + sumScore.ToString();
                });
        }
    }
}