using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // 오디오 믹서 연결
    [SerializeField] private Slider volumeSlider;   // UI 슬라이더 연결
    [SerializeField] private string volumeParameterName = "BGMVolume"; // 파라미터 이름

    void Start()
    {
        if (audioMixer == null || volumeSlider == null) return;

        // 게임 시작 시 기존 볼륨 값을 슬라이더에 로드
        if (audioMixer.GetFloat(volumeParameterName, out float currentVolume))
        {
            // 데시벨 수치(-80dB ~ 0dB)를 슬라이더 값(0 ~ 1)으로 역산
            volumeSlider.value = Mathf.Pow(10f, currentVolume / 20f);
        }

        // 슬라이더 값이 변경될 때마다 볼륨 조절 함수 호출
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float sliderValue)
    {
        if (audioMixer == null) return;

        // 💡 안전장치: 슬라이더가 맨 왼쪽(0)으로 가면 소리를 완전히 끕니다 (-80dB)
        if (sliderValue <= 0.0001f)
        {
            audioMixer.SetFloat(volumeParameterName, -80f);
        }
        else
        {
            // 로그 스케일을 사용해 슬라이더 0~1 값을 오디오 믹서 데시벨(-40dB ~ 0dB)로 변환
            // (*20f 대신 *40f를 쓰면 슬라이더를 조금만 줄여도 소리가 부드럽고 확실하게 줄어듭니다!)
            float volume = Mathf.Log10(sliderValue) * 40f;
            audioMixer.SetFloat(volumeParameterName, volume);
        }
    }
}