using UnityEngine;

public class ParticleSpawnerForDelete : MonoBehaviour
{
    public GameObject particleTemplate;
    public float distanceFromCamera;
    public bool particlesSpawned {get; private set;} = false;
    private Vector3 spawnPoint;
    private WorkspaceRotation workspaceRotation;
    private GameObject particlesInstance;

    public void GetDataFromTouch(Vector2 touchPos, WorkspaceRotation workspaceRotation_) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        workspaceRotation = workspaceRotation_;
        Piece piece;
    
        
        if (Physics.Raycast(ray, out hit, 1000)) {
            if (hit.transform.TryGetComponent(out piece) || // for PiecePG
                (hit.transform.parent != null && hit.transform.parent.parent != null && hit.transform.parent.parent.TryGetComponent(out piece)) /* for standard Piece */) {
                Vector3 fwd = Camera.main.transform.forward;
                Vector3 dir = ray.direction;
                dir *= distanceFromCamera * (fwd.z / dir.z);

                particlesSpawned = true;
                spawnPoint = Camera.main.transform.position + dir;
                if (particlesInstance == null)
                    particlesInstance = Instantiate(particleTemplate);
                ParticleSystem.MainModule settings = particlesInstance.GetComponent<ParticleSystem>().main;
                settings.startColor = new ParticleSystem.MinMaxGradient(piece.color);
                particlesInstance.transform.position = spawnPoint;
            }
        }
        
    }

    void Start() {

    }

    void Update() {
        if (particlesSpawned) {
            if ((!workspaceRotation.oneFingerOn || workspaceRotation.movingStarted || workspaceRotation.raySent)
                #if UNITY_EDITOR
                && !Input.GetMouseButton(1)
                #endif
                )
            {
                particlesSpawned = false;
                if (particlesInstance != null)
                    particlesInstance.GetComponent<ParticleSystem>().Stop(false);

                return;
            }


        }
    }


}
