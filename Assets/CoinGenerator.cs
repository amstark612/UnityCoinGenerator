using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thank you @tran0563 for his help!!!

public class CoinGenerator : MonoBehaviour {
    public GameObject coinPrefab;
    [SerializeField]
    public float distanceBetweenCoins = 4.0f;

    Transform[] cornerPoints;

    void Awake() {
        // cornerPoints = GetComponentsInChildren<Transform>();
    }

    public void GenerateCoins() {
        // for getting total linear distance
        float totalLinearDistance = 0f;

        // get positions of all children as array
        cornerPoints = GetComponentsInChildren<Transform>();

        // create new empty GameObject for easier exporting of coins
        GameObject parent = new GameObject("Coins");

        // skip first transform b/c it's the parent transform
        for (int index = 1; index < cornerPoints.Length - 1; index++) {
            Transform start = cornerPoints[index];
            Transform end = cornerPoints[index + 1];

            Vector3 direction = (end.position - start.position).normalized;
            float angle = 360 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float totalDistance = Vector3.Distance(start.position, end.position);
            totalLinearDistance += totalDistance;       // for route-planning purposes
            int numCoins = (int)(totalDistance / distanceBetweenCoins) + 1;

            Transform[] coinPositions = new Transform[numCoins];

            for (int i = 0; i < numCoins; i++) {
                Vector3 position = start.position + i * distanceBetweenCoins * direction;
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.Euler(0, angle, 0));
                coin.transform.SetParent(parent.transform);
            }
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
}
