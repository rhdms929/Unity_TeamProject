using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
	[Header("Audio Components")]
	public AudioSource bgmSource; // 배경음악 소스
	public Slider volumeSlider;   // 볼륨 슬라이더
	public Toggle musicToggle;    // 음악 켜기/끄기 체크박스

	void Start()
	{
		// 저장된 설정 불러오기 (없으면 기본값 0.5f)
		float savedVolume = PlayerPrefs.GetFloat("BGM_Volume", 0.5f);
		bool isMusicOn = PlayerPrefs.GetInt("BGM_On", 1) == 1;

		// UI와 실제 소리에 적용
		volumeSlider.value = savedVolume;
		musicToggle.isOn = isMusicOn;

		UpdateMusicState();
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

	public void SaveAudioSettings()
	{
		PlayerPrefs.SetFloat("BGM_Volume", volumeSlider.value);
		PlayerPrefs.SetInt("BGM_On", musicToggle.isOn ? 1 : 0);
		PlayerPrefs.Save();
		Debug.Log("오디오 설정 저장 완료");
	}
}