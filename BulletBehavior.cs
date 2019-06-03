using UnityEngine;
using System.Collections;

//controles the behavior of the bullets as they close in on the target to deal damage. Also applies the damage to target instead of the target object processing it.
public class BulletBehavior : MonoBehaviour
{

    public float speed;
    public int damage;
    public int elementType;
    public int elementTier;
    public float aoeRadius;
    public GameObject target;
    Collider2D[] AOETargets;
    public HeroBehaviour targetScript;
    public HeroBehaviour AOETargetScript;
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
        if (gameObject.transform.position.Equals(targetPosition)){

            if(elementType == 1){
                
                AOETargets = Physics2D.OverlapCircleAll (transform.position, aoeRadius, 1 << LayerMask.NameToLayer("EnemyLayer"));

                foreach(Collider2D en in AOETargets){

                    // Check if it has a rigidbody (since there is only one per enemy, on the parent).
                    Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
                    if(rb != null && rb.tag == "Enemy")
                    {
                        //Debug.Log("Applying AOE damage to several enemys");
                        // Find the Enemy script and apply aoe damage (half of normal damage).
                        AOETargetScript = rb.gameObject.GetComponent<HeroBehaviour>();
                        AOETargetScript.Damage(damage / 2, elementType);
                    }
                }
            }
            if (target != null)
            {
                targetScript.Damage(damage, elementType);
            }
            Destroy(gameObject);
        }
    }

}