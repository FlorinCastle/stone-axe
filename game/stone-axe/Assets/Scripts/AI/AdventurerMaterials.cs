using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerMaterials : MonoBehaviour
{
    private List<Color> elfColorList = new List<Color>()
    {
        new Color(255, 238, 225),
        new Color(195, 195, 195),
        new Color(106, 106, 106)
    };
    private List<Color> humanColorList = new List<Color>()
    {
        new Color(255, 199, 155),
        new Color(166, 130, 101),
        new Color(91, 65, 44)
    };
    private List<Color> lizardColorList = new List<Color>()
    {
        new Color(48, 48, 48),
        new Color(132, 0, 0),
        new Color(0, 132, 65)
    };

    public List<Color> ElfColors { get => elfColorList; }
    public List<Color> HumanColors { get => humanColorList; }
    public List<Color> LizardColors { get => lizardColorList; }
}
