using System.Collections;
using Code.DungeonGeneration;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Direction DoorDirection;
    public delegate void DoorTriggered(Direction direction);
    public static event DoorTriggered OnDoorTrigger;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.CompareTag("Player"))
            return;
        //  Delayed transition prevents issues with destroying the object while inside it's method.
        Debug.Log("Door activated");
        StartCoroutine(DelayedTransition());
    }
    IEnumerator DelayedTransition()
    {
        yield return new WaitForEndOfFrame();
        OnDoorTrigger?.Invoke(this.DoorDirection);
    }
}
