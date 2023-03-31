using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SliceableObjectMovement : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private UnityEvent onObjectReachFinishEvent = null;
    
    private bool _canMove;

    private void OnEnable()
    {
        GameManager.Instance.OnAllowMovement += StartMovement;
        GameManager.Instance.OnForbidMovement += StopMovement;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllowMovement -= StartMovement;
        GameManager.Instance.OnForbidMovement -= StopMovement;
    }
    
    private void Start()
    {
        transform.position = startPoint.position;
        _canMove = true;
    }

    private void Update()
    {
        if(_canMove)
            ProcessMovement();
    }

    private void StartMovement() => _canMove = true;
    private void StopMovement() => _canMove = false;

    private void ProcessMovement()
    {
        transform.position = Vector3.MoveTowards
        (
            transform.position,
            endPoint.position,
            Time.deltaTime * movementSpeed
        );

        if (transform.position == endPoint.position)
        {
            _canMove = false;
            onObjectReachFinishEvent?.Invoke();
        }
    }
}
