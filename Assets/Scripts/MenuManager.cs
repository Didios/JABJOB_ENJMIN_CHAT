using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Canvas informations")]
    // scoreboard
    public SatisfactionBar scorebar;
    public TextMeshProUGUI levelText;

    // Menus info
    public Transform canvasMenu;
    public Transform canvasInGame;

    public Vector3 posUse; 
    public Vector3 posStock;
    public Vector3 posCameraMenu;

    // infos
    private bool inGame = false;
    private int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();

        ownerPathFinding = owner.GetComponent<PathFinding>();

        levelText.text = "Start a new Game !";
        Menu();
    }

    private void Update()
    {
        if (inGame)
        {
            if (ownerHand.finish) // lose case
            {
                if (ownerHand.lose)
                {
                    levelText.text = "You Lose";
                }
                else
                {
                    level += 1;
                    levelText.text = "Next Level";
                }
                Menu();
            }
            else if (scorebar.finish) // win case
            {
                level += 1;

                levelText.text = "You Win";
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
        // arrêt mécanique de jeu
        ownerPathFinding.activate = false;
        ownerHand.activate = false;

        playerRigidbody.useGravity = false;
        playerController.activate = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // repositionnement menu
        player.position = posCameraMenu;
        player.rotation = new Quaternion();

        canvasInGame.position = posStock;
        canvasMenu.position = posUse;

        // reset
        scorebar.ResetBar(3);
        inGame = false;
    }
}
