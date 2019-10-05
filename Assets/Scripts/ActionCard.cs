using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionCard : MonoBehaviour {
	public Text cardName;
	public Text manaCost;
	public Image icon;
	public Text description;
	public Data_ActionCard data;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetData(Data_ActionCard data){
		this.data = data;
		cardName.text = data.cardName;
		manaCost.text = data.manaCost.ToString();
		icon.sprite = data.icon;
		description.text = data.description;
	}
}
