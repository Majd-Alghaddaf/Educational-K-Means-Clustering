using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum KMeansConfigSettingsValue
{
    K,
    BoundingBoxSizeX,
    BoundingBoxSizeY,
    BoundingBoxSizeZ,
    MinNumPointsAroundCentroid,
    MaxNumPointsAroundCentroid,
    MaxDistanceFromDatapointToCentroid
}

public class SliderText : MonoBehaviour
{
    [SerializeField] KMeansConfigSettingsValue correspondingSettingsValue;

    private TextMeshProUGUI _sliderValueText;

    void Awake()
    {
        _sliderValueText = GetComponent<TextMeshProUGUI>();
    }

    public void OnSliderValueChanged(float sliderValue)
    {
        _sliderValueText.text = sliderValue.ToString();

        SetCorrespondingKMeansValue(sliderValue);
    }

    private void SetCorrespondingKMeansValue(float sliderValue)
    {
        switch (correspondingSettingsValue)
        {
            case KMeansConfigSettingsValue.K:
                KMeans.Instance.SetKValue((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.BoundingBoxSizeX:
                KMeans.Instance.SetBoundingBoxSizeX((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.BoundingBoxSizeY:
                KMeans.Instance.SetBoundingBoxSizeY((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.BoundingBoxSizeZ:
                KMeans.Instance.SetBoundingBoxSizeZ((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.MinNumPointsAroundCentroid:
                KMeans.Instance.SetMinNumOfPointsAroundCentroid((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.MaxNumPointsAroundCentroid:
                KMeans.Instance.SetMaxNumOfPointsAroundCentroid((int)sliderValue);
                break;
            case KMeansConfigSettingsValue.MaxDistanceFromDatapointToCentroid:
                KMeans.Instance.SetMaxDistanceFromDataPointToCentroid((int)sliderValue);
                break;
            default:
                Debug.LogError("Forgot to add approriate enum for the settings configuration?");
                break;
        }
    }
}
