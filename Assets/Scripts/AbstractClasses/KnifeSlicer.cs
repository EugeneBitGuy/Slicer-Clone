using System;
using System.Collections;
using System.Collections.Generic;
using Deform;
using UnityEngine;

public abstract class KnifeSlicer : MonoBehaviour
{
    [SerializeField] protected BendDeformer deformer = null;
    [SerializeField] protected float deformFactor = 2f;
    [SerializeField] protected float destructionSliceForce = 100f;

    protected KnifeCollisionHandler _collisionHandler;
    protected Plane slicerPlane;
    protected List<Transform> _deformerTransforms = new List<Transform>();
    protected List<GameObject> _outSlices = new List<GameObject>();
    
    protected SlicerState _slicerState;
    
    protected virtual void Start()
    {
        slicerPlane = new Plane(transform.up, transform.position);
        
        _slicerState = SlicerState.Sleeping;
    }

    protected virtual void Update()
    {
        if(_slicerState != SlicerState.Slicing) return;

        ProcessSliceDestruction();
    }

    protected virtual void OnEnable()
    {
        if (_collisionHandler == null)
            _collisionHandler = GetComponent<KnifeCollisionHandler>();

        _collisionHandler.OnRootEnter += StartSlice;
        _collisionHandler.OnEndPointEnter += OnKnifeReachEnd;
    }

    protected virtual void OnDisable()
    {
        _collisionHandler.OnRootEnter -= StartSlice;
        _collisionHandler.OnEndPointEnter -= OnKnifeReachEnd;
    }

    protected virtual void OnKnifeReachEnd()
    {
        StartCoroutine(nameof(FinishSlice));
    }
    
    protected virtual void StartSlice(GameObject sliceableRoot)
    {
        if(sliceableRoot == null || !sliceableRoot.CompareTag("SliceableRoot")) 
            return;
        
        _slicerState = SlicerState.Slicing;
        
        GameManager.Instance.ChangeGameState(GameState.SlicingEvent);
    }
    
    protected IEnumerator FinishSlice()
    {
        _slicerState = SlicerState.Sleeping;
        yield return new WaitForEndOfFrame();

        foreach (GameObject outSlice in _outSlices)
        {
            FinishSliceDestruction(outSlice);
        }
        
        _outSlices.Clear();
        _deformerTransforms.Clear();
        
        GameManager.Instance.ChangeGameState(GameState.Started);
    }
    
    protected virtual void StartSliceDestruction(GameObject outSlice)
    {
        AddSliceDeformer(outSlice);

        _outSlices.Add(outSlice);
        outSlice.tag = "Sliced";
    }

    protected virtual void ProcessSliceDestruction()
    {
        foreach (Transform deformerTransform in _deformerTransforms)
        {
            if (deformerTransform == null) continue;

            var pos = deformerTransform.position;
            pos.y = Mathf.Min(pos.y, transform.position.y);
            deformerTransform.position = pos;
        }
    }
    
    protected virtual void FinishSliceDestruction(GameObject outSlice)
    {
        outSlice.AddComponent<BoxCollider>().isTrigger = true;

        Rigidbody rb = outSlice.GetComponent<Rigidbody>();

        rb.isKinematic = false;

        rb.AddForce(Vector3.back * destructionSliceForce);

        Destroy(outSlice.gameObject, 1f);
    }
    
    protected virtual void AddSliceDeformer(GameObject slice)
    {
        var meshFilter = slice.GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        var mesh = meshFilter.mesh;
        if (mesh == null) return;

        var newDeformer = Instantiate(deformer, Vector3.zero, Quaternion.Euler(0, 90, 0), slice.transform);
        var deformerPosition = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
        var deformerTransform = newDeformer.transform;

        deformerTransform.localPosition = deformerPosition;
        deformerTransform.position = new Vector3(deformerTransform.position.x, transform.position.y, deformerTransform.position.z);
        newDeformer.Factor = deformFactor / mesh.bounds.size.z;

        var deformable = slice.GetComponent<Deformable>();
        deformable?.AddDeformer(newDeformer);

        _deformerTransforms.Add(deformerTransform);

    }
}
