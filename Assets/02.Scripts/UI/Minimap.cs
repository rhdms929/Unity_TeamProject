using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
	public Transform player;
	public Camera minimapCamera; // 미니맵 카메라 연결

	public float zoomStep = 2f;    // 한 번 클릭 시 변할 크기
	public float minZoom = 5f;     // 최대 확대 제한
	public float maxZoom = 20f;    // 최대 축소 제한

	void LateUpdate()
	{
		if (player == null) return;

		// Z축 고정 
		Vector3 newPosition = player.position;
		newPosition.z = -10f;
		transform.position = newPosition;
	}

	// [+] 버튼에 연결할 함수
	public void ZoomIn()
	{
		if (minimapCamera.orthographicSize > minZoom)
			minimapCamera.orthographicSize -= zoomStep;
	}

	// [-] 버튼에 연결할 함수
	public void ZoomOut()
	{
		if (minimapCamera.orthographicSize < maxZoom)
			minimapCamera.orthographicSize += zoomStep;
	}
}
