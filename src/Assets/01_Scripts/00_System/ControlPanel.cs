using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.IO;


public class ControlPanel : MonoBehaviour
{

    //--------------------------------------------------------------------------
    [Header("Project")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public bool demoMode = false;

    public string appVersion = "1.0"; 

    private bool isInit = false;
    public bool lockData = false;
    public Canvas mainUI;

    //--------------------------------------------------------------------------
    [Header("Audio")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public AudioMixer mixer;

    public float initPitch = 12;
    public int initPatternLength = 48;

    public int initMode = 0;
    public int initSeqMode = 0;
    public int initPitchMode = 1;

    public float initStereo = 70;

    public float initrSpeed = 5f;
    public float initsizeOffset = 25;

    public float initXShape = 1;
    public float initYShape = 1;

    public List<string> demoSamplePaths;
    // public List<string> importFolderPaths;

    [HideInInspector]
    public string importFolderDataKey = "importFolderPath";

    public Dictionary<int, List<AudioClip>> sampleFolderDict = new Dictionary<int, List<AudioClip>>();

    //--------------------------------------------------------------------------
    [Header("Buttons")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public Button loadButton;
    public Button saveButton;

    public Button changeSampleDirButton;

    public Button playbackButton;
    public Text playbackButtonText;

    public Button resetButton;
    public Button randomButton;

    public Button fullscreenToggleBtn;

    //--------------------------------------------------------------------------
    [Header("UI Sliders")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public Dropdown sampleDropDown;
    public Slider volumeSlider;
    public string ampDataKey = "masterVolume";

    [Space(10)]

    public Slider rSpeedSlider;
    public InputField rSpeedSliderValue;
    public string rSpeedDataKey = "rSpeed";

    [Space(10)]

    public Slider sizeOffsetSlider;
    public InputField sizeOffsetSliderValue;
    public string sizeOffsetDataKey = "sizeOffset";

    [Space(10)]

    public Slider xSlider;
    public InputField xSliderValue;
    public string xShapeDataKey = "xSize";

    [Space(10)]

    public Slider ySlider;
    public InputField ySliderValue;
    public string yShapeDataKey = "ySize";

    [Space(10)]

    public Slider reverbSlider;
    public InputField reverbSliderValue;
    public string reverbDataKey = "reverb";

    [Space(10)]

    //public Dropdown playModeDropdown;
    public string playModeDataKey = "playMode";

    [Space(10)]

    public Dropdown seqModeDropdown;
    public string seqModeDataKey = "seqMode";

    [Space(10)]

    public Dropdown pitchModeDropdown;
    public string pitchModeDataKey = "pitchMode";

    [Space(10)]

    public Slider scaleSlider;
    public InputField scaleSliderValue;
    public string scaleDataKey = "musicScale";

    [Space(10)]

    public Slider semiPitchSlider;
    public InputField semiPitchValue;
    public string semiPitchDataKey = "semiPitch";

    [Space(10)]

    public Slider patternSlider;
    public InputField patternSliderValue;
    public string patternLengthDataKey = "ptnLength";

    //--------------------------------------------------------------------------
    [Header("Conf")]
    //--------------------------------------------------------------------------

    [Space(15)]

    public bool init = false;
    public bool isPlaying = false;

    // public double playbackPosition = 0;
    public InputField playbackLabel;
    public string playbackPositionDataKey = "playbackPosition";

    public Button logo;

    float zoomSensitivity = 0.1f;



    void togglePlayback()
    {
        if (PlayerPrefs.GetInt("play") == 1)
        {
            PlayerPrefs.SetInt("play", 0);
            playbackButtonText.text = ">";
        } else {
            PlayerPrefs.SetInt("play", 1);
            playbackButtonText.text = "ll";
        }
    }


    void setPlayback(int state)
    {
        if (state == 0) {
            PlayerPrefs.SetInt("play", 0);
            playbackButtonText.text = ">";
        }
        else {
            PlayerPrefs.SetInt("play", 1);
            playbackButtonText.text = "ll";
        }
    }


    void loadProject()
    {
        Debug.Log("Load Project..");
        setPlayback(0);

        //transform.GetComponent<ProjectDataMgmt>().LoadProject();

        updateSamples();
    }


    void saveProject() {
        Debug.Log("Save Project..");

        ProjectData p = new ProjectData();

        p.appVersion = appVersion;

        p.pitchOffset = PlayerPrefs.GetFloat(scaleDataKey);
        p.semiPitch = PlayerPrefs.GetFloat(semiPitchDataKey);
        p.patternSize = PlayerPrefs.GetInt(patternLengthDataKey);

        if (PlayerPrefs.GetFloat(ampDataKey) < 1) {
            p.visMode = true;
        } else {
            p.visMode = false;
        }

        p.playMode = PlayerPrefs.GetInt(playModeDataKey);
        p.seqMode = PlayerPrefs.GetInt(seqModeDataKey);
        p.pitchMode = PlayerPrefs.GetInt(pitchModeDataKey);

        p.stereo = PlayerPrefs.GetFloat(reverbDataKey);       
        p.rSpeed = PlayerPrefs.GetFloat(rSpeedDataKey);
        p.sizeOffset = PlayerPrefs.GetFloat(sizeOffsetDataKey);
        p.shapeX = PlayerPrefs.GetFloat(xShapeDataKey);
        p.shapeY = PlayerPrefs.GetFloat(yShapeDataKey);

        p.selectedSample = sampleDropDown.options[sampleDropDown.value].text;

        p.playbackPosition = transform.GetComponent<PlaybackClock>().playbackPositionOffset.ToString()+ transform.GetComponent<PlaybackClock>().playbackPosition.ToString();

        if (PlayerPrefs.HasKey(importFolderDataKey)) {
            if (PlayerPrefs.GetString(importFolderDataKey) != "") {
                p.importPath = PlayerPrefs.GetString(importFolderDataKey);
            }
        }

        transform.GetComponent<ProjectDataMgmt>().SaveProject(p);
    }


    void resetProject()
    {
        Debug.Log("Reset Project");
        setPlayback(0);

        transform.GetComponent<PlaybackClock>().playbackPosition = 0;

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetInt(playbackPositionDataKey, 0);
        PlayerPrefs.SetFloat(semiPitchDataKey, 0);

        PlayerPrefs.SetFloat(ampDataKey, 70);
        PlayerPrefs.SetInt("showTrails", 0);

        PlayerPrefs.SetInt(patternLengthDataKey, initPatternLength);
        PlayerPrefs.SetInt(playModeDataKey, initMode);

        PlayerPrefs.SetInt(seqModeDataKey, initSeqMode);
        PlayerPrefs.SetInt(pitchModeDataKey, initPitchMode);

        PlayerPrefs.SetFloat(scaleDataKey, initPitch);
        PlayerPrefs.SetFloat(reverbDataKey, initStereo);

        PlayerPrefs.SetFloat(rSpeedDataKey, initrSpeed);
        PlayerPrefs.SetFloat(sizeOffsetDataKey, initsizeOffset);
        PlayerPrefs.SetFloat(xShapeDataKey, initXShape);
        PlayerPrefs.SetFloat(yShapeDataKey, initYShape);
    }


    private WWW GetAudioFromFile(string path) {
        string audioToLoad = string.Format(path);
        WWW request = new WWW(audioToLoad);
        return request;
    }


    void UpdateSampleDir() {
        Debug.Log("Load Samples..");
        setPlayback(0);

        //transform.GetComponent<ProjectDataMgmt>().ImportSampleFolder();
    }


    private IEnumerator LoadAudio(string filePath, int sampledictIndex)
    {
        WWW request = GetAudioFromFile("file://" + filePath);
        yield return request;
        AudioClip audioClip = request.GetAudioClip();
        string fileName = Path.GetFileName(filePath);
        audioClip.name = Path.GetFileName(filePath);

        if (sampleFolderDict.ContainsKey(sampledictIndex)) {
            sampleFolderDict[sampledictIndex].Add(audioClip);
        } else {
            sampleFolderDict[sampledictIndex] = new List<AudioClip>();
            sampleFolderDict[sampledictIndex].Add(audioClip);
        }
    }


    public void updateSamples() {
        Debug.Log("updateSamples");

        sampleDropDown.ClearOptions();
        sampleFolderDict.Clear();
        int idx = 0;

        List<string> dropDownItems = new List<string>();
        string folderPath = PlayerPrefs.GetString(importFolderDataKey);

        if (folderPath != "") {
            string[] filePaths = Directory.GetFiles(folderPath);

            if (filePaths.Length > 0)
            {
                sampleFolderDict[idx] = new List<AudioClip>();
                dropDownItems.Add("Imported");

                // Add ImportFolder Dir
                foreach (string filePath in filePaths) {
                    Debug.Log(Path.GetExtension(filePath).ToLower());

                    if (Path.HasExtension(filePath)) {
                        if (Path.GetExtension(filePath).ToLower() == ".wav") {
                            StartCoroutine(LoadAudio(filePath, idx));
                        }
                    }
                }

                idx += 1;

                // Add ImportFolder Files
                foreach (string filePath in filePaths) {
                    Debug.Log(Path.GetExtension(filePath).ToLower());

                    if (Path.HasExtension(filePath)) {
                        if (Path.GetExtension(filePath).ToLower() == ".wav") {
                            string fileName = Path.GetFileName(filePath);
                            StartCoroutine(LoadAudio(filePath, idx));
                            dropDownItems.Add(fileName);
                            idx += 1;
                        }
                    }
                }
            }
        }

        // Add Default Sample
        foreach (string p in demoSamplePaths) {
            AudioClip[] clipFiles = Resources.LoadAll<AudioClip>(p);
            sampleFolderDict[idx] = new List<AudioClip>();

            foreach (AudioClip a in clipFiles) {
                sampleFolderDict[idx].Add(a);
            }

            dropDownItems.Add(p);
            idx += 1;
        }

        sampleDropDown.AddOptions(dropDownItems);

    }


    void initUI()
    {

        if (PlayerPrefs.HasKey(ampDataKey))
        {
            volumeSlider.value = PlayerPrefs.GetFloat(ampDataKey);
            mixer.SetFloat(ampDataKey, ((float)(volumeSlider.value / 100) * 80) - 80);
        }

        //rSpeed (PlayerPrefs.HasKey(rSpeedDataKrSpeed       {
        //    rSpeedSlider.varSpeedPlayerPrefs.GetFloat(rSpeedDataKey);
        //}

        if (PlayerPrefs.HasKey(rSpeedDataKey))
        {
            rSpeedSlider.value = PlayerPrefs.GetFloat(rSpeedDataKey);
        }

        if (PlayerPrefs.HasKey(sizeOffsetDataKey))
        {
            sizeOffsetSlider.value = PlayerPrefs.GetFloat(sizeOffsetDataKey);
        }

        if (PlayerPrefs.HasKey(xShapeDataKey))
        {
            xSlider.value = PlayerPrefs.GetFloat(xShapeDataKey);
        }

        if (PlayerPrefs.HasKey(yShapeDataKey))
        {
            ySlider.value = PlayerPrefs.GetFloat(yShapeDataKey);
        }

        if (PlayerPrefs.HasKey(scaleDataKey))
        {
            scaleSlider.value = PlayerPrefs.GetFloat(scaleDataKey);
        }

        if (PlayerPrefs.HasKey(patternLengthDataKey))
        {
            patternSlider.value = PlayerPrefs.GetInt(patternLengthDataKey);
        }

        if (PlayerPrefs.HasKey(semiPitchDataKey))
        {
            semiPitchSlider.value = PlayerPrefs.GetFloat(semiPitchDataKey);
        }

        if (PlayerPrefs.HasKey(reverbDataKey))
        {
            reverbSlider.value = PlayerPrefs.GetInt(reverbDataKey);
        }

        if (PlayerPrefs.HasKey(seqModeDataKey))
        {
            seqModeDropdown.value = PlayerPrefs.GetInt(seqModeDataKey);
        }

        if (PlayerPrefs.HasKey(pitchModeDataKey))
        {
            pitchModeDropdown.value = PlayerPrefs.GetInt(pitchModeDataKey);
        }
    }


    void updateData() {

        if (PlayerPrefs.HasKey(ampDataKey)) {
            if (!volumeSlider.IsInvoking()) {
                volumeSlider.value = PlayerPrefs.GetFloat(ampDataKey);
                mixer.SetFloat(ampDataKey, ((float)(volumeSlider.value / 100) * 80) - 80);
            }
        }

        if (!playbackLabel.isFocused) { 
            playbackLabel.text = transform.GetComponent<PlaybackClock>().playbackPosition.ToString();
        }

        if (PlayerPrefs.HasKey(rSpeedDataKey) && !rSpeedSliderValue.isFocused) {
            rSpeedSliderValue.text = PlayerPrefs.GetFloat(rSpeedDataKey).ToString();
            if (!rSpeedSlider.IsInvoking()) { 
                rSpeedSlider.value = PlayerPrefs.GetFloat(rSpeedDataKey);
            }
        }

        if (PlayerPrefs.HasKey(semiPitchDataKey) && !semiPitchValue.isFocused) {
            semiPitchValue.text = PlayerPrefs.GetFloat(semiPitchDataKey).ToString();
            if (!semiPitchSlider.IsInvoking()) {
                semiPitchSlider.value = PlayerPrefs.GetFloat(semiPitchDataKey);
            }
        }

        if (PlayerPrefs.HasKey(sizeOffsetDataKey) && !sizeOffsetSliderValue.isFocused) {
            sizeOffsetSliderValue.text = PlayerPrefs.GetFloat(sizeOffsetDataKey).ToString();
            if (!sizeOffsetSlider.IsInvoking()) {
                sizeOffsetSlider.value = PlayerPrefs.GetFloat(sizeOffsetDataKey);
            }
        }

        if (PlayerPrefs.HasKey(xShapeDataKey) && !xSliderValue.isFocused) {
            xSliderValue.text = PlayerPrefs.GetFloat(xShapeDataKey).ToString();
            if (!xSlider.IsInvoking()) {
                xSlider.value = PlayerPrefs.GetFloat(xShapeDataKey);
            }
        }

        if (PlayerPrefs.HasKey(yShapeDataKey) && !ySliderValue.isFocused) {
            ySliderValue.text = PlayerPrefs.GetFloat(yShapeDataKey).ToString();
            if (!ySlider.IsInvoking()) {
                ySlider.value = PlayerPrefs.GetFloat(yShapeDataKey);
            }
        }

        if (PlayerPrefs.HasKey(scaleDataKey) && !scaleSliderValue.isFocused) {
            scaleSliderValue.text = PlayerPrefs.GetFloat(scaleDataKey).ToString();
            if (!scaleSlider.IsInvoking()) {
                scaleSlider.value = PlayerPrefs.GetFloat(scaleDataKey);
            }
        }

        if (PlayerPrefs.HasKey(patternLengthDataKey) && !patternSliderValue.isFocused) {
            patternSliderValue.text = PlayerPrefs.GetInt(patternLengthDataKey).ToString();
            if (!patternSlider.IsInvoking()) {
                patternSlider.value = PlayerPrefs.GetInt(patternLengthDataKey);
            }
        }

        if (PlayerPrefs.HasKey(reverbDataKey) && !reverbSliderValue.isFocused) {
            reverbSliderValue.text = PlayerPrefs.GetFloat(reverbDataKey).ToString();

            if (!reverbSlider.IsInvoking()) {
                reverbSlider.value = PlayerPrefs.GetFloat(reverbDataKey);
            }

            mixer.SetFloat("reverbWet", (((float)(PlayerPrefs.GetFloat(reverbDataKey) / 100) * 80) - 80));
        }

        //PlayerPrefs.SetFloat(ampDataKey, volumeSlider.value);
    }


    void Zoom(string scroll_dir)
    {
        lockData = true;

        if ((scroll_dir == "in") && PlayerPrefs.GetFloat(sizeOffsetDataKey) > 0) {
            PlayerPrefs.SetFloat(sizeOffsetDataKey, PlayerPrefs.GetFloat(sizeOffsetDataKey) - 0.5f);
            sizeOffsetSlider.value = PlayerPrefs.GetFloat(sizeOffsetDataKey);
        }

        if ((scroll_dir == "out") && PlayerPrefs.GetFloat(sizeOffsetDataKey) < 100) {
            PlayerPrefs.SetFloat(sizeOffsetDataKey, PlayerPrefs.GetFloat(sizeOffsetDataKey) + 0.5f);
            sizeOffsetSlider.value = PlayerPrefs.GetFloat(sizeOffsetDataKey);
        }

        lockData = false;
    }


    void toggleFullscreen() {
        Debug.Log("toggleFullscreen");

        mainUI.gameObject.SetActive(!mainUI.gameObject.activeSelf);

        if (mainUI.gameObject.activeSelf)
        {
            Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            60);
        }
        else
        {
            Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y,
            0);
        }
    }


    void UpdateInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) { Zoom("in"); }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) { Zoom("out"); }

        if (Input.GetKeyUp(KeyCode.F))
        {
            toggleFullscreen();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (PlayerPrefs.GetInt("showTrails") == 1) {
                PlayerPrefs.SetInt("showTrails", 0);
            } else {
                PlayerPrefs.SetInt("showTrails", 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            togglePlayback();
        }

    }


    void playKey() {
        Debug.Log("1");
        Debug.Log(Input.GetKey(""));
    }


    void manualValueFloat(string paramKey, float paramValue, Slider paramSlider)
    {
        if (!lockData)
        {
            paramSlider.value = paramValue;
            PlayerPrefs.SetFloat(paramKey, paramValue);
        }

    }


    void manualValueInt(string paramKey, int paramValue, Slider paramSlider)
    {
        if (!lockData)
        {
            paramSlider.value = paramValue;
            PlayerPrefs.SetInt(paramKey, paramValue);
        }
    }


    public void updatePlaybackPosition(decimal newPosition) {
        transform.GetComponent<PlaybackClock>().playbackPosition = (decimal)newPosition;
        playbackLabel.text = transform.GetComponent<PlaybackClock>().playbackPosition.ToString();
        PlayerPrefs.SetString(playbackPositionDataKey, transform.GetComponent<PlaybackClock>().playbackPosition.ToString());
    }


    void randomizePrefs() {
        transform.GetComponent<ControlPanel>().lockData = true;

        PlayerPrefs.SetFloat(semiPitchDataKey, Random.Range(0.0f, 1f));

        PlayerPrefs.SetFloat(scaleDataKey, Random.Range(0.1f, 64f));
        PlayerPrefs.SetInt(patternLengthDataKey, (int)Random.Range(12, 64));

        PlayerPrefs.SetInt(playModeDataKey, (int)Random.Range(1, 3));

        PlayerPrefs.SetInt(seqModeDataKey, (int)Random.Range(1, 3));
        PlayerPrefs.SetInt(pitchModeDataKey, (int)Random.Range(1, 4));

        PlayerPrefs.SetFloat(reverbDataKey, Random.Range(20f, 80f));

        PlayerPrefs.SetFloat(rSpeedDataKey, Random.Range(0.1f, 10f));
        PlayerPrefs.SetFloat(sizeOffsetDataKey, Random.Range(10f, 30f));

        PlayerPrefs.SetFloat(xShapeDataKey, Random.Range(0.01f, 1f));
        PlayerPrefs.SetFloat(yShapeDataKey, Random.Range(0.01f, 1f));

        //playModeDropdown.value = PlayerPrefs.GetInt(playModeDataKey);
        seqModeDropdown.value = PlayerPrefs.GetInt(seqModeDataKey);
        pitchModeDropdown.value = PlayerPrefs.GetInt(pitchModeDataKey);

        transform.GetComponent<ControlPanel>().lockData = false;
    }


    void Update()
    {
        UpdateInput();

        if (isInit) {
            updateData();
        } else {
            resetProject();

            transform.GetComponent<PlaybackClock>().playbackPosition = 0;
            updateSamples();

            if (!demoMode) {
                loadButton.onClick.AddListener(delegate { loadProject(); });
                saveButton.onClick.AddListener(delegate { saveProject(); });
                changeSampleDirButton.onClick.AddListener(delegate { UpdateSampleDir(); });
            } else {
                loadButton.onClick.AddListener(delegate { Application.OpenURL("https://www.abstraqata.com/shop/xycliae"); });
                saveButton.onClick.AddListener(delegate { Application.OpenURL("https://www.abstraqata.com/shop/xycliae"); });
                changeSampleDirButton.onClick.AddListener(delegate { Application.OpenURL("https://www.abstraqata.com/shop/xycliae"); });

                loadButton.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.2f);
                saveButton.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.2f);
                changeSampleDirButton.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.2f);
            }

            randomButton.onClick.AddListener(randomizePrefs);

            resetButton.onClick.AddListener(resetProject);
            playbackButton.onClick.AddListener(togglePlayback);
            playbackLabel.onEndEdit.AddListener(delegate { updatePlaybackPosition(decimal.Parse(playbackLabel.text)); });

            fullscreenToggleBtn.onClick.AddListener(toggleFullscreen);

            //playModeDropdown.onValueChanged.AddListener(delegate  { PlayerPrefs.SetInt(playModeDataKey, playModeDropdown.value); });
            seqModeDropdown.onValueChanged.AddListener(delegate   { PlayerPrefs.SetInt(seqModeDataKey,   seqModeDropdown.value); });
            pitchModeDropdown.onValueChanged.AddListener(delegate { PlayerPrefs.SetInt(pitchModeDataKey, pitchModeDropdown.value); });

            logo.onClick.AddListener(delegate { Application.OpenURL("https://www.abstraqata.com"); });

            volumeSlider.onValueChanged.AddListener(delegate     { manualValueFloat(ampDataKey, volumeSlider.value, volumeSlider); });
            volumeSlider.onValueChanged.AddListener(delegate     { mixer.SetFloat(ampDataKey, ((float)(volumeSlider.value / 100) * 80) - 80); } );

            semiPitchSlider.onValueChanged.AddListener(delegate  { manualValueFloat(semiPitchDataKey, semiPitchSlider.value, semiPitchSlider); });
            semiPitchValue.onEndEdit.AddListener(delegate        { manualValueFloat(semiPitchDataKey, float.Parse(semiPitchValue.text), semiPitchSlider); });

            rSpeedSlider.onValueChanged.AddListener(delegate     { manualValueFloat(rSpeedDataKey, rSpeedSlider.value, rSpeedSlider); });
            rSpeedSliderValue.onEndEdit.AddListener(delegate     { manualValueFloat(rSpeedDataKey, float.Parse(rSpeedSliderValue.text), rSpeedSlider); });

            sizeOffsetSlider.onValueChanged.AddListener(delegate { manualValueFloat(sizeOffsetDataKey, sizeOffsetSlider.value, sizeOffsetSlider); });
            sizeOffsetSliderValue.onEndEdit.AddListener(delegate { manualValueFloat(sizeOffsetDataKey, float.Parse(sizeOffsetSliderValue.text), sizeOffsetSlider); });

            xSlider.onValueChanged.AddListener(delegate          { manualValueFloat(xShapeDataKey, xSlider.value, xSlider); });
            xSliderValue.onEndEdit.AddListener(delegate          { manualValueFloat(xShapeDataKey, float.Parse(xSliderValue.text), xSlider); });

            ySlider.onValueChanged.AddListener(delegate          { manualValueFloat(yShapeDataKey, ySlider.value, ySlider); });
            ySliderValue.onEndEdit.AddListener(delegate          { manualValueFloat(yShapeDataKey, float.Parse(ySliderValue.text), ySlider); });

            reverbSlider.onValueChanged.AddListener(delegate     { manualValueFloat(reverbDataKey, reverbSlider.value, reverbSlider); });
            reverbSliderValue.onEndEdit.AddListener(delegate     { manualValueFloat(reverbDataKey, float.Parse(reverbSliderValue.text), reverbSlider); });

            scaleSlider.onValueChanged.AddListener(delegate      { manualValueFloat(scaleDataKey, scaleSlider.value, scaleSlider); });
            scaleSliderValue.onEndEdit.AddListener(delegate      { manualValueFloat(scaleDataKey, float.Parse(scaleSliderValue.text), scaleSlider); });

            patternSlider.onValueChanged.AddListener(delegate    { manualValueInt(patternLengthDataKey, (int)patternSlider.value, patternSlider); });
            patternSliderValue.onEndEdit.AddListener(delegate    { manualValueInt(patternLengthDataKey, int.Parse(patternSliderValue.text), patternSlider); });

            initUI();
            isInit = true;
        }

    }


}

