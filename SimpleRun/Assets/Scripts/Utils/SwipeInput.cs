using UnityEngine;

namespace SimpleRun
{
    public enum SwipeDirection
    {
        None,
        Left,
        Right
    }

    public class SwipeInput : MonoBehaviour
    {
        public delegate void Func(SwipeDirection direction);
        public static event Func OnSwipe;

        public static SwipeInput instance = null;

        Touch currentTouch;

        Vector2 beginTouchPosition;
        Vector2 endTouchPosition;


        void Awake()
        {
            _MakeSingleton();
        }

        void Update()
        {
            _InputHandler();
        }

        void _MakeSingleton()
        {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        void _InputHandler()
        {
            if (Input.touchCount <= 0) { return; }

            for (int i = 0; i < Input.touchCount; i++) {
                currentTouch = Input.GetTouch(i);

                switch (currentTouch.phase) {
                    case TouchPhase.Began:
                        beginTouchPosition = currentTouch.position;
                        break;

                    case TouchPhase.Ended:
                        endTouchPosition = currentTouch.position;
                        _FireEvent_OnSwipe(GetSwipeDirection());
                        _Reset();
                        break;

                    default:
                        break;
                }
            }
        }

        void _FireEvent_OnSwipe(SwipeDirection direction)
        {
            if (OnSwipe != null) {
                OnSwipe(direction);
            }
        }

        void _Reset()
        {
            beginTouchPosition = Vector2.zero;
            endTouchPosition = Vector2.zero;
        }

        public Vector2 GetSwipeAxisRaw()
        {
            return (endTouchPosition - beginTouchPosition);
        }

        public Vector2 GetSwipeAxis()
        {
            return (endTouchPosition - beginTouchPosition).normalized;
        }

        public SwipeDirection GetSwipeDirection()
        {
            var swipeAxis = GetSwipeAxis();
            var direction = SwipeDirection.None;

            if (swipeAxis.x > 0.0f) {
                direction = SwipeDirection.Right;
            }
            else if (swipeAxis.x < 0.0f) {
                direction = SwipeDirection.Left;
            }

            return direction;
        }

        public bool IsSwipeAtDirection(SwipeDirection direction)
        {
            return direction == GetSwipeDirection();
        }

        public void ClearSwipe()
        {
            _Reset();
        }
    }
}
