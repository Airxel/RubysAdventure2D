using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesContainer : MonoBehaviour
{
    [SerializeField]
    public int totalEnemies;

    [SerializeField]
    public int remainingEnemies;

    [SerializeField]
    public string enemiesCount;

    public static EnemiesContainer instance {  get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        remainingEnemies = totalEnemies;
    }

    public void AddEnemies()
    {
        totalEnemies += 1;

        Debug.Log("El total de enemigos es " +  totalEnemies);
    }

    public void RemoveEnemies()
    {
        remainingEnemies -= 1;

        Debug.Log("Quedan " + remainingEnemies + " enemigos");
    }
}
