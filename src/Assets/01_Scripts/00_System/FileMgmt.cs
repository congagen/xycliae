//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using System;


//namespace GracesGames.SimpleFileBrowser.Scripts.UI {
//    public class FileMgmt : MonoBehaviour
//    {

//        public Canvas mainCanvas;
//        public GameObject FileBrowserPrefab;
//        public string[] FileExtensions;
//        private GameObject _textToSaveInputField;
//        private GameObject _loadedText;
//        public string _textToSave;
//        public bool PortraitMode;

//        public string demoString = "test123";


//        public void OpenFileBrowser(bool saving)
//        {
//            OpenFileBrowser(saving ? FileBrowserMode.Save : FileBrowserMode.Load);
//        }


//        private void OpenFileBrowser(FileBrowserMode fileBrowserMode)
//        {
//            // Create the file browser and name it
//            GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
//            fileBrowserObject.name = "FileBrowser";
//            fileBrowserObject.GetComponent<FileBrowser>().uiCanvas = mainCanvas;

//            // Set the mode to save or load
//            FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
//            fileBrowserScript.SetupFileBrowser(PortraitMode ? ViewMode.Portrait : ViewMode.Landscape);

//            if (fileBrowserMode == FileBrowserMode.Save)
//            {
//                fileBrowserScript.SaveFilePanel("DemoText", FileExtensions);
//                // Subscribe to OnFileSelect event (call SaveFileUsingPath using path) 
//                fileBrowserScript.OnFileSelect += SaveFileUsingPath;
//            }
//            else
//            {
//                fileBrowserScript.OpenFilePanel(FileExtensions);
//                // Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
//                fileBrowserScript.OnFileSelect += LoadFileUsingPath;
//            }
//        }

//        // Saves a file with the textToSave using a path
//        private void SaveFileUsingPath(string path)
//        {
//            // Make sure path and _textToSave is not null or empty
//            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(_textToSave))
//            {
//                BinaryFormatter bFormatter = new BinaryFormatter();
//                // Create a file using the path
//                FileStream file = File.Create(path);
//                // Serialize the data (textToSave)
//                bFormatter.Serialize(file, _textToSave);
//                // Close the created file
//                file.Close();
//            }
//            else
//            {
//                Debug.Log("Invalid path or empty file given");
//            }
//        }

//        // Loads a file using a path
//        private void LoadFileUsingPath(string path)
//        {
//            if (path.Length != 0)
//            {
//                BinaryFormatter bFormatter = new BinaryFormatter();
//                // Open the file using the path
//                FileStream file = File.OpenRead(path);
//                // Convert the file from a byte array into a string
//                string fileData = bFormatter.Deserialize(file) as string;
//                // We're done working with the file so we can close it
//                file.Close();
//                // Set the LoadedText with the value of the file
//                _loadedText.GetComponent<Text>().text = "Loaded data: \n" + fileData;
//            }
//            else
//            {
//                Debug.Log("Invalid path given");
//            }
//        }

//        void Start()
//        {
//            // OpenFileBrowser(true);
//        }

//    }
//}