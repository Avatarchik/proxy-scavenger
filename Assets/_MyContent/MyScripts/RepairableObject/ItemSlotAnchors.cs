using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class ItemSlotAnchors {
	
	[BoxGroup("Item & Slot Anchors & UI Slot")]
	public GameObject ItemAnchor;

	[BoxGroup("Item & Slot Anchors & UI Slot")]
	public GameObject SlotUIAnchor;

	[BoxGroup("Item & Slot Anchors & UI Slot")]
	public ItemCollectionSlotUI SlotUI;
}
