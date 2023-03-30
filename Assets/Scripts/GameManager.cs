using System;
using System.Collections;
using System.Collections.Generic;
using EzySlice;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsSlicing => _isSlicing;
 
    public event Action OnAllowMovement;
    public event Action OnForbidMovement;

    private bool _isSlicing;
    private bool _knifeReachEnd;
    private bool _knifeIsMoving;

    private List<Rigidbody> upperHullsInCurrentSlice;
    private List<GameObject> lowerHullsInCurrentSlice;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        upperHullsInCurrentSlice = new List<Rigidbody>();
        lowerHullsInCurrentSlice = new List<GameObject>();
    }

    private void Update()
    {
        if (_isSlicing && _knifeReachEnd && !_knifeIsMoving)
        {
            _isSlicing = false;
            _knifeReachEnd = false;

            foreach (var rb in upperHullsInCurrentSlice)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddForce(Vector3.back * 100f);
                Destroy(rb.gameObject, 1f);
            }

            foreach (var lowerHull in lowerHullsInCurrentSlice)
            {
                lowerHull.tag = "Sliceable";
            }
            
            upperHullsInCurrentSlice.Clear();
            lowerHullsInCurrentSlice.Clear();
            
            AllowSliceableMovement();
        }
    }

    public void Slice(GameObject sliceRoot, Transform slicer)
    {
        ForbidSliceableMovement();
        _isSlicing = true;

        Transform parentTransform = sliceRoot.transform;

        foreach (Transform childTransform in parentTransform)
        {
            GameObject toSlice = childTransform.gameObject;
            
            if(!toSlice.CompareTag("Sliceable")) continue;
            
            var material = toSlice.GetComponent<MeshRenderer>().material;

            SlicedHull hull = toSlice.Slice(slicer.position, slicer.up, material);

            if (hull != null)
            {
    
                GameObject upperHull = hull.CreateUpperHull(toSlice, material);

                if (upperHull != null)
                {
                    
                    upperHull.transform.position = toSlice.transform.position;

                    var upperHullCollider = upperHull.AddComponent<BoxCollider>();
                    upperHullCollider.isTrigger = true;
                    
                    var upperHullRigidbody = upperHull.AddComponent<Rigidbody>();
                    upperHullRigidbody.isKinematic = true;
                    
                    upperHullsInCurrentSlice.Add(upperHullRigidbody);

                    //rb.useGravity = true;
                    //rb.AddForce(Vector3.back * 100f);
                    //rb.AddExplosionForce(100,upperHull.transform.position,20);
                }


                GameObject lowerHull = hull.CreateLowerHull(toSlice, material);

                if (lowerHull != null)
                {

                    lowerHull.transform.parent = sliceRoot.transform;
                    lowerHull.transform.position = toSlice.transform.position;
                    
                    var lowerHullCollider = lowerHull.AddComponent<BoxCollider>();
                    lowerHullCollider.isTrigger = true;
                    
                    var lowerHullRigidbody = lowerHull.AddComponent<Rigidbody>();
                    lowerHullRigidbody.isKinematic = true;
                    
                    
                    lowerHullsInCurrentSlice.Add(lowerHull);
                }

                Destroy(toSlice);
            }
            else
            {
                var positionOfObject = (toSlice.transform.position - slicer.position).z;

                if (positionOfObject < 0)
                {
                    upperHullsInCurrentSlice.Add(toSlice.GetComponent<Rigidbody>());
                }
            }
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
