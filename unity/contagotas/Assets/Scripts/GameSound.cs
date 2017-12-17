using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour {

    public GameObject MusicObject;
    public GameObject SFXObject;
    public bool MusicON = true;
    public bool SFXON = true;
    public static GameSound gameSound;

    AudioSource SFXSource;
    AudioSource MusicSource;

    void Awake()
    {
        if (gameSound == null)
        {
            DontDestroyOnLoad(gameObject);
            gameSound = this;
        }
        else if (gameSound != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        SFXSource = SFXObject.GetComponent<AudioSource>();
        MusicSource = MusicObject.GetComponent<AudioSource>();
	}

    public void PlayOneShotMusic(string id, float songVolume = 1.0f)
    {
        if (!MusicON)
            return;

        MusicSource.Stop();
        AudioClip newclip = Resources.Load("music/" + id) as AudioClip;
        MusicSource.volume = songVolume;
        MusicSource.PlayOneShot(newclip);
    }

    public void PlayLoopMusic(string id, float songVolume = 1.0f)
    {
        if (!MusicON)
            return;

        MusicSource.Stop();
        AudioClip newclip = Resources.Load("music/" + id) as AudioClip;
        MusicSource.clip = newclip;
        MusicSource.loop = true;

        MusicSource.volume = songVolume;
        
        MusicSource.Play();
    }

    public void StopMusic(float time = 1.0f)
    {
        StartCoroutine(FadeOut(MusicSource, time));
    }

    public void PlaySFX(string id)
    {
        if (!SFXON)
            return;
        
        AudioClip newclip = Resources.Load("sfx/" + id) as AudioClip;
        SFXSource.PlayOneShot(newclip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void MuteUnmuteSound()
    {
        MusicON = !MusicON;
        if(!MusicON)
        {
            PlayerPrefs.SetString("sound","off");
            MusicSource.volume = 0;
            SFXSource.volume = 0;
        }else{
            PlayerPrefs.SetString("sound", "on");
            MusicSource.volume = 1.0f;
            SFXSource.volume = 1.0f;
        }

        PlayerPrefs.Save();
    }


    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
}
