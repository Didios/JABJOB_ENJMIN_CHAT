using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class LevelConfig
{
    [Header("Informations")]
    public int levelNumber;
    public int scoreToGet;

    [Header("Entities")]
    [SerializeField] private Transform originOwner;
    [HideInInspector] public Transform owner;
    [HideInInspector] public StateInfoController controllerOwner;
    [HideInInspector] public OwnerHandState handOwner;
    //[HideInInspector] public PathFinding pathfindingOwner;
    //[HideInInspector] public OwnerHand handOwner;
    [SerializeField] private Vector3 localSpawnOwner;
    [SerializeField] private Vector3 localSpawnPlayer;

    [Header("Level")]
    [SerializeField] private Transform levelObject;
    public Transform tempLevel;
    private Vector3 posLevel;
    [SerializeField] private Vector3 saveMove;

    // other infos
    private bool hasBeenSet = false;
    [HideInInspector] public Vector3 worldSpawnOwner { get { if (posLevel.sqrMagnitude == 0) return localSpawnOwner + levelObject.position;
            else return localSpawnOwner + posLevel;
        } private set { } }
    [HideInInspector] public Vector3 worldSpawnPlayer { get { if (posLevel.sqrMagnitude == 0) return localSpawnPlayer + levelObject.position;
            else return localSpawnPlayer + posLevel;
        } private set { } }
    [HideInInspector] public Vector3 worldSaveMove { get { return levelObject.position + saveMove; } private set { } }

    public void SetLevel()
    {
        posLevel = levelObject.position;
        if (levelNumber != 0) originOwner.gameObject.SetActive(false);
        levelObject.gameObject.SetActive(false);
        levelObject.position += saveMove;
        //levelObject.gameObject.SetActive(true);

        hasBeenSet = true;
    }

    public void Clean()
    {
        if (tempLevel != null) Object.Destroy(tempLevel.gameObject);
        if (owner != null)
        {
            if (handOwner.hasGrabbed) handOwner.player.parent = handOwner.tempTrans;
            Object.Destroy(owner.gameObject);
        }
    }

    public void Reset()
    {
        if (!hasBeenSet)
        {
            SetLevel();
        }

        Clean();

        // recreate Level
        tempLevel = Object.Instantiate<Transform>(levelObject, posLevel, levelObject.rotation);
        tempLevel.gameObject.SetActive(true);

        if (levelNumber == 0)
        {
            owner = Object.Instantiate<Transform>(originOwner, worldSpawnOwner, originOwner.rotation); //originOwner;
            controllerOwner = owner.GetComponent<StateInfoController>();
            handOwner = owner.GetComponentInChildren<OwnerHandState>();
        }
        else
        {
            ResetOwner();
        }

        Debug.Log($"[LevelConfig]:\n Level N°{levelNumber} Reset");
    }

    public void ResetOwner()
    {
        if (owner != null)
        {
            if (handOwner.hasGrabbed) handOwner.player.parent = handOwner.tempTrans;
                Object.Destroy(owner.gameObject);
        }
        owner = Object.Instantiate<Transform>(originOwner, worldSpawnOwner, originOwner.rotation);
        owner.gameObject.SetActive(true);
        //handOwner = owner.GetComponentInChildren<OwnerHand>();
        //pathfindingOwner = owner.GetComponentInChildren<PathFinding>();
        controllerOwner = owner.GetComponent<StateInfoController>();
        handOwner = owner.GetComponentInChildren<OwnerHandState>();

        Debug.Log($"[LevelConfig]:\n Owner N°{levelNumber} Reset");

    }
}
