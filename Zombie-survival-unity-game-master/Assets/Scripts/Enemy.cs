using UnityEngine;

public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 25;
        private float _maxSpeed;
        public float maxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = Mathf.Clamp(value, 2f, 4f); }
        }
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }
        public int Damage = 10;
        private int _killScore;
        public int killScore
        {
            get { return _killScore; }
            set { _killScore = value; }
        }
  
        public void Init()
        {
            //maxSpeed = Random.Range(2f, 4f);
            curHealth = maxHealth;
            killScore = 1;
        }
    }

    public EnemyStats stats;
    [HideInInspector]
    public EnemyController enemyController;

    private void Awake()
    {
        enemyController = gameObject.GetComponent<EnemyController>();
        stats = new EnemyStats();
        stats.Init();
    }

    private void Update()
    {
        //  if somehow enemy accidentally fall of level
        if (gameObject.transform.position.y <= -10)
        {
            DamageEnemy(9999); //  kill them
        }
    }

    //  apply damage to enemy
    public void DamageEnemy(int damage)
    {
		AudioManager.instance.PlaySound ("EnemyHit");                   //  play hit sound
        stats.curHealth -= damage;                                      //  decrese health
        if (stats.curHealth <= 0)                                       //  if enemy should be dead
        {                                    
            enemyController.animator.SetTrigger("death");               //  trigger animation
            enemyController.isAlive = false;                            //  change state bool to false
            gameObject.layer = LayerMask.NameToLayer("DeadEnemies");    //  move to other layermask, for bullet colision to ignore dead bodies
            gameObject.tag = "Untagged";                                //  remove "Enemy" Tag for wave spawner to know when to start next wave
            GameMaster.KillEnemy(this);                                 //  destroy game object
            GameMaster.ui.SetScoreNumber(GameMaster.gameScore);         //  update gamescore
        }
    }
}
