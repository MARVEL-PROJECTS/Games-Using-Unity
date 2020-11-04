using UnityEngine;

public class MoveBullet : MonoBehaviour {

	private int bulletSpeed = 230;
	private float hitX = -9999;		//	huge negative value that posibly wont be achieved anytime, becouse float comprasion to null is false everytime
	private float distance;
	private Enemy enemyHit;
	private int bulletDamage;

    private void Start()
    {
        bulletDamage = Player.stats.weaponDamage;
    }

    // Update is called once per frame
    private void Update ()
    {

		//	if bullet hasn`t hit anything, fly indefinetily for 1 sec
		if (hitX == -9999) {
			transform.Translate (Vector3.right * Time.deltaTime * bulletSpeed);
			Destroy (gameObject, 1);
		} else {	//	if bullet has hit
			if (Mathf.Abs(distance) >= 4) {	// move, until bullet is within 4 units
				transform.Translate (Vector3.right * Time.deltaTime * bulletSpeed);
				distance = hitX - transform.position.x;				//	update distance
			} else {	//	if bullet is near target
				Destroy(gameObject);	//	..destroy..
				if (enemyHit != null) { //  ..and if enemy was actual enemy, not level..
					enemyHit.DamageEnemy(bulletDamage);     //  .. do damage
				}
			}
		}
		//	destroy bullet for sure after 3 sec for sure if above doesnt complete for some unknown reason
		Destroy (gameObject, 3);
	}

	public void setHit (float _hitX, Enemy _hitObject)	{
		hitX = _hitX;
		enemyHit = _hitObject;
	
		distance = _hitX - transform.position.x;
	}
}
