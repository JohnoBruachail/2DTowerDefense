public abstract class State
{
    protected Hero hero;

    public abstract void Tick();

    public virtual void OnStateEnter(){ 

    }
    public virtual void OnStateExit(){ 
        
    }

    public State(Hero hero)
    {
        this.hero = hero;
    }
}