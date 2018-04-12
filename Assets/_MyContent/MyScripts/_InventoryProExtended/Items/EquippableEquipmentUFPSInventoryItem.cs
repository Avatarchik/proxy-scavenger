using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.InventoryPro.Integration.UFPS;

public class EquippableEquipmentUFPSInventoryItem : EquippableUFPSInventoryItem {


	protected override void Awake ()
	{
		base.Awake ();
		//collection = GetComponent<MyCustomFPSCollection>();
	}

	public override void NotifyItemEquippedVisually(CharacterEquipmentTypeBinder binder)
	{
		var r = binder.currentItem.GetComponent<Rigidbody>();
		r.constraints = RigidbodyConstraints.FreezeAll;
		r.useGravity = false;
	}
}
