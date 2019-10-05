using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	public enum GameState{PAUSE, CHOOSE_THING, PLAY, IN_ANIMATION};
	private int round;
	private int level;
	private bool yourTurn;
	public static GameController instance;
	public GameState curState;
	private Random rand;
	private List<ActionCard> playerHand;
	private List<ActionCard> enemyHand;
	[System.Serializable]
	private class Board{
		public GameObject background;
		public GameObject deck;
		public GameObject graveYard;
		public Text infoArea;
		public GameObject actionCardPrefab;
	}

	private class Player{
		public Hand hand;
		
	}
	private class Enemy{
		public Hand hand;
		
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
	private Queue<CardMotion> motionQueue;
	private Player player;
	private Enemy enemy;
	// Use this for initialization
	void Start () {
		instance = this;
		curState = GameState.PLAY;
		motionQueue = new Queue<CardMotion>();
		player = new Player();
		enemy = new Enemy();
	}
	
	// Update is called once per frame
	void Update () {
		if(motionQueue.Count > 0){
			CardMotionUpdate();
		}
		UpdateInfo();
	}

	public void UpdateInfo(){
		string text = "";
		text = "Level " + level + "\n";
		text += "Round " + round + "\n";
		if(yourTurn){
			text += "Your Turn";
		}else{
			text += "Enemy Turn";
		}
		board.infoArea.text = text;
	}
	public void dealCard(int target, int count){
		GameObject actionCard = Instantiate(board.actionCardPrefab, board.deck.transform);
		Data_ActionCard cardData = DeckManager.instance.DealActionCard();
		actionCard.GetComponent<ActionCard>().SetData(cardData);
		actionCard.SetActive(false);
		if(target == 0){
			MoveCard(actionCard, "PlayerHand", 2, 0.5f);
			player.hand.cards.Add(cardData);
		}else{
			MoveCard(actionCard, "EnemyHand", 2, 0.5f);
			enemy.hand.cards.Add(cardData);
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
	public void MoveCard(GameObject card, Vector3 targetPosition, float motionSpeed, float stayTime){
		CardMotion cardMotion = new CardMotion();
		card.SetActive(true);
		card.transform.SetParent(board.background.transform);
		cardMotion.onMotion = card.transform;
		cardMotion.targetPosition = targetPosition;
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
