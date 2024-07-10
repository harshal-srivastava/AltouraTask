using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Project2Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject room;

    [SerializeField]
    private GameObject playerController;

    [SerializeField]
    private GameObject glbModel;

    [SerializeField]
    private Transform project2ObjectsHolder;

    private void Awake()
    {
        AttachEventSpecificListeners();
    }

    private void AttachEventSpecificListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent += InitializeProject2;
    }

    private void DetachEventSpecficiListeners()
    {
        ProjectSelectionUIManager.Project2InitiatedEvent -= InitializeProject2;
    }

    private void InitializeProject2()
    {
        GameObject roomprefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.Room);
        GameObject playerPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.Player);
        GameObject glbModelPrefab = ResourceLoaderUtil.instance.LoadPrefab(PrefabType.GLBModel);

        room = GameObject.Instantiate(roomprefab, project2ObjectsHolder);
        room.transform.localPosition = new Vector3(0, 0, 0);

        playerController = GameObject.Instantiate(playerPrefab, project2ObjectsHolder);
        playerController.transform.localPosition = new Vector3(0, 1.6f, 0);

        glbModel = GameObject.Instantiate(glbModelPrefab, project2ObjectsHolder);
        glbModel.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void OnDestroy()
    {
        DetachEventSpecficiListeners();
    }
}
