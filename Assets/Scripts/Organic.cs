using UnityEngine;
using System.Collections.Generic;

public abstract class Organic : MonoBehaviour {

	// Gameobject
	public  Renderer colorChange;
	private Terrain  myTerrain;
	
	// Genetics variables
	public int 	   numGenes;
	public Organic offspringPrefab;
	public DNA 	   DNA;
	public int 	   frameShiftChance; //remove this as soon as possible, it's dumb
	public int 	   mutationChance;

	// Age variables
	private int   nextAge;
	public  float age;
	public  float averageAge;
	public  float timeUntilReproduce;

	// Physical Size variables
	public float scale;
	public float deltaScale;
	public int   reproductiveRange;
    public float nutritionLimiter;

	// Nurition variables
	public float nutrition;
	public float nutritionFactor;
	public float healthModifer;
	public float nutritionalNeeds;

    // Health variables
    public float     health;
    public const int deathFactor = 5;
	
	// Initialization
	public void Start () 
	{
		myTerrain = Terrain.activeTerrain;
		if ( !isOnTerrain () ){
			Destroy(this.gameObject);
		}
		
		numGenes = 5;
		DNA = new DNA(numGenes);
		setYTransform (); 	 	 // Place the organic on the map so it's pretty
		setGameObjectName (); 	 // Rename the organic so it's easy to identify
		setNutrition ();  		 // Get the organic's initial nutrition
		setNutritionFactor (5);	 // set the initial scale
		setScale ();
		timeUntilReproduce = 20;

		age = 0;
		DNA.missenseChance = 2;
		DNA.shiftInsertChance = 1;
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
			
			updateScale();
			
			if (timeUntilReproduce <= 0) 
			{
				timeUntilReproduce = reproduce() + Random.Range (0, 6) - 3;
			}
		}
	}

	public List<GameObject> getNearby (string targetTag){
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
	public List<GameObject> getNearby (string targetTag, int radius){
		Collider[] options = Physics.OverlapSphere (this.transform.position, radius);
		List<GameObject> nearby = new List<GameObject> ();
		
		for (int i = 0; i < options.Length; i++) {
			if (options[i].gameObject.tag == targetTag)
				nearby.Add (options[i].gameObject);
		}

		return nearby;
	}

	public void setYTransform (){
		Vector3 currentPosition = transform.position;
		float 	targetHeight  	= myTerrain.SampleHeight (currentPosition);
		Vector3 targetPosition  = new Vector3 (currentPosition.x, targetHeight, currentPosition.z);
		transform.position 		= targetPosition;
	}

	// Sets the Organic's material color on its color change model
	// Called once at start ()
	public void setColor (){
		colorChange.material.color = new Color32 (
			System.Convert.ToByte (DNA.chromos[0], 16), 
			System.Convert.ToByte (DNA.chromos[1], 16), 
			System.Convert.ToByte (DNA.chromos[2], 16), 1);
	}

	// Sets the Organic's name to its first 3 gene pairs
	// Called once at start ()
	public void setGameObjectName (){
		Debug.Log("set name to " + DNA.name);
		gameObject.name = DNA.name;
	}

	// Is the Organic inisde the terrain? Return true if it is, false otherwise
	public bool isOnTerrain (){
		// get the active terrain's box collider, check if the organic object
		// exists within the bounds of the terrain
		return Terrain.activeTerrain.GetComponent<BoxCollider>().bounds.Contains(transform.position);
	}

	// This could be incredibly inefficient
	public bool isSimRunning (){
		return transform.parent.GetComponent<TreeTracker> ().isRunning;
	}

	// 
	public void setNutrition (){
		/// Debug.Log ("Setting nutrition\n");
		nutrition = myTerrain.GetComponent<NutrientMap> ().getValue (transform.position);

        

        if (nutrition < nutritionalNeeds){
            nutritionLimiter = (nutritionalNeeds - nutrition) / nutritionalNeeds;
            health -= (nutritionalNeeds - nutrition) * deathFactor;
        }
        else{
            nutritionLimiter = 1;
        }
	}

	/********************* abstracts *********************/ 

	public abstract void  setNutritionFactor (float root);

	public abstract void  setDeltaScale (float top);

	public abstract void  updateScale ();

	public abstract void  setScale ();

	public abstract float reproduce ();

	public abstract void  setReproductiveRange (int multiplier);

	public abstract void  checkDeath ();

}