using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for functionalities for the project 2
/// This includes dynamically generating the entire level by instantiating prefabs in runtime
/// </summary>
public class Project2Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject room;

    [SerializeField]
    private PlayerMovementController playerController;

    [SerializeField]
    private GameObject glbModel;

    [SerializeField]
    private GameObject project2UI;

    [SerializeField]
    private Transform project2ObjectsHolder;

    [SerializeField]
    private Vector3 playerTeleportationLocation;

    [SerializeField]
    private Vector3 UITeleportationLocation;

    private Vector3 playerLastPosition;
    private Vector3 UILastPosition;
    

    private void Awake()
    {
        AttachEventSpecificListeners();
    }

    /// <summary>
    /// Function to attach game event listeners
    /// Helps in decoupling the referencing to multiple classes
    /// </summary>
    private void AttachEventSpecificListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent += InitializeProject2;
        Project2UIManager.ActivateTeleportationEvent += TeleportPlayerAndUI;
    }

    /// <summary>
    /// Function to detach listeners to respective class game events
    /// This is done as a safe keeping in future if a scene reload is required
    /// Static events couple with delegates don't work so well on scene reloads
    /// So detach them if object is destroyed and it will be attached again when instance of class is created
    /// </summary>
    private void DetachEventSpecficiListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent -= InitializeProject2;
        Project2UIManager.ActivateTeleportationEvent -= TeleportPlayerAndUI;
    }

    /// <summary>
    /// Listener to ProjectSelectionUIManager.Project2InitiatedEvent
    /// This will load all the required prefabs from the resources folder and build out the level
    /// </summary>
    private void InitializeProject2()
    {
        GameObject roomprefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.Room);
        GameObject UI = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.Project2UIPrefab);
        GameObject playerPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.Player);
        GameObject glbModelPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.GLBModel);
        

        //instantiating room
        room = GameObject.Instantiate(roomprefab, project2ObjectsHolder);
        room.transform.localPosition = new Vector3(0, 0, 0);

        //instantiating UI
        project2UI = GameObject.Instantiate(UI, project2ObjectsHolder);
        project2UI.transform.localPosition = new Vector3(-14.76f, 11.5f, -1.52f);
        project2UI.transform.localEulerAngles = new Vector3(0, 90, 0);
        project2UI.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        //instantiating player
        playerController = GameObject.Instantiate(playerPrefab, project2ObjectsHolder).GetComponent<PlayerMovementController>();
        playerController.transform.localPosition = new Vector3(0, 1.6f, 8);

        //initializing camLookAtRef and assigning player camera to camLookAt and project2UIManager
        project2UI.GetComponent<Canvas>().worldCamera = (playerController.transform.GetChild(0).GetComponent<Camera>());
        project2UI.GetComponent<CanvasCameraLookAt>().Initialize(playerController.transform.GetChild(0));
            
        //instantiating the GLB model
        glbModel = GameObject.Instantiate(glbModelPrefab, project2ObjectsHolder);
        glbModel.transform.localScale = new Vector3(6, 6, 6);
        glbModel.transform.localPosition = new Vector3(0, 0.5f, 0);
        BoxCollider modelBox = glbModel.AddComponent<BoxCollider>();
        modelBox.center = new Vector3(0, 0, 1);
        modelBox.size = new Vector3(1, 1, 1.85f);
    }

    /// <summary>
    /// Function to enable the teleportation and start the visual effect for it
    /// </summary>
    /// <param name="reverse"></param>
    private void TeleportPlayerAndUI(bool reverse)
    {
        StartCoroutine(BeginTeleportation(reverse));
    }

    /// <summary>
    /// Coroutine to start the fade in and out effect and then change the position of the player and the UI panel
    /// Same function can be used to teleport the user back to their last location before teleportation
    /// </summary>
    /// <param name="reverse"></param>
    /// <returns></returns>
    private IEnumerator BeginTeleportation(bool reverse)
    {
        playerController.ShowTeleportEffect();
        yield return new WaitForSeconds(0.15f);
        if (reverse)
        {
            playerController.transform.localPosition = playerLastPosition;
            project2UI.transform.localPosition = UILastPosition;
        }
        else
        {
            playerLastPosition = playerController.transform.localPosition;
            UILastPosition = project2UI.transform.localPosition;
            playerController.transform.localPosition = playerTeleportationLocation;
            project2UI.transform.localPosition = UITeleportationLocation;
        }
    }

    private void OnDestroy()
    {
        DetachEventSpecficiListeners();
    }
}
