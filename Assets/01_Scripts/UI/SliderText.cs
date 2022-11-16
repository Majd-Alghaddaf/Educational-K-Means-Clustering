using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderText : MonoBehaviour
{
    private TextMeshProUGUI _sliderValueText;

    void Awake()
    {
        _sliderValueText = GetComponent<TextMeshProUGUI>();
    }

    public void OnSliderValueChanged(float sliderValue)
    {
        _sliderValueText.text = sliderValue.ToString();
    }
}
