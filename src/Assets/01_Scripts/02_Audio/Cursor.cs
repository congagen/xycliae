using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Cursor : MonoBehaviour
{
	public GameObject settingsObj;
	public bool init = false;
	private float sin_val;
	private float cos_val;
    
    private float playbackClock = 0;
    private float pauseClock = 0;

    //-------------------------------------------------

    public GameObject spr_obj;
	public List<GameObject> spiralObjects;

    public bool showTrail = false;

    public int octaves = 3;
	public float scale_interval = 0.5F;

    public int patternLength = 1;

    public float spiralOffset = 10f;
	public List<int> notes_to_play;
	public List<float> m_scale;
	public int cur_n_;

	public AudioSource audioSource;
    

	//-------------------------------------------------



	void BuildScale(float interv, int octaves)
	{
		m_scale.Clear ();
		for (int i = 1; i < octaves; i++) {
			for (int o = 1; o < 12; o++) {
				m_scale.Add ((float)o * (interv * i));
			}
		}
	}


	void UpdatePattern () {

        if (spiralObjects.Count < PlayerPrefs.GetInt("ptnLength")) {
            int newC = (PlayerPrefs.GetInt("ptnLength") - spiralObjects.Count) + 1;

            for (int i = 1; i < newC; i++) {
                GameObject notObject = Instantiate(spr_obj, transform.position, Quaternion.identity) as GameObject;

                notObject.GetComponent<NoteMgmt>().settingsObj = settingsObj;
                spiralObjects.Add(notObject);
                notObject.transform.SetParent(transform);
                notObject.SetActive(false);
            }
        }

        foreach (Transform t in transform) {
            t.gameObject.SetActive(false);
        }

        for (int i = 0; i < spiralObjects.Count - 1; i++) {
            GameObject go = spiralObjects[i];
            go.name = i.ToString();

            if ( i + 1 <= (PlayerPrefs.GetInt("ptnLength")) ) {
                go.SetActive(true);
            } else {
                go.SetActive(false);
            }
        }

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            GameObject go = spiralObjects[i];
            go.GetComponent<NoteMgmt>().noteIndex = i + 1;
        }

        //for (int i = 0; i < PlayerPrefs.GetInt("ptnLength"); i++) {
        //    spiralObjects[i].GetComponent<NoteMgmt>().noteIndex = (i) + 1;
        //}

        patternLength = PlayerPrefs.GetInt("ptnLength");
    }


    void spiralAnimationD()
    {
        float rSpeed = PlayerPrefs.GetFloat("rSpeed");
        float sizeOffset = PlayerPrefs.GetFloat("sizeOffset");

        float confX = PlayerPrefs.GetFloat("xSize") * rSpeed;
        float confY = PlayerPrefs.GetFloat("ySize") * rSpeed;

        float confPtnLength = PlayerPrefs.GetFloat("ptnLength");

        for (int i = 0; i < PlayerPrefs.GetInt("ptnLength") - 1; i++)
        {
            GameObject gob = spiralObjects[i];
            int nIndex = gob.GetComponent<NoteMgmt>().noteIndex;

            sin_val = Mathf.Sin(playbackClock * ((nIndex + spiralOffset) * 0.02F) * confX) * sizeOffset;
            cos_val = Mathf.Cos(playbackClock * ((nIndex + spiralOffset) * 0.02F) * confY) * sizeOffset;

            float x_pos = (transform.position.x + ((sin_val * (playbackClock * (nIndex * 0.002f))) * (nIndex + spiralOffset)));
            float z_pos = (transform.position.z + ((cos_val * (playbackClock * (nIndex * 0.002f))) * (nIndex + spiralOffset)));

            gob.transform.localPosition = new Vector3(x_pos, 0f, z_pos);
        }
    }


    void spiralAnimationA()
    {
        float rSpeed     = PlayerPrefs.GetFloat("rSpeed") * 0.01f;
        float sizeOffset = PlayerPrefs.GetFloat("sizeOffset");

        float confX      = PlayerPrefs.GetFloat("xSize");
        float confY      = PlayerPrefs.GetFloat("ySize");

        float confPtnLength = PlayerPrefs.GetFloat("ptnLength");

        for (int i = 0; i < PlayerPrefs.GetInt("ptnLength"); i++)
        {
            GameObject gob = spiralObjects[i];
            // int nIndex = gob.GetComponent<NoteMgmt>().noteIndex;

            sin_val = Mathf.Sin((playbackClock * rSpeed + confX) * i);
            cos_val = Mathf.Cos((playbackClock * rSpeed + confY) * i);

            float x_pos = (sin_val * ((i + 1) * 0.1f)) * sizeOffset;
            float z_pos = (cos_val * ((i + 1) * 0.1f)) * sizeOffset;

            gob.transform.localPosition = new Vector3(
                transform.position.x + x_pos, 0f, transform.position.z + z_pos
            );
        }
    }


    void init_prj()
    {
        BuildScale(scale_interval, octaves);
        UpdatePattern();
        init = true;
    }


    void updateTrails()
    {
        foreach (Transform t in transform)
        {
            t.GetComponent<TrailRenderer>().enabled = PlayerPrefs.GetInt("showTrails") == 1;
        }
    }


    void Start()
    {
        updateTrails();
    }


    void Update ()
	{
        playbackClock = (float)settingsObj.GetComponent<PlaybackClock>().playbackPosition * 100;

        if (!init) {
			init_prj();
            UpdatePattern();
        }
        else {

            if (patternLength != PlayerPrefs.GetInt("ptnLength")) {
                UpdatePattern();
            }

            if (PlayerPrefs.GetInt("play") == 1)
            {
                //spiralAnimationA();
            }
        }
    }
}
