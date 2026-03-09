using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour, IDamageable
{
    public int maxHp = 10;
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("플레이어 체력 : " + currentHp);

        if (currentHp <= 0)
        {
            Debug.Log("플레이어 사망");
        }
    }
}