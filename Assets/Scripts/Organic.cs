using UnityEngine;
//using System;
using System.Collections.Generic;


public abstract class Organic : MonoBehaviour {

	// Gameobject
	public  Renderer colorChange;
	private Terrain  myTerrain;
	
	// Genetics variables
	public Organic 	  offspringPrefab;
	public string[]   DNA = new string[numGenes];
	public static int numGenes;
	public int 		  frameShiftChance; //remove this as soon as possible, it's dumb
	public int 		  mutationChance;

	// Age variables
	private int   nextAge;
	public  float age;
	public  float averageAge;
	public  float timeUntilReproduce;

	// Physical Size variables
	public float scale;
	public float deltaScale;
	public float nutritionFactor;
	public int   reproductiveRange;

	// Nurition variables
	public float nutrition;
	public float healthModifer;

	
	// Initialization
	public void Start () 
	{
		myTerrain = Terrain.activeTerrain;
		if ( !isOnTerrain () ){
			Destroy (gameObject);
		}
		
		checkStartDNA (); 	 	 // Ensures that the Organic has DNA
		setYTransform (); 	 	 // Place the organic on the map so it's pretty
		setGameObjectName (); 	 // Rename the organic so it's easy to identify
		setNutrition ();  		 // Get the organic's initial nutrition
		setNutritionFactor (5);	 // set the initial scale
		setScale ();
		timeUntilReproduce = 20;

		age = 0;
		mutationChance = 2;
		nextAge = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( isSimRunning () )
		{
			age += Time.deltaTime;// * transform.parent.GetComponent<TreeTracker> ().gameSpeed;
			timeUntilReproduce -= Time.deltaTime;
			
			if (age > nextAge) {
				setNutrition ();
				checkDeath ();
				nextAge++;
				setReproductiveRange (15);
			}
			
			updateScale ();
			
			if (timeUntilReproduce <= 0) 
			{
				timeUntilReproduce = reproduce () + Random.Range (0, 6) - 3;
			}
		}
	}

	public string replace (int j, string newDNA){
		int newValue = Random.Range (0, 16);

		string newHex = newValue.ToString ("X");
///		Debug.Log ("index is " + j + " length is " + newDNA.Length);
		newDNA = newDNA.Remove (j, 1);
	
		newDNA = newDNA.Insert (j, newHex);
///		Debug.Log ("Returning " + newDNA);
///		Debug.Log ("old is " + this.DNA[0]);

		setGameObjectName (); // set the name of the object to match update DNA

		return newDNA;
	}

	public List<GameObject> getNearby (string targetTag)
	{
		Collider[] options = Physics.OverlapSphere (this.transform.position, 30);
		List<GameObject> nearby = new List<GameObject> ();
		
		for (int i = 0; i < options.Length; i++) {
			if (options[i].gameObject.tag == targetTag) {
				nearby.Add (options[i].gameObject);
			}
		}

		return nearby;
	}

	// 
	// 
	// 
	public List<GameObject> getNearby (string targetTag, int radius)
	{
		Collider[] options = Physics.OverlapSphere (this.transform.position, radius);
		List<GameObject> nearby = new List<GameObject> ();
		
		for (int i = 0; i < options.Length; i++) {
			if (options[i].gameObject.tag == targetTag){
				nearby.Add (options[i].gameObject);
			}
		}

		return nearby;
	}

	public void setDNA (string[] newDNA){

		for (int i = 0; i < DNA.Length; i++){
			DNA[i] = newDNA[i];
		}
	}

	public string[] getDNA (){
		return DNA;
	}

	public void setYTransform (){
		Vector3 currentPosition = transform.position;
		float 	targetHeight  	= myTerrain.SampleHeight (currentPosition);
		Vector3 targetPosition  = new Vector3 (currentPosition.x, targetHeight, currentPosition.z);
		transform.position 		= targetPosition;
	}

	public void checkStartDNA (){
		if ( DNA[0].Length == 0 ){

			for ( int i = 0; i < DNA.Length; i++ ){
				int hexValue = (int)Random.Range (15.0f, 255.0f);
				DNA[i] = hexValue.ToString ("X");
			}
		}
	}	

	// Sets the Organic's material color on its color change model
	// Called once at start ()
	public void setColor (){
		colorChange.material.color = new Color32 (System.Convert.ToByte (DNA[0], 16), 
									 System.Convert.ToByte (DNA[1], 16), 
									 System.Convert.ToByte (DNA[2], 16), 1);
	}

	// Sets the Organic's name to its first 3 gene pairs
	// Called once at start ()
	public void setGameObjectName (){
		gameObject.name = DNA[0] + "" + DNA[1] + "" + DNA[2];
	}

	// Mutation for Organic's DNA. At index, inserts a random new gene from 0-F,
	// shifting all other genes to the right one position and truncates the end
	// Called during reproduce ()
	public void frameShiftInsert (int index){
///		Debug.Log ("Frameshifting tree " + name);
		string newGene = ( (int)Random.Range (0.0f, 15.0f)).ToString ("X");
		string unmodifiedDNA = string.Join ("", DNA);
		string modifiedDNA;
		
		Debug.Log ("Gene " + newGene + " is being inserted in " + unmodifiedDNA + " at " + index);
		modifiedDNA = unmodifiedDNA.Insert (index, newGene);
///		Debug.Log ("New gene is " + modifiedDNA);

		for (int i = 0; i < DNA.Length; i++){
			DNA[i] = modifiedDNA.Substring (i*2, 2);
		}

		setGameObjectName (); // set the name of the object to match update DNA
	}

	// Unfinished variation of frameShiftInsert, but deletes something
	// Not entirely sure where I was going with this
	public void frameShiftDelete (int index){
		//int newGene = (int)Random.Range (0.0f, 15.0f);
		//string currentDNA = DNA.ToString ();
	}

	// Is the Organic inisde the terrain? Return true if it is, false otherwise
	public bool isOnTerrain (){
		// get the active terrain's box collider, check if the organic object
		// exists within the bounds of the terrain
		return Terrain.activeTerrain.GetComponent<BoxCollider> ().bounds.Contains (transform.position);
	}

	// This could be incredibly inefficient
	public bool isSimRunning (){
		return transform.parent.GetComponent<TreeTracker> ().isRunning;
	}

	public void setNutrition (){
///		Debug.Log ("Setting nutrition\n");
		nutrition = myTerrain.GetComponent<NutrientMap> ().getValue (transform.position);
	}

	public abstract void  setNutritionFactor (float root);

	public abstract void  setDeltaScale (float top);

	public abstract void  updateScale ();

	public abstract void  setScale ();

	public abstract float reproduce ();

	public abstract void  setReproductiveRange (int multiplier);

	public abstract void  checkDeath ();

}