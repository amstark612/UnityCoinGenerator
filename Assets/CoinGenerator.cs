using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thank you @tran0563 for his help!!!

public class CoinGenerator : MonoBehaviour {
    public GameObject coinPrefab;
    [SerializeField]
    public float distanceBetweenCoins = 4.0f;

    private GameObject parent;
    Transform[] cornerPoints;

    internal class Path {
        internal Vector3 start, direction;
        internal float angle;
        internal float distance;

        internal Path(Vector3 start, Vector3 direction, float angle, float distance) {
            this.start = start;
            this.direction = direction;
            this.angle = angle;
            this.distance = distance;
        }
    }

    public void GenerateCoins() {
        // for getting total linear distance
        float totalLinearDistance = 0f;

        // get positions of all children as array
        cornerPoints = GetComponentsInChildren<Transform>();

        // for storing path directions & angles
        Path[] paths = new Path[cornerPoints.Length - 2];

        // for route planning purposes
        List<float> distances = new List<float>();

        // create new empty GameObject for easier exporting of coins
        parent = new GameObject("Coins");

        // skip first transform b/c it's the parent transform
        for (int index = 1; index < cornerPoints.Length - 1; index++) {
            Transform start = cornerPoints[index];
            Transform end = cornerPoints[index + 1];

            Vector3 direction = (end.position - start.position).normalized;
            float angle = 360 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float distance = Vector3.Distance(start.position, end.position);

            paths[index - 1] = new Path(start.position, direction, angle, distance);

            totalLinearDistance += distance;       // for route-planning purposes


            // int numCoins = (int)(distance / distanceBetweenCoins) + 1;

            // Transform[] coinPositions = new Transform[numCoins];

            // for (int i = 0; i < numCoins; i++) {
            //     Vector3 position = start.position + i * distanceBetweenCoins * direction;
            //     GameObject coin = Instantiate(coinPrefab, position, Quaternion.Euler(0, angle, 0));
            //     coin.transform.SetParent(parent.transform);
            // }
        }

        int totalNumCoins = (int) (totalLinearDistance / distanceBetweenCoins) + 1;
        float ratio = totalNumCoins / totalLinearDistance;

        foreach (Path path in paths) {
            GenerateSegmentCoins(ratio, path);
        }

        // for route planning purposes
        Debug.Log("Distance: " + totalLinearDistance + ", Number of coins: " + parent.transform.childCount);
        double seconds = totalLinearDistance / 2.5;
        int min = (int) (seconds / 60);
        Debug.Log("Min time to completion: " + min + " min " + (int) (seconds - min * 60) + " sec");
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
            Gizmos.color = Color.yellow;

            // get positions of all children as array
            cornerPoints = GetComponentsInChildren<Transform>();

            // skip first transform b/c it's the parent transform
            for (int index = 1; index < cornerPoints.Length - 1; index++) {
                Gizmos.DrawLine(cornerPoints[index].position, cornerPoints[index + 1].position);
            }
        #endif
    }

    private void GenerateSegmentCoins(float ratio, Path path) {
        // number of coins for this segment in proportion to segment distance vs total distance
        int numCoins = (int) System.Math.Round(path.distance * ratio);

        float distBetweenCoins = path.distance / numCoins;

        Debug.Log(distBetweenCoins);

        for (int i = 0; i < numCoins; i++) {
            Vector3 position = path.start + i * distBetweenCoins * path.direction;
            GameObject coin = Instantiate(coinPrefab, position, Quaternion.Euler(0, path.angle, 0));
            coin.transform.SetParent(parent.transform);
        }
    }
}
