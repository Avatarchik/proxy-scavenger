using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class ComponentLayout {

	//[FoldoutGroup("Component Layout")]
	//public int SlotCount;

	//[FoldoutGroup("Component Layout")]
	//public int SlotLayoutVersion;

	[BoxGroup("Component Layout")]
	public ItemSlotAnchors[] Anchors;

}
