using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private GameObject aliveEnemy, deadEnemy;
    
    private EnemyAliveController EAC;

    void Start()
    {
        EAC = GameObject.Find("EnemyAlive").GetComponent<EnemyAliveController>();
        aliveEnemy = transform.Find("Alive").gameObject;
        deadEnemy = transform.Find("Dead").gameObject;

        aliveEnemy.SetActive(true);
        deadEnemy.SetActive(false);
    }

    void Update()
    {
        if (EAC.isDead)
        {
            aliveEnemy.SetActive(false);
            deadEnemy.SetActive(true);

            deadEnemy.transform.position = aliveEnemy.transform.position;
            deadEnemy.transform.rotation = aliveEnemy.transform.rotation;

            Destroy(gameObject, 2);
        }
    }
}
