using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
	private GameObject owner;
	public GameObject castAnimation;
	public GameObject executeAnimation;
	public int damage = 10;
	public int nrOfStrikes = 19;
	public float spawnRadius = 5f;
	public float damageRadius = 1f;

	private GameObject cAnimation;
	private bool casting;
	private List<GameObject> strikes = new List<GameObject>();

	void Start(){
		this.owner = gameObject;
	}

	public void startOnGameObjectCast(){
		if (this.cAnimation != null || this.casting) {
			return;
		}
		this.cAnimation = Instantiate(castAnimation, transform.position, Quaternion.identity);
		this.casting = true;
	}

	public bool isCasting(){
		return this.casting;
	}

	public void startIndicatorAnimations(Vector3 originPosition, float delay){
		for (int i = 0; i < nrOfStrikes; i++) {

			float x = Random.Range(-spawnRadius, spawnRadius);
			float y = Random.Range(-spawnRadius, spawnRadius);
			Vector3 position = originPosition + new Vector3(x,y,0);
			if (i == 0) {
				position = originPosition;
			}
			strikes.Add(Instantiate(castAnimation, position, Quaternion.identity));
		}
		StartCoroutine(executeThunder(delay));
	}

	IEnumerator executeThunder(float delay){
		yield return new WaitForSeconds(delay);

		for (int i = 0; i < strikes.Count; i++) {
			GameObject strike = strikes[i];
			strikes[i] = Instantiate(executeAnimation, strike.transform.position, Quaternion.identity);
			Destroy(strike);
		}

		if (this.cAnimation != null) {
			Destroy(this.cAnimation);
			this.cAnimation = null;
		}
		StartCoroutine(dealDamageAndCleanUp());
	}

	IEnumerator dealDamageAndCleanUp(){
		yield return new WaitForSeconds(0.5f);
		ContactFilter2D filter = new ContactFilter2D();

		for (int i = 0; i< strikes.Count; i++) {
			GameObject strike = strikes[i];
			Vector2 origin = new Vector2(strike.transform.position.x, strike.transform.position.y);
			List<Collider2D> hits = new List<Collider2D>();
			int nrOfHits = Physics2D.OverlapCircle(origin, this.damageRadius, filter.NoFilter(), hits);

			for (int j =0; j < nrOfHits; j++) {
				Collider2D hit = hits[j];
				if (hit.GetComponent<EntityController>() != null && hit.gameObject != this.owner) {
					hit.GetComponent<EntityController>().takeDamage(this.damage);
				}
			}
		}
		this.strikes = new List<GameObject>();
		this.casting = false;
	}

	void OnDestroy(){
		Destroy(this.cAnimation);
		this.cAnimation = null;
	}
}
