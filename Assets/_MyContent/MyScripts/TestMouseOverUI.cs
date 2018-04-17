using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using com.ootii.Messages;
using Devdog.General;

public class TestMouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler// required interface when using the OnPointerEnter method.
{

	//Do this when the cursor enters the rect area of this selectable UI object.
	public void OnPointerEnter (PointerEventData eventData) 
	{
		Debug.Log("Mouse is over UI : " + eventData.pointerCurrentRaycast);
		//MessageDispatcher.SendMessage("MouseOverUI");
	}

	public void OnPointerExit (PointerEventData eventData) 
	{
		Debug.Log("Mouse is out of UI");
		//MessageDispatcher.SendMessage("MouseOutUI");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(eventData.selectedObject)
			Debug.Log("Clicked something : " + eventData.selectedObject.tag);
	}
}