using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : PoolAble
{
    private bool canPickUp;
    private void OnEnable() // 여기도 풀링 할때만 실행되게 enable로 바꿈
    {
        canPickUp = false;
        CancelInvoke();
        Invoke(nameof(EnablePickUp), 0.3f); //풀링하면서 바로 먹어지는 오류 고침
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    void EnablePickUp()
	{
		canPickUp = true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (canPickUp == false) return;

        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.AddGold(1);
            }
            ReleaseObject();
        }
    }
}
