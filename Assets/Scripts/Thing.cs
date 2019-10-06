using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Thing : MonoBehaviour {
	[System.Serializable]
	private class UIDisplay{
		public Text cardName;
		public Text description;
	}
	[SerializeField]private UIDisplay display;
	public bool interactable;
	public void Use(){
		if(!interactable){
			return;
		}
		GameController.instance.ActivateThingPanel(false);
		switch(display.cardName.text){
			case "Nothing":
				return;
			case "Something":
				DeckManager.Data_Thing thing = DeckManager.instance.GetRandomThing();
				SetData(thing.name, thing.description);
				break;
			case "Anything":
				GameController.instance.ActivateThingPanel(true);
				break;
			case "Get a Thing":
				GameController.instance.GetAThing();
				break;
			case "Swap a Thing":
				GameController.instance.SwapAThing();
				break;
			case "Replace a Thing":
				GameController.instance.ReplaceAThing();
				break;
			case "Steal a Thing":
				GameController.instance.StealAThing();
				break;
			case "Destroy a Thing":
				GameController.instance.DestroyAThing();
				break;
			case "Nothing for Enemy":
				GameController.instance.NothingForEnemy();
				break;
		}
		AudioManager.instance.PlaySound("Defensive Card");
	}
	
	public void SetData(string name, string description){
		display.cardName.text = name;
		display.description.text = description;
	}
	public void SetToNothing(){
		SetData("Nothing", "There is nothing you can do with this card.");
		interactable = false;
	}
}
