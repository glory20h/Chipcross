using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; // ¿Àµð¿À ¹Í¼­

    private const float MIN_VOLUME = -4f;
    private const float MUTE_VOLUME = -80f;
    private const float DEFAULT_MUSIC_VOLUME = -1f;
    private const float DEFAULT_SFX_VOLUME = 0f;
    private const float DEFAULT_AMBIENCE_VOLUME = -1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Set Volume Settings
        SetVolume("MusicVol", PlayerPrefs.GetFloat("MusicVol", DEFAULT_MUSIC_VOLUME));
        SetVolume("SFXVol", PlayerPrefs.GetFloat("SFXVol", DEFAULT_SFX_VOLUME));
        SetVolume("AmbienceVol", PlayerPrefs.GetFloat("AmbVol", DEFAULT_AMBIENCE_VOLUME));
    }

    // ¿Àµð¿À ¹Í¼­ÀÇ º¼·ý Á¶Àý
    private void SetVolume(string volumeName, float vol)
    {
        if (vol <= MIN_VOLUME)
        {
            audioMixer.SetFloat(volumeName, MUTE_VOLUME);
        }
        else
        {
            audioMixer.SetFloat(volumeName, -4f * vol * vol);
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
