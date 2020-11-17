
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class ProjectDataMgmt : MonoBehaviour
{

    public static ProjectData parseProjectFile(string jsonString)
    {
        return JsonUtility.FromJson<ProjectData>(jsonString);
    }


    public void SaveProject(ProjectData saveData)
    {
        string savePanelMessage = "Save Project";
        string[] fileExts = new string[] { "txt", "json" };
        string initFname = "Project.json";
        BinaryFormatter bf = new BinaryFormatter();        
    }

}
