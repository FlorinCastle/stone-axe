using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerMaterials : MonoBehaviour
{
    public List<Color32> ElfColors { get; } = new List<Color32>()
    {
        new Color32(255, 238, 225, 255),
        new Color32(195, 195, 195, 255),
        new Color32(106, 106, 106, 255)
    };
    public List<Color32> HumanColors { get; } = new List<Color32>()
    {
        new Color32 (255, 199, 155, 255),
        new Color32 (166, 130, 101, 255),
        new Color32 (91, 65, 44, 255)
    };
    public List<Color32> LizardColors { get; } = new List<Color32>()
    {
        new Color32 (48, 48, 48, 255),
        new Color32 (132, 0, 0, 255),
        new Color32 (0, 132, 65, 255)
    };
}
