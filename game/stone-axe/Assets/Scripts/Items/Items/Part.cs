using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [Header("Part Tracker")]
    [SerializeField] List<PartData> _partDataList;
    [SerializeField] List<PartData> _metalBasedParts;
    [SerializeField] List<PartData> _woodBasedParts;
    [SerializeField] List<PartData> _clothBasedParts;

    private void Awake()
    {
        foreach(PartData part in _partDataList)
        {
            foreach (string validMat in part.ValidMaterials)
            {
                if (validMat == "Metal")
                {
                    _metalBasedParts.Add(part);
                }
                else if (validMat == "Wood")
                {
                    _woodBasedParts.Add(part);
                }
                else if (validMat == "Cloth")
                {
                    _clothBasedParts.Add(part);
                }
                else
                    Debug.LogError("Can not catigorize: " + part.PartName);
            }
        }
    }

    public List<PartData> getAllParts()
    {
        return _partDataList;
    }

    public List<PartData> getMetalParts()
    {
        return _metalBasedParts;
    }

    public List<PartData> getWoodParts()
    {
        return _woodBasedParts;
    }

    public List<PartData> getClothParts()
    {
        return _clothBasedParts;
    }
}
