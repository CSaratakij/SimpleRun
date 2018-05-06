using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        Vector3 offset;


        void Awake()
        {
            _Subscribe_Event();
        }

        //Test..
        void Start()
        {
            GameController.GameStart();
        }

        void Update()
        {
            _InputHandler();
        }

        void OnDestroy()
        {
            _Unsubscribe_Event();
        }

        void _Subscribe_Event()
        {
            GameController.OnGameStart += _OnGameStart;
            SwipeInput.OnSwipe += _OnSwipe;
        }

        void _Unsubscribe_Event()
        {
            GameController.OnGameStart -= _OnGameStart;
            SwipeInput.OnSwipe -= _OnSwipe;
        }

        void _OnGameStart()
        {
            transform.position = LaneInfo.GetCurrentLanePosition() + offset;
        }

        void _OnSwipe(SwipeDirection direction)
        {
            if (direction == SwipeDirection.Left) {
                _MoveToLeftLane();
            }
            else if (direction == SwipeDirection.Right) {
                _MoveToRightLane();
            }
        }

        void _InputHandler()
        {
            if (Input.GetKeyDown(KeyCode.A)) {
                _MoveToLeftLane();
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                _MoveToRightLane();
            }
        }

        void _MoveToLeftLane()
        {
            transform.position = LaneInfo.GetPreviousLanePosition() + offset;
        }

        void _MoveToRightLane()
        {
            transform.position = LaneInfo.GetNextLanePosition() + offset;
        }
    }
}
