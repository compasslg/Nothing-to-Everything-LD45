using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	[System.Serializable]
	public class Sound{
		public string name;
		public AudioClip clip;
		[Range(0f, 1f)]public float clipVolume; 
	}
	[SerializeField]private List<Sound> sounds;
	[Range(0f, 1f)]public float soundEffectVolume;
	public static AudioManager instance;
	private AudioSource source;
	// Use this for initialization
	void Start () {
		instance = this;
		source = GetComponent<AudioSource>();
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void PlaySound(string clipName){
		foreach(Sound sound in sounds){
			if(sound.name.Equals(clipName)){
				source.PlayOneShot(sound.clip, sound.clipVolume * soundEffectVolume);
				Debug.Log("Play sound " + clipName);
				break;
			}
		}
	}
}
