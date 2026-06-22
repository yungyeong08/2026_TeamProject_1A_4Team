using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections; // Coroutine을 쓰기 위해 추가!

public class Startclick : MonoBehaviour
{
    [Header("Audio Setting")]
    public AudioClip clickSound;

    private AudioSource audioSource;
    private bool isTransitioning = false; // 💡 중복 클릭 방지용 방패막이

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 🔊 효과음이 끊기거나 작게 들리지 않도록 3D 설정을 끄고 2D 풀 볼륨으로 세팅합니다.
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D 사운드 고정
        audioSource.volume = 1f;       // 볼륨 최대
    }

    void Update()
    {
        // 씬 전환 중일 때는 클릭 입력을 완전히 무시합니다.
        if (isTransitioning) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                if (EventSystem.current.currentSelectedGameObject != null &&
                    EventSystem.current.currentSelectedGameObject.name != "Image")
                {
                    return;
                }
            }

            // 💡 바로 씬을 바꾸지 않고, 효과음을 끝까지 재생하는 코루틴을 실행합니다!
            StartCoroutine(PlaySoundAndLoadScene());
        }
    }

    // 🔥 효과음이 다 끝난 후에 안전하게 씬을 넘겨주는 마법의 함수입니다.
    IEnumerator PlaySoundAndLoadScene()
    {
        isTransitioning = true; // 대기 시작했으니 클릭 잠금!

        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);

            // 💡 효과음 길이(초)만큼 씬 이동을 잠시 지연시킵니다 (보통 0.2초~0.5초 사이)
            yield return new WaitForSeconds(clickSound.length);
        }
        else
        {
            // 혹시 오디오 파일 연결을 까먹었을 때를 대비해 기본 대기 시간을 줍니다.
            yield return new WaitForSeconds(0.1f);
        }

        // 소리가 다 난 다음 다음 씬으로 안전하게 이동!
        SceneManager.LoadScene("GameScene");
    }
}