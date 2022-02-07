using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Character")]
    // player
    public Transform player;
    private Rigidbody playerRigidbody;
    private FPSController playerController;

    // owner
    public Transform owner;
    private PathFinding ownerPathFinding;
    [SerializeField]
    private OwnerHand ownerHand;

    [Header("Other informations")]
    // scoreboard
    public SatisfactionBar scorebar;

    // Menus info
    public Transform canvasMenu;
    public Transform canvasInGame;

    public Vector3 posUse; 
    public Vector3 posStock;

    // infos
    private bool inGame = false;
    private int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();

        ownerPathFinding = owner.GetComponent<PathFinding>();

        Menu();
    }

    private void Update()
    {
        if (inGame)
        {
            if (ownerHand.finish) // lose case
            {
                Menu();
            }
            else if (scorebar.finish) // win case
            {
                level += 1;

                Menu();
            }
        }
    }

    // Switch between screen mode
    public void Game()
    {
        playerRigidbody.useGravity = true;
        playerController.activate = true;
        playerRigidbody.constraints = RigidbodyConstraints.None;

        ownerPathFinding.activate = true;
        ownerHand.activate = true;

        canvasMenu.position = posStock;
        canvasInGame.position = posUse;

        inGame = true;
    }

    public void Menu()
    {
        playerRigidbody.useGravity = false;
        playerController.activate = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        ownerPathFinding.activate = false;
        ownerHand.activate = false;

        canvasInGame.position = posStock;
        canvasMenu.position = posUse;

        inGame = false;
    }
}
