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
    private float offset;        // for offsetting beginning of paths so all coins are equidistant

    internal class Path {
        internal Vector3 direction;
        internal float angle;
        internal float distance;

        internal Path(Vector3 direction, float angle, float distance) {
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

        // create new empty GameObject for easier exporting of coins
        parent = new GameObject("Coins");

        offset = 0f;

        // skip first transform b/c it's the parent transform
        for (int index = 1; index < cornerPoints.Length - 1; index++) {
            Vector3 start = cornerPoints[index].position;
            Vector3 end = cornerPoints[index + 1].position;

            Vector3 direction = (end - start).normalized;
            float angle = 360 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float pathDistance = Vector3.Distance(start, end);

            totalLinearDistance += pathDistance;       // for route-planning purposes

            Vector3 startPosition = start + offset * direction;

            GenerateCoinsPerSegment(new Path(direction, angle, pathDistance), startPosition);
        }

        // for route planning purposes
        Debug.Log("Distance: " + totalLinearDistance + ", Number of coins: " + parent.transform.childCount);
        double seconds = totalLinearDistance / 3.5;
        int min = (int) (seconds / 60);
        Debug.Log("Min time to completion: " + min + " min " + (int) (seconds - min * 60) + " sec");
    }

    private void GenerateCoinsPerSegment(Path path, Vector3 startPosition) {
        if (offset > path.distance) {
            offset -= path.distance;
        }
        else {
            float distance = offset - distanceBetweenCoins;     // -dist between coins to prevent off by one error (after dropping first coin, distance should = 0)

            int i = 0;
            while (true) {
                    Vector3 position = startPosition + i * distanceBetweenCoins * path.direction;
                    GameObject coin = Instantiate(coinPrefab, position, Quaternion.Euler(0, path.angle, 0));
                    coin.transform.SetParent(parent.transform);
                    coin.name = "Coin";

                    distance += distanceBetweenCoins;
                    i++;

                    if (distance + distanceBetweenCoins >= path.distance) {
                        break;
                    }
            }

            float remainder = path.distance - distance;
            offset = distanceBetweenCoins - remainder;
        }
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
}
