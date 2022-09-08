using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Part : MonoBehaviour
{
    [Header("Part Tracker")]
    [SerializeField] List<PartData> _partDataList;
    [SerializeField] List<TextAsset> partJsons;

    [Header("Sorted")]
    [SerializeField] List<PartData> _metalBasedParts;
    [SerializeField] List<PartData> _basicMetalBasedParts;
    [SerializeField] List<PartData> _magicMetalBasedParts;
    [SerializeField] List<PartData> _woodBasedParts;
    [SerializeField] List<PartData> _clothBasedParts;
    [SerializeField] List<PartData> _wovenClothBasedParts;
    [SerializeField] List<PartData> _leatherBasedParts;
    [SerializeField] List<PartData> _gemstoneBasedParts;


    private void Awake()
    {
        foreach(PartData part in _partDataList)
        {
            foreach (string validMat in part.ValidMaterials)
            {
                if (validMat == "Metal")
                {
                    if (!_metalBasedParts.Contains(part)) _metalBasedParts.Add(part);
                }
                if (validMat == "Basic Metal")
                {
                    if (!_metalBasedParts.Contains(part)) _metalBasedParts.Add(part);
                    if (!_basicMetalBasedParts.Contains(part)) _basicMetalBasedParts.Add(part);
                }
                if (validMat == "Magic Metal")
                {
                    if (!_metalBasedParts.Contains(part)) _metalBasedParts.Add(part);
                    if (!_magicMetalBasedParts.Contains(part)) _magicMetalBasedParts.Add(part);
                }
                if (validMat == "Wood")
                {
                    if (!_woodBasedParts.Contains(part)) _woodBasedParts.Add(part);
                }
                if (validMat == "Cloth")
                {
                    if (!_clothBasedParts.Contains(part)) _clothBasedParts.Add(part);
                }
                if (validMat == "Woven Cloth")
                {
                    if (!_clothBasedParts.Contains(part)) _clothBasedParts.Add(part);
                    if (!_wovenClothBasedParts.Contains(part)) _wovenClothBasedParts.Add(part);
                }
                if (validMat == "Leather")
                {
                    if (!_clothBasedParts.Contains(part)) _clothBasedParts.Add(part);
                    if (!_leatherBasedParts.Contains(part)) _leatherBasedParts.Add(part);
                }
                if (validMat == "Gemstone")
                {
                    if (!_gemstoneBasedParts.Contains(part)) _gemstoneBasedParts.Add(part);
                }
                if (validMat != "Metal" && validMat != "Basic Metal" && validMat != "Magic Metal" && validMat != "Wood" && validMat != "Cloth" && validMat != "Woven Cloth" && validMat != "Leather" && validMat != "Gemstone")
                    Debug.LogError("Can not catigorize: " + part.PartName);
            }
        }
    }

    public List<PartData> getAllParts() { return _partDataList; }
    public List<TextAsset> getAllPartJsonData() { return partJsons; }

    public PartJsonData getPartJsonData(string partName)// (int partIDin)
    {
        foreach (TextAsset part in partJsons)
        {
            string partCheck = loadJson(part).partName;
            //int ID = loadJson(part).partID;
            if (partCheck == partName)
                return loadJson(part);
        }
        Debug.LogWarning("Unable to find Valid Part for ID: [" + partName + "]");
        return null;
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

    private PartJsonData loadJson(TextAsset jsonText)
    {
        return JsonUtility.FromJson<PartJsonData>(File.ReadAllText(Application.dataPath + AssetDatabase.GetAssetPath(jsonText).Replace("Assets", "")));
    }
}
