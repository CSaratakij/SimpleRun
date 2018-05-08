using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class ObstacleSpawner : MonoBehaviour
    {
        const char EMPTY_SPACE_MARK = '0';
        const char OBSTACLE_MARK = '1';


        [SerializeField]
        GameObject[] blueprints;

        [SerializeField]
        [Range(1, 10)]
        int maximumPooling;


        byte[] obstaclePattern;
        string[] strObstaclePattern;

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
            _CreateObstaclePattern(LaneInfo.MaximumLane);
            _FireAvailableObstacle();
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

        void _CreateObstaclePattern(int maxLane)
        {
            obstaclePattern = new byte[maxLane * 2];

            for (int i = 0; i < obstaclePattern.Length; i++) {
                if ((i + 1) <= maxLane) {
                    obstaclePattern[i] = (byte)(1 << i);
                }
                else {
                    obstaclePattern[i] = (byte)(((~obstaclePattern[i - maxLane]) & 0xf));
                    obstaclePattern[i] = (byte)(obstaclePattern[i] << (8 - maxLane));
                    obstaclePattern[i] = (byte)(obstaclePattern[i] >> (8 - maxLane));
                }
            }

            strObstaclePattern = _ObstaclePatternsToString();
        }

        void _FireAvailableObstacle()
        {
            StartCoroutine(_FireAvailableObstacle_Callback());
        }

        IEnumerator _FireAvailableObstacle_Callback()
        {
            while (true) {
                yield return new WaitForSeconds(0.8f);

                //Pick random pattern.
                int pickPatternIndex = (int)UnityEngine.Random.Range(0, obstaclePattern.Length - 1);

                //Map it.
                string pickPatternMap = strObstaclePattern[pickPatternIndex];

                for (int i = 0; i < pickPatternMap.Length; i++) {
                    if (pickPatternMap[i] == OBSTACLE_MARK) {

                        //random avaliable obstacle in pattern
                        int pickObstacleIndex = (int)UnityEngine.Random.Range(0, 3);

                        switch ((BlueprintType)pickObstacleIndex) {

                            case BlueprintType.Coin:
                                foreach (GameObject obj in coinPool) {

                                    if (!obj.activeSelf) {
                                        obj.transform.position = originInEachLanes[i];
                                        obj.SetActive(true);
                                        break;
                                    }
                                }
                                break;

                            case BlueprintType.Wall:
                                foreach (GameObject obj in wallPool) {

                                    if (!obj.activeSelf) {
                                        obj.transform.position = originInEachLanes[i];
                                        obj.SetActive(true);
                                        break;
                                    }
                                }
                                break;

                            case BlueprintType.Spike:
                                foreach (GameObject obj in spikePool) {

                                    if (!obj.activeSelf) {
                                        obj.transform.position = originInEachLanes[i];
                                        obj.SetActive(true);
                                        break;
                                    }
                                }

                                break;
                        }
                    }
                    else {
                        continue;
                    }
                }
            }
        }

        string[] _ObstaclePatternsToString()
        {
            string[] result = new string[obstaclePattern.Length];

            for (int i = 0; i < result.Length; i++) {
                string pattern = Convert.ToString(obstaclePattern[i], 2).PadLeft(LaneInfo.MaximumLane, '0');
                result[i] = pattern;
            }

            return result;
        }
    }
}
