using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class ItemSlotAnchors {
	
	[FoldoutGroup("Item & Slot Anchors & UI Slot")]
	public GameObject ItemAnchor;

	[FoldoutGroup("Item & Slot Anchors & UI Slot")]
	public GameObject SlotUIAnchor;

	[FoldoutGroup("Item & Slot Anchors & UI Slot")]
	public ItemCollectionSlotUI SlotUI;
}
