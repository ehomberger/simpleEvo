using UnityEngine;
using System.Collections;


/**
 * Ok just to keep track of DNA:
 * first segement is color. each digit is between 0 and f in hex. They determine the first digit of each RGB value.
 * second segment is average speed. I think it can only mutate up or down one.
 * third is average growth. Same as speed for now.
 * fourth starts as junk data but can grant the ability to link to other cells.
 * fifth can become the ability to survive in other cells.
 * sixth can become abillity to anchor to walls
 * seventh is defense.
 * eighth is average age before split
 * ninth is consumption method. This comes later.
 * */



public struct needs{
	public float energy;
	public float saftey;
}

public struct attributes{
	public Color color;
	public float speed;
	public float growth;
	public bool hardy;
	public bool join;
}


public class Cell : MonoBehaviour {

	public needs indi;
	public attributes attr;
	public int mutChance = 2;
	public Material mat;
	public Material baseMat;
	private string[][] DNA;
	public int DNASegments;
	private float age;
	private int def;
	private int aveAge;
	private int lastAge;
	private GameObject target;



	// Use this for initialization
	void Start () {
		DNA = new string[8][3];
		mat = new Material (baseMat);
		indi = new needs ();
		attr = new attributes ();
		age = 0;
		lastAge = 0;
	}


	void born(string[][] newDNA){
		DNA = newDNA;
		setColor ();


	}

	void setColor(){
		int red, green, blue;
		red = System.Convert.ToInt32(DNA[0][0], 16) * 16;
		green = System.Convert.ToInt32(DNA[0][1], 16) * 16;
		blue = System.Convert.ToInt32(DNA[0][2], 16) * 16;

		red += Random.Range (0, 16);
		green += Random.Range (0, 16);
		blue += Random.Range (0, 16);

		mat.color = new Color (red, green, blue);


	}

	int setSpeed(){
		return (System.Convert.ToInt32 (DNA [1]) + Random.Range (-10, 11));
	}

	int setGrowth(){
		return (System.Convert.ToInt32 (DNA [2]) + Random.Range (-10, 11));
	}

	bool checkLink(){
		return false;
	}

	bool checkHardy(){
		return false;
	}

	bool checkAnchor(){
		return false;
	}

	int setDef(){
		return (System.Convert.ToInt32 (DNA [7]) + Random.Range (-10, 11));
	}

	int setAveAge(){
		return (System.Convert.ToInt32 (DNA [8]) + Random.Range (-10, 11));
	}



	void checkDie(){
		int random = Random.Range (0, 100);
		float bar = 50 * (Mathf.Sqrt (age / aveAge));
		if (random < bar) {
			Destroy(this);
		}

	}

	int mutateValue(string oldDNA){
		int old = System.Convert.ToInt32 (oldDNA)
	}

	void devide(){
		string[][] newDNA = DNA;
		int random, newValue

		for (int j = 0; j < 3; j++){
			random = Random.Range(0, 100);
			if (random < mutChance){
				replaceChar = Random.Range(0, 16).ToString('X');
				DNA[0][j] = replaceChar;
			}
		}


		for (int j = 0; j < 3; j++){
			random = Random.Range(0, 100);
			if (random < mutChance){
				mutate
			}
		}

	}
	


	// Update is called once per frame
	void Update () {
		checkDie ();

		age += Time.deltaTime;

		if (age > lastAge + 1) {
			lastAge++;
		}


		if (lastAge > aveAge && indi.energy > 80) {
			devide();
		}
	}
}
