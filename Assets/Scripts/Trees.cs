using UnityEngine;
using System.Collections;

public class Trees : Organic {
	// Use this for initialization
	public int differences;
	string[] perfect;
	new void Start(){
		base.Start ();

		base.aveAge = 50;

		base.mat.color = new Color32(System.Convert.ToByte(DNA[0], 16), System.Convert.ToByte(DNA[1], 16), System.Convert.ToByte(DNA[2], 16), 1);
		this.GetComponent<MeshRenderer> ().material = mat;

		differences = 0;

		perfect = new string[3];
		perfect [0] = "00";
		perfect [1] = "00";
		perfect [2] = "99";

		for (int i = 0; i < perfect.Length; i++) {
			for (int j = 0; j < perfect[i].Length; j++){
				if (DNA[i][j] == perfect[i][j]){
					differences++;
				}
			}
		}
	}

	public override void checkDeath(){
		float threshold;
		threshold = 50 +(age - aveAge*2) + (differences * 2);
		int attempt;

		attempt = Random.Range (0, 100);

		if (attempt < threshold) {
			Destroy(this.gameObject);
		}

	}

	public override float repro(){
		Debug.Log ("Reproducing");
		GameObject[] options = base.getNearby ("Tree");
		Trees child;
		int random;
		float randomX, randomZ;
		string[] childDNA = new string[3];
		string[] chosen;
		string newSection = "";

		Debug.Log ("choosing mate from " + options.Length + " options " + options[0].name);
	
		if (options.Length > 1) {

			random = Random.Range (1, options.Length);
			chosen = options[random].GetComponent<Trees>().getDNA();

			for (int i = 0; i < base.DNA.Length; i++) {
				for (int j = 0; j < base.DNA[i].Length; j++) {
					if (Random.Range (0, 2) == 1) {
						newSection += base.DNA[i][j];
					} 
					else {
						newSection += chosen [i] [j];
					}

					//Debug.Log("newSection is" + newSection);

				}
				childDNA[i] = newSection;
				newSection = "";
			}
		} 
		else {
			for (int i = 0; i < this.DNA.Length; i++){
				childDNA[i] = base.DNA[i];
			}
		}


		random = Random.Range (0, 100);

		for (int i = 0; i < base.DNA.Length; i++){
			for (int j = 0; j < base.DNA[i].Length; j++){
				if(Random.Range (0, 100) < base.mutChance){
					childDNA[i] = base.replace(j, childDNA[i]);
				}
			}
		}

		randomX = Random.Range (-20, 20);
		randomZ = Random.Range (-20, 20);

		child = Instantiate (base.prefab) as Trees;

		Debug.Log ("Setting DNA to " + childDNA [0] + " " + childDNA [1] + " " + childDNA [2]);
		child.setDNA(childDNA);
		child.transform.position = new Vector3(this.transform.position.x + randomX, this.transform.position.y, this.transform.position.z + randomZ);
		mat.color = new Color32(System.Convert.ToByte(DNA[0], 16), System.Convert.ToByte(DNA[1], 16), System.Convert.ToByte(DNA[2], 16), 1);
		return 10;
	}


}
