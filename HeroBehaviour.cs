using UnityEngine;
using System.Collections;

public class HeroBehaviour : MonoBehaviour
{
    public bool alive = true;
    public int goldValue = 50;
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float normalSpeed = 1.0f;
    public float currentSpeed = 1.0f;
    public bool isPoisoned;
    public bool isFrozen;
    public int poisonedTimer = 0;

    //Healthbar Info
    public float maxHealth;
    public float currentHealth;
    public float damageModifier;
    public float armor;
    private float originalScale;
    GameObject healthBar;
    private GameManagerBehaviour gameManager;
    Vector3 tmpScale;

    private int state;

    //Current States
    //walkingUp
    //walkingLeft
    //walkingDown
    //walkingRight
    //Dead




    public GameObject teleportAnimationObject;
    private GameObject characterUp;
    private GameObject characterRight;
    private GameObject characterDown;
    private GameObject characterLeft;
    AudioSource audioSource;
    Vector3 startPosition;
    Vector3 endPosition;
    float pathLength;
    float totalTimeForPath;
    float currentTimeOnPath;


    // Use this for initialization
    void Start(){

        lastWaypointSwitchTime = Time.time;

        healthBar = this.transform.Find("HealthBar").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();

        originalScale = healthBar.transform.localScale.x;
        tmpScale = healthBar.transform.localScale;

        characterUp = this.transform.Find("MaleCharacterUp").gameObject;
        characterRight = this.transform.Find("MaleCharacterRight").gameObject;
        characterDown = this.transform.Find("MaleCharacterDown").gameObject;
        characterLeft = this.transform.Find("MaleCharacterLeft").gameObject;

        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        HealthUpdate();

        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;

        //Calculation used to alter speed also repositions the character.

        //could redesign or freeze character in place for a few seconds.

        currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        if(alive){

            if(isFrozen){
                
                lastWaypointSwitchTime += Time.deltaTime;
                characterDown.GetComponent<Animator>().enabled=false;
                characterLeft.GetComponent<Animator>().enabled=false;
                characterRight.GetComponent<Animator>().enabled=false;
                characterUp.GetComponent<Animator>().enabled=false;
                
            }
            if(!isFrozen){
                //t += Time.deltaTime;
                pathLength = Vector2.Distance(startPosition, endPosition);
                totalTimeForPath = pathLength / currentSpeed;
                currentTimeOnPath = Time.time - lastWaypointSwitchTime;
                gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            }

            if (gameObject.transform.position.Equals(endPosition)){
                //if we've not reached the end of the road yet
                if (currentWaypoint < waypoints.Length - 2)
                {
                    // Switch to next waypoint
                    currentWaypoint++;
                    lastWaypointSwitchTime = Time.time;

                    RotateIntoMoveDirection();
                }
                else
                {
                    //hero reaches the treasure.

                    teleportAnimationObject.SetActive(true);
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

                    //GameManagerBehavior gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
                    gameManager.Health -= 1;
                    
                }
            }
        }
    }

    public void damage(int damage, int damageType){

        damageModifier = 100 / (100 + armor);
        currentHealth -= Mathf.Max(damage, 0) * damageModifier;
        tmpScale.x = currentHealth / maxHealth * originalScale;
        healthBar.transform.localScale = tmpScale;

        if(damageType == 2){
            isFrozen = true;
            Invoke("Thaw", 1);
        }
        else if(damageType == 3){
            isPoisoned = true;
        }

        if (alive && currentHealth <= 0){

            gameManager.Gold += goldValue;
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

            characterUp.GetComponent<Animator>().SetTrigger("Kill");
            characterRight.GetComponent<Animator>().SetTrigger("Kill");
            characterDown.GetComponent<Animator>().SetTrigger("Kill");
            characterLeft.GetComponent<Animator>().SetTrigger("Kill");
            
            alive = false;
            Invoke("Die", 5);
        }
    }

    private void Die(){
        Destroy(gameObject);
    }

    private void HealthUpdate(){

        if(isPoisoned == true){
            poisonedTimer = 0;
            Debug.Log("Poison Applied");
            InvokeRepeating("ApplyPoison", 1f, 1f);
            isPoisoned = false;
        }
    }

    private void ApplyPoison(){
        Debug.Log("Applying Poison Damage");
        currentHealth -= Mathf.Max(5, 0);
        tmpScale.x = currentHealth / maxHealth * originalScale;
        healthBar.transform.localScale = tmpScale;

        if(poisonedTimer > 3){
            CancelInvoke("ApplyPoison");
        }
        poisonedTimer++;
    }

    private void Thaw(){
        Debug.Log("Frozen Removed");
        CancelInvoke("Thaw");
        isFrozen = false;
            characterDown.GetComponent<Animator>().enabled=true;
            characterLeft.GetComponent<Animator>().enabled=true;
            characterRight.GetComponent<Animator>().enabled=true;
            characterUp.GetComponent<Animator>().enabled=true;
    }

    //how character is rotated after hitting a waypoint
    private void RotateIntoMoveDirection()
    {
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);

        float differenceX = Mathf.Abs(newStartPosition.x - newEndPosition.x);
        float differenceY = Mathf.Abs(newStartPosition.y - newEndPosition.y);

        Debug.Log("******************************************************");

        Debug.Log("Start position x:" + newStartPosition.x + " End position x:" + newEndPosition.x);
        Debug.Log("Start position y:" + newStartPosition.y + " End position y:" + newEndPosition.y);

        Debug.Log("Difference in x:" + differenceX + " Difference in y:" + differenceY);

        

        //disable character sprites
        characterUp.SetActive(false);
        characterRight.SetActive(false);
        characterDown.SetActive(false);
        characterLeft.SetActive(false);

        //depending on direction and distance of next waypoint activate correctly orentated character sprite.
        if(differenceX > differenceY){
            if(newEndPosition.x < newStartPosition.x){
                Debug.Log("Go Left");
                characterLeft.SetActive(true);
            }
            else{
                Debug.Log("Go Right");
                characterRight.SetActive(true);
            }
        }
        else{
            if(newEndPosition.y < newStartPosition.y){
                Debug.Log("Go Down");
                characterDown.SetActive(true);
            }
            else{
                Debug.Log("Go Up");
                characterUp.SetActive(true);
            }
        }
    }

    public float DistanceToGoal()
    {
        float distance = 0;
        distance += Vector2.Distance(
            gameObject.transform.position,
            waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }

}
