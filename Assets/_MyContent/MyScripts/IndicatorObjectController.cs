using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IndicatorState {
	Disabled = 0,
	Positive = 1,
	Negative = 2,
}

public class IndicatorObjectController : MonoBehaviour {

	public GameObject PositiveObject;
	public GameObject NegativeObject;
	public GameObject DiabledObject;

	public void Start(){
		Disabled();
	}

	public void SetState(IndicatorState i){
		switch(i){
		case IndicatorState.Disabled:
			Disabled();
			break;
		case IndicatorState.Positive:
			Positive();
			break;
		case IndicatorState.Negative:
			Negative();
			break;
		}
	}

	public void Positive(){
		PositiveObject.SetActive(true);
		NegativeObject.SetActive(false);
		DiabledObject.SetActive(false);
	}

	public void Negative(){
		PositiveObject.SetActive(false);
		NegativeObject.SetActive(true);
		DiabledObject.SetActive(false);
	}

	public void Disabled(){
		PositiveObject.SetActive(false);
		NegativeObject.SetActive(false);
		DiabledObject.SetActive(true);
	}
}
