using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler
{
	[CreateAssetMenu(fileName = "Data", menuName = "InteractableItem", order = 2)]
	public class InventoryInteractableItemObject : ScriptableObject 
	{
		[BoxGroup("Interactable Item")]
		public GameObject InventoryItem;
		[BoxGroup("Interactable Item")]
		public GameObject mountingObject;

		public InventoryItemBase GetInventoryItemBase(){
			InventoryItemBase i = InventoryItem.GetComponent<InventoryItemBase>();
			return i;
		}
	}
}
