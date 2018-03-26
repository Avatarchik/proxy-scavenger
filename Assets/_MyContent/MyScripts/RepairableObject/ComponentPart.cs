using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class ComponentPart {

	[FoldoutGroup("Parts")]
	public RepairUnitPart repairablePart;

	[FoldoutGroup("Parts")]
	public GameObject workingInventoryItem;
	[FoldoutGroup("Parts")]
	public GameObject brokenInventoryItem;
	[FoldoutGroup("Parts")]
	public GameObject mountingObject;

	public GameObject GetWorkingItem(){
		return workingInventoryItem;
	}

	public GameObject GetBrokenItem(){
		return brokenInventoryItem;
	}

	public RepairUnitPart GetPart(){
		return repairablePart;
	}

	public GameObject GetMount(){
		return mountingObject;
	}
}
