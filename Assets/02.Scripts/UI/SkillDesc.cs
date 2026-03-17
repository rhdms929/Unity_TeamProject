using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public string skillDescription; // 인스펙터에서 적어줄 스킬 설명
	public GameObject skillDesc;
	public TMPro.TextMeshProUGUI descriptionText; // 설명창의 텍스트 연결

	private Coroutine waitCoroutine;    //	대기 변수

	// 마우스를 올렸을 때 호출
	public void OnPointerEnter(PointerEventData eventData)
	{
		ResetTimer();
		waitCoroutine = StartCoroutine(ShowAfterDelay(3f)); // 새로운 코루틴 시작
	}

	private void ResetTimer()
	{
		if (waitCoroutine != null)
		{
			StopCoroutine(waitCoroutine); // 기존 코루틴이 있으면 중지
			waitCoroutine = null; // 변수 초기화
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ResetTimer();
		if (skillDesc != null)
		{
			skillDesc.SetActive(false); // 설명창 숨기기
		}
	}

	IEnumerator ShowAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);

		// 5초 뒤 실행될 내용들
		if (skillDesc != null && descriptionText != null)
		{
			descriptionText.text = skillDescription;
			skillDesc.SetActive(true);

			// 위치 조정 및 깜빡임 방지 설정
			skillDesc.transform.position = Input.mousePosition + new Vector3(0, 300, 0);
			skillDesc.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
			descriptionText.raycastTarget = false;
		}

	}
}
