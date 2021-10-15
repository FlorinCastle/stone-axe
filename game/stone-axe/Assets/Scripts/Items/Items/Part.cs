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
    [SerializeField] List<PartData> _gemstoneBasedParts;

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
                if (validMat == "Wood")
                {
                    _woodBasedParts.Add(part);
                }
                if (validMat == "Cloth")
                {
                    _clothBasedParts.Add(part);
                }
                if (validMat == "Gemstone")
                {
                    _gemstoneBasedParts.Add(part);
                }
                if (validMat != "Metal" && validMat != "Wood" && validMat != "Cloth" && validMat != "Gemstone")
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

    public List<PartData> getGemstoneParts()
    {
        return _gemstoneBasedParts;
    }
}
