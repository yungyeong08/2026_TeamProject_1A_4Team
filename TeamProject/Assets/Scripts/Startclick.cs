using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // UI 충돌 검사를 위해 반드시 필요합니다!
using UnityEngine.UI; // 💡 이미지 컴포넌트 접근을 위해 추가

public class Startclick : MonoBehaviour
{
    [Header("Click Target")]
    // 💡 유니티 인스펙터에서 클릭했을 때 넘어 가게 할 이미지(오브젝트)를 여기에 드래그 앤 드롭 하세요.
    public GameObject targetImageObject;

    void Update()
    {
        // 1. 마우스 왼쪽 버튼 클릭이 감지되었을 때
        if (Input.GetMouseButtonDown(0))
        {
            // 2. 현재 마우스 위치에 UI(버튼 등)가 있는지 확인
            // EventSystem.current.IsPointerOverGameObject()가 true이면 UI를 누른 상태입니다.
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                // 💡 [추가된 로직] 클릭한 UI가 우리가 지정한 '그 이미지'가 맞는지 확인합니다.
                // 마우스 위치에서 UI 레이캐스트를 받아와 타겟 오브젝트와 일치하는지 체크합니다.
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                bool isTargetClicked = false;
                foreach (RaycastResult result in results)
                {
                    // 클릭된 UI 중 우리가 등록한 targetImageObject가 있다면 타겟 클릭으로 인정
                    if (result.gameObject == targetImageObject)
                    {
                        isTargetClicked = true;
                        break;
                    }
                }

                // 지정한 이미지를 클릭한 것이 맞다면 씬 전환을 실행합니다!
                if (isTargetClicked)
                {
                    SceneManager.LoadScene("GameScene");
                    return;
                }

                // 만약 다른 UI 버튼을 누른 상태라면, 기존 코드대로 함수를 빠져나갑니다.
                return;
            }

            // 3. UI 버튼이 아닌 빈 화면을 눌렀을 때는 아무것도 하지 않고 무시합니다.
            // (기존에는 여기서 씬이 넘어갔으나, 이제 이미지를 누를 때만 넘어가도록 흐름이 고정됩니다.)
        }
    }
}