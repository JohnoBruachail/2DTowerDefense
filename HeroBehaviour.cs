using UnityEngine;
using System.Collections;

public class HeroBehaviour : MonoBehaviour
{
    public bool alive = true;
    public bool endOfPath;
    public int manaValue = 50;
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float normalSpeed = 1.0f;
    public float slowSpeedModifier = 2.0f;
    public bool isPoisoned;
    public bool isFrozen;
    public bool isSlowed;
    public int poisonedTimer = 0;

    private int poisonStack = 0;

    //Healthbar Info
    public float maxHealth;
    public float currentHealth;
    public bool regensHealth;
    public float healthRegen;
    public float damageModifier;
    public float armor;
    private float originalScale;
    GameObject healthBar;
    GameObject healthBarBackground;
    private GameManagerBehaviour gameManager;
    Vector3 tmpScale;

    Color32 normalHealthBarColor = new Color32(200,42,42,255);
    Color32 frozenHealthBarColor = new Color32(42,42,200,255);
    Color32 poisonedHealthBarColor = new Color32(42,200,42,255);

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
    public AudioClip explosion;
    Vector3 startPosition;
    Vector3 endPosition;
    float pathDistance;
    float totalTimeForPath;
    float currentTimeOnPath;


    // Use this for initialization
    void Start(){

        lastWaypointSwitchTime = Time.time;

        startPosition = waypoints[currentWaypoint].transform.position;
        endPosition = waypoints[currentWaypoint + 1].transform.position;
        pathDistance = Vector2.Distance(startPosition, endPosition);

        healthBar = this.transform.Find("HealthBar").gameObject;
        healthBarBackground = this.transform.Find("HealthBarBackground").gameObject;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();

        originalScale = healthBar.transform.localScale.x;
        tmpScale = healthBar.transform.localScale;

        characterUp = this.transform.Find("MaleCharacterUp").gameObject;
        characterRight = this.transform.Find("MaleCharacterRight").gameObject;
        characterDown = this.transform.Find("MaleCharacterDown").gameObject;
        characterLeft = this.transform.Find("MaleCharacterLeft").gameObject;

        audioSource = this.GetComponent<AudioSource>();

        if(regensHealth){
            InvokeRepeating("HealthRegen", 1f, 1f);
        }

        RotateIntoMoveDirection();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement(); 
    }

    void UpdateMovement(){

        //takes the start and end points of the characters current path.
        
        //Calculation used to alter speed also repositions the character.
        //could redesign to teleport or freeze character in place for a few seconds.

        if(alive){

            if(!isSlowed && !isFrozen){

                totalTimeForPath = pathDistance / normalSpeed;
                currentTimeOnPath = Time.time - lastWaypointSwitchTime;
                gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            }
            else if(isSlowed){

                totalTimeForPath = pathDistance / slowSpeedModifier;
                currentTimeOnPath = Time.time - lastWaypointSwitchTime;
                gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
            }
            if(isFrozen){
                
                lastWaypointSwitchTime += Time.deltaTime;
            }
            if (gameObject.transform.position.Equals(endPosition)){
                //if we've not reached the end of the road yet
                if (currentWaypoint < waypoints.Length - 2)
                {
                    // Switch to next waypoint
                    currentWaypoint++;
                    startPosition = waypoints[currentWaypoint].transform.position;
                    endPosition = waypoints[currentWaypoint + 1].transform.position;
                    lastWaypointSwitchTime = Time.time;
                    pathDistance = Vector2.Distance(startPosition, endPosition);

                    RotateIntoMoveDirection();
                }
                else
                {
                    //starts a series of animations and effects to for the hero to steal the gold and teleport away after reaching the treasure.
                    if(endOfPath == false){
                        endOfPath = true;
                        //hero reaches the treasure.
                        characterUp.GetComponent<Animator>().SetTrigger("StealGold");
                        characterRight.GetComponent<Animator>().SetTrigger("StealGold");
                        characterDown.GetComponent<Animator>().SetTrigger("StealGold");
                        characterLeft.GetComponent<Animator>().SetTrigger("StealGold");

                        Invoke("StealTreasure", 0.5f);
                    }
                }
            }
        }
    }

    void StealTreasure(){
        //AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        audioSource.PlayOneShot(explosion);
        gameManager.Treasure -= 50;

        teleportAnimationObject.GetComponent<Animator>().SetTrigger("GoldStolen");

        Invoke("Die", 1.0f);
        //I wish for the character to fade into trasnparency while the teleport is animated and then be deleted at the end.
    }

    void OnMouseUp()
    {
        if(gameManager.spellActive != 0){
            if(gameManager.spellActive == 1){
                ReduceArmor();
                gameManager.spellAnimator.spellAnimations[0].SetActive(false);
                gameManager.spellAnimator.spellAnimations[0].GetComponent<Animator>().SetTrigger("Cast");
            }
            else if(gameManager.spellActive == 2){
                isFrozen = true;
                healthBar.GetComponent<SpriteRenderer>().color = frozenHealthBarColor;

                Invoke("Thaw", 10);

                characterDown.GetComponent<Animator>().enabled=false;
                characterLeft.GetComponent<Animator>().enabled=false;
                characterRight.GetComponent<Animator>().enabled=false;
                characterUp.GetComponent<Animator>().enabled=false;

                gameManager.spellAnimator.spellAnimations[1].SetActive(false);
                gameManager.spellAnimator.spellAnimations[1].GetComponent<Animator>().SetTrigger("Cast");
            }
            else if(gameManager.spellActive == 3){

                startPosition = waypoints[0].transform.position;
                endPosition = waypoints[1].transform.position;
                lastWaypointSwitchTime = Time.time;
                pathDistance = Vector2.Distance(startPosition, endPosition);

                gameManager.spellAnimator.spellAnimations[2].SetActive(false);
                gameManager.spellAnimator.spellAnimations[2].GetComponent<Animator>().SetTrigger("Cast");
            }
            gameManager.spellActive = 0;
        }
    }

    public void HealthRegen(){

        if(currentHealth < maxHealth){
            currentHealth = currentHealth + healthRegen;

            tmpScale.x = currentHealth / maxHealth * originalScale;
            //the new healthbars transform local scale is applied to the health bar
            healthBar.transform.localScale = tmpScale;
        }
    }

    public void Damage(int damage, int damageType){

        //the damageModifier value removes the damage applied by the attack
        damageModifier = 100 / (100 + armor);
        //damage is applied to the health of the character
        currentHealth -= Mathf.Max(damage, 0) * damageModifier;
        //the healthbars original scale is multiplied by the original scale of the healthbar to scale it correctly.
        tmpScale.x = currentHealth / maxHealth * originalScale;
        //the new healthbars transform local scale is applied to the health bar
        healthBar.transform.localScale = tmpScale;

        if(damageType == 2 && !isSlowed){
            isSlowed = true;


            startPosition = transform.position;          
            lastWaypointSwitchTime = Time.time;
            pathDistance = Vector2.Distance(startPosition, endPosition);

            Invoke("NormalSpeed", 5);
        }
        else if(damageType == 3){

            if(isPoisoned == false){
                isPoisoned = true;
                poisonedTimer = 0;
                poisonStack = 1;
                Debug.Log("Poison Applied");
                InvokeRepeating("ApplyPoison", 1f, 1f);
                healthBar.GetComponent<SpriteRenderer>().color = poisonedHealthBarColor;
            }
            else{
                Debug.Log("Poison Stack increase and timer reset");
                poisonedTimer = 0;
                if(poisonStack < 10){
                    poisonStack++;
                }
            }
        }

        if (alive && currentHealth <= 0){

            gameManager.Mana += manaValue * gameManager.manaModifier;
            gameManager.heroesSlain++;
            //AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

            characterUp.GetComponent<Animator>().SetTrigger("Kill");
            characterRight.GetComponent<Animator>().SetTrigger("Kill");
            characterDown.GetComponent<Animator>().SetTrigger("Kill");
            characterLeft.GetComponent<Animator>().SetTrigger("Kill");
            
            alive = false;
            healthBar.SetActive(false);
            healthBarBackground.SetActive(false);
            Invoke("Die", 5);
        }
    }

    private void Die(){
        Destroy(gameObject);
    }

    //when hit with a poison attack isPoisoned becomes true.
    //when first poisoned the apply poison method is invoked repeating.
    //a stack of poison is then added to the character for each poison attack and reset the timer.
    //each invoke will deal poison damage and increase the timer.

    private void ApplyPoison(){
        poisonedTimer++;
        Debug.Log("Applying Poison Damage: " + Mathf.Max(poisonStack, 0) + "When stacks are: " + poisonStack);
        currentHealth -= Mathf.Max(poisonStack, 0);
        tmpScale.x = currentHealth / maxHealth * originalScale;
        healthBar.transform.localScale = tmpScale;

        if(poisonedTimer > 8){
            Debug.Log("Poison Effect Expired");
            CancelInvoke("ApplyPoison");
            isPoisoned = false;
            healthBar.GetComponent<SpriteRenderer>().color = normalHealthBarColor;
        }
    }

    private void ReduceArmor(){
        Debug.Log("Armor Reduced");
        armor = 0;
    }

    private void NormalSpeed(){
        Debug.Log("Slow Removed");
        CancelInvoke("NormalSpeed");

        startPosition = transform.position;          
        lastWaypointSwitchTime = Time.time;
        pathDistance = Vector2.Distance(startPosition, endPosition);

        isSlowed = false;
    }

    private void Thaw(){
        Debug.Log("Frozen Removed");

        isFrozen = false;
        healthBar.GetComponent<SpriteRenderer>().color = normalHealthBarColor;

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

        //Debug.Log("******************************************************");

        //Debug.Log("Start position x:" + newStartPosition.x + " End position x:" + newEndPosition.x);
        //Debug.Log("Start position y:" + newStartPosition.y + " End position y:" + newEndPosition.y);

        //Debug.Log("Difference in x:" + differenceX + " Difference in y:" + differenceY);

        

        //disable character sprites
        characterUp.SetActive(false);
        characterRight.SetActive(false);
        characterDown.SetActive(false);
        characterLeft.SetActive(false);

        //depending on direction and distance of next waypoint activate correctly orentated character sprite.
        if(differenceX > differenceY){
            if(newEndPosition.x < newStartPosition.x){
                //Debug.Log("Go Left");
                characterLeft.SetActive(true);
            }
            else{
                //Debug.Log("Go Right");
                characterRight.SetActive(true);
            }
        }
        else{
            if(newEndPosition.y < newStartPosition.y){
                //Debug.Log("Go Down");
                characterDown.SetActive(true);
            }
            else{
                //Debug.Log("Go Up");
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
