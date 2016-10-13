﻿using UnityEngine;
using System.Collections.Generic;

public class Trees : Organic {
	// Use this for initialization
	public int differences;
	
	//string[] perfect;
	new void Start(){
		base.Start ();
		base.averageAge = 50;
		reproductiveRange = 30;
		frameShiftChance = 5; //1 is .5%, 2 is 1%, so on
//		base.material.color = new Color32(System.Convert.ToByte(DNA[0], 16), System.Convert.ToByte(DNA[1], 16), System.Convert.ToByte(DNA[2], 16), 1);
//		GetComponentsInChildren<SpriteRenderer>()[0].material = material;
//		GetComponentsInChildren<Renderer>[0].material;
		setColor();
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

	public override void checkDeath(){
		float threshold;
//		threshold = 50 +(age - averageAge*2) + (differences * 2);
		threshold = 75; //placeholder value, ~25% of trees will die 
		int attempt;

		attempt = Random.Range (0, 100);

		if (attempt < threshold) {
//		Destroy(this.gameObject);
		}

	}

	public override float reproduce(){
		Debug.Log ("Reproducing");
		List<GameObject> options = base.getNearby("Tree");
		Trees offspring;
		int random;
		float randomX, randomZ;
		string[] offspringDNA = new string[DNA.Length];
		string[] chosen;
		string newSection = "";

//		Debug.Log ("choosing mate from " + options.Count + " options " + options[0].name);
	
		if (options.Count > 1) {
			random = Random.Range (1, options.Count);
			chosen = options[random].GetComponent<Trees>().getDNA();

			// For each gene, pick one from either parent
			for (int i = 0; i < base.DNA.Length; i++) {
				// pick each gene from one parent
				// this should be faster than the commented code
				int geneA = (int)Random.Range(1,2);
				int geneB = (int)Random.Range(1,2);

				if( geneA == 1 ) newSection += DNA[i][0];
				else			 newSection += chosen[i][0];

				if( geneB == 1 ) newSection += DNA[i][1];
				else			 newSection += chosen[i][1]; 

				offspringDNA[i] = newSection;
				newSection = "";
			}
		}
		else {
			offspringDNA = DNA;
		}


		offspring = Instantiate (base.offspringPrefab) as Trees;
		offspring.transform.parent = gameObject.transform.parent;
		offspring.setDNA(offspringDNA);
		Debug.Log ("Setting DNA to " + offspringDNA [0] + " " + offspringDNA [1] + " " + offspringDNA [2]);

		for (int i = 0; i < base.DNA.Length; i++){
			for (int j = 0; j < base.DNA[i].Length; j++){
				if(Random.Range (0, 100) < base.mutationChance){
					offspringDNA[i] = base.replace(j, offspringDNA[i]);
				}
				if (Random.Range(0f, 200f) <= frameShiftChance){ //frameshift chance is .5
					int index = (int)Random.Range(1, DNA.Length*2) - 1;
					offspring.frameShiftInsert(index);
				}
			}
		}

		randomX = Random.Range (-reproductiveRange, reproductiveRange);
		randomZ = Random.Range (-reproductiveRange, reproductiveRange);


		// Ensure the tree doesn't spawn inside of another's collder
		Vector3 boxDimensions = new Vector3(5, 2, 5);
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
		offspring.transform.position = new Vector3(this.transform.position.x + randomX, this.transform.position.y, this.transform.position.z + randomZ);
		
		return 10;
	}

	// Scale of trees is linear, should replace with some lnx function
	public override void setNutritionFactor(){
		nutritionFactor = Mathf.Pow(nutrition, 1.0f/5.0f);
	}

	public override void setDeltaScale(float top){
		deltaScale = nutritionFactor * (top / scale);
	}

	public override void updateScale(){
		setDeltaScale(0.001f);
		scale += deltaScale;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	public override void setScale(){
		scale = 0.05f;
		transform.localScale = new Vector3(scale, scale, scale);
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