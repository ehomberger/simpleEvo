using UnityEngine;
//using System;
using System.Collections;


public abstract class Organic_old : MonoBehaviour {

	public Organic prefab;
	public Material mat;
	public float reproduce;
	public string[] DNA = new string[3];
	//private needs individual;
	private int range;
	public float age;
	public float aveAge;
	private int nextAge;
	public int mutChance;
	// Use this for initialization
	public void Start ()
	{
		mat = new Material (mat);
		reproduce = 20;
		age = 0;
		mutChance = 2;
		nextAge = 1;
	}

	// Update is called once per frame
	void Update ()
	{
		age += Time.deltaTime;
		reproduce -= Time.deltaTime;
		if (age > nextAge) {
			checkDeath();
			nextAge++;
		}
		if (reproduce <= 0)
		{
			reproduce = repro();
		}
	}

	public string replace(int j, string newDNA){
		int newValue = Random.Range (0, 16);

		string newHex = newValue.ToString ("X");
		Debug.Log ("index is " + j + " length is " + newDNA.Length);
		newDNA = newDNA.Remove (j, 1);

		newDNA = newDNA.Insert(j, newHex);
		Debug.Log ("Returning " + newDNA);
		Debug.Log("old is " + this.DNA[0]);
		return newDNA;
	}

	public GameObject[] getNearby(string name)
	{
		int num = 0;
		int j = 0;
		Collider[] options = Physics.OverlapSphere (this.transform.position, 30);
		for (int i = 0; i < options.Length; i++) {
			if (options[i].gameObject.tag == name){
				num ++;
			}
		}
		GameObject[] obj = new GameObject[num];
		for (int i = 0; i < options.Length; i++) {
			if (options[i].gameObject.tag == name){
				obj[j] = options[i].gameObject;
				j++;
			}

		}

		return obj;
	}

	public void setDNA(string[] newDNA){

		for (int i = 0; i < newDNA.Length; i++){
			DNA[i] = newDNA[i];
		}
		mat.color = new Color32(System.Convert.ToByte(DNA[0], 16), System.Convert.ToByte(DNA[1], 16), System.Convert.ToByte(DNA[2], 16), 1);
	}

	public string[] getDNA(){
		return DNA;
	}

	public abstract float repro();

	public abstract void checkDeath();

}
