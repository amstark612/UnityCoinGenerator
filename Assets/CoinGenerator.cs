using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thank you @tran0563 for his help!!!

public class CoinGenerator : MonoBehaviour {
    public GameObject coinPrefab;
    [SerializeField]
    public float distanceBetweenCoins = 4.0f;
    
    Transform start, end;
    Transform[] positions;

    void Awake() {
        start = transform.Find("Start");
        end = transform.Find("End");
    }

    public void GenerateCoins() {
        Vector3 direction = (end.position - start.position).normalized;
        float angle = 360 - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        float totalDistance = Vector3.Distance(start.position, end.position);
        int numCoins = (int)(totalDistance / distanceBetweenCoins) + 1;

        positions = new Transform[numCoins];

        for (int i = 0; i < numCoins; i++) {
            Vector3 position = start.position + i * distanceBetweenCoins * direction;
            Instantiate(coinPrefab, position, Quaternion.Euler(0, angle, 0));
        }
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
            start = transform.Find("Start");
            end = transform.Find("End");
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start.position, end.position);
        #endif
    }
}
