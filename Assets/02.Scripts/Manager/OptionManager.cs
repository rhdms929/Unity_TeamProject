using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
	[Header("Audio Components")]
	public AudioSource bgmSource; // 배경음악 소스
	public Slider volumeSlider;   // 볼륨 슬라이더
	public Toggle musicToggle;    // 음악 켜기/끄기 체크박스

	[Header("Graphic Settings")]
	public TMP_Dropdown resolutionDropdown;    // 해상도 드롭다운 연결용

	// 해상도 리스트
	List<string> resOptions = new List<string> { "1920x1080", "1280x720", "800x600" };

	void Start()
	{
		// 오디오 설정 불러오기 (없으면 기본값 0.5f)
		float savedVolume = PlayerPrefs.GetFloat("BGM_Volume", 0.5f);
		bool isMusicOn = PlayerPrefs.GetInt("BGM_On", 1) == 1;

		volumeSlider.value = savedVolume;
		musicToggle.isOn = isMusicOn;
		bgmSource.volume = savedVolume; // 시작할 때 소리 크기도 적용
		UpdateMusicState();

		// 해상도 드롭다운 초기화
		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(resOptions);

		// 저장된 해상도 인덱스 불러오기 (기본값 0: 1920x1080)
		int savedRes = PlayerPrefs.GetInt("ResIndex", 0);
		resolutionDropdown.value = savedRes;
		ApplyResolution(savedRes);

	}

	// 슬라이더 바꿀 때 실행될 함수
	public void OnVolumeChanged()
	{
		bgmSource.volume = volumeSlider.value;
	}

	// 토글 체크박스 바꿀 때 실행될 함수
	public void OnMusicToggleChanged()
	{
		UpdateMusicState();
	}

	private void UpdateMusicState()
	{
		bgmSource.mute = !musicToggle.isOn;
	}
	public void OnResolutionChanged()
	{
		ApplyResolution(resolutionDropdown.value);
	}
	private void ApplyResolution(int index)
	{
		// 전체화면 모드 유지하면서 해상도만 변경
		if (index == 0) Screen.SetResolution(1920, 1080, Screen.fullScreen);
		else if (index == 1) Screen.SetResolution(1280, 720, Screen.fullScreen);
		else if (index == 2) Screen.SetResolution(800, 600, Screen.fullScreen);
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("BGM_Volume", volumeSlider.value);
		PlayerPrefs.SetInt("BGM_On", musicToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("ResIndex", resolutionDropdown.value);

		PlayerPrefs.Save();
		//Debug.Log("모든 설정(오디오/해상도) 저장 완료")
	}
}