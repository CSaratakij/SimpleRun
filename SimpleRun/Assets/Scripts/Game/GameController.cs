using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class GameController
    {
        public delegate void Func();
        public static event Func OnGameStart;

        static bool isGameStart;


        static void _FireEvent_OnGameStart()
        {
            if (OnGameStart != null) {
                OnGameStart();
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
    }
}
