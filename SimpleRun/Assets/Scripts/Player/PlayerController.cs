using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        Vector3 offset;

        [SerializeField]
        LayerMask obstacleMask;

        [SerializeField]
        Vector3 origin;

        [SerializeField]
        Vector2 size;


        void Awake()
        {
            _Subscribe_Event();
        }

        void Update()
        {
            _InputHandler();
        }

        void FixedUpdate()
        {
            //physics cast here..
            //if detect obstacle -> game over..
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
            if (!GameController.IsGameStart) { return; }
            if (GameController.IsGamePause) { return; }

            if (direction == SwipeDirection.Left) {
                _MoveToLeftLane();
            }
            else if (direction == SwipeDirection.Right) {
                _MoveToRightLane();
            }
        }

        void _InputHandler()
        {
            if (!GameController.IsGameStart) { return; }
            if (GameController.IsGamePause) { return; }

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
