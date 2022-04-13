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
    [HideInInspector] public PathFinding pathfindingOwner;
    [HideInInspector] public OwnerHand handOwner;
    [SerializeField] private Vector3 localSpawnOwner;
    [SerializeField] private Vector3 localSpawnPlayer;

    [Header("Level")]
    [SerializeField] private Transform levelObject;
    private Transform tempLevel;
    private Vector3 posLevel;
    [SerializeField] private Vector3 saveMove;

    // other infos
    private bool hasBeenSet = false;
    [HideInInspector] public Vector3 worldSpawnOwner { get { return localSpawnOwner + posLevel; } private set { } }
    [HideInInspector] public Vector3 worldSpawnPlayer { get { return localSpawnPlayer + posLevel; } private set { } }
    [HideInInspector] public Vector3 worldSaveMove { get { return levelObject.position + saveMove; } private set { } }

    private void SetLevel()
    {
        posLevel = levelObject.position;
        levelObject.gameObject.SetActive(false);
        levelObject.position += saveMove;
        //levelObject.gameObject.SetActive(true);
    }

    public void Clean()
    {
        if (tempLevel != null) Object.Destroy(tempLevel.gameObject);
        if (owner != null) Object.Destroy(owner.gameObject);
    }

    public void Reset()
    {
        if (!hasBeenSet)
        {
            SetLevel();
            hasBeenSet = true;
        }

        Clean();

        // recreate Level
        tempLevel = Object.Instantiate<Transform>(levelObject, posLevel, levelObject.rotation);
        tempLevel.gameObject.SetActive(true);
        ResetOwner();

        Debug.Log($"[LevelConfig]:\n Level N°{levelNumber} Reset");
    }

    public void ResetOwner()
    {
        if (owner != null) Object.Destroy(owner.gameObject);
        owner = Object.Instantiate<Transform>(originOwner, worldSpawnOwner, originOwner.rotation);
        handOwner = owner.GetComponentInChildren<OwnerHand>();
        pathfindingOwner = owner.GetComponentInChildren<PathFinding>();

        Debug.Log($"[LevelConfig]:\n Owner N°{levelNumber} Reset");
    }
}
