using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeMovement : MonoBehaviour
{
    private const float UpMovementSpeedFactor = 10f;
    
    
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;

    [SerializeField] private InputAction actions;

    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float movementSpeed = 2f;
    
    private void Start()
    {
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }
    
    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }

    private void Update()
    {
        ProcessMovement(actions.inProgress);
    }
    

    void ProcessMovement(bool isButtonPressed)
    {
        var targetPosition = isButtonPressed ? endTransform.position : startTransform.position;
        var targetRotation = isButtonPressed ? endTransform.rotation : startTransform.rotation;
        var targetMovementSpeed = isButtonPressed ? movementSpeed : UpMovementSpeedFactor * movementSpeed;
        var targetRotationSpeed = isButtonPressed ? rotationSpeed : UpMovementSpeedFactor * rotationSpeed;


        transform.position =
            Vector3.MoveTowards(transform.position, targetPosition, targetMovementSpeed * Time.deltaTime);

        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, targetRotationSpeed * Time.deltaTime);

        if(transform.position == endTransform.position)
            GameManager.Instance.SetKnifeReachEnd(true);

        GameManager.Instance.SetKnifeIsMoving(isButtonPressed);
    }

}
