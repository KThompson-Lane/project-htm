using Code.Runtime;
using UnityEngine;

public abstract class Attack : MonoBehaviour // Used for Enemy Attacking
{
    //private EnemyAttackSO _enemyAttackSo;
    
    protected float RateOfFire;
    protected int Damage;
    
    protected float CoolDown = 0.0f;
    protected float Interval; // time between shots
    
    [SerializeField]
    protected HealthManager healthManager; // for player health

    protected Animator Animator;

    public virtual void Start()
    {
        Interval = 60 / RateOfFire; //1 second / rate of fire
        Animator = GetComponent<Animator>();
    }

    public void SetHealthManager(HealthManager hm)
    {
        healthManager = hm;
    }
}