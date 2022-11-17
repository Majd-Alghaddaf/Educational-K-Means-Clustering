using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour
{
    public Color clusterColor;

    private List<GameObject> _assignedDataPoints = new List<GameObject>();

    private void Start()
    {
        GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", clusterColor);
    }

    public void ClearAssignedDataPoints()
    {
        _assignedDataPoints.Clear();
    }

    public void AddAssignedDataPoint(GameObject newlyAssignedDataPoint)
    {
        _assignedDataPoints.Add(newlyAssignedDataPoint);
    }

    public List<GameObject> GetAssignedDataPoints()
    {
        return _assignedDataPoints;
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
