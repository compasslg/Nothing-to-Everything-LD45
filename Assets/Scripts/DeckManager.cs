using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DeckManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LoadActionCards(){
        StreamReader reader = new StreamReader("Assets/Data/actioncards.data");
        string[] line = null;
        string[] firstLine = reader.ReadLine().Split('\t');
        while(!reader.EndOfStream){
            line = reader.ReadLine().Split('\t');
		}
	}
}
