using UnityEngine;

public class WorkspaceRotation : MonoBehaviour
{
    public GameScript GameManager;
    public ScreenOrientationScript screenOrientationScript;
    private Rect area;
    public float rotationSpeed = 0.3f;
    public float spinSpeed = 1f;
    public float decelerationRate = 0.3f;
    public float timeForDelete = 0.4f;
    public bool raySent {get; private set;} = false;
    private Vector3 lastMousePos;
    private Vector3 rotate;
    public bool oneFingerOn {get; private set;} = false;
    public bool twoFingersOn {get; private set;} = false;
    public bool movingStarted {get; private set;} = false;
    private float timeOfTheTouch = 0f;
    private int firstFingerId;
    private int firstFingerIndex;
    private int secondFingerId;
    private int secondFingerIndex;


    void Update()
    {
        Vector3 towardsCamera = Camera.main.transform.position - transform.position;
        Vector3 rightDir = new Vector3(1f, 0f, 0f);
        Vector3 upDir = Vector3.Cross(rightDir, towardsCamera);
        rightDir = transform.InverseTransformDirection(rightDir);
        upDir = transform.InverseTransformDirection(upDir);


        #if !UNITY_EDITOR
        if (twoFingersOn) {
            bool foundFirst = false, foundSecond = false;
            for (int i = 0; i < Input.touchCount; i++) {
                if (Input.GetTouch(i).fingerId == firstFingerId) {
                    foundFirst = true;
                    firstFingerIndex = i;
                }
                if (Input.GetTouch(i).fingerId == secondFingerId) {
                    foundSecond = true;
                    secondFingerIndex = i;
                }
            }
            if (!foundFirst || !foundSecond) twoFingersOn = false;

            // if just one finger is missing, set oneFingerOn to true
            // if firstFinger is missing, set secondFineger as firstFinger
            if (foundFirst && foundSecond) {}
            else if (foundFirst || foundSecond) {
                oneFingerOn = true;
                if (foundSecond) {
                    firstFingerId = secondFingerId;
                    firstFingerIndex = secondFingerIndex;
                }
            }
        }
        if (oneFingerOn) {
            bool cancel = true;
            for (int i = 0; i < Input.touchCount; i++) {
                if (Input.GetTouch(i).fingerId == firstFingerId) {
                    firstFingerIndex = i;
                    cancel = false;
                }
            }
            if (cancel) {
                oneFingerOn = false;
                movingStarted = false;
                timeOfTheTouch = 0f;
                raySent = false;
            }
        }

        // searching for touches
        if (!twoFingersOn)
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            if (screenOrientationScript.TouchableAreaInput(touch.position) &&
                touch.phase == TouchPhase.Began) 
            {
                if (!oneFingerOn) {
                    firstFingerId = touch.fingerId;
                    firstFingerIndex = i;
                    oneFingerOn = true;
                } else {
                    if (touch.fingerId != firstFingerId) {
                        secondFingerId = touch.fingerId;
                        secondFingerIndex = i;
                        oneFingerOn = false; twoFingersOn = true;
                    }
                }
            }
        }


        // rotating with 2 fingers
        if (twoFingersOn) {
            Touch firstTouch = Input.GetTouch(firstFingerIndex);
            Touch secondTouch = Input.GetTouch(secondFingerIndex);
            // where 2 fingers are
            Vector2 touchPos00 = firstTouch.position;
            Vector2 touchPos01 = secondTouch.position;
        
            // where 2 fingers were (subtraction instead of addition, to check)
            Vector2 touchPos10 = firstTouch.position - firstTouch.deltaPosition;
            Vector2 touchPos11 = secondTouch.position - secondTouch.deltaPosition;
            
            // middle positions between 2 fingers
            Vector2 middlePos0 = new Vector2((touchPos00.x + touchPos01.x) / 2f, (touchPos00.y + touchPos01.y) / 2f);   // current touches
            Vector2 middlePos1 = new Vector2((touchPos10.x + touchPos11.x) / 2f, (touchPos10.y + touchPos11.y) / 2f);   // previous touches

            // calculate angles
            float angle0 = Vector2.SignedAngle(
                new Vector2(touchPos00.x - middlePos0.x, touchPos00.y - middlePos0.y),
                Vector2.up
            );
            float angle1 = Vector2.SignedAngle(
                new Vector2(touchPos10.x - middlePos1.x, touchPos10.y - middlePos1.y),
                Vector2.up
            );
            
            // fix angle
            rotate.z = (angle1 - angle0) * -1f * spinSpeed;
            if (angle0 > 170f && angle1 < -170f) {rotate.z = -(180f - angle0 + 180f + angle1) * spinSpeed;}
            if (angle0 < -170f && angle1 > 170f) {rotate.z =  (180f - angle0 + 180f + angle1) * spinSpeed;}

            // calculate rotations
            rotate.x = (middlePos0.y - middlePos1.y) * rotationSpeed * 0.7f;
            rotate.y = (middlePos0.x - middlePos1.x) * rotationSpeed * -0.7f;
        }
        else if (oneFingerOn){
            Touch touch = Input.GetTouch(firstFingerIndex);
            // moving with 1 finger
            if (touch.phase == TouchPhase.Moved) {
                movingStarted = true;

                rotate.x = touch.deltaPosition.y * rotationSpeed * 1f;
                rotate.y = touch.deltaPosition.x * rotationSpeed * -1f;
            }
            if (touch.phase == TouchPhase.Stationary && movingStarted) {
                rotate.x = 0f; rotate.y = 0f;
            }
            // removing
            if (touch.phase == TouchPhase.Stationary && !movingStarted && !raySent && !GameManager.finishedGame) {
                // spawn particles after 0.1sec
                if (timeOfTheTouch >= 0.1f && !GameManager.GetComponent<ParticleSpawnerForDelete>().particlesSpawned)
                    GameManager.GetComponent<ParticleSpawnerForDelete>().GetDataFromTouch(touch.position, this);

                if (timeOfTheTouch >= timeForDelete) {
                    // waited [timeFordelete] sec, deleting if hitted an piece
                    GameManager.GetDataFromTouch(touch.position);
                    raySent = true;
                }
                timeOfTheTouch += Time.deltaTime;
            }
            rotate.z = Vector3.Lerp(rotate, Vector3.zero, decelerationRate).z;
        }
        else if (!twoFingersOn && !oneFingerOn) {
            rotate = Vector3.Lerp(rotate, Vector3.zero, decelerationRate);
        }
        #endif
        #if UNITY_EDITOR
        // moving with mouse
        if (Input.GetMouseButton(0)) {
            Vector3 move = Input.mousePosition - lastMousePos;
            
            if (screenOrientationScript.TouchableAreaInput(Input.mousePosition)) {
                Vector2 rotate = new Vector2(
                    move.y * rotationSpeed * 1f,
                    move.x * rotationSpeed * -1f
                );
                
                transform.rotation *= Quaternion.AngleAxis(rotate.x, rightDir);
                transform.rotation *= Quaternion.AngleAxis(rotate.y, upDir);
            }

        } else  // remove with mouse
        if (Input.GetMouseButton(1) && !GameManager.finishedGame) {
            timeOfTheTouch += Time.deltaTime;
            if (!GameManager.GetComponent<ParticleSpawnerForDelete>().particlesSpawned)
                GameManager.GetComponent<ParticleSpawnerForDelete>().GetDataFromTouch(new Vector2(Input.mousePosition.x, Input.mousePosition.y), this);
            if (timeOfTheTouch >= 1f && !raySent) {
                GameManager.GetDataFromTouch(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                raySent = true;
            }
        }
        else {
            raySent = false;
            timeOfTheTouch = 0f;
        }
        lastMousePos = Input.mousePosition;
        #else
        
        transform.rotation *= Quaternion.AngleAxis(rotate.x, rightDir);
        transform.rotation *= Quaternion.AngleAxis(rotate.y, upDir);
        transform.rotation *= Quaternion.AngleAxis(rotate.z, transform.InverseTransformDirection(towardsCamera));

        #endif
        
        
    }
}
