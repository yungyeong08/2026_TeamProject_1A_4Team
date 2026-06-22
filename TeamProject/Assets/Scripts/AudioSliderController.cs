using UnityEngine;
using UnityEngine.Audio; // 💡 오디오 믹서 제어를 위해 필수
using UnityEngine.UI;    // 💡 슬라이더 UI 제어를 위해 필수

public class AudioSliderController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // 1단계에서 만든 믹서 연결
    [SerializeField] private Slider volumeSlider;   // UI 슬라이더 연결

    void Start()
    {
        // 게임이 시작될 때 현재 믹서의 볼륨을 슬라이더 값에 반영 (초기화)
        if (audioMixer.GetFloat("MasterVolume", out float currentVolume))
        {
            // 데시벨 수치를 슬라이더 값(0~1)으로 역산하는 공식
            volumeSlider.value = Mathf.Pow(10f, currentVolume / 20f);
        }

        // 슬라이더 값이 변경될 때마다 SetVolume 함수가 호출되도록 리스너 등록
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        // 유니티 오디오 믹서 볼륨은 '데시벨(dB)' 단위를 씁니다 (-80dB ~ 0dB)
        // 슬라이더의 0~1 값을 로그 스케일을 사용해 적절한 데시벨로 변환합니다.
        float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        audioMixer.SetFloat("MasterVolume", volume);
    }
}