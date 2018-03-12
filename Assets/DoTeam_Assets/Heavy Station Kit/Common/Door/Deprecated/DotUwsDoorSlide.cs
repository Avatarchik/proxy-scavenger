// In order to the doors (as for prefab Door) automatically trigger when approaching a character,
// object FirstPersonСontroller should be marked with the tag "Player"

using UnityEngine;
using System.Collections;

public class uws_setings {
	public const float _door_max_shiftY = 3.48f;
	public const float _door_broken_shiftY = 0.93f;
}

public class DotUwsDoorSlide : MonoBehaviour {

	// _doorMode: 0 - inactive, 1 - active, 2 - blocked 
	private int _doorMode = 1; 
	// state (initial state): 0 - Open, 1 - Close, 2 - Broken
	private int _state = 1;
	private Animator _animator = null;
	private AudioSource _doorSnd = null;
	private Transform _slider = null;
	private AudioClip[] _sounds = new AudioClip[2];
	private AudioClip _beep = new AudioClip();
	private bool _sndLoaded = false;
	// _plaingSnd: 0 - Open, 1 - Close  
	private int _plaingSnd = -1; 

	void Start () {
		Init();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			switch (_doorMode) {
			case 1:
				SlideDoor (0); // Open door
				break;
			case 2:
				if (_beep != null) {
					_doorSnd.clip = _beep;
					_doorSnd.time = 0f;
					_doorSnd.Play();
				}
				break;
			}
		}
	}	

	void OnTriggerExit(Collider other){
		if( (_doorMode == 1) && (other.gameObject.tag == "Player") ){
			SlideDoor(1); // Close door
		}
	}	

	public int GetModeState(){
		return _doorMode*10+_state;
	}

	private void Init(){
		if (_slider == null) { _slider = transform.parent.transform.Find ("Door_Slider"); }
		if (_doorSnd == null) {	_doorSnd = transform.parent.transform.Find ("Door_Sound").GetComponent<AudioSource> (); }
		if (_animator == null) { _animator = _slider.GetComponent<Animator> ();	}
		if (!_sndLoaded) {
			_sounds [0] = Resources.Load<AudioClip> ("Open_Sound"); 
		 	_sounds [1] = Resources.Load<AudioClip> ("Close_Sound");	
			#if UNITY_EDITOR
			if ((_sounds [0] == null) || (_sounds [1] == null)) { Debug.LogWarning("Audioclips \"Open_Sound\" and / or \"Close_Sound\" not found in the \"Resources\" folder"); }
			#endif
			_sndLoaded = true;
		}
		if (_beep == null) {_beep = Resources.Load<AudioClip> ("Blocked"); }
		#if UNITY_EDITOR
		if (_animator==null){ Debug.LogWarning("Door animation is not configured, active mode not avaible"); }
		if (_beep == null) { Debug.LogWarning("Audioclip \"Blocked\" not found in the \"Resources\" folder"); }
		#endif
	}

	// mode: 0 - inactive, 1 - active, 2 - blocked  
	public void ChangeMode(int mode, int state){
		Init();
		int prevModeState = _doorMode * 10 + _state;
		if (_slider == null) { _slider = transform.parent.transform.Find ("Door_Slider"); }
		Vector3 _pos = _slider.localPosition;
		if ( (Application.isPlaying) &&
			(prevModeState!=2) &&
			(state!=2) &&
			(
				(_pos.y<-uws_setings._door_max_shiftY+0.01f) && (state==1) ||
				(_pos.y>-0.01f) && (state==0) || 
				(_pos.y>-uws_setings._door_max_shiftY+0.009f) && (_pos.y<-0.009f)
			)
		){
			SlideDoor ((state==0)?0:1);
		} else {
			_pos.y = (state == 0) ? -uws_setings._door_max_shiftY : ((state == 1) ? 0f : -uws_setings._door_broken_shiftY);
			_slider.localPosition = _pos;
		}
		_doorMode = mode;
		_state = state;
	}

	// state: 0 - Open, 1 - Close
	public void SlideDoor(int state){ 
		if (_animator != null) {
			Vector3 _pos = _slider.localPosition;
			if ((_pos.y < -uws_setings._door_max_shiftY + 0.01f) && (state == 0) || (_pos.y > -0.01f) && (state == 1)) {
				return;
			}
			string _anim = "Door_" + ((state == 0) ? "Open" : "Close");
			AnimatorStateInfo _st = _animator.GetCurrentAnimatorStateInfo (0);
			if (!_st.IsName (_anim)) {
				float _time = _st.normalizedTime;
				_time = (_time < 1.0f && (_st.IsName ("Door_Open") || _st.IsName ("Door_Close"))) ? 1 - _time : 0.0f;
				if (_sndLoaded) {
					float _timeSnd = 0.0f;
					if (_doorSnd.isPlaying && (state > 0) && (_plaingSnd != state)) {
						_timeSnd = _sounds [state].length - _doorSnd.time;
					}
					_doorSnd.clip = _sounds [state];
					_doorSnd.time = _timeSnd;
					_plaingSnd = state;
					_doorSnd.Play ();
				}
				_animator.Play (_anim, -1, _time);
			}
		}
	}

}
