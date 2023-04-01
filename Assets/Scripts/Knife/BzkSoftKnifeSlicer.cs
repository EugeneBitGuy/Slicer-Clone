using BzKovSoft.ObjectSlicer.Samples;
using UnityEngine;

public sealed class BzkSoftKnifeSlicer : KnifeSlicer
{
    protected override void StartSlice(GameObject sliceableRoot)
    {
        base.StartSlice(sliceableRoot);
        
        var sliceableObjects = sliceableRoot.GetComponentsInChildren<ObjectSlicerSample>();
        
        foreach (var sliceable in sliceableObjects)
        {
            MakeSlice(sliceable);
        }
    }

    private void MakeSlice(ObjectSlicerSample sliceable)
    {
        if (sliceable == null) return;
        
        if(_materialProvider == null) return;

        sliceable.defaultSliceMaterial = _materialProvider.FindMaterialByName(gameObject.name);

        sliceable.Slice(slicerPlane, res =>
        {
            if (res.sliced)
            {
                var slice = res.outObjectPos;

                StartSliceDestruction(slice);
            }
            else
            {
                bool isOutSideOfPlane = slicerPlane.GetSide(sliceable.GetComponent<Collider>().bounds.min);

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
}
