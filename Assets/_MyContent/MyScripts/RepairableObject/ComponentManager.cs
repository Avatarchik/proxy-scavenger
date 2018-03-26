using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

public class ComponentManager : MonoBehaviour {

	public ComponentPart[] ComponentParts;

	public ComponentPart[] GetComponentParts(){
		return ComponentParts;
	}
}
