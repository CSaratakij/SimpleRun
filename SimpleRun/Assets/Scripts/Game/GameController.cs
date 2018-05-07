using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class GameController
    {
        public delegate void Func();
        public delegate void FuncOnPause(bool value);

        public static event Func OnGameStart;
        public static event Func OnGameOver;
        public static event FuncOnPause OnGamePause;

        public static bool IsGameStart { get { return isGameStart; } }
        public static bool IsGamePause { get { return isGamePause; } }


        static bool isGameStart;
        static bool isGamePause;


        static void _FireEvent_OnGameStart()
        {
            if (OnGameStart != null) {
                OnGameStart();
            }
        }

        static void _FireEvent_OnGamePause()
        {
            if (OnGamePause != null) {
                OnGamePause(isGamePause);
            }
        }

        static void _FireEvent_OnGameOver()
        {
            if (OnGameOver != null) {
                OnGameOver();
            }
        }

        static void _Initialize()
        {
            LaneInfo.SetUpLane(3);
            LaneInfo.GoToCenterLane();
        }

        public static void GameStart()
        {
            if (isGameStart) { return; }
            _Initialize();

            isGameStart = true;
            _FireEvent_OnGameStart();
        }

        public static void GamePause(bool value)
        {
            if (isGamePause == value) { return; }

            isGamePause = value;
            Time.timeScale = (isGamePause) ? 0.0f : 1.0f;

            _FireEvent_OnGamePause();
        }

        public static void TogglePause()
        {
            GamePause(!isGamePause);
        }

        public static void GameOver()
        {
            if (!isGameStart) { return; }
            isGameStart = false;
            _FireEvent_OnGameOver();
        }
    }
}
