using UnityEngine;
using System.Collections.Generic;

public class Trees : Organic {

	public int differences;
	private Transform treeTrunk;
	
	//string[] perfect;
	new void Start(){
		base.Start ();
		base.averageAge  = 50;
		frameShiftChance = 5; //1 is .5%, 2 is 1%, so on
		setColor();
		setReproductiveRange(15);//15 isn't anything special. worry about this later
		setTreeTrunk();
		differences = 0;

// 		perfect = new string[3];
// 		perfect [0] = "00";
// 		perfect [1] = "00";
// 		perfect [2] = "99";

// 		for (int i = 0; i < perfect.Length; i++) {
// 			for (int j = 0; j < perfect[i].Length; j++){
// 				if (DNA[i][j] == perfect[i][j]){
// 					differences++;
// 				}
//		 	}
// 		}
	}

	//
	//
	public override void checkDeath(){
		float threshold;
		threshold = 25; //placeholder value, ~25% of trees will die 
		int attempt;

		attempt = Random.Range (0, 100);

		if (attempt < threshold) {
			//Destroy(this.gameObject);
		}

	}
	//meirl
	public void tryToDie(){
		checkDeath();
	}

	// Find other trees near this one, pick one at random (that should change) 
	// and create offspring with shared DNA and possibly mutated genes. 
	// Offspring is placed on map 
	public override float reproduce(){
		Debug.Log ("Reproducing");
		List<GameObject> options = base.getNearby("Tree");
		Trees offspring;
		float randomX, randomZ;
		int   random;
		DNA   offspringDNA = new DNA(numGenes);
		DNA   chosen = new DNA(numGenes);

		Debug.Log ("choosing mate from " + options.Count + " options ");
	
		// Create new DNA string for offspring, mutations happen here
		if (options.Count > 1) {
			random = Random.Range(1, options.Count);
			chosen = options[random].GetComponent<Trees>().DNA;
			offspringDNA = DNA.fertilize(chosen);
		}
		else {
			offspringDNA = DNA;
		}
		// Run the mutations on these genes, probability of occuring is built in
		// so there's not a 100% chance that these will happen
		offspringDNA.missenseMutate();
		offspringDNA.frameShiftInsert();
		
		// Create new tree gameObject and place it on terrain without colliding
		// with other trees
		offspring = Instantiate(base.offspringPrefab) as Trees;
		offspring.transform.parent = gameObject.transform.parent;
		offspring.DNA = offspringDNA;

		randomX = Random.Range (-reproductiveRange, reproductiveRange);
		randomZ = Random.Range (-reproductiveRange, reproductiveRange);

		// Ensure the tree doesn't spawn inside of another's collder, allow 5 attempts
		// at finding a spot without another tree already there, destroy if it does
		Vector3 boxDimensions = new Vector3(5, 2, 5); //! warning magic numbers
		Vector3 spawnPosition = new Vector3(randomX, Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 0, randomZ)), randomZ);
		int attempts = 0;
		while( Physics.OverlapBox(spawnPosition, boxDimensions).Length > 1 && attempts < 5)
		{
			randomX = Random.Range (-reproductiveRange, reproductiveRange);
			randomZ = Random.Range (-reproductiveRange, reproductiveRange);
			spawnPosition.x = randomX;
			spawnPosition.z = randomZ;
			attempts++;
		}
		if (attempts == 5) Destroy(offspring);
		else offspring.transform.position = new Vector3(this.transform.position.x + randomX, this.transform.position.y, this.transform.position.z + randomZ);

		
		return 10;
	}

	// Determine the needs of the tree to survive
	public override void setNutritionalNeeds(){
		nutritionalNeeds = 0.1f * volume;
	}

	// Scale of trees is linear, should missenseMutate with some lnx function
	public override void setNutritionFactor(float root){
		nutritionFactor = Mathf.Pow(nutrition, 1.0f / root);
	}

	// Set the amount the tree changes scale every update
	public override void setDeltaScale(float top){
		deltaScale = nutritionFactor * (top / scale);
	}

	// Update the tree's scale
	public override void updateScale(){
		setDeltaScale(0.0005f);
		scale += deltaScale;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	// Initialization function to give tree initial scale
	public override void setScale(){
		scale = 0.05f;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	// Set volume of tree, used in calculating how much food the tree needs
	// Currently just based on volume of the transform child with tag "Trunk"
	// (i.e. the tree's trunk)
	public override void setVolume(){
		volume = (float)treeTrunk.GetComponent<MeshRenderer>().bounds.size.x;
		volume *= (float)treeTrunk.GetComponent<MeshRenderer>().bounds.size.z;
		volume *= (float)treeTrunk.GetComponent<MeshRenderer>().bounds.size.y;
	}

	public override void setReproductiveRange(int multiplier){
		reproductiveRange = (int)(multiplier * Mathf.Log(scale + 0.9f) + 5.0f);
	}

	public void setTreeTrunk(){
		foreach(Transform child in transform){
			if(child.tag == "Trunk"){
				treeTrunk = child;
				return;
			}
		}
	}
}


/* Gene guide
 * 
 *
 *
 *
 */

 /*
   Tree starts at school 0.25f
   Trees have nutrition from 0.000 - 1.000
   Maxium scale of tree is based on cuberoot(DNA[3])
 */

 /*
tree's nutritionFactor is fifth root of nutrition
nutritionFactor is a coefficient for the growthRate
growthRate is delta scale based on size of the tree
delta scale is 1/growthRate
change in size over time is 
 */