using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHolder : MonoBehaviour
{
    public List<Transform> tSpawnPoints = new List<Transform>();
    public List<Transform> tUsedPoints = new List<Transform>();

    public Transform GetSpawnPoint()
    {
        int iIdx = Random.Range(0, tSpawnPoints.Count - 1);
        Transform ret = tSpawnPoints[iIdx];
        tSpawnPoints.RemoveAt(iIdx);
        tUsedPoints.Add(ret);

        return ret;
    }

    public void Reset()
    {
        for(int i = 0; i < tUsedPoints.Count; ++i)
        {
            tSpawnPoints.Add(tUsedPoints[i]);
            tUsedPoints.RemoveAt(i);
        }
    }
}
