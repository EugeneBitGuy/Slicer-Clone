using System.Linq;
using UnityEngine;

namespace Knife
{
    public class SlicerMaterialProvider : MonoBehaviour
    {
        [Header("Cutoff materials")]
        [Tooltip("Materials ths can be used as cutoff after slicing")]
        [SerializeField] private Material[] materialsForSliceable = null;

        /// <summary>
        /// Search for material by its name
        /// </summary>
        /// <param name="objectName">The name of the object for which the material is sought</param>
        /// <returns>Material if search was successful and null if no material was found</returns>
        public Material FindMaterialByName(string objectName)
        {
            if (materialsForSliceable == null) return null;

            var material = materialsForSliceable
                .First(mat => mat.name.Split('_')[0] == objectName.Split('_')[0]);

            if (material == null) return null;

            return material;
        }
    }
}