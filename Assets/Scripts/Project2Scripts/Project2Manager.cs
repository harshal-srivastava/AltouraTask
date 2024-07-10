using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void AttachEventSpecificListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent += InitializeProject2;
        Project2UIManager.ActivateTeleportationEvent += TeleportPlayerAndUI;
    }

    private void DetachEventSpecficiListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent -= InitializeProject2;
        Project2UIManager.ActivateTeleportationEvent -= TeleportPlayerAndUI;
    }

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
            
        //instantiating model
        glbModel = GameObject.Instantiate(glbModelPrefab, project2ObjectsHolder);
        glbModel.transform.localScale = new Vector3(6, 6, 6);
        glbModel.transform.localPosition = new Vector3(0, 0.5f, 0);
        BoxCollider modelBox = glbModel.AddComponent<BoxCollider>();
        modelBox.center = new Vector3(0, 0, 1);
        modelBox.size = new Vector3(1, 1, 1.85f);
    }

    private void TeleportPlayerAndUI(bool reverse)
    {
        StartCoroutine(BeginTeleportation(reverse));
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            InitializeProject2();
        }
    }

    private void OnDestroy()
    {
        DetachEventSpecficiListeners();
    }
}
