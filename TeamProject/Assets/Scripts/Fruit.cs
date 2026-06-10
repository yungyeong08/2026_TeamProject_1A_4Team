using UnityEngine;

public class Fruit : MonoBehaviour
{
    // 💡 유니티 인스펙터 창에서 이 과일의 단계를 매겨줍니다.
    // [중요] 컴퓨터 기준이므로: 1단계 과일은 '0', 2단계는 '1', 3단계는 '2'... 순으로 적어주세요!
    public int fruitLevel = 0;

    private bool isMerged = false; // 💡 두 번 합성되는 것을 막는 안전장치 플래그

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 부딪힌 상대방 오브젝트도 "Fruit" 스크립트를 가지고 있는지 확인
        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (otherFruit != null)
        {
            // 💡 1. 나와 같은 단계의 과일과 부딪혔는지 확인
            // 💡 2. 이미 합성 처리가 시작된 과일인지 확인 (중복 실행 방지)
            if (this.fruitLevel == otherFruit.fruitLevel && !isMerged && !otherFruit.isMerged)
            {
                // 두 과일 중 '메모리 상에서 더 먼저 생성된 쪽(ID가 작은 쪽)' 하나만 합성을 주도하도록 제한합니다.
                if (this.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // 양쪽 모두 더 이상 다른 합성을 하지 못하도록 플래그를 고정합니다.
                    this.isMerged = true;
                    otherFruit.isMerged = true;

                    // 두 과일의 중간 위치 계산
                    Vector3 mergePosition = (this.transform.position + collision.transform.position) / 2f;

                    // 매니저 스크립트(FruitGame)를 찾아 합성 요청 (현재 내 레벨 번호를 보냄)
                    FruitGame gameManager = FindFirstObjectByType<FruitGame>();
                    if (gameManager != null)
                    {
                        gameManager.MergeFruits(this.fruitLevel, mergePosition);
                    }

                    // 합성 완료 후 원래 있던 과일 2개는 화면에서 삭제
                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}