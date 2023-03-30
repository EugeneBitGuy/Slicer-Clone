using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnAllowMovement;
    public event Action OnForbidMovement;

    private bool _isSlicing;
    private bool _knifeReachEnd;
    private bool _knifeIsMoving;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    //object can move only if slice event finished (when object reached end) and when user stops to hold button

    private void Update()
    {
        if (_isSlicing && _knifeReachEnd && !_knifeIsMoving)
        {
            _isSlicing = false;
            _knifeReachEnd = false;
            
            AllowSliceableMovement();
        }
    }

    public void Slice(GameObject gameObject)
    {
        ForbidSliceableMovement();
        _isSlicing = true;
    }

    private void AllowSliceableMovement()
    {
        OnAllowMovement?.Invoke();
    }
    
    private void ForbidSliceableMovement()
    {
        OnForbidMovement?.Invoke();
    }

    public void SetKnifeReachEnd(bool hasReachedEnd)
    {
        _knifeReachEnd = hasReachedEnd;
    }

    public void SetKnifeIsMoving(bool isMoving)
    {
        _knifeIsMoving = isMoving;
    }
}
