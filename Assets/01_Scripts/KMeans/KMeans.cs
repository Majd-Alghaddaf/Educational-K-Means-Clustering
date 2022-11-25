using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Linq;

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
    [SerializeField][Range(0,25)] int maxDistanceFromDataPointToCentroid;

    [Space(30f)]
    [Header("K Means Algorithm Variables")]
    [Space(15f)]
    [SerializeField][Range(2,10)] int k;

    [Space(30f)]
    [Header("Variable References")]
    [Space(15f)]
    [SerializeField] TextMeshProUGUI sseValueText;

    private List<GameObject> centroids = new List<GameObject>();
    private List<GameObject> dataPoints = new List<GameObject>();
    private float _sse = 0;

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
        KMeansPlusPlus();
        UpdateSSEValueText();
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
            int numOfPointsAroundCentroid = minNumOfPointsAroundCentroid >= maxNumOfPointsAroundCentroid ? minNumOfPointsAroundCentroid : Random.Range(minNumOfPointsAroundCentroid, maxNumOfPointsAroundCentroid);

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

    private void KMeansPlusPlus()
    {
        Dictionary<GameObject, GameObject> dataPointToNearestChosenCentroid = new Dictionary<GameObject, GameObject>();
        foreach(GameObject dataPoint in dataPoints)
        {
            dataPointToNearestChosenCentroid.Add(dataPoint, centroids[0]);
        }

        for (int i = 0; i < k; i++)
        {
            if(i == 0) // if first centroid, choose random datapoint
            {
                int randomIndex = Random.Range(0, dataPointToNearestChosenCentroid.Count);
                centroids[i].transform.position = dataPointToNearestChosenCentroid.ElementAt(randomIndex).Key.transform.position;
                dataPointToNearestChosenCentroid.Remove(dataPointToNearestChosenCentroid.ElementAt(randomIndex).Key);
            }
            else
            {
                float totalSqrMagnitude = 0;
                for (int j = 0; j < dataPointToNearestChosenCentroid.Count; j++)
                {
                    // initializing
                    GameObject closestChosenCentroidToDataPoint = centroids[0];
                    GameObject dataPointCopy = dataPointToNearestChosenCentroid.ElementAt(j).Key;
                    float shortestSqrMagnitude = (centroids[0].transform.position - dataPointCopy.transform.position).sqrMagnitude;

                    for (int l = 1; l < i; l++) // uniquely looping through the already chosen centroids
                    {
                        float sqrMagnitude = (centroids[l].transform.position - dataPointCopy.transform.position).sqrMagnitude;

                        if (sqrMagnitude < shortestSqrMagnitude)
                        {
                            shortestSqrMagnitude = sqrMagnitude;
                            closestChosenCentroidToDataPoint = centroids[l];
                        }
                    }

                    totalSqrMagnitude += shortestSqrMagnitude;
                    dataPointToNearestChosenCentroid[dataPointCopy] = closestChosenCentroidToDataPoint;
                }
                // at this point, every key value pair should have a datapoint and its nearest chosen centroid
                // we continue by applying a weighted probability distribution
                float randomNumber = Random.Range(0, totalSqrMagnitude);

                GameObject selectedDataPoint = dataPointToNearestChosenCentroid.ElementAt(0).Key;
                for (int j = 0; j < dataPointToNearestChosenCentroid.Count; j++)
                {
                    float weight = (dataPointToNearestChosenCentroid.ElementAt(j).Value.transform.position - dataPointToNearestChosenCentroid.ElementAt(j).Key.transform.position).sqrMagnitude;
                    if (randomNumber < weight)
                    {
                        selectedDataPoint = dataPointToNearestChosenCentroid.ElementAt(j).Key;
                        break;
                    }

                    randomNumber = randomNumber - weight;
                }

                centroids[i].transform.position = selectedDataPoint.transform.position;
                dataPointToNearestChosenCentroid.Remove(selectedDataPoint);
            }
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
        CalculateSSE();
        UpdateSSEValueText();
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

            if (float.IsNaN(newCentroidPosition.x) || float.IsNaN(newCentroidPosition.y) || float.IsNaN(newCentroidPosition.z))
                continue;

            centroid.transform.DOMove(newCentroidPosition, timeSpentOnCentroidTweening).SetEase(centroidTweeningEaseType);
        }
    }

    private void CalculateSSE()
    {
        _sse = 0;
        foreach(GameObject centroid in centroids)
        {
            Centroid centroidComponent = centroid.GetComponent<Centroid>();
            foreach(GameObject dataPoint in centroidComponent.GetAssignedDataPoints())
            {
                _sse += (centroid.transform.position - dataPoint.transform.position).sqrMagnitude;
            }
        }
    }

    private void UpdateSSEValueText()
    {
        sseValueText.text = _sse == 0 ? "" : _sse.ToString();

    }
    #endregion

    #region Config Settings Setters
    public void SetKValue(int value) => k = value;
    public void SetBoundingBoxSizeX(int value) => boundingBoxDimensions = new Vector3(value, boundingBoxDimensions.y, boundingBoxDimensions.z);
    public void SetBoundingBoxSizeY(int value) => boundingBoxDimensions = new Vector3(boundingBoxDimensions.x, value, boundingBoxDimensions.z);
    public void SetBoundingBoxSizeZ(int value) => boundingBoxDimensions = new Vector3(boundingBoxDimensions.x, boundingBoxDimensions.y, value);
    public void SetMinNumOfPointsAroundCentroid(int value) => minNumOfPointsAroundCentroid = value;
    public void SetMaxNumOfPointsAroundCentroid(int value) => maxNumOfPointsAroundCentroid = value;
    public void SetMaxDistanceFromDataPointToCentroid(int value) => maxDistanceFromDataPointToCentroid = value;
    #endregion
}
