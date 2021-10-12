using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private FullScreenMode screenMode;
    [SerializeField] private TMP_Dropdown resol;
    [SerializeField] private TMP_Dropdown mode;

    private List<string> resolutions;
    void Awake()
    {
        resolutions = new List<string>();
            foreach (Resolution resolution in Screen.resolutions)
            {
               if(resolution.width / resolution.height == 16/9) resolutions.Add(resolution.width + " x " + resolution.height);
            }
            resol.AddOptions(resolutions);
    }

    public void SetResol()
    {
        Screen.SetResolution(Screen.resolutions[resol.value].width, Screen.resolutions[resol.value].height, screenMode);
    }

    public void SetMode()
    {
        switch (mode.value)
            {
                case 0:
                    screenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 1:
                    screenMode = FullScreenMode.Windowed;
                    break;
            }
        
        Screen.SetResolution(Screen.resolutions[resol.value].width, Screen.resolutions[resol.value].height, screenMode);
    }
}
