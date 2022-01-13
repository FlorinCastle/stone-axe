using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsControl : MonoBehaviour
{
    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private Dropdown _qualityDropdown;
    [SerializeField] private Dropdown _textureDropdown;
    [SerializeField] private Dropdown _aaDropdown;
    [SerializeField] private Resolution[] resolutions;


    // Start is called before the first frame update
    void Start()
    {
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", _qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", _textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", _aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            _qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            _qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            _resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            _resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            _textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            _textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            _aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            _aaDropdown.value = 1;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
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
