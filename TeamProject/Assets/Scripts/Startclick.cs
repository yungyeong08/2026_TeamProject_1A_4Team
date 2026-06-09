using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // UI 충돌 검사를 위해 반드시 필요합니다!

public class Startclick : MonoBehaviour
{
    void Update()
    {
        // 1. 마우스 왼쪽 버튼 클릭이 감지되었을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 2. 현재 마우스 위치에 UI(버튼 등)가 있는지 확인
            // EventSystem.current.IsPointerOverGameObject()가 true이면 UI를 누른 상태입니다.
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // UI 버튼을 눌렀으므로, 화면 클릭으로 인한 씬 전환은 무시하고 함수를 빠져나갑니다.
                return;
            }

            // 3. UI 버튼이 아닌 빈 화면을 눌렀을 때만 씬 전환 실행
            SceneManager.LoadScene("GameScene");
        }
    }
}