using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARGamePiece : MonoBehaviour
{
    private ARTapToPlaceObjects groundPlacementRef;

    public bool friend;
    public bool foe;

    private void Awake()
    {
        groundPlacementRef = FindObjectOfType<ARTapToPlaceObjects>();
    }

    private void OnMouseDown()
    {
        int n = 0;
        ARGamePiece parentPiece = gameObject.transform.parent.GetComponent<ARGamePiece>();
        Debug.Log("clicked piece (this): " + this.name + "\nparentPiece: " + parentPiece);
        //////groundPlacementRef.SpawnerRef.RemovePieceFromSpawnedList(this);
        foreach (ARGamePiece p in groundPlacementRef.SpawnerRef.spawnedPieces)
        {
            Debug.Log(n + " " + p);
            n++;
        }

        //Debug.Log("this in list " + groundPlacementRef.SpawnerRef.spawnedPieces.Contains(parentPiece));
        //bool removedChild = groundPlacementRef.SpawnerRef.spawnedPieces.Remove(this.GetComponentInParent<ARGamePiece>());
        //Debug.Log("child removed " + removedChild);

        bool removed = groundPlacementRef.SpawnerRef.spawnedPieces.Remove(parentPiece); // remove game piece from list
        Debug.Log("object removed " + removed);
        Destroy(gameObject); // remove game object from the game scene
    }
}
