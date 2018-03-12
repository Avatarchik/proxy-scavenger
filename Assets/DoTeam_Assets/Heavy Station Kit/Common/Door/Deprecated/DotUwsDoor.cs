using UnityEngine;
using System.Collections;

public enum uws_doorMode {active,blocked,inactiveClosed,inactiveBroken,inactiveOpen};

[ExecuteInEditMode]
public class DotUwsDoor : MonoBehaviour {

	// Public setting
	public uws_doorMode mode = uws_doorMode.active;

	// Private vars
	private uws_doorMode prevMode = uws_doorMode.active;
	private Renderer _doorGlassRenderer = null;
	private DotUwsDoorSlide _sliderScript = null;
	private Collider _collider = null;
	private Light[] _lights = new Light[4];
	private int _lights_cnt = 0;
	private bool _next = false;
	// Mode colors
	private Material glassOn = null;
	private Material glassBlocked = null;
	private Material glassOff = null;
	private Color lightOn = new Color (0f, 1f, 1f / 12f);
	private Color lightBlocked = new Color (1f, 0f, 0f);
	private Color lightOff = new Color(0f,0f,0f);

	void Start(){
		CollectComponents ();
		Update ();
	}

	void Update () {
		if ((prevMode == mode) && _next) { return; }
		if (Application.isEditor) {	CollectComponents (); }
		switch (mode) {
		case uws_doorMode.active: SetDoorState(1,1,lightOn,true,glassOn);	break;
		case uws_doorMode.blocked: SetDoorState(2,1,lightBlocked,true,glassBlocked); break;
		case uws_doorMode.inactiveOpen: SetDoorState(0,0,lightOff,false,glassOff); break;
		case uws_doorMode.inactiveClosed: SetDoorState(0,1,lightOff,false,glassOff); break;
		case uws_doorMode.inactiveBroken: SetDoorState(0,2,lightOff,false,glassOff); break;
		}
		prevMode = mode;	
		_next = true;
	}

	// mode (action mode): 0 - inactive, 1 - active, 2 - blocked 
	// state (initial state): 0 - Open, 1 - Close, 2 - Broken
	// act: true - light enabled, false - disabled
	private void SetDoorState(int mode, int state, Color light_color, bool light_act, Material mat){ 
		if ((mode*10+state != _sliderScript.GetModeState ()) && (_sliderScript != null)) {
			// enable/disable door collider
			if (_collider != null) { _collider.enabled = (mode > 0); }
			// set door panel position
			_sliderScript.ChangeMode (mode,state);
			// set lights
			for (int i = 0; i < _lights_cnt; i++) { 
				_lights [i].color = light_color;
				_lights [i].enabled = light_act;
			}
			// set material
			_doorGlassRenderer.sharedMaterial = mat;
		}

	}

	private void CollectComponents(){
		foreach(Transform child in transform){
			switch (child.name.Substring(5,3)) {
			case "Gla": _doorGlassRenderer = child.GetComponent<Renderer>();break;
			case "Col":
				_sliderScript = child.GetComponent<DotUwsDoorSlide>();
				_collider = child.GetComponent<Collider>();
				break;
			case "Lig": if (_lights_cnt < 4) {_lights [_lights_cnt] = child.GetComponent<Light> (); _lights_cnt++; } break;
			}
		}
		glassOn = Resources.Load<Material>("Glass_Green");
		glassBlocked = Resources.Load<Material>("Glass_Red");
		glassOff = Resources.Load<Material>("Glass_Dark");
		#if UNITY_EDITOR
		if(glassOn == null) { Debug.LogWarning ("Matherial \"Glass_Green\" not found in \"Resource\" folder");}
		if(glassOn == null) { Debug.LogWarning ("Matherial \"Glass_Red\" not found in \"Resource\" folder");}
		if(glassOn == null) { Debug.LogWarning ("Matherial \"Glass_Dark\" not found in \"Resource\" folder");}
		if(_sliderScript == null){ Debug.LogWarning ("Script \"DorHskDoorSlide\" must be attached to the \"Door_Collider\" component");}
		#endif
	}

}
