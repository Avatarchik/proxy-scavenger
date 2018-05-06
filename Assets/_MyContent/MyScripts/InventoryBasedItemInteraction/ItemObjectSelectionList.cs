using UnityEngine;
using System.Collections;
using Devdog.InventoryPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace mindler
{
	[CreateAssetMenu(fileName = "Data", menuName = "InteractableItemList", order = 2)]
	public class ItemObjectSelectionList : ScriptableObject 
	{

		[BoxGroup("Interactive Inventory Item Type")]
		public InteractiveInventoryItemType Type = InteractiveInventoryItemType.None;

		[BoxGroup("Item Selection")]
		public InventoryInteractableItemObject[] ItemSelectionList;

		public InventoryInteractableItemObject[] GetItemList()
		{
			return ItemSelectionList;
		}

		public InventoryInteractableItemObject GetRandomItemFromList()
		{
			int randomItem = 0;
			if(ItemSelectionList.Length > 1)
			{
				randomItem = UnityEngine.Random.Range(0, ItemSelectionList.Length);
			}

			InventoryInteractableItemObject item = ItemSelectionList[randomItem];

			return item;
		}
	}
}
