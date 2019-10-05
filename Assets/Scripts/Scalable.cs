using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalable : MonoBehaviour {
	public bool scaleUp;
	public bool scaling;
	public float scaleSpeed;
	// Use this for initialization
	void Start () {
		scaleUp = false;
		scaling = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(scaling == false){
			return;
		}
		Vector3 oldScale = gameObject.transform.localScale;
		float change = Time.deltaTime * scaleSpeed;
		if(scaleUp){
			if(oldScale.x < 1.2){
				transform.localScale = new Vector3(oldScale.x + change, oldScale.y + change, oldScale.z);
			}else{
				transform.localScale = new Vector3(1.2f, 1.2f, 1);
				scaling = false;				
			}
		}else{
			if(oldScale.x > 1){
				transform.localScale = new Vector3(oldScale.x - change, oldScale.y - change, oldScale.z);
			}else{
				transform.localScale = new Vector3(1, 1, 1);
				scaling = false;
			}
		}
	}
	public void Scale(){
		scaleUp = true;
		scaling = true;
	}
	public void Unscale(){
		scaleUp = false;
		scaling = true;
	}
}
