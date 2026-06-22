using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio; // 💡 오디오 믹서 동기화를 위해 필수 추가!

public class chatController : MonoBehaviour
{
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;

    public string writerText = "";

    // ⭐ 글자 출력 속도를 조절하는 변수
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Character Images")]
    public GameObject imageKimYeoJu;   // 김여주 이미지
    public GameObject imageKangDoYoon; // 강도윤 이미지
    public GameObject imageBoy;        // 의문의 소년(연하남) 이미지
    public GameObject imageSunbae;     // 옥상 선배 이미지

    [Header("Fade Effect Setting")]
    public Image fadeImage; // 💡 유니티 창의 Image (2)를 여기에 연결해 주세요
    public float fadeDuration = 1.5f; // 화면이 어두워지는 시간 (1.5초)
    public float blackScreenDelay = 1.0f; // 검은 화면이 유지될 시간 (1초)

    [Header("Scene Management")]
    public string skipTargetScene = "GameScene1";       // 💡 스킵 버튼을 누르면 갈 타겟 씬을 GameScene1으로 변경!

    [Header("Chat Audio Setting")]
    public AudioClip nextChatSound;
    private AudioSource chatAudioSource;

    // 💡 메인화면에서 조절한 볼륨을 게임씬에서도 그대로 강제 로드하기 위한 오디오 믹서 변수
    [Header("Global Audio Mixer")]
    public AudioMixer globalAudioMixer;

    // 💡 내부적으로만 사용하는 고정 타겟 씬 변수입니다.
    private string storyCompleteScene = "GameScene1";

    private bool isTransitioning = false; // 중복 전환 방지용 플래그

    void Start()
    {
        // ================================================================
        // 🔊 메인화면에서 슬라이더로 줄여놓은 믹서 볼륨 값을 게임씬 시작할 때 강제로 한 번 더 동기화합니다.
        if (globalAudioMixer != null)
        {
            if (globalAudioMixer.GetFloat("BGMVolume", out float bgmVol))
                globalAudioMixer.SetFloat("BGMVolume", bgmVol);

            if (globalAudioMixer.GetFloat("SFXVolume", out float sfxVol))
                globalAudioMixer.SetFloat("SFXVolume", sfxVol);
        }
        // ================================================================

        // 🔊 효과음을 재생해 줄 스피커(AudioSource)를 자동으로 세팅합니다.
        chatAudioSource = GetComponent<AudioSource>();
        if (chatAudioSource == null)
        {
            chatAudioSource = gameObject.AddComponent<AudioSource>();
        }

        // 💡 실시간 코드로 생성된 오디오 소스의 출구(Output)를 오디오 믹서의 "SFX" 그룹으로 강제 고정합니다.
        if (globalAudioMixer != null)
        {
            AudioMixerGroup[] groups = globalAudioMixer.FindMatchingGroups("SFX");
            if (groups.Length > 0)
            {
                chatAudioSource.outputAudioMixerGroup = groups[0];
            }
        }

        // 오디오가 잘리거나 입체음향 때문에 안 들리는 것을 막기 위해 2D 풀 볼륨으로 세팅합니다.
        chatAudioSource.playOnAwake = false;
        chatAudioSource.spatialBlend = 0f; // 2D 사운드 고정
        chatAudioSource.volume = 1f;       // 볼륨 최대

        // 💡 인스펙터에 어떤 값이 남아있든 실행 시점에 두 경로 모두 "GameScene1"으로 완벽히 강제 고정합니다.
        storyCompleteScene = "GameScene1";
        skipTargetScene = "GameScene1";

        // 게임 시작 시 페이드 이미지(Image (2))를 자동으로 초기화하고 꺼둡니다.
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }

        StartCoroutine(TextPractice());
    }

    IEnumerator NormalChat(string narrator, string narration)
    {
        int a = 0;
        CharacterName.text = narrator;
        writerText = "";

        // 대사가 시작되기 전에 이름과 대사 내용을 분석해서 이미지를 켜고 끕니다.
        ChangeCharacterImage(narrator, narration);

        for (a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;
            yield return new WaitForSeconds(textSpeed);
        }

        // 마우스 클릭 대기 루프
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 🔊 마우스 클릭이 감지되어 다음 대사로 넘어가기 직전에 효과음을 1회 재생합니다.
                if (nextChatSound != null && chatAudioSource != null)
                {
                    chatAudioSource.PlayOneShot(nextChatSound);
                }
                break;
            }
            yield return null;
        }
    }

    void ChangeCharacterImage(string name, string narration)
    {
        // 먼저 모든 캐릭터 이미지를 화면에서 숨깁니다 (초기화)
        if (imageKimYeoJu != null) imageKimYeoJu.SetActive(false);
        if (imageKangDoYoon != null) imageKangDoYoon.SetActive(false);
        if (imageBoy != null) imageBoy.SetActive(false);
        if (imageSunbae != null) imageSunbae.SetActive(false);

        if (name == "김여주")
        {
            if (imageKimYeoJu != null) imageKimYeoJu.SetActive(true);
        }
        else if (name == "강도윤")
        {
            if (imageKangDoYoon != null) imageKangDoYoon.SetActive(true);
        }
        else if (name == "???")
        {
            // 이름이 "???" 일 때 대사 내용에 따라 소년과 선배 이미지를 분리하여 활성화합니다.
            if (narration.Contains("누나") || narration.Contains("알아요") || narration.Contains("되죠"))
            {
                if (imageBoy != null) imageBoy.SetActive(true);
            }
            else if (narration.Contains("얼굴이네") || narration.Contains("글쎄") || narration.Contains("후배님") || narration.Contains("보게 될 것"))
            {
                if (imageSunbae != null) imageSunbae.SetActive(true);
            }
        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("", "새 학기."));
        yield return StartCoroutine(NormalChat("", "따스한 봄바람이 교복 치맛자락을 흔들었다."));
        yield return StartCoroutine(NormalChat("", "...이어야 했는데."));
        yield return StartCoroutine(NormalChat("김여주", "으아아악! 늦었다!!"));
        yield return StartCoroutine(NormalChat("", "알람을 끄고 다시 잠들어버린 탓에,"));
        yield return StartCoroutine(NormalChat("", "김여주는 학교를 향해 전력질주하고 있었다."));
        yield return StartCoroutine(NormalChat("", "빵 한 조각을 입에 문 채 뛰어가는 모습은"));
        yield return StartCoroutine(NormalChat("", "누가 봐도 지각생 그 자체."));
        yield return StartCoroutine(NormalChat("김여주", "이번엔 진짜 혼난다..."));
        yield return StartCoroutine(NormalChat("", "그리고 그 예상은 정확했다."));
        yield return StartCoroutine(NormalChat("", "교문 앞."));
        yield return StartCoroutine(NormalChat("", "학생회장 완장을 찬 익숙한 소년이 팔짱을 낀 채 서 있었다."));
        yield return StartCoroutine(NormalChat("강도윤", "또 너냐."));
        yield return StartCoroutine(NormalChat("", "여주는 그대로 굳어버렸다."));
        yield return StartCoroutine(NormalChat("김여주", "도윤아... 한 번만 봐주면 안 될까?"));
        yield return StartCoroutine(NormalChat("강도윤", "안 돼."));
        yield return StartCoroutine(NormalChat("김여주", "우린 소꿉친구잖아!"));
        yield return StartCoroutine(NormalChat("강도윤", "그래서 더 안 돼."));
        yield return StartCoroutine(NormalChat("", "그렇게 말하면서도 명부에는 아무것도 적지 않는 강도윤."));
        yield return StartCoroutine(NormalChat("", "하지만 여주는 눈치채지 못했다."));
        yield return StartCoroutine(NormalChat("", "우여곡절 끝에 수업이 시작되고."));
        yield return StartCoroutine(NormalChat("", "쉬는 시간."));
        yield return StartCoroutine(NormalChat("", "복도를 걷던 여주는 갑자기 누군가와 부딪힌다."));
        yield return StartCoroutine(NormalChat("???", "앗."));
        yield return StartCoroutine(NormalChat("", "공이 굴러 떨어진다."));
        yield return StartCoroutine(NormalChat("", "여주가 고개를 들자,"));
        yield return StartCoroutine(NormalChat("", "키가 큰 남학생이 당황한 얼굴로 서 있었다."));
        yield return StartCoroutine(NormalChat("???", "괜찮아요?!"));
        yield return StartCoroutine(NormalChat("", "강아지 같은 눈망울."));
        yield return StartCoroutine(NormalChat("", "운동부 차림."));
        yield return StartCoroutine(NormalChat("", "그리고 이상할 정도로 반짝이는 눈빛."));
        yield return StartCoroutine(NormalChat("???", "...누나."));
        yield return StartCoroutine(NormalChat("응?", "응?"));
        yield return StartCoroutine(NormalChat("???", "저 누나 알아요."));
        yield return StartCoroutine(NormalChat("김여주", "처음 보는데?"));
        yield return StartCoroutine(NormalChat("???", "...이제 알면 되죠!"));
        yield return StartCoroutine(NormalChat("", "그 말을 남긴 소년은 활짝 웃는다."));
        yield return StartCoroutine(NormalChat("", "그 순간."));
        yield return StartCoroutine(NormalChat("", "왠지 모르게 앞으로가 불안해졌다."));
        yield return StartCoroutine(NormalChat("", "점심시간."));
        yield return StartCoroutine(NormalChat("", "옥상 문을 열자 낯선 선배 한 명이 난간에 기대 서 있었다."));
        yield return StartCoroutine(NormalChat("", "봄바람이 머리카락을 스친다."));
        yield return StartCoroutine(NormalChat("", "선배는 여주를 바라보더니 천천히 웃었다."));
        yield return StartCoroutine(NormalChat("???", "처음 보는 얼굴이네."));
        yield return StartCoroutine(NormalChat("김여주", "선배는요?"));
        yield return StartCoroutine(NormalChat("???", "글쎄."));
        yield return StartCoroutine(NormalChat("", "마치 재밌는 장난감을 발견한 사람 같은 표정."));
        yield return StartCoroutine(NormalChat("???", "후배님."));
        yield return StartCoroutine(NormalChat("김여주", "네?"));
        yield return StartCoroutine(NormalChat("???", "우리 앞으로 자주 보게 될 것 같은데."));
        yield return StartCoroutine(NormalChat("", "여주는 그 의미을 알 수 없었다."));
        yield return StartCoroutine(NormalChat("", "하지만 선배는 이미 알고 있는 것처럼 웃고 있었다."));
        yield return StartCoroutine(NormalChat("", "그날 하교길."));
        yield return StartCoroutine(NormalChat("", "여주는 문득 생각했다."));
        yield return StartCoroutine(NormalChat("", "평범한 하루였다."));
        yield return StartCoroutine(NormalChat("", "조금 지각했고."));
        yield return StartCoroutine(NormalChat("", "조금 혼났고."));
        yield return StartCoroutine(NormalChat("", "조금 이상한 사람들을 만났다."));
        yield return StartCoroutine(NormalChat("", "그뿐이었다."));
        yield return StartCoroutine(NormalChat("", "그런데..."));
        yield return StartCoroutine(NormalChat("", "그 작은 만남들이."));
        yield return StartCoroutine(NormalChat("", "앞으로 자신의 학교생활을 크게 바꿔놓을 거라는 사실은."));
        yield return StartCoroutine(NormalChat("", "여주는 아직 아무것도 모르고 있었다."));

        // 모든 대사가 끝나면 무조건 GameScene1으로 이동합니다.
        StartCoroutine(FadeOutAndLoadScene(storyCompleteScene));
    }

    // 💡 스킵 버튼 클릭 시 작동하는 부분
    public void SkipStory()
    {
        if (isTransitioning) return;

        StopAllCoroutines();
        // 💡 skipTargetScene 변수가 GameScene1으로 고정되었으므로 페이드 아웃 후 GameScene1으로 이동합니다.
        StartCoroutine(FadeOutAndLoadScene(skipTargetScene));
    }

    IEnumerator FadeOutAndLoadScene(string targetScene)
    {
        isTransitioning = true;
        float timer = 0f;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color initColor = fadeImage.color;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                initColor.a = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = initColor;
                yield return null;
            }

            initColor.a = 1f;
            fadeImage.color = initColor;
        }

        yield return new WaitForSeconds(blackScreenDelay);
        SceneManager.LoadScene(targetScene);
    }
}