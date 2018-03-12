using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpenAutoClose : MonoBehaviour {

	// Static data
	private static bool _initiated = false;
	private static AudioClip[] _sounds = new AudioClip[2];
	private static bool _snd_loaded = false;

	// Instance specific data
	private bool _active = false;
	private Animator _animator = null;
	private AudioSource _gateSnd = null;

	void Start(){
		GameObject _go = null;
		if (!_initiated) {
			_initiated = true;
			_go = GameObject.FindGameObjectWithTag ("Player");
			if (_go == null) {
				Debug.LogWarning ("Not found FirstPersonController component with tag \"Player\"!");
			} else {
				
				_sounds[0] = Resources.Load("Open_Sound") as AudioClip;
				_sounds[1] = Resources.Load("Close_Sound") as AudioClip;
				_snd_loaded = (_sounds[0]!=null) && (_sounds[1] != null);
				if (!_snd_loaded) {
					Debug.LogWarning("Silence mode:  audioclips \"Open_Sound\" and / or \"Close_Sound\" not found in the \"Resources\" directory");
				}
			}
		}
		if (_initiated) {
			foreach(Transform child in transform.parent.transform.parent.transform){
				switch(child.name){
				case "Gate2": _animator = child.GetComponent<Animator>(); break;
				case "Gate2_Sound": _gateSnd = child.GetComponent<AudioSource>(); break;
				}
			};
		}
	}

	void Update() {
		if(_active && Input.GetKeyDown(KeyCode.E)){
			OpenGate();
		}
	}

	void OnTriggerEnter (Collider other) {
		_active = (other.gameObject.tag == "Player");
	}

	void OnTriggerExit (Collider other) {
		if(other.gameObject.tag == "Player"){
			_active = false;
		}
	}

	public void OpenGate(){
		string st = "Open";
		if (_animator != null) {
			_animator.Play ("G1_" + st);
		}

		if (_snd_loaded) {
			_gateSnd.clip = _sounds [(st == "Open") ? 0 : 1];
			_gateSnd.Play ();
		}
		StartCoroutine(DoorPause());
		CloseGate();
	}

	public void CloseGate(){
		string st = "Close";
		if (_animator != null) {
			_animator.Play ("G1_" + st);
		}

		if (_snd_loaded) {
			_gateSnd.clip = _sounds [(st == "Open") ? 0 : 1];
			_gateSnd.Play ();
		}
	}

	IEnumerator DoorPause()
	{
		yield return new WaitForSeconds(2);
	}
}
