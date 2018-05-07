using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleRun
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField]
        RectTransform[] views;


        RectTransform currentView = null;


        enum GameMenuView
        {
            MainMenu,
            InGameMenu,
            PauseMenu,
            GameOverMenu
        }


        void Awake()
        {
            _Subscribe_Event();
        }

        void Start()
        {
            _HideAllView();
            _ShowView(GameMenuView.MainMenu);
        }

        void OnDestroy()
        {
            _Unsubscribe_Event();
        }

        void _Subscribe_Event()
        {
            GameController.OnGameStart += _OnGameStart;
            GameController.OnGamePause += _OnGamePause;
            GameController.OnGameOver += _OnGameOver;
        }

        void _Unsubscribe_Event()
        {
            GameController.OnGameStart -= _OnGameStart;
            GameController.OnGamePause -= _OnGamePause;
            GameController.OnGameOver -= _OnGameOver;
        }

        void _OnGameStart()
        {
            _ShowView(GameMenuView.InGameMenu);
        }

        void _OnGamePause(bool isPause)
        {
            if (isPause) {
                _ShowView(GameMenuView.PauseMenu);
            }
            else {
                _ShowView(GameMenuView.InGameMenu);
            }
        }

        void _OnGameOver()
        {
            _ShowView(GameMenuView.GameOverMenu);
        }

        void _ShowView(GameMenuView view)
        {
            if (currentView != null) {
                currentView.gameObject.SetActive(false);
            }

            currentView = views[(int)view];
            currentView.gameObject.SetActive(true);
        }

        void _HideAllView()
        {
            foreach (RectTransform view in views) {
                view.gameObject.SetActive(false);
            }
        }
    }
}
