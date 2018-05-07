using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField]
        float moveForce;

        [SerializeField]
        Vector3 moveDirection;

        
        void Update()
        {
            _MoveHandler();
        }

        void _MoveHandler()
        {
            transform.Translate(moveDirection * moveForce * Time.deltaTime);

            if (transform.position.y < (LaneInfo.LanePositions[0].y - 5.0f)) {
                gameObject.SetActive(false);
            }
        }
    }
}
