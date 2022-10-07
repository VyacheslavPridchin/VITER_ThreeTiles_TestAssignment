using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PocketController : MonoBehaviour
{
    public SpriteRenderer pocketRegion;
    [HideInInspector]
    public UnityEvent GraySquareChanged = new UnityEvent();
    [SerializeField]
    private bool lockGraySquad = false;
    public GameObject MyGraySquare { get; set; }
    private UnityAction actionStorage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Rectangle") return;
        if (MyGraySquare != null) return;

        GraySquareController gsController = collision.GetComponent<GraySquareController>();
        actionStorage = () =>
        {
            SetGraySquareToPocket(collision.transform);
            GraySquareChanged?.Invoke();
        };
        gsController.MouseUp.AddListener(actionStorage);
        gsController.intendedPosition = transform.position;
        gsController.intendedSize = pocketRegion.size;
        gsController.showIntendedPositionAndSize = true;

        // Set GraySquare elements in Pocket on Start
        if (Input.touchCount <= 0 && !Input.GetMouseButton(0))
            SetGraySquareToPocket(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Rectangle") return;

        GraySquareController gsController = collision.GetComponent<GraySquareController>();
        gsController.MouseUp.RemoveListener(actionStorage);
        gsController.showIntendedPositionAndSize = false;

        if (collision.gameObject == MyGraySquare)
        {
            gsController.myPocketController = null;
            gsController.myPrevPocketController = this;
            MyGraySquare = null;
            GraySquareChanged?.Invoke();
        }
    }

    public void SetGraySquareToPocket(Transform transform)
    {
        GraySquareController gsController = transform.GetComponent<GraySquareController>();
        gsController.graySquareSprite.transform.position = gsController.graySquareSprite.transform.position - (this.transform.position - transform.position);
        gsController.myPocketController = this;
        gsController.myPrevPocketController = null;
        transform.position = this.transform.position;
        MyGraySquare = transform.gameObject;

        if (lockGraySquad) gsController.isLocked = true;
    }

    public void RemoveGraySquare(float timeOffset)
    {
        GameObject removeableGO = MyGraySquare;
        MyGraySquare = null;
        StartCoroutine(DestroyWithAnimation(removeableGO, timeOffset));
    }

    private IEnumerator DestroyWithAnimation(GameObject removeableGO, float timeOffset)
    {
        yield return new WaitForSecondsRealtime(0.1f);

        Transform spriteRenderer = removeableGO.transform.GetChild(0);

        Vector3 targetVector = Vector3.one * 1.3f;
        while (spriteRenderer.localScale.x < 1.2f)
        {
            spriteRenderer.localScale = Vector3.Lerp(spriteRenderer.localScale, targetVector, 4f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            //Debug.Log("Change color");
        }

        yield return new WaitForSecondsRealtime(timeOffset);

        targetVector = Vector3.zero;
        while (spriteRenderer.localScale.x > 0.01f)
        {
            spriteRenderer.localScale = Vector3.Lerp(spriteRenderer.localScale, targetVector, 7f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            //Debug.Log("Change color");
        }

        Destroy(removeableGO);
    }
}
