using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : PoolAble
{
	public float moveSpeed = 6f;

	private Vector3 targetPos;
	private bool canMove;

	private void OnEnable()
	{
		canMove = false;

		GameObject icon = GameObject.Find("GoldIcon");

		if (icon != null)
		{
			Vector3 screenPos = icon.transform.position;
			targetPos = Camera.main.ScreenToWorldPoint(screenPos);
			targetPos.z = 0;
		}

		CancelInvoke();
		Invoke(nameof(EnableMove), 0.3f);
	}

	void EnableMove()
	{
		canMove = true;
	}

	void Update()
	{
		if (!canMove) return;

		transform.position = Vector3.MoveTowards(
			transform.position,
			targetPos,
			moveSpeed * Time.deltaTime
		);

		if (Vector3.Distance(transform.position, targetPos) < 0.1f)
		{
			GameManager.instance.AddGold(1);
			ReleaseObject();
		}
	}
}