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
    private CharacterController playerCharacterController;

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

    [Header("Info Level")]
    [SerializeField]
    private List<Vector3> spawnpoints = new List<Vector3>();
    [SerializeField]
    private List<int> scoreToObtain = new List<int>();

    // infos
    private bool inGame = false;
    private int lvl = -1;
    private Vector3 spawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerCharacterController = player.GetComponent<CharacterController>();

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

                    Menu();//fonction ecran perdu retour au spawnpoint
                }
                else
                {
                    levelText.text = "Next Level";
                    NextLevel(false);
                }
            }
            else if (scorebar.finish) // win case
            {
                levelText.text = "You Win";
                NextLevel(false);
            }
        }

        // CheatCode
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N)) NextLevel(false);
#endif
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
        inGame = false;
    }

    public void NextLevel(bool ui)
    {
        if (inGame && ui) return;

        lvl += 1;

        if (lvl >= spawnpoints.Count)
        {
            levelText.text = "You Finish C.A.T\nThanks for Playing !";
            Menu();
        }
        else
        {
            spawnpoint = spawnpoints[lvl];

            playerCharacterController.enabled = false;
            player.position = spawnpoint;
            playerCharacterController.enabled = true;

            scorebar.ResetBar(scoreToObtain[lvl]);

            Game();
        }
    }
}
