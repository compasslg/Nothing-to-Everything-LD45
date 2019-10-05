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
	private Color defaultColor;
	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<Image>().color;
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
	public Data_ActionCard GetData(){
		return data;
	}
	public void Play(){
		if(!interactable){
			return;
		}
		if(!GameController.instance.PlayerPlay(this)){
			GetComponent<Image>().color = Color.red;
		}
	}
	public void ReturnNormalColor(){
		GetComponent<Image>().color = defaultColor;
	}
}
