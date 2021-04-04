using UnityEngine;
using System.Collections.Generic;

public class ParticleSpawnerForFinish : MonoBehaviour
{
    public ScreenOrientationScript screenOrientationScript;
    public GameObject particleTemplate;
    public Transform workspace;
    public float distanceFromCamera;
    public RectTransform touchableArea;
    public float margin = 50f;
    public int rangeMin;
    public int rangeMax;
    public float marginWithin = 10f;
    public float minimumDistanceBetweenInstances = 50f;
    public float scaleWithDistance = 7f;
    public float distanceFormCameraRange = 100f;
    public float startDelayRange = 2f;

    // for screen orientation change
    public void ApplySettings(float camDis, float partDis, float camDisRange) {
        distanceFromCamera = camDis;
        minimumDistanceBetweenInstances = partDis;
        distanceFormCameraRange = camDisRange;
    }

    private void SpawnParticlesRandomly(Vector3 leftBottom, float width, float height, int parts) {
        if (rangeMax < rangeMin) {int temp = rangeMax; rangeMax = rangeMin; rangeMin = temp;}
        int amount = Random.Range(rangeMin / parts, rangeMax / parts > 0 ? rangeMax / parts : 1);

        Vector3 fwd = Camera.main.transform.forward;
        List<Vector3> allPositions = new List<Vector3>();

        for (int i = 0; i < amount; i++) {
            GameObject particleInstance = Instantiate<GameObject>(particleTemplate);

            // generate position with given distance from others
            Vector3 positionOnCanvas;
            bool breakLoop = false;
            int tooMuchTries = 0;
            do {
                positionOnCanvas = leftBottom;
                positionOnCanvas.x += marginWithin + Random.Range(0f, width - marginWithin * 2f);
                positionOnCanvas.y += marginWithin + Random.Range(0f, height - marginWithin * 2f);
                
                if (i == 0) {
                    allPositions.Add(positionOnCanvas);
                    breakLoop = true;
                } else {
                    breakLoop = true;
                    foreach (Vector3 x in allPositions) {
                        if (Vector3.Distance(x, positionOnCanvas) <= minimumDistanceBetweenInstances) {
                            breakLoop = false;
                            break;
                        }
                    }
                    if (breakLoop) allPositions.Add(positionOnCanvas);
                }
                tooMuchTries++;
            } while (!breakLoop && tooMuchTries < 100);
            if (tooMuchTries == 100) Debug.Log("too much tries");

            // direction from position where are spawned (on canvas) to camera
            Vector3 dir = positionOnCanvas - Camera.main.transform.position;

            // distance from camera with included random value
            float customDistanceFromCamera = (distanceFromCamera)+ Random.Range(-distanceFormCameraRange / 2, distanceFormCameraRange / 2);
            dir *= customDistanceFromCamera * (fwd.z / dir.z);
            Vector3 spawnPoint = Camera.main.transform.position + dir;

            particleInstance.transform.position = spawnPoint;
            ParticleSystem.MainModule settings = particleInstance.GetComponent<ParticleSystem>().main;
            settings.startDelay = Random.Range(0f, startDelayRange);

            Debug.DrawLine(positionOnCanvas, Camera.main.transform.position, Color.red, 3f);
        }
    }
    public void PlayFinishParticlesRandomly(int parts = 1) {
        // corners calculation (including margins)
        Vector3[] corners = new Vector3[4];
        touchableArea.GetWorldCorners(corners);
        Vector3 leftBottom = corners[0];
        leftBottom.x += margin; leftBottom.y += margin;
        float width = corners[2].x - corners[1].x - margin * 2;
        float height = corners[1].y - corners[0].y - margin * 2;    

        PlayFinishParticlesFromWorkspace();

        for (int i = 0; i < parts; i++) {
            Vector3 leftBottomForPart = leftBottom;
            leftBottomForPart.x += (width / parts) * (i);
            leftBottomForPart.y += (height / parts) * (i);
            SpawnParticlesRandomly(leftBottomForPart, width / parts, height / parts, parts);
        }
    }

    public void PlayFinishParticlesFromWorkspace() {
        // calculate spawn position
        float workspaceDistanceFactor = (distanceFromCamera) / workspace.position.z;
        Vector3 workspaceFixedPosition = workspace.position * workspaceDistanceFactor;
        Debug.DrawLine(Camera.main.transform.position, workspaceFixedPosition, Color.black, 3f);

        if (rangeMax < rangeMin) {int temp = rangeMax; rangeMax = rangeMin; rangeMin = temp;}
        int amount = Random.Range(rangeMin, rangeMax);

        for (int i = 0; i < amount; i++) {
            GameObject particleInstance = Instantiate<GameObject>(particleTemplate);

            // particle system should rotate along local y axis

            // rotate by random angle
            float minAngle = -45f, maxAngle = 45f;
            particleInstance.transform.Rotate(workspaceFixedPosition - Camera.main.transform.position, Random.Range(minAngle, maxAngle), Space.World);
            

            // direction from position where are spawned (on canvas) to camera
            // Vector3 dir = positionOnCanvas - Camera.main.transform.position;

            // distance from camera with included random value
            // float customDistanceFromCamera = distanceFromCamera + Random.Range(-distanceFormCameraRange / 2, distanceFormCameraRange / 2);
            // dir *= customDistanceFromCamera * (fwd.z / dir.z);
            // Vector3 spawnPoint = Camera.main.transform.position + dir;

            particleInstance.transform.position = workspaceFixedPosition;
            ParticleSystem.MainModule settings = particleInstance.GetComponent<ParticleSystem>().main;
            settings.startDelay = Random.Range(0f, startDelayRange);
            
        }


    }

    void Start() {

    }

    void Update() {
        
    }


}
