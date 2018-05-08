using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleRun
{
    public class ScoreView : MonoBehaviour
    {
        const string TEXT_SCORE_FORMAT = "Score : {0}";


        [SerializeField]
        Text txtScore;


        void OnEnable()
        {
            _UpdateText(Global.Score);
        }

        void Awake()
        {
            _Subscribe_Event();
        }

        void OnDestroy()
        {
            _Unsubscribe_Event();
        }

        void _Subscribe_Event()
        {
            Global.OnScoreChanged += _OnScoreChanged;
        }

        void _Unsubscribe_Event()
        {
            Global.OnScoreChanged -= _OnScoreChanged;
        }

        void _OnScoreChanged(int value)
        {
            _UpdateText(value);
        }

        void _UpdateText(int score)
        {
            txtScore.text = string.Format(TEXT_SCORE_FORMAT, score);
        }
    }
}
