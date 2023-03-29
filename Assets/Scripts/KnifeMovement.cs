using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeMovement : MonoBehaviour
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;

    [SerializeField] private InputAction actions;

    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float movementSpeed = 2f;

    private void Start()
    {
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    private void Update()
    {
        if (actions.inProgress)
            MoveKnifeDown();
        else
            MoveKnifeUp();
    }

    private void MoveKnifeUp()
    {
        transform.position = Vector3.Lerp(transform.position, startTransform.position, movementSpeed * Time.deltaTime);

        transform.rotation =
            Quaternion.Lerp(transform.rotation, startTransform.rotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveKnifeDown()
    {
        transform.position = Vector3.Lerp(transform.position, endTransform.position, movementSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Lerp(transform.rotation, endTransform.rotation, rotationSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}
