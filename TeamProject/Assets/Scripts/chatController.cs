using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class chatController : MonoBehaviour
{
    public TextMeshProUGUI ChatText;
    public TextMeshProUGUI CharacterName;

    public string writerText = "";

    // ⭐ 글자 출력 속도를 조절하는 변수 (숫자가 작을수록 빠르고, 클수록 느려집니다)
    [SerializeField] private float textSpeed = 0.05f;

    void Start()
    {
        StartCoroutine(TextPractice());
    }

    IEnumerator NormalChat(string narrator, string narration)
    {
        int a = 0;
        CharacterName.text = narrator;
        writerText = "";

        for (a = 0; a < narration.Length; a++)
        {
            writerText += narration[a];
            ChatText.text = writerText;

            // ⭐ 기존 yield return null; 을 아래 코드로 변경!
            // 0.05초마다 한 글자씩 출력되게 만듭니다.
            yield return new WaitForSeconds(textSpeed);
        }

        // 마우스 클릭 대기 루프 (이 부분은 마우스 클릭 프레임을 놓치지 않기 위해 null을 유지해야 합니다)
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
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
        yield return StartCoroutine(NormalChat("김여주", "응?"));
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
        yield return StartCoroutine(NormalChat("", "여주는 그 의미를 알 수 없었다."));
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

    }
}