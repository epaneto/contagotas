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

    public void PlayOneShotMusic(string id)
    {
        if (!MusicON)
            return;
        
        AudioClip newclip = Resources.Load("music/" + id) as AudioClip;
        MusicSource.PlayOneShot(newclip);
    }

    public void PlayLoopMusic(string id)
    {
        if (!MusicON)
            return;

        AudioClip newclip = Resources.Load("music/" + id) as AudioClip;
        MusicSource.clip = newclip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOut(MusicSource, 1.0f));
    }

    public void PlaySFX(string id)
    {
        if (!SFXON)
            return;
        
        AudioClip newclip = Resources.Load("music/" + id) as AudioClip;
        SFXSource.PlayOneShot(newclip);
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
