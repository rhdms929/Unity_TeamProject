using System.Collections;
using UnityEngine;

public class GoldItem : PoolAble
{
	public float startSpeed = 5f;      // 처음 속도 
	public float acceleration = 10f;   // 가속도

	private Vector3 targetPos;
	private float currentSpeed;
	private bool flyToUI = false;

	void OnEnable()
	{
		flyToUI = false;
		SetupTargetAndFly();
	}

	void SetupTargetAndFly()
	{
		if (GameManager.instance != null && GameManager.instance.goldIcon != null)
		{
			// UI 아이콘의 스크린 좌표를 월드 좌표로 변환 (월드 좌표로 맞춰야 자연스럽게 됨)
			Vector3 screenPos = GameManager.instance.goldIcon.position;
			Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
			worldTarget.z = 0f;
			targetPos = worldTarget;
		}

		currentSpeed = startSpeed;
		flyToUI = true;
	}

	void Update()
	{
		if (!flyToUI) return;

		// 타겟 방향으로 가속하며 이동
		currentSpeed += acceleration * Time.deltaTime;

		transform.position = Vector3.MoveTowards(
			transform.position,
			targetPos,
			currentSpeed * Time.deltaTime
		);

		// UI 도착 시 처리
		if (Vector3.Distance(transform.position, targetPos) < 0.1f)
		{
			GameManager.instance.AddGold(1);
			ReleaseObject();
		}
	}
}