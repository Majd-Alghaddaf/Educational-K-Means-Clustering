using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KMeans : MonoBehaviour
{
    [Header("Clusters Configuration")]
    [Space(15f)]
    [SerializeField] private Vector3 boundingBoxDimensions;
    [SerializeField] private List<Color> clusterColorsList = new List<Color>();

    [Header("Centroid Generation")]
    [SerializeField] GameObject centroidPrefab;
    [SerializeField] GameObject centroidsContainer;
    [SerializeField] float timeSpentOnCentroidTweening;
    [SerializeField] Ease centroidTweeningEaseType;

    [Header("Data Points Generation")]
    [SerializeField] GameObject dataPointPrefab;
    [SerializeField] GameObject dataPointsContainer;
    [SerializeField][Range(0,100)] int minNumOfPointsAroundCentroid;
    [SerializeField][Range(0,100)] int maxNumOfPointsAroundCentroid;
    [SerializeField][Range(0f,25f)] float maxDistanceFromDataPointToCentroid;

    [Space(30f)]
    [Header("K Means Algorithm Variables")]
    [Space(15f)]
    [SerializeField][Range(2,10)] int k;

    private List<GameObject> centroids = new List<GameObject>();
    private List<GameObject> dataPoints = new List<GameObject>();

    #region Singleton
    private static KMeans _instance;
    public static KMeans Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private void Start()
    {
        GenerateClusters();
    }

    #region Clusters Generation
    public void GenerateClusters()
    {
        GenerateCentroids();
        GenerateDataPoints();
        RandomizeCentroidsPositions();
    }

    private void GenerateCentroids()
    {
        ClearCentroids();

        for (int i = 0; i < k; i++)
        {
            float x = Random.Range(-boundingBoxDimensions.x / 2, boundingBoxDimensions.x / 2);
            float y = Random.Range(-boundingBoxDimensions.y / 2, boundingBoxDimensions.y / 2);
            float z = Random.Range(-boundingBoxDimensions.z / 2, boundingBoxDimensions.z / 2);

            GameObject newCentroid = Instantiate(centroidPrefab, new Vector3(x, y, z), Quaternion.identity, centroidsContainer.transform);
            newCentroid.GetComponent<Centroid>().clusterColor = clusterColorsList[i]; 
            centroids.Add(newCentroid);
        }
    }

    private void ClearCentroids()
    {
        foreach(GameObject centroid in centroids)
        {
            Destroy(centroid);
        }
        centroids.Clear();
    }

    private void GenerateDataPoints()
    {
        ClearDataPoints();

        foreach (GameObject centroid in centroids)
        {
            int numOfPointsAroundCentroid = Random.Range(minNumOfPointsAroundCentroid, maxNumOfPointsAroundCentroid);

            for (int i = 0; i < numOfPointsAroundCentroid; i++)
            {
                float x = Random.Range(centroid.transform.position.x - maxDistanceFromDataPointToCentroid, centroid.transform.position.x + maxDistanceFromDataPointToCentroid);
                float y = Random.Range(centroid.transform.position.y - maxDistanceFromDataPointToCentroid, centroid.transform.position.y + maxDistanceFromDataPointToCentroid);
                float z = Random.Range(centroid.transform.position.z - maxDistanceFromDataPointToCentroid, centroid.transform.position.z + maxDistanceFromDataPointToCentroid);

                GameObject newDataPoint = Instantiate(dataPointPrefab, new Vector3(x, y, z), Quaternion.identity, dataPointsContainer.transform);
                dataPoints.Add(newDataPoint);

                Centroid centroidComponent = centroid.GetComponent<Centroid>();
                centroidComponent.AddAssignedDataPoint(newDataPoint);

                DataPoint dataPointComponent = newDataPoint.GetComponent<DataPoint>();
                dataPointComponent.SetColor(centroidComponent.clusterColor);
            }
        }
    }

    private void ClearDataPoints()
    {
        foreach (GameObject dataPoint in dataPoints)
        {
            Destroy(dataPoint);
        }
        dataPoints.Clear();
    }

    private void RandomizeCentroidsPositions()
    {
        foreach(GameObject centroid in centroids)
        {
            float x = Random.Range(-boundingBoxDimensions.x / 2, boundingBoxDimensions.x / 2);
            float y = Random.Range(-boundingBoxDimensions.y / 2, boundingBoxDimensions.y / 2);
            float z = Random.Range(-boundingBoxDimensions.z / 2, boundingBoxDimensions.z / 2);

            centroid.transform.position = new Vector3(x, y, z);
        }
    }
    #endregion

    #region Algorithm
    public void Step()
    {
        ClearAssignedDataPointsForCentroids();

        foreach(GameObject dataPoint in dataPoints)
        {
            GameObject closestCentroidToDataPoint = centroids[0];
            float shortestSqrMagnitude = (centroids[0].transform.position - dataPoint.transform.position).sqrMagnitude;

            for (int i = 1; i < centroids.Count; i++)
            {
                float sqrMagnitude = (centroids[i].transform.position - dataPoint.transform.position).sqrMagnitude;

                if(sqrMagnitude < shortestSqrMagnitude)
                {
                    shortestSqrMagnitude = sqrMagnitude;
                    closestCentroidToDataPoint = centroids[i];
                }
            }

            Centroid closestCentroidComponent = closestCentroidToDataPoint.GetComponent<Centroid>();
            closestCentroidComponent.AddAssignedDataPoint(dataPoint);

            DataPoint dataPointComponent = dataPoint.GetComponent<DataPoint>();
            dataPointComponent.SetColor(closestCentroidComponent.clusterColor);
            dataPointComponent.UpdateColor();
        }

        UpdateCentroidsPositions();
    }

    private void ClearAssignedDataPointsForCentroids()
    {
        foreach(GameObject centroid in centroids)
        {
            centroid.GetComponent<Centroid>().ClearAssignedDataPoints();
        }
    }

    private void UpdateCentroidsPositions()
    {
        foreach(GameObject centroid in centroids)
        {
            Centroid centroidComponent = centroid.GetComponent<Centroid>();
            Vector3 newCentroidPosition = centroidComponent.CalculateCenterOfCluster();

            centroid.transform.DOMove(newCentroidPosition, timeSpentOnCentroidTweening).SetEase(centroidTweeningEaseType);
        }
    }
    #endregion

    #region Config Settings Setters
    public void SetKValue(int value) => k = value;
    public void SetBoundingBoxSizeX(int value) => boundingBoxDimensions = new Vector3(value, boundingBoxDimensions.y, boundingBoxDimensions.z);
    public void SetBoundingBoxSizeY(int value) => boundingBoxDimensions = new Vector3(boundingBoxDimensions.x, value, boundingBoxDimensions.z);
    public void SetBoundingBoxSizeZ(int value) => boundingBoxDimensions = new Vector3(boundingBoxDimensions.x, boundingBoxDimensions.y, value);
    public void SetMinNumOfPointsAroundCentroid(int value) => minNumOfPointsAroundCentroid = value;
    public void SetMaxNumOfPointsAroundCentroid(int value) => maxNumOfPointsAroundCentroid = value;
    public void SetMaxDistanceFromDataPointToCentroid(float value) => maxDistanceFromDataPointToCentroid = value;
    #endregion
}
