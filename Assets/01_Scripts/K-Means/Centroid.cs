using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour
{
    public Color clusterColor;

    private List<GameObject> _assignedDataPoints = new List<GameObject>();

    private void Start()
    {
        GetComponentInChildren<Renderer>().material.color = clusterColor;
    }

    public void GenerateAndSetNewRandomColor()
    {
        clusterColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void ClearAssignedDataPoints()
    {
        _assignedDataPoints.Clear();
    }

    public void AddAssignedDataPoint(GameObject newlyAssignedDataPoint)
    {
        _assignedDataPoints.Add(newlyAssignedDataPoint);
    }

    public Vector3 CalculateCenterOfCluster()
    {
        Vector3 centerOfClusterPosition = Vector3.zero;

        foreach(GameObject dataPoint in _assignedDataPoints)
        {
            centerOfClusterPosition += dataPoint.transform.position;
        }

        centerOfClusterPosition /= _assignedDataPoints.Count;

        return centerOfClusterPosition;
    }
}
