using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using mindler.dungeonship;

namespace mindler
{
	[CreateAssetMenu(fileName = "Data", menuName = "InteractableItem", order = 2)]
	public class InventoryInteractableItemObject : ScriptableObject 
	{
		
		[BoxGroup("Interactable Item")]
		public GameObject InventoryItem;
		[BoxGroup("Interactable Item")]
		public GameObject BrokenInventoryItemVariant;

		[BoxGroup("Interactable Item Mount")]
		public GameObject mountingObject;

		[BoxGroup("Repair Item Part")]
		public RepairUnitPart RepairUnit = RepairUnitPart.None;

		[BoxGroup("Ship Part")]
		public ShipPart ShipCorePart = ShipPart.None;

		public InventoryItemBase GetInventoryItemBase(){
			InventoryItemBase i = InventoryItem.GetComponent<InventoryItemBase>();
			return i;
		}

		public InventoryItemBase GetBrokenInventoryItemBase(){
			if(BrokenInventoryItemVariant != null){
				InventoryItemBase i = BrokenInventoryItemVariant.GetComponent<InventoryItemBase>();
				return i;
			} else {
				return null;
			}
		}
	}
}
