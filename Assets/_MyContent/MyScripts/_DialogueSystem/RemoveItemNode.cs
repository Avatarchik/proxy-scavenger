using Devdog.General;
using Devdog.General.ThirdParty.UniLinq;
using Devdog.InventoryPro;
using Devdog.QuestSystemPro;
using Devdog.QuestSystemPro.Dialogue;

[System.Serializable]
[Category("MyProject")]
public class RemoveItemNode : ActionNodeBase {

	[ShowInNode]
	[HideGroup]
	public ItemAmountRow[] items;


	protected ItemAmountRow[] GetRows()
	{
		return items.Select(o => new ItemAmountRow(o.item, o.amount)).ToArray();
	}

	public override void OnExecute(IDialogueOwner dialogueOwner)
	{
		var inventoryItems = GetRows();
		if (InventoryManager.CanRemoveItems(inventoryItems))
		{
			foreach (var item in inventoryItems)
			{
				var i = UnityEngine.Object.Instantiate<InventoryItemBase>(item.item);
				InventoryManager.RemoveItem(i.ID,1,false);
			}

			Finish(true);
			return;
		}

		Failed(QuestManager.instance.languageDatabase.outOfRange); // Couldn't add items
	}
}
