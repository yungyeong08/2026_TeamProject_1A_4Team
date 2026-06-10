using System.Collections; // 코루틴(IEnumerator) 사용을 위해 필수 추가
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitGame : MonoBehaviour
{
    [Header("Fruit Settings")]
    // 💡 유니티 인스펙터에서 1~5단계 프리팹 총 5개를 순서대로 넣어주세요. (0번=1단계, 4번=5단계)
    public GameObject[] fruitPerfabs;
    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f }; // 1~5단계 크기 설정 (총 5개)
    public float fruitStartHigh = 6.0f;

    [Header("Game Control")]
    public float gameWidth = 6.0f;
    public bool isGameOver = false;
    public Camera mainCamera;

    public GameObject currentFruit;
    public int currentFruitType;

    private float fruitTimer = -1f;
    private bool isWaitingForNextFruit = false;

    [Header("Score Settings")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Clear UI Settings")]
    public GameObject clearTextObject; // 💡 클리어 문구 UI 오브젝트 (인스펙터에서 Canvas 내부의 Text 연결)
    public float clearDelayTime = 3.0f; // 클리어 문구를 보여줄 시간 (3초)

    private bool isClearing = false; // 클리어 연출 중 조작을 막기 위한 플래그

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // 시작할 때는 클리어 문구를 화면에서 숨깁니다.
        if (clearTextObject != null) clearTextObject.SetActive(false);

        UpdateScoreUI();
        SpawnnewFruit();
    }

    void Update()
    {
        // 게임오버이거나 이미 스테이지를 클리어한 상태라면 조작을 막습니다.
        if (isGameOver || isClearing) return;

        // 1. 새로운 과일 스폰 타이머
        if (isWaitingForNextFruit)
        {
            fruitTimer -= Time.deltaTime;
            if (fruitTimer <= 0)
            {
                SpawnnewFruit();
            }
        }

        // 2. 현재 잡고 있는 과일 이동 로직
        if (currentFruit != null && !currentFruit.Equals(null))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -mainCamera.transform.position.z;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            float halfSize = 0.25f;
            if (currentFruitType >= 0 && currentFruitType < fruitSize.Length)
            {
                halfSize = fruitSize[currentFruitType] / 2f;
            }

            float leftLimit = -gameWidth / 2f + halfSize;
            float rightLimit = gameWidth / 2f - halfSize;

            float clampedX = Mathf.Clamp(worldPosition.x, leftLimit, rightLimit);
            currentFruit.transform.position = new Vector3(clampedX, fruitStartHigh, 0);
        }

        // 3. 클릭 시 과일 떨어뜨리기
        if (Input.GetMouseButtonDown(0) && currentFruit != null && !isWaitingForNextFruit)
        {
            DropFruit();
        }
    }

    void SpawnnewFruit()
    {
        if (isGameOver || isClearing) return;

        isWaitingForNextFruit = false;

        // 처음에 떨어지는 과일은 1~3단계(인덱스 0, 1, 2) 중 무작위 생성
        currentFruitType = Random.Range(0, 3);
        if (fruitPerfabs == null || fruitPerfabs.Length == 0) return;

        Vector3 spawnPosition = new Vector3(0, fruitStartHigh, 0);
        currentFruit = Instantiate(fruitPerfabs[currentFruitType], spawnPosition, Quaternion.identity);

        if (currentFruitType < fruitSize.Length)
        {
            currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType], 1);
        }

        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void DropFruit()
    {
        if (currentFruit == null) return;
        if (currentFruitType < 0 || currentFruitType >= fruitPerfabs.Length) return;

        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1.0f;

            currentFruit = null;
            fruitTimer = 1.0f;
            isWaitingForNextFruit = true;
        }
    }

    // 과일이 충돌하여 합쳐질 때 다른 스크립트(Fruit 등)에서 호출하는 메서드
    public void MergeFruits(int fruitType, Vector3 position)
    {
        if (isGameOver || isClearing) return;

        // 다음 단계 계산 (인덱스 기준)
        int nextLevel = fruitType + 1;

        // 💡 만약 이미 최종인 5단계(인덱스 4)끼리 부딪혀서 인덱스가 5가 되려고 한다면 예외 처리
        if (nextLevel >= 5)
        {
            Debug.Log("최종 단계인 5단계 과일끼리 합쳐졌습니다! 클리어 루틴을 재실행합니다.");
            StartCoroutine(ClearRoutine());
            return;
        }

        // 프리팹 배열 범위 내에 있다면 다음 단계 과일 생성
        if (nextLevel < fruitPerfabs.Length && fruitPerfabs[nextLevel] != null)
        {
            // 다음 단계 과일 오브젝트를 바닥에 먼저 띄웁니다.
            GameObject nextFruit = Instantiate(fruitPerfabs[nextLevel], position, Quaternion.identity);

            if (nextLevel < fruitSize.Length)
            {
                float nextSize = fruitSize[nextLevel];
                nextFruit.transform.localScale = new Vector3(nextSize, nextSize, 1.0f);
            }

            AddScore((nextLevel + 1) * 10);
            Debug.Log((nextLevel + 1) + "단계 과일 생성 완료!");

            // 💡 [정밀 검사 완료] 새로 생성된 과일이 인덱스 4(즉, 최종 5단계)일 때만 클리어 루틴 발동!
            if (nextLevel == 4)
            {
                Debug.Log("★ 축하합니다! 최종 5단계 과일 탄생 ★");
                StartCoroutine(ClearRoutine());
            }
        }
    }

    // 클리어 문구를 보여주고 대기하는 코루틴 함수
    IEnumerator ClearRoutine()
    {
        isClearing = true; // 대기 시간 동안 마우스 클릭 및 추가 조작 방지
        Debug.Log("클리어 연출 시작.");

        // 마우스를 따라다니며 손에 쥐고 있던 과일이 있다면 깔끔하게 지워줍니다.
        if (currentFruit != null)
        {
            Destroy(currentFruit);
        }

        // 1. 화면에 "STAGE CLEAR!" 문구 UI 활성화
        if (clearTextObject != null)
        {
            clearTextObject.SetActive(true);
        }

        // 2. 설정한 시간(3초) 동안 코드 흐름을 멈추고 대기합니다.
        // 바닥에 완성된 5단계 과일과 클리어 텍스트가 동시에 보이게 됩니다!
        yield return new WaitForSeconds(clearDelayTime);

        // 3. 대기 시간이 끝난 후 다음 씬으로 전환합니다.
        LoadNextScene();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.LogWarning("다음 씬이 빌드 설정(Build Settings)에 등록되어 있지 않습니다!");
        }
    }
}