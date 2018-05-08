using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class Global
    {
        public delegate void Func(int value);
        public static event Func OnScoreChanged;

        public static int Score { get { return _score; } }


        static int _score;


        public static void AddScore(int value)
        {
            _score += value;
            _FireEvent_OnScoreChanged();
        }

        public static void ClearScore()
        {
            _score = 0;
        }

        static void _FireEvent_OnScoreChanged()
        {
            if (OnScoreChanged != null) {
                OnScoreChanged(_score);
            }
        }
    }
}
