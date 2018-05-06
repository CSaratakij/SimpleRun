using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleRun
{
    public class LaneInfo
    {
        static int currentMaximumLane;
        static int currentLaneIndex;

        static Vector3[] lanePositions;


        public static void SetUpLane(int value = 1)
        {
            currentMaximumLane = value;
            lanePositions = new Vector3[currentMaximumLane];

            var camera = Camera.main;

            var pixelWidth = camera.pixelWidth;
            var pixelHeight = camera.pixelHeight;

            var unitWidth = (int)(pixelWidth / currentMaximumLane);
            var centerOfUnitWidth = (int)(unitWidth / 2);

            for (int i = 0; i < lanePositions.Length; i++) {
                var targetScreenPoint = new Vector3((unitWidth * (i + 1)) - centerOfUnitWidth, 0.0f, 0.0f);
                lanePositions[i] = camera.ScreenToWorldPoint(targetScreenPoint);
                lanePositions[i].z = 0.0f;
            }
        }

        public static void GoToCenterLane()
        {
            currentLaneIndex = (int)(lanePositions.Length / currentMaximumLane);
        }

        public static Vector3 GetCurrentLanePosition()
        {
            return lanePositions[currentLaneIndex];
        }

        public static Vector3 GetNextLanePosition()
        {
            if ((currentLaneIndex + 1) < currentMaximumLane) {
                currentLaneIndex += 1;
            }

            return GetCurrentLanePosition();
        }

        public static Vector3 GetPreviousLanePosition()
        {
            if ((currentLaneIndex - 1) >= 0) {
                currentLaneIndex -= 1;
            }

            return GetCurrentLanePosition();
        }
    }
}
