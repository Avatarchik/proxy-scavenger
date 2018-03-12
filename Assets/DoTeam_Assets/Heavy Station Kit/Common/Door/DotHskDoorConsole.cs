using UnityEngine;
using System.Collections;

public class DotHskDoorConsole : MonoBehaviour {

	public DotHskDoor Door2Script;
	public bool multiControlMode = false;
	public Texture banner;
	public DotAnimatedTexture AnimatedTextureScript;

	private bool _operate = false;

	// Use this for initialization
	void Start () {
		if ((Door2Script == null) || (AnimatedTextureScript == null)) {
			Debug.LogWarning("Controlled Door property not assigned in Inspector or DotAnimatedTexture not attached");
		}
		UpdateAnimationScreenMode ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_operate) {
			string s = Input.inputString;
			if ((s.Length == 0) || (Door2Script == null) || (AnimatedTextureScript == null)) { return; }
			switch (s.Substring (0, 1)) {
			case "1": Door2Script.mode = dotHskDoorMode.active;break;
			case "2": Door2Script.mode = dotHskDoorMode.blocked;break;
			case "3": Door2Script.mode = dotHskDoorMode.inactiveClosed;break;
			case "4": Door2Script.mode = dotHskDoorMode.inactiveOpen;break;
			}
			UpdateAnimationScreenMode ();
		} else {
			if (multiControlMode) {
				UpdateAnimationScreenMode ();
			}
		}
	}

	void OnTriggerEnter () {
		_operate = true;
	}

	void OnTriggerExit () {
		_operate = false;
	}

	void OnGUI(){
		if (_operate) {
			float _tw = banner.width;
			float _th = banner.height;
			GUI.DrawTexture (new Rect ((Screen.width - _tw) / 2, Screen.height - 36 - _th, _tw, _th), banner, ScaleMode.ScaleToFit, true); 
		}
	}

	private void UpdateAnimationScreenMode(){
		if (Door2Script != null) {
			switch (Door2Script.mode) {
			case dotHskDoorMode.active:	AnimatedTextureScript.activeSequence = 0; break;
			case dotHskDoorMode.blocked: AnimatedTextureScript.activeSequence = 1; break;
			case dotHskDoorMode.inactiveClosed: AnimatedTextureScript.activeSequence = 2; break;
			case dotHskDoorMode.inactiveOpen: AnimatedTextureScript.activeSequence = 3; break;
			}
		}
	}

}
