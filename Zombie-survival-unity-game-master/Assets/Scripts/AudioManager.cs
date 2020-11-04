using UnityEngine;

[System.Serializable]
public class Sound	{
	
	public string name;
	public AudioClip clip;
	private AudioSource source;
	[Range(0f, 1f)]
	public float volume = 0.7f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;
	[Range(0f, 0.5f)]
	public float volumeRandomnesRange = 0.2f;
	[Range(0f, 0.5f)]
	public float pitchRandomnesRange = 0.1f;

	public void SetSource(AudioSource _source)
	{
		source = _source;
		source.clip = clip;
	}

	public void Play()	
	{
		source.pitch = pitch * (1 + Random.Range (-pitchRandomnesRange / 2f, pitchRandomnesRange / 2f));
		source.volume = volume * (1 + Random.Range (-volumeRandomnesRange / 2f, volumeRandomnesRange / 2f));
		source.Play ();
	}

}

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	[SerializeField]
	Sound[] sounds;

	void Awake()
	{
		if (instance != null) {
			Debug.LogError ("More than one audio manager in scene");
		} else {
			instance = this;
		}
	}

	private void Start() 
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name);
			_go.transform.SetParent (this.transform);
			sounds[i].SetSource (_go.AddComponent<AudioSource> ());
		
		}
	}

	public void PlaySound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++) {
			if (sounds[i].name == _name) {
				sounds [i].Play ();
				return;
			}
		}
		Debug.LogWarning ("AudiManager: Sound not found, " + _name);
	}

}
