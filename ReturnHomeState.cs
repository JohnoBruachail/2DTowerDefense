using UnityEngine;

public class ReturnHomeState : State
{
    private Vector3 destination;

    public ReturnHomeState(Hero hero) : base(hero)
    {
        
    }

    public override void Tick()
    {
        hero.MoveToward(destination);

        if (ReachedHome())
        {
            hero.SetState(new AdvanceState(hero));
        }
    }

    public override void OnStateEnter()
    {
        hero.GetComponent<Renderer>().material.color = Color.blue;
    }

    private bool ReachedHome()
    {
        return Vector3.Distance(hero.transform.position, destination) < 0.5f;
    }
}