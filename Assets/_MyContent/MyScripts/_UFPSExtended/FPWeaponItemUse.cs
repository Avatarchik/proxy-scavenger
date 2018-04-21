using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPWeaponItemUse : vp_FPWeapon {

	public override void Wield(bool isWielding = true)
	{
		base.Wield();
		var c = this.gameObject.transform.GetChild(0);
		vp_Layer.Set(c.gameObject, vp_Layer.Weapon, true);
		if(c.GetChild(0) != null){
			vp_Layer.Set(c.GetChild(0).gameObject, vp_Layer.Weapon, true);

		}
	}
}
