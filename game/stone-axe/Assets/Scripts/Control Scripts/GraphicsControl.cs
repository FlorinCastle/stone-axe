using UnityEngine;
using UnityEngine.UI;

public class GraphicsControl : MonoBehaviour
{
    [SerializeField] private Dropdown _resolutionDropdown;
    private Dropdown _qualityDropdown;
    private Dropdown _textureDropdown;
    private Dropdown _aaDropdown;
    [SerializeField] private Resolution[] resolutions;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        _qualityDropdown.value = 6;
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        _qualityDropdown.value = 6;
    }
    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6)
            QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                _textureDropdown.value = 3;
                _aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                _textureDropdown.value = 2;
                _aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                _textureDropdown.value = 1;
                _aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                _textureDropdown.value = 0;
                _aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                _textureDropdown.value = 0;
                _aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                _textureDropdown.value = 0;
                _aaDropdown.value = 2;
                break;
        }
        _qualityDropdown.value = qualityIndex;
    }

    public void SetFullscreen (bool isFullscreen) { Screen.fullScreen = isFullscreen; }
}
