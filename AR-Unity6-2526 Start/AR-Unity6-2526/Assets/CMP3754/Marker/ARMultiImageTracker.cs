//AR Multiple Image Tracker
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARMultiImageTracker : MonoBehaviour
{
    //prefabs that will be spawned
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();
    //reference to tracked image manager
    private ARTrackedImageManager trackedImageManager;
    //spawned prefabs, indexed by name
    private Dictionary<string, GameObject> arObjects;

    private void Start()
    {
        //locate the tracked image manager component 
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        if (trackedImageManager == null) return;

        //this tells the tracked image manager to call OnImagesTrackedChanged when the tracking system detects a change
        trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        
        //create a dictionary to index prerfab instatnces we are about to spawn
        arObjects = new Dictionary<string, GameObject>();
        //instantiate and initialise prefab instances, for use as markers
        foreach(var pref in prefabsToSpawn)
        {
            var arObj = Instantiate(pref, Vector3.zero, Quaternion.identity);
            arObj.name = pref.name;
            arObj.gameObject.SetActive(false);
            arObjects.Add(arObj.name, arObj);
        }
    }

    private void OnDestroy() 
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        //for any events that are reported, call UpdateTrackedImage()
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
        }
        
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            UpdateTrackedImage(trackedImage.Value); //NB value pairs in removed list
        }
    }

    private void UpdateTrackedImage(ARTrackedImage image)
    {
        //update tracked image marker based on reported tracking state
        //if not tracked or limited tracking, hide the marker
        //otherwise set it as active and update position and orientation
        if(image== null) return;    
        if(image.trackingState is TrackingState.Limited)
        {
            arObjects[image.referenceImage.name].gameObject.SetActive(false);
            return;
        }
        if (image.trackingState is TrackingState.None)
        {
            arObjects[image.referenceImage.name].gameObject.SetActive(false);
            return;
        }
        arObjects[image.referenceImage.name].gameObject.SetActive(true);
        arObjects[image.referenceImage.name].transform.position = image.transform.position;
        arObjects[image.referenceImage.name].transform.rotation = image.transform.rotation;
    }
}
