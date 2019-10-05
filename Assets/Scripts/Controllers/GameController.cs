using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	public enum GameState{DEAL_CARD, DEALING_MOTION, PLAYER_TURN, PLAYER_MOTION, ENEMY_TURN, ENEMY_MOTION, NEXTLEVEL, GAMEOVER};
	private int round;
	private int level;
	public static GameController instance;
	public GameState curState;
	[System.Serializable]
	private class Board{
		public GameObject background;
		public GameObject deck;
		public GameObject graveYard;
		public Text infoArea;
		public GameObject actionCardPrefab;
	}
	[System.Serializable]
	private class Player{
		public Hand hand;
		public Stat hp, mp;
		public BarRefresher hpBar;
		public BarRefresher mpBar;
	}
	[System.Serializable]
	private class Enemy{
		public Hand hand;
		public Stat hp, mp;
		public BarRefresher hpBar;
		public BarRefresher mpBar;
	}
/*
	private class CardMotion{
		public Transform onMotion;
		public bool stayingCenter;
		public bool goingCenter;
		public bool goingGrave;
		public float stayTime;
		public float t;
		public float motionSpeed;
		public Vector3 initialPosition;
		public Vector3 targetPosition;
		public Transform targetParent;
	} */
	private class CardMotion{
		public Transform onMotion;
		public Vector3 initialPosition;
		public Vector3 targetPosition;
		public Transform targetParent;
		public float t;
		public float motionSpeed;
		public float stayTime;
	}
	public float gameSpeed;
	[SerializeField]private Board board;
	[SerializeField]private Player player;
	[SerializeField]private Enemy enemy;
	private Queue<CardMotion> motionQueue;
	// Use this for initialization
	void Start () {
		player.hp = new Stat("HP", 10, true);
		player.mp = new Stat("MP", 10, true);
		player.hpBar.SetStat(player.hp);
		player.mpBar.SetStat(player.mp);
		enemy.hp = new Stat("HP", 10, true);
		enemy.mp = new Stat("MP", 10, true);
		enemy.hpBar.SetStat(enemy.hp);
		enemy.mpBar.SetStat(enemy.mp);
		
		instance = this;
		curState = GameState.DEAL_CARD;
		motionQueue = new Queue<CardMotion>();
		round = 1;
		level = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(motionQueue.Count > 0){
			CardMotionUpdate();
			return;
		}
		string text = "Level " + level + "\n";
		text += "Round " + round + "\n";
		switch(curState){
			case GameState.DEAL_CARD:
				text += "Card Dealing";
				DealCard(0);
				DealCard(1);
				curState = GameState.DEALING_MOTION;
				break;
			case GameState.PLAYER_TURN:
				text += "Your Turn";
				if(enemy.hp.GetValue() <= 0){
					curState = GameState.NEXTLEVEL;
				}
				else if(player.hand.GetCardCount() == 0 || player.mp.GetValue() < player.hand.GetMinimumManaCost()){
					curState = GameState.ENEMY_TURN;
				}
				break;
			case GameState.ENEMY_TURN:
				text += "Enemy Turn";
				if(player.hp.GetValue() <= 0){
					curState = GameState.GAMEOVER;
				}
				else if(enemy.hand.GetCardCount() == 0 || enemy.mp.GetValue() < enemy.hand.GetMinimumManaCost()){
					curState = GameState.DEAL_CARD;
				}
				break;
			case GameState.DEALING_MOTION:
				curState = GameState.PLAYER_TURN;
				break;
			case GameState.GAMEOVER:
				text += "Game Over";
				break;
		}
		board.infoArea.text = text;
	}

		
	public void DealCard(int target){
		int count = 0;
		if(target == 0){
			count = round  - player.hand.GetCardCount() - 1;
		}else{
			count = round - enemy.hand.GetCardCount();
		}
		for(int i = 0; i < count; i++){
			GameObject cardObj = Instantiate(board.actionCardPrefab, board.deck.transform);
			Data_ActionCard cardData = DeckManager.instance.DealActionCard();
			ActionCard actionCard = cardObj.GetComponent<ActionCard>();
			actionCard.SetData(cardData);
			cardObj.SetActive(false);
			if(target == 0){
				actionCard.interactable = true;
				MoveCard(cardObj, "PlayerHand", 2, 0.5f);
				player.hand.AddCard(cardData);
			}else{
				MoveCard(cardObj, "EnemyHand", 2, 0.5f);
				enemy.hand.AddCard(cardData);
			}
		}
	}

	public void CardMotionUpdate(){
		CardMotion cardMotion = motionQueue.Peek();
		if(cardMotion.t < 1){
			cardMotion.t += Time.deltaTime * gameSpeed * cardMotion.motionSpeed;
			// constraint it to be less than 1
			if(cardMotion.t > 1){
				cardMotion.t = 1;
			}
			// Interporlation
			cardMotion.onMotion.position = (1-cardMotion.t) * cardMotion.initialPosition + cardMotion.t * cardMotion.targetPosition;
		}
		// staying
		else if(cardMotion.stayTime > 0){
			cardMotion.stayTime -= Time.deltaTime;
		}
		// end motion
		else{
			if(cardMotion.targetParent != null){
				cardMotion.onMotion.SetParent(cardMotion.targetParent);
			}
			motionQueue.Dequeue();
			if(motionQueue.Count > 0){
				CardMotion nextMotion = motionQueue.Peek();
				if(nextMotion.onMotion == cardMotion.onMotion){
					nextMotion.initialPosition = cardMotion.targetPosition;
					nextMotion.onMotion.SetParent(board.background.transform);
				}
			}
		}
	}
	public void PlayerPlay(ActionCard card){
		if(curState == GameState.PLAYER_TURN){
			MoveCardCenter(gameObject, 2, 0.8f);
			MoveCard(gameObject, "Graveyard", 2, 0.5f);
			player.mp.UpdateValue(-card.GetData().manaCost);
			player.hand.RemoveCard(card.GetData());
		}
	}
	public void MoveCardCenter(GameObject card, float motionSpeed, float stayTime){
		CardMotion cardMotion = new CardMotion();
		card.SetActive(true);
		card.transform.SetParent(board.background.transform);
		cardMotion.onMotion = card.transform;
		cardMotion.targetPosition = board.background.transform.position;
		cardMotion.initialPosition = card.transform.position;
		cardMotion.targetParent = null;
		cardMotion.t = 0;
		cardMotion.motionSpeed = motionSpeed;
		cardMotion.stayTime = stayTime;
		Debug.Log(card.transform.position);
		motionQueue.Enqueue(cardMotion);
	}
	public void MoveCard(GameObject card, string targetName, float motionSpeed, float stayTime){
		CardMotion cardMotion = new CardMotion();
		card.SetActive(true);
		if(targetName.Equals("PlayerHand")){
			cardMotion.targetParent = player.hand.transform;
		}else if(targetName.Equals("EnemyHand")){
			cardMotion.targetParent = enemy.hand.transform;
		}else if(targetName.Equals("Graveyard")){
			cardMotion.targetParent = board.graveYard.transform;
		}else{
			cardMotion.targetParent = board.background.transform;
		}
		card.transform.SetParent(board.background.transform);
		cardMotion.onMotion = card.transform;
		cardMotion.initialPosition = card.transform.position;
		cardMotion.targetPosition = cardMotion.targetParent.transform.position;
		cardMotion.t = 0;
		cardMotion.motionSpeed = motionSpeed;
		cardMotion.stayTime = stayTime;
		motionQueue.Enqueue(cardMotion);
	}
	

	/*
	public void CardMotionUpdate(){
		
		// moving to a target
		if(cardMotion.t < 1){
			if(cardMotion.stayingCenter){
				cardMotion.t += Time.deltaTime / cardMotion.stayTime;				
			}else{
				cardMotion.t += Time.deltaTime * gameSpeed;
				// Interporlation
				cardMotion.onMotion.localPosition = (1-cardMotion.t) * cardMotion.initialPosition + cardMotion.t * cardMotion.targetPosition;
			}
		}
		// change motion/target
		else{
			cardMotion.onMotion.localPosition = cardMotion.targetPosition;
			cardMotion.t = 0;
			
			// From going center to staying in center
			if(cardMotion.goingCenter){
				cardMotion.goingCenter = false;
				cardMotion.stayingCenter = true;
				cardMotion.t = 0;
			}
			// From going grave to finish playing
			else if(cardMotion.goingGrave){
				cardMotion.goingGrave = false;
				cardMotion.onMotion.SetParent(board.graveYard.transform);
				cardMotion.onMotion = null;
			}
			// from staying in the center to going to the grave
			else if(cardMotion.stayingCenter){
				cardMotion.t = 0;
				cardMotion.stayingCenter = false;
				cardMotion.goingGrave = true;
				cardMotion.initialPosition = transform.localPosition;
				cardMotion.targetPosition = board.graveYard.transform.localPosition;
			}
		}

	}*/
}
