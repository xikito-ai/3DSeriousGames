using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObjects : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    private GameObject placedObject;
    private Vector3 placedObjectDimMin;
    private Vector3 placedObjectDimMax;
    private Quaternion placedObjectRotation;

    private ARSpawner spawnerRef;
    private ARGameManager gameManager;

    private ARSessionOrigin arOrigin;
    private ARRaycastManager raycastManager;
    private bool placementPoseIsValid = false;
    private Pose placementPose;
    private Vector3 centerOfBaseGround = new Vector3(0.5f, 0.5f);
    private bool groundIsSet = false;

    public GameObject PlacedObject { get => placedObject; }
    public Vector3 PlacedObjectDimMin { get => placedObjectDimMin; set => placedObjectDimMin = value; }
    public Vector3 PlacedObjectDimMax { get => placedObjectDimMax; set => placedObjectDimMax = value; }
    public Quaternion PlacedObjectRotation { get => placedObjectRotation; set => placedObjectRotation = value; }
    public ARSpawner SpawnerRef { get => spawnerRef; set => spawnerRef = value; }

    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<ARGameManager>();
        SpawnerRef = FindObjectOfType<ARSpawner>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        PlaceGround();
    }

    private void PlaceGround()
    {
        if(!groundIsSet)
        {
            if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                objectToPlace.transform.SetPositionAndRotation(placementIndicator.transform.position, placementIndicator.transform.rotation);
                placedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);

                // set the ground only once
                groundIsSet = true;

                // setting variables related to current placedObject
                SpawnerRef = placedObject.GetComponentInChildren<ARSpawner>();
                placedObjectDimMin = placedObject.GetComponentInChildren<MeshCollider>().bounds.min;
                placedObjectDimMax = placedObject.GetComponentInChildren<MeshCollider>().bounds.max;
                placedObjectRotation = placedObject.GetComponentInChildren<MeshCollider>().transform.rotation;
            }
        }
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid && !groundIsSet)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(centerOfBaseGround);
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            // rotate placementIndicator with the camera
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z);
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public void ResetGround()
    {
        // remove current ground
        Destroy(placedObject.gameObject);
        Debug.Log("removed current ground: " + placedObject == null);

        // reset ground
        groundIsSet = false;
        PlaceGround();

        // restart game (reset all ingame values)
        gameManager.NewGame();
    }

    public bool GroundSet()
    {
        return groundIsSet;
    }
}
