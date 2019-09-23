using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public int width = 512;
    public int height = 512;
    void Start()
    {
        bool isFullScreen = false; // should be windowed to run in arbitrary resolution

        Screen.SetResolution(width, height, isFullScreen);
    }
}
