using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ARSpawner : MonoBehaviour
{
    private ARGameManager gameManager;
    private ARTapToPlaceObjects groundPlacement;
    private Vector3 groundDimMin;
    private Vector3 groundDimMax;

    public ARGamePiece[] gamePieces; //prefab list
    public List<ARGamePiece> spawnedPieces; //spawned pieces in the game

    public int foeCount;
    public int friendCount;
    public int friendMinusFoe;

    private float waitSpawnTime = 5f;
    public static float maxSpawns = 20f;
    private bool freeSpawnSpotsAvailable = false;

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        groundPlacement = FindObjectOfType<ARTapToPlaceObjects>();
        gameManager = FindObjectOfType<ARGameManager>();
    }

    private void Start()
    {
        Debug.Log("ground is set: " + groundPlacement.GroundSet());
        if (groundPlacement.GroundSet())
        {
            groundDimMin = groundPlacement.PlacedObjectDimMin;
            groundDimMax = groundPlacement.PlacedObjectDimMax;
        }
    }

    void Update()
    {
        if (groundPlacement.GroundSet())
        {
            if(spawnCoroutine != null)
            {
                return; // already spawning
            }

            CheckSpawnSpots();
            Debug.Log("free spots: " + freeSpawnSpotsAvailable);

            if (freeSpawnSpotsAvailable)
            {
                spawnCoroutine = StartCoroutine(SpawnRandomGamePieces());
            }

            CountFriendAndFoes();
        }
    }

    private void PlaceObjectAtPosition(GameObject placedObject, Vector3 position)
    {
        Instantiate(placedObject, position, Quaternion.identity);
    }

    private void CheckSpawnSpots()
    {
        int spawned;
        if (spawnedPieces == null)
        {
            spawned = 0;
        }
        else
        {
            spawned = spawnedPieces.Count;
        }

        float freeSpots = maxSpawns - spawned;

        if (freeSpots > 0)
        {
            freeSpawnSpotsAvailable = true;
        }
        else
        {
            freeSpawnSpotsAvailable = false;
        }
    }

    private IEnumerator SpawnRandomGamePieces()
    {
        yield return new WaitForSeconds(Random.Range(1f, waitSpawnTime));

        ARGamePiece spawned = Instantiate<ARGamePiece>(RandomGamePiece(gamePieces), RandomPosition(), Quaternion.identity);
        spawnedPieces.Add(spawned.GetComponentInChildren<ARGamePiece>());

        Debug.Log("spawned list count after spawn: " + spawnedPieces.Count + "\n" + spawned.name + " insantiated at: " + spawned.gameObject.transform.position);

        spawnCoroutine = null;
    }

    public void SpawnRandomFoeGamePiece()
    {
        List<ARGamePiece> foes = null;
        foreach (ARGamePiece piece in gamePieces)
        {
            if (piece.foe == true)
            {
                Debug.Log(0);
                foes.Add(piece);
            }
        }

        ARGamePiece spawned = Instantiate<ARGamePiece>(RandomGamePiece(foes.ToArray()), RandomPosition(), Quaternion.identity);
        spawnedPieces.Add(spawned);

        Debug.Log(spawned.name + " FOE insantiated at: " + spawned.gameObject.transform.position);
    }

    private ARGamePiece RandomGamePiece(ARGamePiece[] gamePieces)
    {
        return gamePieces[Random.Range(0, gamePieces.Length)];
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(groundDimMin.x, groundDimMax.x);
        float y = Random.Range(groundDimMin.y, groundDimMax.y);
        float z = Random.Range(groundDimMin.z, groundDimMax.z);

        return new Vector3(x, y, z);
    }

    private Vector3 AdjustedGamePiecePosition(Vector3 position, GameObject piece)
    {
        Debug.Log(piece.GetComponent<Renderer>());
        Vector3 piecePos = piece.GetComponent<Renderer>().bounds.size;
        return new Vector3(position.x + piecePos.x, position.y + piecePos.y, position.z + piecePos.z);
    }

    public void CountFriendAndFoes()
    {
        ARGamePiece[] piecesOnGround = FindObjectsOfType<ARGamePiece>();

        foeCount = 0;
        friendCount = 0;

        foreach (ARGamePiece piece in spawnedPieces)
        {
            if (piece.foe == true && piece.friend == false)
            {
                //foe
                foeCount++;
            }
            else if (piece.foe == false && piece.friend == true)
            {
                //friend
                friendCount++;
            }
            else
            {
                //invalid
                Debug.Log("Cannot be friend and foe at the same time!");
            }
        }

        friendMinusFoe = friendCount - foeCount;

        Debug.Log($"current game pieces on board: {spawnedPieces.Count}\nfriendCount: {friendCount}, foeCount: {foeCount}\n points: {friendMinusFoe}");
    }

    //public bool RemovedPieceFromSpawnedList(ARGamePiece piece)
    //{
    //    bool removed = spawnedPieces.Remove(piece);
    //    return removed;
    //}
}
