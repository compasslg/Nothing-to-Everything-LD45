using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ActionCard : MonoBehaviour{
	[System.Serializable]
	private class UIDisplay{
		public Text cardName;
		public Text manaCost;
		public Image icon;
		public Text description;
	}
	
	[SerializeField]private UIDisplay display;
	public Data_ActionCard data;
	public bool interactable;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SetData(Data_ActionCard data){
		this.data = data;
		display.cardName.text = data.cardName;
		display.manaCost.text = data.manaCost.ToString();
		display.icon.sprite = data.icon;
		display.description.text = data.description;
	}
	public void Play(){
		if(interactable && GameController.instance.curState == GameController.GameState.PLAY){
			GameController.instance.MoveCard(gameObject, new Vector3(0, 0, 0), 2, 0.8f);
			GameController.instance.MoveCard(gameObject, "Graveyard", 2, 0.5f);
		}
	}
}
