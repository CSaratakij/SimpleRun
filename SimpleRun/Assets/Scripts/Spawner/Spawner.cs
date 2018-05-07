using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        GameObject[] blueprints;

        [SerializeField]
        [Range(1, 30)]
        int maximumPooling;


        Vector3[] originInEachLanes;


        GameObject[] coinPool;
        GameObject[] wallPool;
        GameObject[] spikePool;


        enum BlueprintType
        {
            Coin,
            Wall,
            Spike
        }

        Vector3[] origins;


        void Awake()
        {
            _Initialize();
            _Subscribe_Event();
        }

        void OnDestroy()
        {
            _Unsubscribe_Event();
        }

        void _Subscribe_Event()
        {
            GameController.OnGameStart += _OnGameStart;
        }

        void _Unsubscribe_Event()
        {
            GameController.OnGameStart -= _OnGameStart;
        }

        void _OnGameStart()
        {
            _CalculateObstacleOrigin();
        }

        void _Initialize()
        {
            _CreatePool();
        }

        void _CreatePool()
        {
            coinPool = new GameObject[maximumPooling];
            wallPool = new GameObject[maximumPooling];
            spikePool = new GameObject[maximumPooling];

            for (int i = 0; i < maximumPooling; i++) {
                coinPool[i] = (GameObject)Instantiate(blueprints[(int)BlueprintType.Coin]);
                coinPool[i].SetActive(false);

                wallPool[i] = (GameObject)Instantiate(blueprints[(int)BlueprintType.Wall]);
                wallPool[i].SetActive(false);

                spikePool[i] = (GameObject)Instantiate(blueprints[(int)BlueprintType.Spike]);
                spikePool[i].SetActive(false);
            }
        }

        void _CalculateObstacleOrigin()
        {
            originInEachLanes = new Vector3[LaneInfo.MaximumLane];

            var pixelHeight = Camera.main.pixelHeight;
            var targetScreenPoint = new Vector3(0.0f, pixelHeight, 0.0f);

            var originHeight = Camera.main.ScreenToWorldPoint(targetScreenPoint);

            for (int i = 0; i < LaneInfo.MaximumLane; i++) {
                originInEachLanes[i].x = LaneInfo.LanePositions[i].x;
                originInEachLanes[i].y = originHeight.y;
                originInEachLanes[i].z = 0.0f;
            }
        }
    }
}
