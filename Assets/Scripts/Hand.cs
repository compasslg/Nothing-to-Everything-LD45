using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	private List<ActionCard> cards;
	void Start(){
		cards = new List<ActionCard>();
	}
	public void AddCard(ActionCard card){
		cards.Add(card);
	}
	public int GetCardCount(){
		return cards.Count;
	}
	public void RemoveCard(ActionCard card){
		cards.Remove(card);
	}
	public List<ActionCard> GetAllCards(){
		return cards;
	}
	public int GetMinimumManaCost(){
		if(cards.Count == 0){
			return 0;
		}
		int min = cards[0].GetData().manaCost;
		foreach(ActionCard card in cards){
			Data_ActionCard cardData = card.GetData();
			if(cardData.manaCost < min){
				min = cardData.manaCost;
			}
		}
		return min;
	}
	public void ClearHand(){
		if(cards == null){
			return;
		}
		cards.Clear();
		for(int i = 0; i < transform.childCount; i++){
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}
