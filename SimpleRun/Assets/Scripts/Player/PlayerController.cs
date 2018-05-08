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
        Transform origin;

        [SerializeField]
        Vector2 size;

        [SerializeField]
        LayerMask obstacleMask;


        bool isCanMove;

        AudioSource audioSource;
        Collider2D hit;


        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            _Subscribe_Event();
        }

        void Update()
        {
            _InputHandler();
        }

        void FixedUpdate()
        {
            if (!GameController.IsGameStart) { 
                hit = null;
                return; 
            }

            hit = Physics2D.OverlapBox(origin.position, size, 0.0f, obstacleMask);
            _HitObstacleHandler();
        }

        void OnDestroy()
        {
            _Unsubscribe_Event();
        }

        void _Subscribe_Event()
        {
            SwipeInput.OnSwipe += _OnSwipe;
            GameController.OnGameStart += _OnGameStart;
            GameController.OnGamePause += _OnGamePause;
        }

        void _Unsubscribe_Event()
        {
            SwipeInput.OnSwipe -= _OnSwipe;
            GameController.OnGameStart -= _OnGameStart;
            GameController.OnGamePause -= _OnGamePause;
        }

        void _OnSwipe(SwipeDirection direction)
        {
            if (!GameController.IsGameStart) { return; }
            if (GameController.IsGamePause || !isCanMove) { 
                SwipeInput.instance.ClearSwipe();
                return; 
            }

            if (direction == SwipeDirection.Left) {
                _MoveToLeftLane();
            }
            else if (direction == SwipeDirection.Right) {
                _MoveToRightLane();
            }
        }

        void _OnGameStart()
        {
            transform.position = LaneInfo.GetCurrentLanePosition() + offset;
            isCanMove = true;
        }

        void _OnGamePause(bool isPause)
        {
            SwipeInput.instance.ClearSwipe();

            if (isPause) {
                isCanMove = false;
            }
            else {
                StartCoroutine(_Pause_Callback());
            }
        }

        IEnumerator _Pause_Callback()
        {
            SwipeInput.instance.ClearSwipe();
            yield return new WaitForSeconds(0.2f);
            isCanMove = true;
        }

        void _InputHandler()
        {
            if (!GameController.IsGameStart) { return; }
            if (GameController.IsGamePause) { return; }
            if (!isCanMove) { return; }

            if (Input.GetKeyDown(KeyCode.A)) {
                _MoveToLeftLane();
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                _MoveToRightLane();
            }
        }

        void _HitObstacleHandler()
        {
            if (hit) {
                if (hit.transform.CompareTag("coin")) {
                    Global.AddScore(1);
                    audioSource.Play();
                    hit.gameObject.SetActive(false);
                }
                else {
                    GameController.GameOver();
                }
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
