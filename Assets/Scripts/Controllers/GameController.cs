using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {
	public enum GameState{DEAL_CARD, DEALING_MOTION, PLAYER_TURN, PLAYER_MOTION, ENEMY_TURN, ENEMY_MOTION, NEXTLEVEL, GAMEOVER, VICTORY};
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
		public Text thingInfoArea;
		public GameObject actionCardPrefab;
	}
	[System.Serializable]
	private class Player{
		public Hand hand;
		public Stat hp, mp;
		public BarRefresher hpBar;
		public BarRefresher mpBar;
		public bool dodge;
		public int dmgBlock;
		public Text statusInfo;
	}
	private class CardMotion{
		public Transform onMotion;
		public Vector3 initialPosition;
		public Vector3 targetPosition;
		public Transform targetParent;
		public float t;
		public float motionSpeed;
		public float stayTime;
		public bool applyEffect;
	}
	public float gameSpeed;
	[SerializeField]private Board board;
	[SerializeField]private Player player, enemy;
	[SerializeField]private GameObject thingPanel;
	[SerializeField]private Thing thing;
	[SerializeField]private Image nextLevelBlock;
	[SerializeField]private Text nextLevelText;
	[SerializeField]private GameObject endTurnButton;
	[SerializeField]private CanvasGroup victoryPanel, gameOverPanel;
	private Queue<CardMotion> motionQueue;
	// Use this for initialization
	void Start () {
		player.hp = new Stat("HP", 20, true);
		player.mp = new Stat("MP", 7, true);
		player.hpBar.SetStat(player.hp);
		player.mpBar.SetStat(player.mp);
		enemy.hp = new Stat("HP", 14, true);
		enemy.mp = new Stat("MP", 7, true);
		enemy.hpBar.SetStat(enemy.hp);
		enemy.mpBar.SetStat(enemy.mp);
		
		instance = this;
		curState = GameState.NEXTLEVEL;
		motionQueue = new Queue<CardMotion>();
		round = 0;
		level = 0;
		StartCoroutine(NextLevelUpdate());
	}
	
	// Update is called once per frame
	void Update () {
		if(motionQueue.Count > 0){
			endTurnButton.SetActive(false);
			CardMotionUpdate();
			return;
		}
		board.thingInfoArea.text = "";
		string text = "Level " + level + "\n";
		text += "Round " + round + "\n";
		switch(curState){
			case GameState.DEAL_CARD:
				round++;
				// default mana regen
				if(level > 2){
					thing.SetData("Anything", "You can choose any thing you want.");
				}
				else if(level > 1){
					thing.SetData("Something", "Get a random thing.");
				}else{
					thing.SetData("Nothing", "There is nothing you can do with this card.");
				}
				player.mp.UpdateValue(5);
				enemy.mp.UpdateValue(5);

				text += "Card Dealing";
				DealCard(0);
				DealCard(1);
				curState = GameState.DEALING_MOTION;
				break;
			case GameState.PLAYER_TURN:
				text += "Your Turn";
				if(enemy.hp.GetValue() <= 0){
					if(level == 10){
						victoryPanel.gameObject.SetActive(true);
						curState = GameState.VICTORY;
						return;
					}
					curState = GameState.NEXTLEVEL;
					StartCoroutine(NextLevelUpdate());
				}else{
					endTurnButton.SetActive(true);
				}
				break;
			case GameState.ENEMY_TURN:
				text += "Enemy Turn";
				if(player.hp.GetValue() <= 0){
					curState = GameState.GAMEOVER;
					gameOverPanel.gameObject.SetActive(true);
				}
				else if(enemy.hand.GetCardCount() == 0 || enemy.mp.GetValue() < enemy.hand.GetMinimumManaCost()){
					curState = GameState.DEAL_CARD;
				}else{
					EnemyAI();
				}
				break;
			case GameState.DEALING_MOTION:
				curState = GameState.PLAYER_TURN;
				break;
			case GameState.GAMEOVER:
				text += "Game Over";
				gameOverPanel.alpha += gameSpeed * Time.deltaTime;
				break;
			case GameState.VICTORY:
				text += "Victory";
				victoryPanel.alpha += gameSpeed * Time.deltaTime;
				break;
		}
		board.infoArea.text = text;
	}


	IEnumerator NextLevelUpdate(){
		level++;
		round = 0;
		nextLevelBlock.gameObject.SetActive(true);
		nextLevelText.gameObject.SetActive(true);
		nextLevelText.text = "Level " + level;
		float t0 = 0;
		while(t0 < 1 && level != 1){
			t0 += 0.05f * gameSpeed;
			if(t0 > 1){
				t0 = 1;
			}
			nextLevelBlock.color = (1 - t0) * Color.clear + t0 * Color.white;
			nextLevelText.color = (1 - t0) * Color.clear + t0 * Color.white;
			yield return new WaitForSeconds(0.05f);
		}
		ClearGraveyard();
		ClearTarget(player);
		ClearTarget(enemy);
		thing.interactable = true;
		yield return new WaitForSeconds(0.5f);
		float t1 = 0;
		while(t1 < 1){
			t1 += 0.05f * gameSpeed;
			if(t1 > 1){
				t1 = 1;
			}
			nextLevelBlock.color = (1 - t1) * Color.white + Color.clear;
			nextLevelText.color = (1 - t1) * Color.clear + Color.clear;
			yield return new WaitForSeconds(0.05f);
		}
		nextLevelBlock.gameObject.SetActive(false);
		nextLevelText.gameObject.SetActive(false);
		curState = GameState.DEAL_CARD;

	}
	private void ClearGraveyard(){
		for(int i = 0; i < board.graveYard.transform.childCount; i++){
			Destroy(board.graveYard.transform.GetChild(i).gameObject);
		}
	}
	private void ClearTarget(Player target){
		int extraHealth = level * 4;
		if(target == player){
			extraHealth += 6;
		}
		target.hp.SetBestValue(10 + extraHealth);
		target.mp.SetBestValue(6 + level);
		target.hp.SetValue(target.hp.GetBestValue());
		target.mp.SetValue(target.mp.GetBestValue());
		target.dodge = false;
		target.dmgBlock = 0;
		target.statusInfo.text = "";
		target.hand.ClearHand();
	}
	private void UpdateStatusInfo(Player target){
		string text = "";
		if(target.dodge){
			text += "Dodge next attack.\n";
		}
		if(target.dmgBlock > 0){
			text += "Block " + target.dmgBlock + " damage.";
		}
		target.statusInfo.text = text;
	}
	public void DealCard(int target){
		int count = Mathf.Min(round, 4);
		if(target == 0){
			count -= player.hand.GetCardCount() + 1;
			//Debug.Log("Player " + target + " has " + player.hand.GetCardCount() + "cards.");
		}else{
			count -= enemy.hand.GetCardCount();
			//Debug.Log("Player " + target + " has " + enemy.hand.GetCardCount() + "cards.");
		}
		Debug.Log("Deal " + count + " cards to Player " + target);
		for(int i = 0; i < count; i++){
			GameObject cardObj = Instantiate(board.actionCardPrefab, board.deck.transform);
			Data_ActionCard cardData = DeckManager.instance.DealActionCard();
			ActionCard actionCard = cardObj.GetComponent<ActionCard>();
			actionCard.SetData(cardData);
			cardObj.SetActive(false);
			if(target == 0){
				actionCard.interactable = true;
				MoveCard(cardObj, "PlayerHand", 2, 0.5f);
				Debug.Log("Player has " + enemy.hand.GetCardCount() + " cards");
				player.hand.AddCard(actionCard);
			}else{
				MoveCard(cardObj, "EnemyHand", 2, 0.5f);
				enemy.hand.AddCard(actionCard);
				Debug.Log("Enemy has " + enemy.hand.GetCardCount() + " cards");
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
			if(cardMotion.applyEffect){
				Data_ActionCard card = cardMotion.onMotion.GetComponent<ActionCard>().GetData();
				Player cardUser, opponent;
				if(curState == GameState.PLAYER_TURN){
					cardUser = player;
					opponent = enemy;
				}else{
					cardUser = enemy;
					opponent = player;
				}
				// apply effect
				if(card.cardType.Equals("Offense")){
					if(opponent.dodge){
						opponent.dodge = false;
					}else if(opponent.dmgBlock > 0){
						int nextDmg = card.enemyHPLoss - opponent.dmgBlock;
						opponent.dmgBlock = 0;
						if(nextDmg < 0){
							nextDmg = 0;
						}
						opponent.hp.UpdateValue(-nextDmg);
						opponent.mp.UpdateValue(-card.enemyMPLoss);
					}else{
						opponent.hp.UpdateValue(-card.enemyHPLoss);
						opponent.mp.UpdateValue(-card.enemyMPLoss);
					}
				}else{
					cardUser.hp.UpdateValue(card.selfHPRegen);
					cardUser.mp.UpdateValue(card.selfMPRegen);
					if(!cardUser.dodge){
						cardUser.dodge = card.evasion;
					}
					if(cardUser.dmgBlock <= 0){
						cardUser.dmgBlock = card.dmgBlock;
					}
				}
				
				UpdateStatusInfo(enemy);
				UpdateStatusInfo(player);
			}
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
	public void EndTurn(){
		if(curState == GameState.PLAYER_TURN){
			curState = GameState.ENEMY_TURN;
		}
		endTurnButton.SetActive(false);
	}
	public bool PlayerPlay(ActionCard card){
		if(thingPanel.activeSelf){
			return false;
		}
		Data_ActionCard cardData = card.GetData();
		if(curState == GameState.PLAYER_TURN && player.mp.GetValue() >= cardData.manaCost){
			card.interactable = false;
			MoveCardCenter(card.gameObject, 2, 0.8f, true);
			MoveCard(card.gameObject, "Graveyard", 2, 0.5f);
			player.mp.UpdateValue(-cardData.manaCost);
			player.hand.RemoveCard(card);
			Debug.Log("You use '" + card.GetData().cardName + "', \nand he now has "  + player.hand.GetCardCount() + " cards.");
			return true;
		}
		return false;
	}
	
	public bool EnemyPlay(ActionCard card){
		Data_ActionCard cardData = card.GetData();
		if(curState == GameState.ENEMY_TURN && enemy.mp.GetValue() >= cardData.manaCost){
			MoveCardCenter(card.gameObject, 2, 0.8f, true);
			MoveCard(card.gameObject, "Graveyard", 2, 0.5f);
			enemy.mp.UpdateValue(-cardData.manaCost);
			enemy.hand.RemoveCard(card);
			return true;
		}
		return false;
	}

	private void EnemyAI(){
		List<ActionCard> cards = enemy.hand.GetAllCards();
		foreach(ActionCard card in cards){
			if(EnemyPlay(card)){
				Debug.Log("Enemy use '" + card.GetData().cardName + "', \nand he now has "  + cards.Count + " cards.");
				break; 
			}
		}
	}
	public void MoveCardCenter(GameObject card, float motionSpeed, float stayTime, bool applyEffect){
		CardMotion cardMotion = new CardMotion();
		card.SetActive(true);
		card.GetComponent<Scalable>().isScalable = false;
		card.transform.SetParent(board.background.transform);
		cardMotion.onMotion = card.transform;
		cardMotion.targetPosition = board.background.transform.position;
		cardMotion.initialPosition = card.transform.position;
		cardMotion.targetParent = null;
		cardMotion.t = 0;
		cardMotion.motionSpeed = motionSpeed;
		cardMotion.stayTime = stayTime;
		cardMotion.applyEffect = applyEffect;
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
		cardMotion.applyEffect = false;
		motionQueue.Enqueue(cardMotion);
	}


	public void ActivateThingPanel(bool active){
		thingPanel.SetActive(active);
	}
	public bool SwapAThing(){
		board.thingInfoArea.text = "Swap a Thing";
		List<ActionCard> enemyCards = enemy.hand.GetAllCards();
		List<ActionCard> playerCards = player.hand.GetAllCards();
		if(playerCards.Count == 0 || enemyCards.Count == 0){
			return false;
		}
		ActionCard enemyCard = enemyCards[Random.Range(0, enemyCards.Count)];
		ActionCard playerCard = playerCards[Random.Range(0, playerCards.Count)];
		enemyCards.Remove(enemyCard);
		playerCards.Remove(playerCard);
		enemyCards.Add(playerCard);
		playerCards.Add(enemyCard);
		enemyCard.interactable = true;
		playerCard.interactable = false;
		MoveCard(enemyCard.gameObject, "PlayerHand", 3, 0);
		MoveCard(playerCard.gameObject, "EnemyHand", 3, 0);
		return true;
	}
	public void DestroyAThing(){
		board.thingInfoArea.text = "Destroy a Thing";
		List<ActionCard> enemyCards = enemy.hand.GetAllCards();
		if(enemyCards.Count == 0){
			return;
		}
		ActionCard enemyCard = enemyCards[Random.Range(0, enemyCards.Count)];
		enemyCards.Remove(enemyCard);
		MoveCard(enemyCard.gameObject, "Graveyard", 3, 0.5f);

	}
	public void GetAThing(){
		board.thingInfoArea.text = "Get a Thing";
		DealCard(0);
	}
	public void StealAThing(){
		board.thingInfoArea.text = "Steal a Thing";
		List<ActionCard> enemyCards = enemy.hand.GetAllCards();
		if(enemyCards.Count == 0){
			return;
		}
		ActionCard enemyCard = enemyCards[Random.Range(0, enemyCards.Count)];
		enemyCards.Remove(enemyCard);
		enemyCard.interactable = true;
		MoveCard(enemyCard.gameObject, "PlayerHand", 3, 0);
		player.hand.AddCard(enemyCard);
	}
	public void NothingForEnemy(){
		board.thingInfoArea.text = "Nothing for Enemy";
		List<ActionCard> enemyCards = enemy.hand.GetAllCards();
		foreach(ActionCard card in enemyCards){
			MoveCard(card.gameObject, "Graveyard", 4, 0);
		}
		enemyCards.Clear();
	}
	public void ReplaceAThing(){
		board.thingInfoArea.text = "Replace a Thing";
		List<ActionCard> playerCards = player.hand.GetAllCards();
		if(playerCards.Count == 0){
			return;
		}
		ActionCard card = playerCards[Random.Range(0, playerCards.Count)];
		playerCards.Remove(card);
		MoveCard(card.gameObject, "Graveyard", 3, 0.5f);
		DealCard(0);
	}
	public void TitleScreen(){
		SceneManager.LoadScene("Title");
	}
	
}
