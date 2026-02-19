using UnityEngine;
using System.Collections.Generic;

public class SpawnTetris : MonoBehaviour
{
    public Transform[] createPieces;
    public List<GameObject> showPiece;
    public int nextPiece;

    void Start()
    {
        nextPiece = Random.Range(0, 7);
        NextPiece();
    }

    public void NextPiece()
    {
        Instantiate(createPieces[nextPiece], transform.position, Quaternion.identity);
        nextPiece = Random.Range(0, 7);
        for (int i = 0; i < showPiece.Count; i++)
        {
            showPiece[i].SetActive(false);
        }
        showPiece[nextPiece].SetActive(true);
    }
}