using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceContainer : MonoBehaviour {

	// Use this for initialization

	public List<VoicePack> myVoicePacks;

	public List<VoicePack> LockedVoicePacks;


}

[System.Serializable]
public class AudioTub
{
	public List<AudioClip> myClips;

	public void playRandomClip(AudioSource src)
	{
		src.PlayOneShot(myClips[Random.Range(0, myClips.Count)]);
	}

	public void playRandomClip(AudioSource src, float volume)
	{
		src.volume = volume;
		src.PlayOneShot(myClips[Random.Range(0, myClips.Count)]);
	}


	public AudioClip getRandomClip()
	{

		return myClips[Random.Range(0, myClips.Count)];
	}
}