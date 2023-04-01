using System;
using System.Collections;
using System.Collections.Generic;
using Deform;
using Knife;
using Managers;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class KnifeSlicer : MonoBehaviour
    {
        [Header("Deformer options")]
        
        [Tooltip("Prefab of deformer that will apply on the slice")]
        [SerializeField] protected BendDeformer deformer = null;
        
        [Tooltip("Factor value of deformation")]
        [SerializeField] protected float deformFactor = 2f;
        
        [Tooltip("Force value that will be applied to slice after cutout")]
        [SerializeField] protected float destructionSliceForce = 100f;

        protected SlicerMaterialProvider _materialProvider;
        
        protected KnifeCollisionHandler _collisionHandler;
        
        protected Plane _slicerPlane;
        
        protected List<Transform> _deformerTransforms = new List<Transform>();
        protected List<GameObject> _outSlices = new List<GameObject>();
    
        protected SlicerState _slicerState;
    
        protected virtual void Start()
        {
            _materialProvider = GetComponent<SlicerMaterialProvider>();
        
            _slicerPlane = new Plane(transform.up, transform.position);
        
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
        
            if(_collisionHandler == null)
                throw new Exception("Knife slicer cannot exist without collision handler");


            _collisionHandler.OnRootEnter += StartSlice;
            _collisionHandler.OnEndPointEnter += OnKnifeReachEnd;
        }

        protected virtual void OnDisable()
        {
            _collisionHandler.OnRootEnter -= StartSlice;
            _collisionHandler.OnEndPointEnter -= OnKnifeReachEnd;
        }

        /// <summary>
        /// Stops slice because of knife reached destination point
        /// </summary>
        protected virtual void OnKnifeReachEnd()
        {
            StartCoroutine(nameof(FinishSlice));
        }
    
        /// <summary>
        /// Slices all object in child of parameter dut to slicerPLane
        /// Changes game state to SlicingEvent
        /// </summary>
        /// <param name="sliceableRoot">Root object that contains sliceables</param>
        protected virtual void StartSlice(GameObject sliceableRoot)
        {
            if(sliceableRoot == null || !sliceableRoot.CompareTag("SliceableRoot")) 
                return;
            
            Handheld.Vibrate();
            
            _slicerState = SlicerState.Slicing;
        
            GameManager.Instance.ChangeGameState(GameState.SlicingEvent);
        }
        
        /// <summary>
        /// Handles behaviour of slicer and slices of root object
        /// Changes game state to Started
        /// </summary>
        /// <returns></returns>
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
    
        /// <summary>
        /// Handles destruction of slice that was on positive side of plane
        /// </summary>
        /// <param name="outSlice">Game object that should be destructed</param>
        protected virtual void StartSliceDestruction(GameObject outSlice)
        {
            AddSliceDeformer(outSlice);

            _outSlices.Add(outSlice);
            outSlice.tag = "Sliced";
        }

        /// <summary>
        /// Handles deformation of every positive slice in this slicing event
        /// </summary>
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
        
        /// <summary>
        /// Provides final destructions to slice and destroys it
        /// </summary>
        /// <param name="outSlice">Game object that should be destructed</param>
    
        protected virtual void FinishSliceDestruction(GameObject outSlice)
        {
            Rigidbody rb = outSlice.GetComponent<Rigidbody>();

            rb.isKinematic = false;

            rb.AddForce(Vector3.back * destructionSliceForce);

            Destroy(outSlice.gameObject, 1f);
        }
    
        
        /// <summary>
        /// Adds deformer to slice
        /// </summary>
        /// <param name="slice">Game object that should be destructed</param>
        protected virtual void AddSliceDeformer(GameObject slice)
        {
            var meshFilter = slice.GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            var mesh = meshFilter.mesh;
            if (mesh == null) return;

            var newDeformer = 
                Instantiate(deformer, Vector3.zero, Quaternion.Euler(0, 90, 0), slice.transform);
            var deformerPosition = new Vector3(mesh.bounds.center.x, 0, mesh.bounds.center.z);
            var deformerTransform = newDeformer.transform;

            deformerTransform.localPosition = deformerPosition;
            deformerTransform.position = 
                new Vector3(deformerTransform.position.x, transform.position.y, deformerTransform.position.z);
            newDeformer.Factor = deformFactor / mesh.bounds.size.z;

            var deformable = slice.GetComponent<Deformable>();
            deformable?.AddDeformer(newDeformer);

            _deformerTransforms.Add(deformerTransform);

        }
    }
}
