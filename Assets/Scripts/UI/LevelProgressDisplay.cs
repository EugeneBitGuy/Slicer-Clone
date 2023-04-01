using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplay : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform rootPosition;

    private void Start()
    {
        _slider.minValue = -startPosition.position.z;
        _slider.maxValue = -endPosition.position.z;
    }

    private void Update()
    {
        _slider.value = -rootPosition.position.z;
    }
}
