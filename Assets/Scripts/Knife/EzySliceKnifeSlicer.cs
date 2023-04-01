using System.Collections.Generic;
using AbstractClasses;
using Deform;
using EzySlice;
using UnityEngine;

namespace Knife
{
    public class EzySliceKnifeSlicer : KnifeSlicer
    {
        private List<GameObject> lowerObject = new List<GameObject>();

        /// <summary>
        /// Starts slicing due to EzySlice Algorithm
        /// </summary>
        /// <param name="sliceableRoot">Root object that contains sliceables</param>
        protected override void StartSlice(GameObject sliceableRoot)
        {
            base.StartSlice(sliceableRoot);

            Transform rootTranform = sliceableRoot.transform;

            foreach (Transform potentialSliceable in rootTranform)
            {
                if (!potentialSliceable.gameObject.CompareTag("Sliceable")) continue;

                MakeSlice(potentialSliceable.gameObject);
            }
        }

        
        /// <summary>
        /// Create hulls due to slicer plane and sliceable functions
        /// </summary>
        /// <param name="sliceable">Sliceable object</param>
        private void MakeSlice(GameObject sliceable)
        {
            Material sliceMaterial = _materialProvider.FindMaterialByName(sliceable.name);

            if (sliceMaterial == null) return;

            SlicedHull hull = sliceable.Slice(transform.position, transform.up, sliceMaterial);

            if (hull != null)
            {
                var upperHull = hull.CreateUpperHull(sliceable, sliceMaterial);
                upperHull.name = sliceable.name + "_upper";

                var lowerHull = hull.CreateLowerHull(sliceable, sliceMaterial);
                lowerHull.name = sliceable.name + "_lower";


                AddComponentToHull(upperHull);
                PlaceHull(upperHull, sliceable);
                AddComponentToHull(lowerHull);
                PlaceHull(lowerHull, sliceable);

                Destroy(sliceable);

                StartSliceDestruction(upperHull);

                lowerObject.Add(lowerHull);
            }
            else
            {
                bool isOutSideOfPlane = _slicerPlane.GetSide(sliceable.GetComponent<Collider>().bounds.min);

                if (isOutSideOfPlane && !sliceable.CompareTag("Sliced"))
                {
                    StartSliceDestruction(sliceable.gameObject);
                }
            }
        }

        /// <summary>
        /// Adds important components to hull
        /// </summary>
        /// <param name="hull">Slice received from slicing</param>
        private void AddComponentToHull(GameObject hull)
        {
            if (hull == null) return;

            hull.AddComponent<BoxCollider>().isTrigger = true;
            hull.AddComponent<Rigidbody>().isKinematic = true;
            hull.AddComponent<Deformable>();
        }

        /// <summary>
        /// Sets hull position due to original object
        /// </summary>
        /// <param name="hull">Slice received from slicing</param>
        private void PlaceHull(GameObject hull, GameObject sliceable)
        {
            hull.transform.parent = sliceable.transform.parent;
            hull.transform.position = sliceable.transform.position;
        }
        protected override void OnKnifeReachEnd()
        {
            lowerObject.ForEach(go => { go.tag = "Sliceable"; });

            base.OnKnifeReachEnd();

            lowerObject.Clear();
        }
    }
}