using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public void OnToggle(bool value)
    {
        KMeans.Instance.SetKMeansPlusPlus(value);
    }
}
