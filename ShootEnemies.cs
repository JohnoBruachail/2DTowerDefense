using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//controls the creation of new bullets that travel towards the target in range.

public class ShootEnemies : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private Monster monster;
    private GameManagerBehaviour gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        monster = gameObject.GetComponentInChildren<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange){

            float distanceToGoal = enemy.GetComponent<HeroBehaviour>().DistanceToGoal();
            if(enemy.GetComponent<HeroBehaviour>().alive == false){
                enemiesInRange.Remove(enemy);
            }
            if (distanceToGoal < minimalEnemyDistance){

                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }
        if (target != null){

            if (Time.time - lastShotTime > (monster.CurrentType.powerTier[monster.currentPowerTier].fireRate / gameManager.speedModifier))
            {
                Shoot(target.GetComponent<Collider2D>());
                lastShotTime = Time.time;
            }
            Vector3 direction = gameObject.transform.position - target.transform.position;
            gameObject.transform.rotation = Quaternion.AngleAxis(
                Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI,
                new Vector3(0, 0, 1));
        }
    }

    private void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            //add the enemy to the enemyinrange list as a potential target.
            enemiesInRange.Add(other.gameObject);
            //take the enemys enemydestructioindelegate method and assign it this classes onEnemyDestroy method
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate += OnEnemyDestroy;
            //Upon destruction of a game object, Unity calls this method automatically, and it checks whether the 
            //delegate is not null. In that case, you call it with the gameObject as a parameter. This lets all 
            //listeners that are registered as delegates know the enemy was destroyed.


            //what I actually need to do is at the moment the targets alive var is set to false remove it from the target list.
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            del.enemyDelegate -= OnEnemyDestroy;
        }
    }

    private void Shoot(Collider2D target)
    {
        GameObject bulletPrefab = monster.CurrentType.elementTier[monster.currentElementTier].bullet;

        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bulletPrefab.transform.position.z;
        targetPosition.z = bulletPrefab.transform.position.z;

        GameObject newBullet = (GameObject) Instantiate(bulletPrefab);
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;

        Animator animator = monster.CurrentType.visualization.GetComponent<Animator>();
        animator.SetTrigger("fireShot");
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }

}
