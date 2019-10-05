using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	private List<Data_ActionCard> cards;
	// Use this for initialization
	void Start () {
		cards = new List<Data_ActionCard>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddCard(Data_ActionCard card){
		if(cards == null){
			cards = new List<Data_ActionCard>();
		}
		cards.Add(card);
	}
	public int GetCardCount(){
		return cards.Count;
	}
	public void RemoveCard(Data_ActionCard card){
		cards.Remove(card);
	}
	public int GetMinimumManaCost(){
		if(cards.Count == 0){
			return 0;
		}
		int min = cards[0].manaCost;
		foreach(Data_ActionCard card in cards){
			if(card.manaCost < min){
				min = card.manaCost;
			}
		}
		return min;
	}
}
