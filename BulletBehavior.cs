/*

Bullet damageTypes is broken down into sub catigorys

0: normal
1: slow
2: Poison

*/


using UnityEngine;
using System.Collections;

//controles the behavior of the bullets as they close in on the target to deal damage. Also applies the damage to target instead of the target object processing it.
public class BulletBehavior : MonoBehaviour
{

    public float speed;
    public int damage;
    public int elementType;
    public int elementTier;
    public GameObject target;
    public HeroBehaviour targetScript;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private float distance;
    private float startTime;
    private GameManagerBehaviour gameManager;

    AudioSource audio;
    public AudioClip shoot;
    

    // Use this for initialization
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();

        startTime = Time.time;
        distance = Vector2.Distance(startPosition, targetPosition);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();

        targetScript = target.GetComponent<HeroBehaviour>();

        audio.PlayOneShot(shoot);
    }

    // Update is called once per frame
    void Update()
    {
        // 1 
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        // 2 
        if (gameObject.transform.position.Equals(targetPosition))
        {
            if (target != null)
            {
                targetScript.Damage(damage, elementType);
            }
            Destroy(gameObject);
        }
    }

}



/*

This code was created by me, enjoy.

*/