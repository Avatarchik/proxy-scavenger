using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;

public class Modifications : EquippableInventoryItem {

	public override LinkedList<ItemInfoRow[]> GetInfo ()
	{
		var list = new LinkedList<ItemInfoRow[]>();

		list.AddLast(new ItemInfoRow[]{
			//new ItemInfoRow("Category", category.name),
			new ItemInfoRow("Type", equipmentType.name),
		});

		var extra = new List<ItemInfoRow>(0);

		if (extra.Count > 0)
		{
			list.AddLast(extra.ToArray());
		}

		var extraProperties = new List<ItemInfoRow>();
		foreach (var property in stats)
		{
			var prop = property.stat;
			if (prop == null)
			{
				continue;
			}

			if(prop.showInUI)
			{
				if(property.isFactor && property.isSingleValue)
					extraProperties.Add(new ItemInfoRow(prop.statName, (property.floatValue - 1.0f) * 100 + "%", prop.color, prop.color));
				else
					extraProperties.Add(new ItemInfoRow(prop.statName, property.value, prop.color, prop.color));
			}
		}

		if(extraProperties.Count > 0)
			list.AddLast(extraProperties.ToArray());

		return list;
		//return base.GetInfo ();
	}
}
