using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	public AudioMixer audioMixer;                   //����� �ͼ�

    private void Awake()
    {
		DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Set Volume Settings
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVol", -1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 0f));
        SetAmbienceVolume(PlayerPrefs.GetFloat("AmbVol", -1f));
    }

    //������ͼ��� ������� ���� ����
    void SetMusicVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("MusicVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("MusicVol", -4f * vol * vol);
        }
    }

    //������ͼ��� ȿ���� ���� ����
    void SetSFXVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("SFXVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFXVol", -4f * vol * vol);
        }
    }

    //������ͼ��� ȯ���� ���� ����
    void SetAmbienceVolume(float vol)
    {
        if (vol <= -4f)
        {
            audioMixer.SetFloat("AmbienceVol", -80f);
        }
        else
        {
            audioMixer.SetFloat("AmbienceVol", -4f * vol * vol);
        }
    }

    //public AudioMixerGroup mixerGroup;
    //public Sound[] sounds;

    /*
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			//s.source.outputAudioMixerGroup = mixerGroup;
		}
	}
    */

    /*
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;

		s.source.Play();
	}
	*/
}
