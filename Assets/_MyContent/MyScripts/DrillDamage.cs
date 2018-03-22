using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillDamage : MonoBehaviour {

	public float MaxHP = 1f;
	public float CurrentHP = 1f;
	public float DrillDamageAmount = 0.25f;
	public float CurrentRotation = 0f;
	public float RotateAmount = 22.5f;


	public void DamageFromDrill(float dmg){
		if(CurrentHP <= 0){
			CurrentHP = 0;
		} else if (CurrentHP - DrillDamageAmount <= 0){
			CurrentHP = 0;
		} else {
			CurrentHP += -DrillDamageAmount;
		}

		if(CurrentRotation >= 90f){
			//this.transform.Rotate(0,90,0);
		} else if (CurrentRotation + RotateAmount >= 90){
			//this.transform.Rotate(0,90,0);
		} else {
			CurrentRotation += RotateAmount;
			this.transform.Rotate(0,CurrentRotation,0);
		}
	}
}
