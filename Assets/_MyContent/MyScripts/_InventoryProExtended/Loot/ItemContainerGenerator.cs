using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;


public partial class ItemContainerGenerator : MonoBehaviour, IInventoryItemContainerGenerator {

	public IInventoryItemContainer container { get; protected set; }

	//public InventoryItemGeneratorItem item;

	public bool generateAtGameStart = true;

	//public InventoryItemBase[] items;
	[SerializeField]
	public GenerateItemsWithoutFilter itemsToGenerate;

	public int minAmountTotal;
	public int maxAmountTotal;

	public IItemGenerator generator{ get; protected set; }

	protected void Awake()
	{
		container = GetComponent<IInventoryItemContainer>();

		//generator = new FilterGroupsItemGenerator(filterGroups);
		//generator = itemsToGenerate;
		//generator.SetItems(itemsToGenerate.GetItems());
		//generator.SetItems(items, 1);

		if (generateAtGameStart)
		{
			container.items = GenerateItems(); // Create instances is required to get stack size to work (Can't change stacksize on prefab)
			foreach (var item in container.items)
			{
				//item.transform.SetParent(transform);
			}
		}
	}

	public InventoryItemBase[] GenerateItems(){
		InventoryItemBase[] l = new InventoryItemBase[itemsToGenerate.filterGroups.Length];
		int i = 0;
		foreach(var f in itemsToGenerate.filterGroups){
			if (Random.value > f.chanceFactor)
				continue;

			var item = GameObject.Instantiate<InventoryItemBase>(f.item);
			item.transform.parent = this.transform;
			item.currentStackSize = (uint)Random.Range(f.minStackSize, f.maxStackSize);
			item.gameObject.SetActive(false);
			l[i] = item;
			i++;
		}
		return l;
	}
}
