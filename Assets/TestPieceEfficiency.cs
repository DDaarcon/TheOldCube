using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class TestPieceEfficiency : MonoBehaviour
{
    public GameObject piecePGPrefeb;
    private List<GameObject> stackOfPieces;

    private readonly bool[,] setting = new bool[4, 4]{
                {I, I, I ,I}, 
                {I, I, I, I},
                {I, I, I, I},
                {I, I, I, I}};
    private float stackHeight = 20f;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.init(1000);
        stackOfPieces = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            GameObject newPiece = Instantiate(piecePGPrefeb, new Vector3(transform.position.x, stackHeight, transform.position.z), transform.rotation);
            stackOfPieces.Add(newPiece);
            newPiece.GetComponent<PiecePG>().ChangeSetting(setting);
            stackHeight += 20f;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            while (stackOfPieces.Count > 0) {
                stackOfPieces[stackOfPieces.Count - 1].GetComponent<PiecePG>().InitializeDecay(2f);
                stackOfPieces.RemoveAt(stackOfPieces.Count - 1);
                stackHeight -= 20f;
            }
        }
    }
}
