using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateStaticBlockOfObjects : MonoBehaviour
{
	public int density;
	public GameObject[] possibleSprites;
	public float[] chance;

	private GameObject[] sprites;

	void Awake(){

		// Generate some nice looking forest.
		// This script should take the various objects we want to generate
		// with some probability of them being loaded.
		// This script is meant for one big block of collision so only for looks.

		if (Application.IsPlaying(gameObject)) {
			Debug.Log("I'm run in play mode");
		}else {
			Debug.Log("I was run in edit mode");
			generateObjects();
		}
	}

	void OnDestroy(){
		if (sprites != null) {
			for (int i = 0; i < sprites.Length; i++) {
				if (sprites[i] != null) {
					DestroyImmediate(sprites[i]);
				}
			}
		}
	}

	private void generateObjects(){

		// x and yMargins should not have to be used.
		// xCount and yCount are the number of objects we want
		// Each new position should be based on the size of the sprite width/height used to figure out those numbers
		// Tried this but it needs some tweaks.. for some reason...
		// fml

		BoxCollider2D box = GetComponent<BoxCollider2D>();

		float spriteWidth = (possibleSprites[0].GetComponent<SpriteRenderer>().sprite.bounds.max.x - possibleSprites[0].GetComponent<SpriteRenderer>().sprite.bounds.min.x);
		float spriteHeight = (possibleSprites[0].GetComponent<SpriteRenderer>().sprite.bounds.max.y - possibleSprites[0].GetComponent<SpriteRenderer>().sprite.bounds.min.y);
		float xCount = density * (box.bounds.max.x - box.bounds.min.x) / spriteWidth;
		float yCount = density *  (box.bounds.max.x - box.bounds.min.x) / spriteHeight;
		Debug.Log("about to generate");
		Debug.Log(xCount +"*"+ yCount);
		sprites = new GameObject[(int)(xCount * yCount)];

		for (int i = 0; i < xCount; i++) {
			for (int j = 0; j < xCount; j++) {
				float r = Random.value;
				Vector3 pos =new Vector3(box.bounds.min.x+ spriteWidth/density * (i+1), box.bounds.min.y + spriteHeight/density * (j+1), 0);
				GameObject g = null;
				if (r < chance[2]) {
					g = Instantiate(possibleSprites[2], pos, Quaternion.identity);
				} else if (r < chance[1]) {
					g = Instantiate(possibleSprites[1], pos, Quaternion.identity);
				} else if (r < chance[0]) {
					g = Instantiate(possibleSprites[0], pos, Quaternion.identity);
				}
				if (g != null) {
					g.transform.parent = transform;
					sprites[sprites.Length-1] = g;
				}
			}
		}
	}
}
