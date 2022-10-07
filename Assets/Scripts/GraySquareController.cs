using UnityEngine;
using UnityEngine.Events;

public class GraySquareController : MonoBehaviour
{
    public SpriteRenderer graySquareSprite;
    [HideInInspector]
    public Vector2 intendedPosition = Vector2.zero;
    [HideInInspector]
    public Vector2 intendedSize = Vector2.zero;
    [HideInInspector]
    public bool showIntendedPositionAndSize = false;
    [HideInInspector]
    public UnityEvent MouseUp = new UnityEvent();
    public bool isLocked = false;
    private float startPosX = 0f;
    private float startPosY = 0f;
    private int lastOrderInLayer;
    public PocketController myPocketController { get; set; }
    public PocketController myPrevPocketController { get; set; }

    private void OnMouseDown()
    {
        if (isLocked) return;

        lastOrderInLayer = graySquareSprite.sortingOrder;
        graySquareSprite.sortingOrder = 3;

        Vector2 mousePosition = GetMousePosition();

        startPosX = mousePosition.x - transform.localPosition.x;
        startPosY = mousePosition.y - transform.localPosition.y;
    }

    private void OnMouseDrag()
    {
        if (isLocked) return;

        Vector2 mousePosition = GetMousePosition();
        transform.localPosition = new Vector2(mousePosition.x - startPosX, mousePosition.y - startPosY);
    }

    private void OnMouseUp()
    {
        if (isLocked) return;

        MouseUp?.Invoke();
        graySquareSprite.sortingOrder = lastOrderInLayer;

        if (myPocketController == null) myPrevPocketController.SetGraySquareToPocket(transform);
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePosition;

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            mousePosition = Input.touches[0].position;
        else
            mousePosition = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void Update()
    {
        if (showIntendedPositionAndSize)
        {
            graySquareSprite.transform.position = Vector2.Lerp(graySquareSprite.transform.position, intendedPosition, 10f * Time.deltaTime);
            graySquareSprite.size = Vector2.Lerp(graySquareSprite.size, intendedSize / transform.lossyScale, 10f * Time.deltaTime);
        }
        else
        {
            graySquareSprite.transform.position = Vector2.Lerp(graySquareSprite.transform.position, transform.position, 15f * Time.deltaTime);
            if (myPocketController == null)
                graySquareSprite.size = Vector2.Lerp(graySquareSprite.size, new Vector2(1f, 1f), 10f * Time.deltaTime);
            else
                graySquareSprite.size = Vector2.Lerp(graySquareSprite.size, myPocketController.pocketRegion.size / transform.lossyScale, 10f * Time.deltaTime);

        }
    }
}
