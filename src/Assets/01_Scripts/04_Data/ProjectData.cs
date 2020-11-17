using UnityEngine;
using System.IO;
using System.Collections;

[System.Serializable]
public class ProjectData
{
    public string projectName = "";
    public string projectId = "";
    public string appVersion = "";

    public bool visMode = false;

    public string selectedSample = "";

    public float playbackDirection = 1;
    public string playbackPosition = "0";

    public float pitchOffset = 1;
    public float semiPitch = 0;
    public int patternSize = 48;

    public int playMode = 1;

    public int seqMode = 1;
    public int pitchMode = 1;

    public float stereo = 80;

    public float rSpeed = 1;
    public float sizeOffset = 1;

    public float shapeX = 1;
    public float shapeY = 1;

    public string importPath = "";

}