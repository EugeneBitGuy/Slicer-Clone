using System.Linq;
using UnityEngine;

public class SlicerMaterialProvider : MonoBehaviour
{
    [SerializeField] private Material[] materialsForSliceable = null;

    public Material FindMaterialByName(string objectName)
    {
        if (materialsForSliceable == null) return null;

        var material = materialsForSliceable.First(mat => mat.name.Split('_')[0] == objectName.Split('_')[0]);

        if (material == null) return null;

        return material;
    }
}
