using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	public enum GameState{PAUSE, CHOOSE_THING, PLAY};
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
		public GameObject playerHand;
		public Text infoArea;
	}
	[SerializeField]private Board board;
	// Use this for initialization
	void Start () {
		instance = this;
		curState = GameState.PLAY;
	}
	
	// Update is called once per frame
	void Update () {
		
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
	public void dealCard(){
	}
}
