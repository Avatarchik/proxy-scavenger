using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[System.Serializable]
public partial class ShipPieceState {


	[BoxGroup("Ship Part & State")]
	[SerializeField]
	public ShipPart ShipComponent = ShipPart.Engine;

	[BoxGroup("Ship Part & State")]
	[SerializeField]
	public ShipPartState ShipComponentState = ShipPartState.Broken;

	public void init(ShipPart p, ShipPartState s){
		ShipComponent = p;
		ShipComponentState = s;
	}
}
