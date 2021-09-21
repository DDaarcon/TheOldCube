using UnityEngine;
using System;

public class CubeToGrid : MonoBehaviour
{
    public GameScript gameManager;
    public ScreenOrientationScript screenOrientationScript;
    public Transform touchableArea;
    public float time = 0.5f;
    public bool gridViewIsOn {get; private set;} = false;
    public bool duringTween {get; private set;} = false;


    enum Situation {Horizontal, Vertical};
    private Vector3 center;
    private Situation situation;
    private Vector3[] positionForSides;
    private Vector3[] previousPositions;
    private Vector3[] previousRotations;
    
    private void CalculatePositions(ScreenOrientation orientation) {
        Vector3 downDirection = new Vector3();
        Vector3 rightDirection = new Vector3();
        if (/* orientation == ScreenOrientation.Portrait */ true) {
            center = touchableArea.position;
            float distanceFactor = transform.position.z / center.z;
            center *= distanceFactor;
            
            Vector3 centerToCam = Camera.main.transform.position - center;
            rightDirection = Camera.main.transform.right;
            downDirection = Vector3.Cross(centerToCam, rightDirection).normalized;
        }
        if (orientation == ScreenOrientation.LandscapeLeft ||
            orientation == ScreenOrientation.LandscapeRight) 
        {

        }
        
        int blocks = (int)gameManager.variant;
        float length = GameScript.lengthOfSide;

        positionForSides = new Vector3[6];
        positionForSides[0] = center - (downDirection * blocks * length / 2f);
        positionForSides[1] = positionForSides[0] + (rightDirection * blocks * length);
        positionForSides[2] = positionForSides[0] - (downDirection * blocks * length);
        positionForSides[3] = center + (downDirection * blocks * length / 2f);
        positionForSides[4] = positionForSides[0] - (rightDirection * blocks * length);
        positionForSides[5] = positionForSides[3] + (downDirection * blocks * length);

        // Debug.DrawLine(Camera.main.transform.position, positionForSides[0], Color.blue, 1f);
        // Debug.DrawLine(Camera.main.transform.position, positionForSides[1], Color.blue, 1f);
        // Debug.DrawLine(Camera.main.transform.position, positionForSides[2], Color.blue, 1f);
        // Debug.DrawLine(Camera.main.transform.position, positionForSides[3], Color.blue, 1f);
        // Debug.DrawLine(Camera.main.transform.position, positionForSides[4], Color.blue, 1f);
        // Debug.DrawLine(Camera.main.transform.position, positionForSides[5], Color.blue, 1f);
    }

    public void TurnOnGridView() {
        if (!gridViewIsOn && !duringTween)
        {
            CalculatePositions(screenOrientationScript.screenOrientation);

            GameObject[] pieces = gameManager.GetFinalPieces();
            previousPositions = /* gameManager.positionForSides */ new Vector3[6];
            previousRotations = /* gameManager.rotationForSides */ new Vector3[6];
            LTDescr lastTween = null;
            Vector3 posToCamDir = Camera.main.transform.position - positionForSides[0];

            for (int i = 0; i < pieces.Length; i++) {
                if (gameManager.placedSidesArray[i])
                {
                    duringTween = true;
                    previousPositions[i] = pieces[i].transform.position;
                    previousRotations[i] = pieces[i].transform.rotation.eulerAngles;
                    LeanTween.move(pieces[i], positionForSides[i], time);
                    LeanTween.rotate(pieces[i], (Quaternion.LookRotation(posToCamDir) * Quaternion.AngleAxis(90f, Vector3.right)).eulerAngles, time);
                }
            }
            if (lastTween != null)
                lastTween.setOnComplete(() => {duringTween = false;});


            gridViewIsOn = true;
        }
    }

    public void TurnOffGridView() {
        if (gridViewIsOn)
        {
            GameObject[] pieces = gameManager.GetFinalPieces();
            LTDescr lastTween = null;

            for (int i = 0; i < pieces.Length; i++)
            {
                if (gameManager.placedSidesArray[i])
                {
                    duringTween = true;
                    lastTween = LeanTween.move(pieces[i], previousPositions[i], time);
                    LeanTween.rotate(pieces[i], previousRotations[i], time);
                }
            }
            if (lastTween != null)
                lastTween.setOnComplete(() => {duringTween = false;});

            gridViewIsOn = false;
        }
    }

    private void RotateWorkspaceTowardsCamera() {
    }

    private void Start() {
        CalculatePositions(screenOrientationScript.screenOrientation);
        
    }
    private void Update() {

        #if UNITY_EDITOR

        if (Input.GetKey(KeyCode.G)) {
            TurnOnGridView();
        } else {
            TurnOffGridView();
        }
        #endif
    }
}
