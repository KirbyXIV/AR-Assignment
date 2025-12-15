//AR Multiple Image Tracker
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;


public class ARMultiImageTracker : MonoBehaviour
{
    //prefabs that will be spawned
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();
    //reference to tracked image manager
    private ARTrackedImageManager trackedImageManager;
    //spawned prefabs, indexed by name
    private Dictionary<string, GameObject> arObjects;

    // Badge tracking
    [SerializeField] private GameObject catVRbadgeUI; // UI to show when scanning an image
    [SerializeField] private GameObject UniLogobadgeUI; // UI to show when scanning an image
    [SerializeField] private GameObject CSSbadgeUI; // UI to show when scanning an image
    [SerializeField] private GameObject ArcadeMachinebadgeUI; // UI to show when scanning an image
    [SerializeField] private GameObject tooltipUI; // UI to show when scanning an image
    [SerializeField] private GameObject allBadgesCompleteUI; // UI to show when all badges collected
    [SerializeField] private TextMeshProUGUI badgeProgressText; // Text to show progress like "1/3"
    [SerializeField] private int totalBadges = 3; // How many badges needed to complete
    private List<string> collectedBadges = new List<string>();

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

        // Show initial progress
        UpdateBadgeProgressUI();
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

    private void UpdateBadgeProgressUI()
    {
        if (badgeProgressText != null)
        {
            badgeProgressText.text = "Badges: " + collectedBadges.Count + "/" + totalBadges;
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
        
        if (image.referenceImage.name == "Apple")
        {
            // Show UI when scanning
            if (catVRbadgeUI != null)
            {
                catVRbadgeUI.SetActive(true);
                // First time scanning this image?
                if (!collectedBadges.Contains("Apple"))
                {
                    // Add to collected list
                    collectedBadges.Add("Apple");
                    Debug.Log("Badge collected: catVR! Total: " + collectedBadges.Count);

                    // Update progress text
                    UpdateBadgeProgressUI();

                    // Check if all badges collected
                    if (collectedBadges.Count >= totalBadges)
                    {
                        Debug.Log("All badges collected!");
                        if (allBadgesCompleteUI != null)
                        {
                            allBadgesCompleteUI.SetActive(true);
                            tooltipUI.SetActive(false);

                        }
                    }
                }
            }
        }
        if (image.referenceImage.name == "CSS")
        {
            if (CSSbadgeUI != null)
            {
                CSSbadgeUI.SetActive(true);
                if (!collectedBadges.Contains("CSS"))
                {
                    collectedBadges.Add("CSS");
                    Debug.Log("Badge collected: CSS! Total: " + collectedBadges.Count);

                    // Update progress text
                    UpdateBadgeProgressUI();



                    if (collectedBadges.Count >= totalBadges)
                    {
                        Debug.Log("All badges collected!");
                        if (allBadgesCompleteUI != null)
                        {
                            allBadgesCompleteUI.SetActive(true);
                            tooltipUI.SetActive(false);

                        }
                    }
                }
            } 
        }
        if (image.referenceImage.name == "UniLogo")
        {

            if (UniLogobadgeUI != null)
            {
                UniLogobadgeUI.SetActive(true);
                if (!collectedBadges.Contains("UniLogo"))
                {
                    collectedBadges.Add("UniLogo");
                    Debug.Log("Badge collected: UniLogo! Total: " + collectedBadges.Count);

                    // Update progress text
                    UpdateBadgeProgressUI();

                    if (collectedBadges.Count >= totalBadges)
                    {
                        Debug.Log("All badges collected!");
                        if (allBadgesCompleteUI != null)
                        {
                            allBadgesCompleteUI.SetActive(true);
                            tooltipUI.SetActive(false);

                        }
                    }
                }
            }
        }
        if (image.referenceImage.name == "ArcadeMachine")
        {

            if (ArcadeMachinebadgeUI != null)
            {
                ArcadeMachinebadgeUI.SetActive(true);
                if (!collectedBadges.Contains("ArcadeMachine"))
                {
                    collectedBadges.Add("ArcadeMachine");
                    Debug.Log("Badge collected: ArcadeMachine! Total: " + collectedBadges.Count);

                    // Update progress text
                    UpdateBadgeProgressUI();

                    if (collectedBadges.Count >= totalBadges)
                    {
                        Debug.Log("All badges collected!");
                        if (allBadgesCompleteUI != null)
                        {
                            allBadgesCompleteUI.SetActive(true);
                            tooltipUI.SetActive(false);
                        }
                    }
                }
            }
        }
    }
    public void ResetButton()
    {
        UniLogobadgeUI.SetActive(false);
        CSSbadgeUI.SetActive(false);
        catVRbadgeUI.SetActive(false);
        ArcadeMachinebadgeUI.SetActive(false);
    }
}
