using UnityEngine;
using System.Collections;

public class DotUwsDoorCloser : StateMachineBehaviour {

	private DotUwsDoor _mainScript;
	private Transform _slider = null;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (_slider == null) {
			_slider = animator.transform.parent.Find("Door_Slider");
			_mainScript = _slider.transform.parent.GetComponent<DotUwsDoor> ();
		}
		if (_slider != null) {
			if ((_mainScript != null) && (_mainScript.mode == uws_doorMode.inactiveBroken)) { return; }
			Vector3 _pos = _slider.localPosition;
			_pos.y = (Mathf.Abs(_pos.y) < uws_setings._door_max_shiftY / 2.0) ? 0.0f : -uws_setings._door_max_shiftY;
			_slider.localPosition = _pos;
		}
	}

}
