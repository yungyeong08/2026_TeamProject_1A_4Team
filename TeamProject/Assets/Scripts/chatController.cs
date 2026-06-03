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
        yield return StartCoroutine(NormalChat("남주", "앙기모띠"));
        yield return StartCoroutine(NormalChat("여주", "앙기모띠"));
        yield return StartCoroutine(NormalChat("남주", "앙야르띠"));
        yield return StartCoroutine(NormalChat("여주", "앙야르띠"));
    }
}