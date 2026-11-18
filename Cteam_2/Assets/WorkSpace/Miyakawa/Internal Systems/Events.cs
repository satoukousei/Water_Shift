using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{

    public UnityEvent EndFadein;

    public UnityEvent PlayerGoal; 

    public UnityEvent PlayerDeath;

    public UnityEvent EndGoalAnimation = new UnityEvent();

}
