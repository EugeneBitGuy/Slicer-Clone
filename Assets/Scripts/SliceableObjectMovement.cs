using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceableObjectMovement : MonoBehaviour
{
    private const float TwoPie = Mathf.PI * 2f;
    private const float MovementFactorDivisor = 2f;
    private const float MovementFactorOffset = 1f;
    
    
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float movementPeriod = 5f;

    private float _startTime;

    private void Start()
    {
        transform.position = startPoint.position;
        _startTime = Time.time;
    }

    private void Update()
    {
        if(movementPeriod <= Mathf.Epsilon)
            return;
        
        float elapsedTime = Time.time - _startTime;

        float numberOfFullCycles = elapsedTime / movementPeriod;

        float sinValue = Mathf.Sin(numberOfFullCycles * TwoPie);

        float movementFactor = (sinValue + MovementFactorOffset) / MovementFactorDivisor;

        Vector3 offset = (endPoint.position - startPoint.position) * movementFactor;

        transform.position = startPoint.position + offset;
    }
}
