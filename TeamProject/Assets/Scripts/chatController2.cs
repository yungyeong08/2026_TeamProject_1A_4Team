using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class chatController2 : MonoBehaviour
{
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;

    public string writerText = "";

    // ⭐ 글자 출력 속도
    [SerializeField] private float textSpeed = 0.05f;

    [Header("Character Images (딱 하나씩만!)")]
    public GameObject imageDoYoon;  // 강도윤 이미지
    public GameObject imageYeoJu;   // 김여주 이미지

    [Header("Fade Effect Setting")]
    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public float blackScreenDelay = 1.0f;

    [Header("Scene Management")]
    public string skipTargetScene = "MargeGame";
    public string storyCompleteScene = "GameScene2";
    public string mainSceneName = "Main"; // 💡 거절 후 최종 종료 시 이동할 메인 씬 이름

    [Header("Chat Audio Setting")]
    public AudioClip nextChatSound; // 🔊 효과음 파일
    private AudioSource chatAudioSource;

    [Header("In-Chat Choice Buttons")]
    public Button choiceButton1;     // 선택지 1번 버튼 (거절/부적 주기)
    public Button choiceButton2;     // 선택지 2번 버튼 (허락/손가락에 끼워주기)

    private bool isTransitioning = false;
    private int selectedChoice = 0;   // 유저가 선택한 번호를 저장할 변수 (0: 대기, 1: 거절, 2: 허락)

    void Start()
    {
        // 🔊 오디오 소스 자동 세팅
        chatAudioSource = GetComponent<AudioSource>();
        if (chatAudioSource == null)
        {
            chatAudioSource = gameObject.AddComponent<AudioSource>();
        }
        chatAudioSource.playOnAwake = false;
        chatAudioSource.spatialBlend = 0f;

        // 페이드 이미지 초기화
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }

        // 💡 대화 시작할 때는 대화창 안의 버튼들을 숨겨둡니다.
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);

        // 💡 버튼에 클릭 이벤트 함수를 연결합니다.
        if (choiceButton1 != null) choiceButton1.onClick.AddListener(() => OnClickChoice(1));
        if (choiceButton2 != null) choiceButton2.onClick.AddListener(() => OnClickChoice(2));

        StartCoroutine(TextPractice());
    }

    IEnumerator NormalChat(string narrator, string narration)
    {
        int a = 0;
        CharacterName.text = narrator;
        writerText = "";

        ChangeCharacterImage(narrator);

        for (a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;
            yield return new WaitForSeconds(textSpeed);
        }

        // 마우스 클릭 대기
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (nextChatSound != null && chatAudioSource != null)
                {
                    chatAudioSource.PlayOneShot(nextChatSound);
                }
                break;
            }
            yield return null;
        }
    }

    void ChangeCharacterImage(string name)
    {
        if (imageDoYoon != null) imageDoYoon.SetActive(false);
        if (imageYeoJu != null) imageYeoJu.SetActive(false);

        if (name == "강도윤")
        {
            if (imageDoYoon != null) imageDoYoon.SetActive(true);
        }
        else if (name == "김여주")
        {
            if (imageYeoJu != null) imageYeoJu.SetActive(true);
        }
    }

    // 💡 대화창 내 버튼을 눌렀을 때 실행될 함수
    void OnClickChoice(int choiceIndex)
    {
        selectedChoice = choiceIndex;

        // 💡 선택이 완료되었으므로 대화창 내 버튼들을 다시 숨깁니다.
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(false);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(false);
    }

    IEnumerator TextPractice()
    {
        // --- 프롤로그 ---
        yield return StartCoroutine(NormalChat("", "새벽 2시, 사방이 고요한 학교."));
        yield return StartCoroutine(NormalChat("", "불이 켜진 곳은 학생회실뿐이다."));
        yield return StartCoroutine(NormalChat("", "학교 축제를 코앞에 두고, 김여주의 동아리 기획서에 치명적인 오류가 발견되었다."));
        yield return StartCoroutine(NormalChat("", "결국 밤을 새워서라도 서류를 전면 수정해야 하는 상황."));
        yield return StartCoroutine(NormalChat("", "텅 빈 학생회실에는 탁상스탠드 불빛 하나와 키보드 두드리는 소리만 가득하다."));
        yield return StartCoroutine(NormalChat("", "남주는 안경을 쓴 채 무서운 집중력으로 모니터를 노려보며 마우스를 딸깍거리고 있다."));

        // --- 대화 시작 ---
        yield return StartCoroutine(NormalChat("김여주", "(하품을 참으며) 도윤아… 진짜 미안. 나 때문에 너까지 밤새고…"));
        yield return StartCoroutine(NormalChat("강도윤", "(시선은 모니터에 고정한 채 까칠하게) 미안하면 입 다물고 타자나 빨리 쳐. 오타 내기만 해봐, 손가락을 가만 안 둘 테니까."));
        yield return StartCoroutine(NormalChat("김여주", "말을 꼭 그렇게 무섭게 하냐…"));
        yield return StartCoroutine(NormalChat("강도윤", "누가 기획서를 이딴 식으로 써오래? 나니까 이 시간에 군말 없이 수습해 주는 줄 알아? 다른 사람이었으면 진작에 동아리 폐부야."));

        // --- 지문 ---
        yield return StartCoroutine(NormalChat("", "입으로 폭풍 잔소리를 쏟아내지만, 도윤의 손가락은 김여주의 기획서를 누구보다 완벽하고 멋지게 다듬어주고 있다."));
        yield return StartCoroutine(NormalChat("", "사실 도윤은 학생회장 권한으로 밤샘 허가를 받으면서, \"다른 부원들은 내가 다 끝낼 테니까 먼저 퇴근해\"라며 전부 집으로 돌려보냈다."));
        yield return StartCoroutine(NormalChat("", "김여주와 단둘이 남기 위해서."));

        // --- 새벽 3시 반 ---
        yield return StartCoroutine(NormalChat("", "새벽 3시 반."));
        yield return StartCoroutine(NormalChat("", "낮부터 뛰어다닌 김여주의 눈꺼풀이 결국 천근만근 무거워진다."));
        yield return StartCoroutine(NormalChat("", "꾸벅꾸벅 졸던 김여주의 고개가 중심을 잃고 옆으로 툭 떨어진다."));
        yield return StartCoroutine(NormalChat("", "탁-"));
        yield return StartCoroutine(NormalChat("", "김여주의 머리가 닿은 곳은 차가운 책상이 아니라, 언제 다가왔는지 모를 강도윤의 단단한 어깨였다."));

        // --- 어깨 밀착 상황 ---
        yield return StartCoroutine(NormalChat("강도윤", "(몸이 석상처럼 굳어 버리며) ...야. 야, 김여주?"));
        yield return StartCoroutine(NormalChat("", "조심스럽게 불러보지만, 김여주는 이미 깊은 잠에 빠져 숨을 고르고 있다."));
        yield return StartCoroutine(NormalChat("", "새벽의 정적 속에서 강도윤의 심장 소리가 터질 것처럼 커지기 시작한다."));
        yield return StartCoroutine(NormalChat("", "어깨를 타고 전해지는 김여주의 온기 때문에 얼굴이 귀끝까지 새빨갛게 물든다."));
        yield return StartCoroutine(NormalChat("", "강도윤은 졸린 눈을 비비는 김여주의 얼굴을 한참 동안 물끄러미 바라본다."));
        yield return StartCoroutine(NormalChat("", "툴툴거리던 까칠한 표정은 온데간데없고, 눈동자엔 7년 동안 숨겨온 애틋함과 다정함만이 가득 차 있었다."));
        yield return StartCoroutine(NormalChat("강도윤", "(들릴 듯 말 듯 한 목소리로 중얼거리며) ...이러니까 내가 널 두고 갈 수가 없지."));
        yield return StartCoroutine(NormalChat("", "강도윤은 김여주가 깨지 않도록 숨도 조심스럽게 쉬며, 한쪽 어깨를 완전히 내어준다."));
        yield return StartCoroutine(NormalChat("", "그리고 남은 한 손으로만 느릿하게 마우스를 움직이며 남은 서류를 정리하기 시작한다."));

        // --- 다음 날 아침 ---
        yield return StartCoroutine(NormalChat("", "다음 날 아침, 눈부신 햇살이 들어오는 학생회실."));
        yield return StartCoroutine(NormalChat("", "김여주가 눈을 뜨자, 어느새 덮여있는 강도윤의 교복 마이와 완벽하게 출력이 끝난 기획서 뭉치가 보인다."));
        yield return StartCoroutine(NormalChat("", "강도윤은 책상에 엎드린 채 잠들어 있다. 평소의 날카로운 분위기는 사라지고, 자는 모습은 영락없이 7년 전 소꿉친구 시절 그대로다."));
        yield return StartCoroutine(NormalChat("", "김여주가 인기척을 내자, 도윤이 부스스 눈을 뜬다. 눈이 마주치자마자 도윤은 깜짝 놀라며 벌떡 일어난다."));
        yield return StartCoroutine(NormalChat("", "어깨가 잔뜩 결리는지 앓는 소리를 내면서도, 창밖을 보며 짐짓 소리를 지른다."));

        // --- 아침 티격태격 ---
        yield return StartCoroutine(NormalChat("강도윤", "야!! 너 때문에 어깨 담 걸렸잖아! 책임져!"));
        yield return StartCoroutine(NormalChat("김여주", "어? 미안.. 근데 서류 다 끝난 거야?"));
        yield return StartCoroutine(NormalChat("강도윤", "당연하지, 내가 누구냐? 학생회장이야. …그것보다 너, 밤새 내 어깨 베고 자면서 침 흘린 건 아냐?"));
        yield return StartCoroutine(NormalChat("김여주", "내가?! 진짜!?"));
        yield return StartCoroutine(NormalChat("강도윤", "(사실 안 흘렸지만 뻔뻔하게 놀리며) 어. 그러니까 오늘 축제 시작하면 맛있는 거 사 와. 츄러스랑 음료수, 대용량으로."));

        // --- 선택지 직전 빌드업 ---
        yield return StartCoroutine(NormalChat("김여주", "알았어, 사 갈게! …근데 도윤아, 이거 봐봐. 네가 밤새 수정해 준 기획서 출력물 사이에 이게 끼어 있더라고."));
        yield return StartCoroutine(NormalChat("", "여주는 어젯밤 기획서 최종본과 함께 책상 위에 올려져 있던 반짝이는 반지를 들어 올렸다. 축제 동아리 부스의 메인 아이템이자 고민의 결과물이었다."));
        yield return StartCoroutine(NormalChat("김여주", "이거… 혹시 네가 미리 완성해 둔 샘플이야? 진짜 예쁘다."));
        yield return StartCoroutine(NormalChat("강도윤", "(당황해서 귀끝이 빨개지며) 아, 아니거든?! 그냥 기획서만 보니까 감이 안 와서 대충 굴러다니는 재료로 형태만 잡아본 거야! 착각하지 마."));

        yield return StartCoroutine(NormalChat("", "도윤은 괜히 민망한지 어깨를 으쓱이며 학생회실 문 쪽으로 걸어갔다. 이때 여주의 선택은?"));

    // 💡 [돌아오기 레이블] 재선택 시 대화창을 지우며 이곳으로 다시 이동합니다.
    ChoiceLoop:

        // ❌ [대화창 끄기] 선택지 버튼이 나오기 직전 대화창의 이름과 텍스트를 깨끗이 비웁니다.
        if (CharacterName != null) CharacterName.text = "";
        if (ChatText != null) ChatText.text = "";

        // 💡 [분기점 설정: 대화창 내에 배치된 버튼들을 활성화]
        selectedChoice = 0;
        if (choiceButton1 != null) choiceButton1.gameObject.SetActive(true);
        if (choiceButton2 != null) choiceButton2.gameObject.SetActive(true);

        // 유저가 대화창 내 버튼 중 하나를 누를 때까지 코루틴 대기
        while (selectedChoice == 0)
        {
            yield return null;
        }

        // 💡 [선택에 따른 스토리 분기 처리]
        if (selectedChoice == 1)
        {
            // --- ❌ 거절했을 때 (돌아가기 팝업 및 취소 검증 추가) ---
            yield return StartCoroutine(NormalChat("김여주", "어깨 담 걸린 거 책임지라며! 이거 내가 제일 아끼는 행운의 반지 디자인인데, 밤샘 보답으로 너 줄게. 가질래?"));
            yield return StartCoroutine(NormalChat("강도윤", "하, 너 진짜 바보냐? 사내자리가 이런 걸 왜 껴? …라고 할 줄 알았냐? 보답치고는 소박하지만, 축제 대박 나라고 준 부적이라 생각하고 받아둔다."));
            yield return StartCoroutine(NormalChat("", "도윤은 여주가 건넨 반지를 뺏기듯 낚아채 주머니 깊숙이 넣었다."));
            yield return StartCoroutine(NormalChat("강도윤", "늦지 않게 동아리 부스로 가 봐. 기획서 완벽하니까 무조건 대박 날 거다."));

            // 💡 [돌아가기 선택지 제시]
            yield return StartCoroutine(NormalChat("", "도윤이 부스를 향해 멀어집니다. 이대로 이야기를 끝내고 메인화면으로 돌아갈까요?"));

            // ❌ [대화창 끄기] 메인화면 최종 확인 버튼이 나오기 전에도 대화창을 비웁니다.
            if (CharacterName != null) CharacterName.text = "";
            if (ChatText != null) ChatText.text = "";

            selectedChoice = 0;
            // 버튼의 텍스트 컴포넌트를 코드로 순간 변경하여 예/아니오 시스템으로 활용합니다.
            if (choiceButton1 != null)
            {
                choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = "네, 메인화면으로 갑니다.";
                choiceButton1.gameObject.SetActive(true);
            }
            if (choiceButton2 != null)
            {
                choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = "아니오, 다시 선택할래요.";
                choiceButton2.gameObject.SetActive(true);
            }

            while (selectedChoice == 0) yield return null;

            // 다시 버튼 텍스트 원상복구 (추천 1안 적용)
            if (choiceButton1 != null) choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = "밤새 고생한 보답이야! 행운의 부적이라 생각하고 받아둬.";
            if (choiceButton2 != null) choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = "손가락 줘봐. 우리 축제 파트너 표시로 내가 직접 끼워줄게!";

            if (selectedChoice == 2)
            {
                // 유저가 다시 선택하고 싶다고 했으므로 처음 분기점 위치로 되돌아감
                goto ChoiceLoop;
            }

            // 진짜 메인으로 나가겠다고 확정했을 때 에필로그 진행 후 Main 씬으로 리다이렉트
            yield return StartCoroutine(NormalChat("", "강도윤은 괜히 서류 뭉치를 탕탕 정리하며 학생회실을 나선다."));
            yield return StartCoroutine(NormalChat("", "복도로 나온 강도윤은 아무도 없는 걸 확인하고는, 김여주의 머리가 닿았던 왼쪽 어깨를 손으로 감싸 쥐며 바보처럼 흐뭇하게 웃는다."));
            yield return StartCoroutine(NormalChat("", "(강도윤 속마음 : 아.. 심장 터지는 줄 알았네. 매년 축제 준비 밤샘했으면 좋겠다.)"));
            yield return StartCoroutine(NormalChat("부원 A", "학생회장님 어제 밤새우셨다더니 피곤하긴커녕 왜 저렇게 기분이 좋아 보이냐?"));
            yield return StartCoroutine(NormalChat("부원 B", "말도 마. 아까 보니까 혼자 어깨 주무르면서 광대가 승천하고 계시더라. 완벽주의자 걔, 축제 끝나기도 전에 혼자 축제 분위기네…"));

            // ❌ 거절 엔딩 확정이므로 메인 씬(mainSceneName = "Main")으로 이동합니다.
            StartCoroutine(FadeOutAndLoadScene(mainSceneName));
        }
        else if (selectedChoice == 2)
        {
            // --- ⭕ 허락했을 때 (직접 끼워주며 심쿵 유발 후 머지 플레이씬으로 즉시 전환) ---
            yield return StartCoroutine(NormalChat("김여주", "고생했으니까 상 주는 거야. 손가락 줘봐! 우리 축제 파트너라는 표시로 내가 직접 끼워줄게."));
            yield return StartCoroutine(NormalChat("강도윤", "(완전 얼음 상태가 되며) 야, 너… 지금 무슨 소리를 하는… 읍!"));
            yield return StartCoroutine(NormalChat("", "도윤은 당황해서 손사래를 치려다가, 여주가 진지하게 손을 잡자 석상처럼 굳어버렸다. 여주는 도윤의 약지에 반지를 쏙 끼워주었다. 사이즈가 신기할 정도로 딱 맞았다."));
            yield return StartCoroutine(NormalChat("강도윤", "...너 진짜 겁이 없는 거냐, 눈치가 없는 거냐? (귀는 물론 얼굴까지 터질 듯 붉어지며) …몰라, 이거 츄러스보다 비싼 거니까 절대 안 돌려줄 거야."));
            yield return StartCoroutine(NormalChat("", "도윤은 반지가 끼워진 제 왼손 약지를 멍하니 보더니, 고개를 푹 숙인 채 학생회실을 뛰쳐나가 버렸다."));

            // ⭕ 허락 루트이므로 머지 게임 씬(MargeGame)으로 바로 넘어갑니다!
            StartCoroutine(FadeOutAndLoadScene(skipTargetScene));
        }
    }

    public void SkipStory()
    {
        if (isTransitioning) return;

        StopAllCoroutines();
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