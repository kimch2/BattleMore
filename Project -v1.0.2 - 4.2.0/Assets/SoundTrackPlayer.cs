using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;

public class SoundTrackPlayer : MonoBehaviour {

	public enum SongName
	{
		 EtheralSkies, BattleTremors, BurningPlasma,HeartOfIron, StrangerBattles,TheOtherWorld
	}


	[Tooltip("Make sure the order of these enums matches those in the song playlist")]
	public List<SongName> levelPlayList = new List<SongName>(){ SongName.EtheralSkies, SongName.BattleTremors, SongName.BurningPlasma, SongName.HeartOfIron, 
		SongName.StrangerBattles};
	public SoundTrackPlayList myPlayList;
	AudioSource mySrc;

	int currentIndex = 0;
	public static SoundTrackPlayer main;

	public bool ShowSoundTrackDebug;
	void Awake()
	{
		main = this;
	}

	void Start () {
		mySrc = GetComponent<AudioSource> ();
		mySrc.loop = false;
		playNextTrack ();
	}

	public void playVictoryTrack()
	{
		
	}

	public void playDefeatTrack()
	{
		crossFadeTrack (1,Resources.Load<AudioClip>("DefeatScreenTrack"));
	}

	void playNextTrack()
	{


		mySrc.clip = myPlayList.myTracks [ (int)levelPlayList[ currentIndex]];
		Debug.Log ("Sound track " + mySrc.clip.name);
		mySrc.Play ();
	
		currentIndex++;
		if (currentIndex >=levelPlayList.Count) {
			currentIndex = 0;}
		//Invoke ("playNextTrack", mySrc.clip.length -1.5f);

	}
		
	void Update(){

		if(ShowSoundTrackDebug){
		Debug.Log ("Sound " + mySrc.isPlaying + "  " + mySrc.time);
		}
		if(!mySrc.isPlaying){
			if(mySrc.time > mySrc.clip.length -1){
		//if (Time.unscaledTime > nextPlayTime) {
				playNextTrack ();
			}
		//}
		}

	}

	public void crossFadeTrack(AudioClip clip)
	{
		StartCoroutine (crossFade(3,clip));
	}

	public void crossFadeTrack(float fadeTime, AudioClip clip)
	{
		StartCoroutine (crossFade(fadeTime,clip));
	}

	IEnumerator crossFade(float fadeTime, AudioClip clip)
	{
		float startVolume = mySrc.volume;
		float FadeRate = startVolume / (fadeTime * .5f);
		for (float i = 0; i < fadeTime / 2; i += Time.deltaTime) {
			mySrc.volume -= FadeRate * Time.deltaTime;
			yield return null;
		}
		mySrc.Stop ();
		mySrc.PlayOneShot (clip);
		for (float i = 0; i < fadeTime / 5; i += Time.deltaTime) {
			mySrc.volume += FadeRate * Time.deltaTime;
			yield return null;
		}


	}



}
