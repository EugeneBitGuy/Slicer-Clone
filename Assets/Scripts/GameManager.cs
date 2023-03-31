using System;
using System.Collections.Generic;
using BzKovSoft.ObjectSlicer.Samples;
using Deform;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnAllowMovement;
    public event Action OnForbidMovement;

    public bool IsSlicing => _isSlicing;

    [SerializeField] private BendDeformer _deformer;
    

    private bool _isSlicing;
    private bool _knifeReachEnd;
    private bool _knifeIsMoving;

    private List<DeadSliceController> outSlicesObjects;
    private List<Transform> deformerTransforms;


    private KnifeMovement _knifeMovement;

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
        deformerTransforms = new List<Transform>();

        _knifeMovement = FindObjectOfType<KnifeMovement>();
    }

    //object can move only if slice event finished (when object reached end) and when user stops to hold button

    private void Update()
    {
        if (_isSlicing)
        {
            foreach (var deformerTransform in deformerTransforms)
            {
                if(deformerTransform == null) continue;
                
                var pos = deformerTransform.position;
                pos.y = Mathf.Min(pos.y, _knifeMovement.transform.position.y);
                deformerTransform.position = pos;
            }

            if (_knifeReachEnd)
            {
                foreach (var slice in outSlicesObjects)
                {
                    if (slice == null) continue;
                    slice.Throw();
                }
                
                if (!_knifeIsMoving)
                {
                    AllowSliceableMovement();
                    _isSlicing = false;
                    _knifeReachEnd = false;
                
                    outSlicesObjects.Clear();
                    deformerTransforms.Clear();
                }
            }
        }
    }

    public void Slice(GameObject gameObject, Transform slicer)
    {

        void KillSlice(GameObject slice)
        {
            outSlicesObjects.Add(slice.AddComponent<DeadSliceController>());

            var sharedMesh = slice.GetComponent<MeshFilter>().mesh;


            var newDeformer = Instantiate(_deformer, Vector3.one, Quaternion.Euler(0f, 90f, 0f),
                slice.transform);
            
            Vector3 deformerPosition = new Vector3(sharedMesh.bounds.center.x, 0, sharedMesh.bounds.center.z);
            var deformerTransform = newDeformer.transform;
            deformerTransform.localPosition = deformerPosition;
            var pos = deformerTransform.position;
            pos.y = slicer.position.y;
            deformerTransform.position = pos;
            
            newDeformer.Factor = 2f /  sharedMesh.bounds.size.z;
                    
            deformerTransforms.Add(deformerTransform);

            slice.GetComponent<Deformable>().AddDeformer(newDeformer);

            slice.tag = "Sliced";
        }
        
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
                    
                    KillSlice(slice);
                }
                else
                {
                    bool side = plane.GetSide(sliceable.GetComponent<Collider>().bounds.min);

                    if (side && !sliceable.CompareTag("Sliced"))
                    {
                        KillSlice(sliceable.gameObject);

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
