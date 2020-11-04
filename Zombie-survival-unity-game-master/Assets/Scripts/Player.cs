using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats {
		public int maxHealth = 100;

        //  current Players health
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        //  current Player Weapon Damage
        private int _weaponDamage;
		public int weaponDamage
        {
            get { return _weaponDamage; }
            set { _weaponDamage = Mathf.Clamp(value, 0, value); }
        }

        public void Init()
        {
            curHealth = maxHealth;
            weaponDamage = 20;
        }
    }

	public static PlayerStats stats;

    private void Awake()
    {
        stats = new PlayerStats();
        stats.Init();
    }
    private void Start()
    {
        GameMaster.ui.SetHealth(stats.curHealth, stats.maxHealth);
    }
    private void Update ()
    {
		if (gameObject.transform.position.y <= -10) {
			DamagePlayer (9999);
		}

	}


	public void DamagePlayer (int _damage)
    {
		stats.curHealth -= _damage;
		AudioManager.instance.PlaySound ("PlayerHurt");
		//Debug.Log ("Damaging player for: " + _damage + " damage, health left: " + stats.curHealth);
		if (stats.curHealth <= 0) {
            GameMaster.gm.EndGame();
			GameMaster.KillPlayer (this);
		}
        GameMaster.ui.SetHealth(stats.curHealth,stats.maxHealth);
	}
}
