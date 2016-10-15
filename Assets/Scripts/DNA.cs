using UnityEngine;
using System.Collections.Generic;

public class DNA {

    public static int numGenes;
	public int Length = numGenes;
	public static int Width = 2;
    public string[] chromos;
	public string name;

    public int missenseChance;
    public int shiftInsertChance;
    public int shiftDeleteChance;
	public int reversalChance;
	public int duplicationChance;
	public int nonsenseChance;
	public int repeatExpansionChance;

    public DNA(int j) {
        numGenes = j;
		chromos = new string[j];
		checkChromos();
    }


	public void setDNA (string[] newChromos){
		for (int i = 0; i < numGenes; i++) 
			chromos[i] = newChromos[i];
	}

	public string[] getDNA (){
		return chromos;
	}
	
	public void checkChromos (){

		if ( chromos[0].IsNullOrWhiteSpace() ){
			for ( int i = 0; i < numGenes; i++ ){
				int hexValue = (int)Random.Range (0.0f, 255.0f);
				chromos[i] = hexValue.ToString ("X2");
			}

			setName();
		}
	}

	public void checkDNA (){
		checkChromos();
	}

    /********************* Mutations *********************/ 
	 // ✓ missenseMutate
	 // ✓ frameShiftInsert
	 // ✗ frameShiftDelete / deletion
	 // ✗ geneReversal
	 // ✗ duplication
	 // ✗ repeatExpansion
	 // ✗ nonsenseMutate

	// In given DNA string, at position j, pick new hex value at random, replace
	// the gene at j with this new shiny ranom value. Returns that new string
	public void missenseMutate (int gene, int index){
		int    newValue = Random.Range (0, 16);
		string newHex   = newValue.ToString ("X");
		string original = this.chromos[gene];

		chromos[gene] = chromos[gene].Remove (index, 1);
		chromos[gene] = chromos[gene].Insert (index, newHex);
		setName ();
		
		Debug.Log("Missense Mutation occurred, " + original + " is now " + chromos[gene]);
	}

	// Calls missenseMutate on random gene, with classes's missenseChance
	public void missenseMutate() {
		int random = Random.Range(0, 100);

		if (random <= missenseChance){
			int gene  = Random.Range(0, numGenes);
			int alele = Random.Range(0, Width);	
			Debug.Log("Missensed at " + alele);
			missenseMutate(gene, alele);
		}
	}

	// Mutation for Organic's DNA. At index, inserts a random new gene from 0-F,
	// shifting all other genes to the right one position and truncates the end
	// Called during reproduce ()
	public void frameShiftInsert (int index){
		string newGene = ((int)Random.Range (0.0f, 15.0f)).ToString ("X");
		string unmodifiedDNA = string.Join ("", chromos);
		string modifiedDNA;
		
		modifiedDNA = unmodifiedDNA.Insert (index, newGene);

		for (int i = 0; i < numGenes - 1; i++){
			chromos[i] = modifiedDNA.Substring (i*2, 2);
		}

		setName(); // set the name of the object to match update DNA
		
		Debug.Log("Insertion Frameshift occurred, " + newGene + " was inserted at "
				  + index);
	}

	// Calls frameShiftInsert on random gene, with classes's shiftInsertChance
	public void frameShiftInsert(){
		int random = Random.Range(0, 100);

		if (random <= shiftInsertChance){
			int alele = Random.Range(0, Width*numGenes - 1);
			Debug.Log("Frameshifted at " + alele);	
			frameShiftInsert(alele);
		}
	}

	// Unfinished variation of frameShiftInsert, but deletes something
	// Not entirely sure where I was going with this
	public void frameShiftDelete (int index){
		//int newGene = (int)Random.Range (0.0f, 15.0f);
		//string currentDNA = DNA.ToString ();
	}


	public void setName(string newName){
		name = newName;
	}

	public void setName(){
		name = chromos[0] + "" + chromos[1] + "" + chromos[2];
	}

	public override string ToString(){
		return "" + chromos[0] + "" + chromos[1] + "" + chromos[2];
	}
}

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
        }
        return true;
    }
}