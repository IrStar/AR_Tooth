using HoloToolkit.Unity.InputModule;
using UnityEngine;

/// <summary>
/// GestureAction performs custom actions based on
/// which gesture is being performed.
/// </summary>
public class GestureAction : MonoBehaviour, IInputClickHandler, INavigationHandler
{
    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 1.0f;

    [Tooltip("translation max speed controls amount of translation.")]
    public float TranslationSensitivity = 5.0f;

    private float maxScale = 0.04f;
    private float minScale = 0.01f;

    private bool isSelected = false;

    private IInputSource inputSource;
    private uint inputSourceId;
    private Vector3 prevHandPosition;

    // Use this for initialization
    private void Start () {
    }

    private void OnDestroy() {
    }

    // IInputClickHandler
    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        isSelected = !isSelected;
        eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
    }

    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.

        if (!isSelected)
        {
            inputSource = eventData.InputSource;
            inputSourceId = eventData.SourceId;

            Vector3 currentHandPosition = Vector3.zero;
            InteractionSourceInfo sourceKind;
            inputSource.TryGetSourceKind(inputSourceId, out sourceKind);
            switch (sourceKind)
            {
                case InteractionSourceInfo.Hand:
                    inputSource.TryGetGripPosition(inputSourceId, out currentHandPosition);
                    break;
                case InteractionSourceInfo.Controller:
                    inputSource.TryGetPointerPosition(inputSourceId, out currentHandPosition);
                    break;
            }
            prevHandPosition = currentHandPosition;
        }
        //gameObject.SendMessageUpwards("OnSelect", SendMessageOptions.DontRequireReceiver);
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (isSelected)
        {
            // 左右移动为旋转，上下移动为缩放
            float dx = eventData.NormalizedOffset.x;
            float dy = eventData.NormalizedOffset.y;
            if (System.Math.Abs(dx) > System.Math.Abs(dy))
            {
                // 2.c: Calculate a float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity.
                // This will help control the amount of rotation.
                float rotationFactor = dx * RotationSensitivity;

                // 2.c: transform.Rotate around the Y axis using rotationFactor.
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
            }
            else
            {
                Vector3 dScale = Vector3.zero;
                //设置每一帧缩放的大小
                float scaleValue = 0.0002f;
                if (dy > 0)
                {
                    scaleValue = -1 * scaleValue;
                }
                //当缩放超出设置的最大，最小范围时直接返回
                if (transform.localScale.x > maxScale && scaleValue > 0)
                { return; }
                if (transform.localScale.x < minScale && scaleValue < 0)
                { return; }
                //根据比例计算每个方向上的缩放大小
                dScale.x = scaleValue;
                dScale.y = scaleValue;
                dScale.z = scaleValue;
                transform.localScale += dScale;
            }
        } else
        {
            Vector3 currentHandPosition = Vector3.zero;
            InteractionSourceInfo sourceKind;
            inputSource.TryGetSourceKind(inputSourceId, out sourceKind);
            switch (sourceKind)
            {
                case InteractionSourceInfo.Hand:
                    inputSource.TryGetGripPosition(inputSourceId, out currentHandPosition);
                    break;
                case InteractionSourceInfo.Controller:
                    inputSource.TryGetPointerPosition(inputSourceId, out currentHandPosition);
                    break;
            }
            transform.position += (currentHandPosition - prevHandPosition) * TranslationSensitivity;
            prevHandPosition = currentHandPosition;
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        inputSource = null;
        inputSourceId = 0;
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
        inputSource = null;
        inputSourceId = 0;
    }
}
