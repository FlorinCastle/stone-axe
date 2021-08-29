using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : MonoBehaviour
{
    [Header("Material Tracking")]
    [SerializeField] List<MaterialData> _materialDataList;
    [SerializeField] List<MaterialData> _metalMaterials;
    [SerializeField] List<MaterialData> _woodMaterials;
    [SerializeField] List<MaterialData> _clothMaterials;

    private void Awake()
    {
        foreach(MaterialData mat in _materialDataList)
        {
            if (mat.MaterialType == "Metal")
            {
                //Debug.Log("material found metal: " + mat.Material);
                _metalMaterials.Add(mat);
            }
            else if (mat.MaterialType == "Wood")
            {
                //Debug.Log("material found wood: " + mat.Material);
                _woodMaterials.Add(mat);
            }
            else if (mat.MaterialType == "Cloth")
            {
                //Debug.Log("material found cloth: " + mat.Material);
                _clothMaterials.Add(mat);
            }
            else
                Debug.LogError("Can not catigorize: " + mat.Material);
        }
    }
}
