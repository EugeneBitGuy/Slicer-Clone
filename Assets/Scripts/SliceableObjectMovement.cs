using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceableObjectMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float movementSpeed = 5f;
    
    private void Start()
    {
        transform.position = startPoint.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards
        (
            transform.position, 
            endPoint.position,
            Time.deltaTime * movementSpeed
        );
    }
}
