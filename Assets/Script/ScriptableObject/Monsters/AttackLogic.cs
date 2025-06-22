using UnityEngine;

public class AttackLogic : MonoBehaviour
{
    public virtual void Execute1()
    {
        AttackOne();
    }
    public virtual void Execute2()
    {
        AttackTwo();
    }
    public virtual void Execute3()
    {
        AttackThree();
    }

    protected virtual void AttackOne()
    {

    }

    protected virtual void AttackTwo()
    {

    }

    protected virtual void AttackThree()
    {

    }
}
