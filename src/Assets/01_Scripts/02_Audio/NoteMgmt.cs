using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class NoteMgmt: MonoBehaviour
{
    public GameObject settingsObj;

    public int noteIndex;
    private float noteColor;
    public int playCount = 0;

    public float minAmp = 0.05f;
    private float ampVolMulti = 20f;

    public bool showTrail = false;

    private float playbackClock = 0;
    float sinVal = 0;
    float cosVal = 0;

    private bool locked;
    private AudioSource audioPlayer;

    private Color activeColor  = new Color(0.25f, 0.25f, 0.25f, 1f);
    private Color defaultColor = new Color(0.15f, 0.15f, 0.9f, 1f);

    public float trailR = 0;
    public float trailG = 0;
    public float trailB = 0;

    float timeSinceHit = 0;


    void Start () {
        locked = true;
        audioPlayer = transform.GetComponent<AudioSource>();

        //for (int i = 1; i < 128; i++) {
        //    AudioSource a = gameObject.AddComponent<AudioSource>();
        //    a = audioPlayer;
        //}    
    }


    void playNote() // Mathf.Pow(2, (note+transpose)/12.0);
    {
        audioPlayer.enabled = true;
        audioPlayer.mute = false;
        audioPlayer.ignoreListenerPause = true;
        audioPlayer.ignoreListenerVolume = true;

        playCount += 1;
        int currentFolder = settingsObj.GetComponent<ControlPanel>().sampleDropDown.value;
        List<AudioClip> sampleList = settingsObj.GetComponent<ControlPanel>().sampleFolderDict[currentFolder];
        if (sampleList.Count == 0) { return; }

        float semi = PlayerPrefs.GetFloat(settingsObj.GetComponent<ControlPanel>().semiPitchDataKey);

        if (audioPlayer.clip != sampleList[0]) {
            audioPlayer.clip = sampleList[0];
        }

        bool multisample = sampleList.Count > 1;
        float amplitude = 0.5f;

        float musicScale = PlayerPrefs.GetFloat(settingsObj.GetComponent<ControlPanel>().scaleDataKey);

        audioPlayer.priority = 0;
        audioPlayer.volume = 1;
        audioPlayer.pitch = 1 + semi;

        if (multisample) {
            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 0) {
                float um = Mathf.PI / (sampleList.Count);
                int sIndex = Random.Range(0, sampleList.Count - 1);
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 1) {
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)noteIndex * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 2) {
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)noteIndex * piSize)));
                audioPlayer.clip = sampleList[(sampleList.Count - 1) - sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 3) {
                float piSize = Mathf.PI / sampleList.Count;
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin((float)playCount * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 4) {
                float piSize = Mathf.PI / (sampleList.Count);
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin(((float)playCount * noteIndex) * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }

            if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().seqModeDataKey) == 5) {
                float piSize = Mathf.PI / (float)PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().patternLengthDataKey);
                int sIndex = (int)((sampleList.Count - 1) * Mathf.Abs(Mathf.Sin(noteIndex * piSize)));
                audioPlayer.clip = sampleList[sIndex];
            }
        } else {
            audioPlayer.clip = sampleList[0];
            audioPlayer.volume = 1;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 0) {
            audioPlayer.pitch = 1 + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 1) {
            float p = 3f / (musicScale / ((float)noteIndex + 1f));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 2) {
            float p = (float)Mathf.Pow(2.0f, ((float)noteIndex / musicScale));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 2) {
            float p = (float)Mathf.Pow(2.0f, ((float)noteIndex / musicScale));
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 3) {
            float ah = Mathf.PI / musicScale;
            float p = musicScale * (Mathf.Abs((float)Mathf.Sin(playCount * (float)noteIndex)) * ah);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 4) {
            float ah = Mathf.PI / musicScale;
            float p = musicScale * (Mathf.Abs((float)Mathf.Sin(playCount)) * ah);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        if (PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().pitchModeDataKey) == 5) {
            float p = 3 / Random.Range(0.1f, musicScale);
            amplitude = ((0.8f - minAmp) / ((float)(p + 1) * ampVolMulti)) + minAmp;
            audioPlayer.pitch = p + semi;
        }

        //Debug.Log("Samples: " + sampleList.Count.ToString());
        //Debug.Log("Amp:     " + amplitude.ToString());

        if (!audioPlayer.isPlaying) {
            audioPlayer.PlayOneShot(audioPlayer.clip, amplitude);
        } else {
            audioPlayer.Stop();
            audioPlayer.PlayOneShot(audioPlayer.clip, amplitude);
        }

    }


    void noteAnimationA() {
        float rSpeed = PlayerPrefs.GetFloat("rSpeed") * 0.01f;
        float sizeOffset = PlayerPrefs.GetFloat("sizeOffset");

        float confX = PlayerPrefs.GetFloat("xSize") * rSpeed;
        float confY = PlayerPrefs.GetFloat("ySize") * rSpeed;

        sinVal = Mathf.Sin((playbackClock * rSpeed + confX) * noteIndex);
        cosVal = Mathf.Cos((playbackClock * rSpeed + confY) * noteIndex);

        float x_pos = (sinVal * ((noteIndex + 1) * 0.1f)) * sizeOffset;
        float z_pos = (cosVal * ((noteIndex + 1) * 0.1f)) * sizeOffset;

        transform.position = new Vector3(x_pos, 0f, z_pos);
    }


    void Update () {
        playbackClock = (float)settingsObj.GetComponent<PlaybackClock>().playbackPosition * 100;

        if (PlayerPrefs.GetInt("play") == 1) {
            noteAnimationA();

            if (PlayerPrefs.GetFloat(settingsObj.GetComponent<ControlPanel>().ampDataKey) > 1) {

                if (transform.localPosition.z < 0.0F) {
                    if (locked == false) {
                        timeSinceHit = 1;

                        playNote();
                        locked = true;
                    }
                }

                if (transform.localPosition.z > 0.0F)
                {
                    locked = false;
                }

                timeSinceHit += 0.01f;
                float v = 1.0f / timeSinceHit;
                transform.GetComponent<MeshRenderer>().material.color = new Color(0, v, 1, 1);
            } else {
                transform.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);

                //float m = 0.001f; //1f / ((Mathf.Abs(Mathf.Sin(1 + Time.time)) * 0.0001f));

                //if (transform.GetComponent<TrailRenderer>().isVisible) {
                //    trailR = Mathf.Abs(Mathf.Sin((m) + Time.time));
                //    trailG = Mathf.Abs(Mathf.Cos((m) + Time.time));
                //    trailB = 1f - Mathf.Abs(Mathf.Sin(((transform.position.x + transform.position.z) * m) + Time.time));

                //    //transform.GetComponent<TrailRenderer>().widthMultiplier = 1f + (float)(512 / PlayerPrefs.GetInt(settingsObj.GetComponent<ControlPanel>().patternLengthDataKey));

                //    transform.GetComponent<TrailRenderer>().startColor = new Color(
                //        trailR, trailG, trailB, 1
                //        );

                //    transform.GetComponent<TrailRenderer>().endColor = new Color(
                //        trailB, trailG, trailR, 1
                //        );
                //}
            }
        }
    }


}