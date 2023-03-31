using System;
using System.Collections;
using System.Collections.Generic;
using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Samples;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnAllowMovement;
    public event Action OnForbidMovement;

    public bool IsSlicing => _isSlicing;

    private bool _isSlicing;
    private bool _knifeReachEnd;
    private bool _knifeIsMoving;

    private List<DeadSliceController> outSlicesObjects;

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

    private void Start()
    {
        outSlicesObjects = new List<DeadSliceController>();
    }

    //object can move only if slice event finished (when object reached end) and when user stops to hold button

    private void Update()
    {
        if (_isSlicing && _knifeReachEnd)
        {
            foreach (var slice in outSlicesObjects)
            {
                if(slice == null) continue;
                
                slice.Throw();
            }

            if (!_knifeIsMoving)
            {
                AllowSliceableMovement();
                _isSlicing = false;
                _knifeReachEnd = false;
                
                outSlicesObjects.Clear();
            }
        }
    }

    public void Slice(GameObject gameObject, Transform slicer)
    {
        ForbidSliceableMovement();

        _isSlicing = true;

        var sliceableObjects = gameObject.GetComponentsInChildren<ObjectSlicerSample>();

        Plane plane = new Plane(slicer.up, slicer.position);

        foreach (var sliceable in sliceableObjects)
        {
            if (sliceable == null) continue;

            sliceable.Slice(plane, res =>
            {
                if(res.sliced)
                {

                    var slice = res.outObjectPos;

                    outSlicesObjects.Add(slice.AddComponent<DeadSliceController>());
                }
                else
                {
                    bool side = plane.GetSide(sliceable.GetComponent<Collider>().bounds.center);

                    if (side)
                    {
                        outSlicesObjects.Add(sliceable.gameObject.AddComponent<DeadSliceController>());
                    }
                }
                
            });

        }
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
