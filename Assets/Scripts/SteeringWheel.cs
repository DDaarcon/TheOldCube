using UnityEngine;
using static Enums;

public class SteeringWheel : MonoBehaviour
{
    private Vector3 relatePosition;
    private Transform touchPoint;
    private bool touchingPointer = false;
    private bool touchingWheel = false;
    private float distanceOfPointFromCenter;
    public GameScript gameManager;
    public RotOrPos rotOrPos;
    [SerializeField] private float pointerOffset;

    private Vector3[] predefinedPoints;
    private int positionsSet = 0;
    private int index = 0;

    private int fingerId;
    private int fingerIndex;

    public void TouchedPointer() {
        touchingPointer = true;
        // last touch must be touch that triggered this method
        #if !UNITY_EDITOR
        fingerId = Input.GetTouch(Input.touchCount - 1).fingerId;
        fingerIndex = Input.touchCount - 1;
        #endif
    }
    public void TouchedWheel() {
        touchingWheel = true;
        // last touch must be touch that triggered this method
        #if !UNITY_EDITOR
        fingerId = Input.GetTouch(Input.touchCount - 1).fingerId;
        fingerIndex = Input.touchCount - 1;
        #endif
    }

    // Start is called before the first frame update

    void Start()
    {
        // Remember not to set any positions on start!
        
    }

    private void SetPositions() {
        touchPoint = transform.GetChild(0);

        Vector3[] mainCircleCorners = new Vector3[4], childCircleCorners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(mainCircleCorners);
        touchPoint.GetComponent<RectTransform>().GetWorldCorners(childCircleCorners);
        float widthMain = mainCircleCorners[3].x - mainCircleCorners[0].x;
        float widthChild = childCircleCorners[3].x - childCircleCorners[0].x;

        distanceOfPointFromCenter = (widthMain / 2f - widthChild * 0.2f) + pointerOffset;
        relatePosition = transform.position;

        predefinedPoints = new Vector3[4]{
            new Vector3(relatePosition.x - distanceOfPointFromCenter, relatePosition.y, relatePosition.z),
            new Vector3(relatePosition.x, relatePosition.y + distanceOfPointFromCenter, relatePosition.z),
            new Vector3(relatePosition.x + distanceOfPointFromCenter, relatePosition.y, relatePosition.z),
            new Vector3(relatePosition.x, relatePosition.y - distanceOfPointFromCenter, relatePosition.z)
        };

        touchPoint.position = predefinedPoints[0];
    }

    public void ResetPositions() {
        positionsSet = 0;
    }

    void Update()
    {
        if (positionsSet < 2) {
            if (positionsSet == 1)
                SetPositions();
            positionsSet++;
            return;
        }
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W)) {
            SetPositions();
        }
        #endif

        bool lastStateTouchingPointer = touchingPointer;

        // checking if still touching
        #if UNITY_EDITOR
        if (!Input.GetMouseButton(0)) {
                touchingPointer = false;
                touchingWheel = false;
        }
        #else
        bool stillTouching = false;
        for (int i = 0; i < Input.touchCount; i++) {
            if (Input.GetTouch(i).fingerId == this.fingerId) {
                fingerIndex = i;
                stillTouching = true;
            }
        }
        if (!stillTouching) {
            touchingPointer = false;
            touchingWheel = false;
        }
        #endif
        
        if (touchingWheel && !touchingPointer) {
            Vector3 touchPositionScreen;
            #if UNITY_EDITOR
            touchPositionScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 700);
            #else
            Touch touch = Input.GetTouch(fingerIndex);
            touchPositionScreen = new Vector3(touch.position.x, touch.position.y, 700);
            #endif
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touchPositionScreen);
            if (Vector3.Distance(touchPos, relatePosition) >= 25f) {
                float shortest = 1000f;
                for (int i = 0; i < 4; i++) {
                    float calculated = Vector3.Distance(touchPos, predefinedPoints[i]);
                    if (calculated < shortest) {
                        shortest = calculated;
                        index = i;
                    }
                }
                touchingPointer = true;
                touchPoint.position = relatePosition + ((new Vector3(touchPos.x - relatePosition.x, touchPos.y - relatePosition.y)).normalized * distanceOfPointFromCenter);
                gameManager.GetDataFromWheel(index, rotOrPos, true);
            }
        }
        if (touchingPointer) {
            if (Input.touchCount != 0) {
                // Screen touching
                Touch touch = Input.GetTouch(fingerIndex);
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 700));
                Vector3 point = new Vector3(touchPos.x - relatePosition.x, touchPos.y - relatePosition.y);
                point = point.normalized * distanceOfPointFromCenter;
                touchPoint.position = relatePosition + point;
            } 
            #if UNITY_EDITOR
            else {
                // Mouse
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 700));
                Vector3 point = new Vector3(mousePos.x - relatePosition.x, mousePos.y - relatePosition.y);
                point = point.normalized * distanceOfPointFromCenter;
                touchPoint.position = relatePosition + point;
            }
            #endif

            float shortest = 1000f;
            for (int i = 0; i < 4; i++) {
                float calculated = Vector3.Distance(touchPoint.position, predefinedPoints[i]);
                if (calculated < shortest) {
                    shortest = calculated;
                    index = i;
                }
            }
            gameManager.GetDataFromWheel(index, rotOrPos);
        }
        
        if (!touchingPointer && lastStateTouchingPointer){
            touchPoint.position = predefinedPoints[index];
        }
    }
}
