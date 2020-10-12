using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTile : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public List<GameObject> obstacles;

    public void GetRandomObstacle()
    {
        DisableAllObstacles();

        int randomObstacleIndex = Random.Range(0, obstacles.Count);
        obstacles[randomObstacleIndex].SetActive(true);
    }

    public void DisableAllObstacles()
    {
        foreach (var item in obstacles)
        {
            item.SetActive(false);
        }
    }
}
