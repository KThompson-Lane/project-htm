using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerAttackSO", menuName = "ScriptableObject/Player")]
public class PlayerAttackSO : ScriptableObject
{
    public int baseDamage;
    public int damage = 1;
    public float range = 0f; //todo - sort!
    public float baseRateOfFire;
    public float rateOfFire = 120f;
    
    [System.NonSerialized] public UnityEvent<int> DamageModifiedEvent;
    [System.NonSerialized] public UnityEvent<float> RoFModifiedEvent;
    
    private void OnEnable()
    {
        damage = baseDamage;
        rateOfFire = baseRateOfFire;
        // Set up events
        DamageModifiedEvent ??= new UnityEvent<int>();
        RoFModifiedEvent ??= new UnityEvent<float>();
    }
    
    public void ModifyDamage(int amount)
    {
        damage += amount;
        DamageModifiedEvent.Invoke(damage);
    }
    
    public void ModifyRoF(float amount)
    {
        rateOfFire += amount;
        RoFModifiedEvent.Invoke(rateOfFire);
    }
}
