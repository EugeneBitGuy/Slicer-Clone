using System.Collections.Generic;
using AbstractClasses;
using BzKovSoft.ObjectSlicer.Samples;
using UnityEngine;

namespace Knife
{
    public sealed class BzkSoftKnifeSlicer : KnifeSlicer
    {
        private List<GameObject> outObjectsNeg = new List<GameObject>();
        
        /// <summary>
        /// Starts slicing due to BzkSoft Algorithm
        /// </summary>
        /// <param name="sliceableRoot">Root object that contains sliceables</param>
        protected override void StartSlice(GameObject sliceableRoot)
        {
            base.StartSlice(sliceableRoot);
        
            Transform rootTranform = sliceableRoot.transform;

            foreach (Transform potentialSliceable in rootTranform)
            {
                if (!potentialSliceable.gameObject.CompareTag("Sliceable")) continue;

                var sliceable = potentialSliceable.gameObject.GetComponent<ObjectSlicerSample>();

                if (sliceable == null)
                    sliceable = potentialSliceable.gameObject.AddComponent<ObjectSlicerSample>();
                
                MakeSlice(sliceable);
            }
        }
    
        /// <summary>
        /// Create hulls due to slicer plane and sliceable functions
        /// </summary>
        /// <param name="sliceable">Sliceable object</param>
        private void MakeSlice(ObjectSlicerSample sliceable)
        {
            if (sliceable == null) return;
        
            if(_materialProvider == null) return;
            
            Material sliceMaterial = _materialProvider.FindMaterialByName(sliceable.gameObject.name);

            if (sliceMaterial == null) return;

            sliceable.defaultSliceMaterial = sliceMaterial;

            sliceable.Slice(_slicerPlane, res =>
            {
                if (res.sliced)
                {
                    var slice = res.outObjectPos;

                    StartSliceDestruction(slice);

                    res.outObjectNeg.tag = "Sliced";
                    
                    outObjectsNeg.Add(res.outObjectNeg);
                }
                else
                {
                    bool isOutSideOfPlane = _slicerPlane.GetSide(sliceable.GetComponent<Collider>().bounds.min);

                    if (isOutSideOfPlane && !sliceable.CompareTag("Sliced"))
                    {
                        StartSliceDestruction(sliceable.gameObject);
                    }
                }
            });
        }

        protected override void FinishSliceDestruction(GameObject outSlice)
        {
            Destroy(outSlice.GetComponent<MeshCollider>());
            outSlice.AddComponent<BoxCollider>().isTrigger = true;
            base.FinishSliceDestruction(outSlice);
        }

        protected override void OnKnifeReachEnd()
        {
            outObjectsNeg.ForEach(obj => { obj.tag = "Sliceable";});
            base.OnKnifeReachEnd();
            outObjectsNeg.Clear();
        }
    }
}
