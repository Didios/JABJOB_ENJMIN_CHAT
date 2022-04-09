using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    //owner
    private PathFinding actualOwner;
    private OwnerHand actualOwnerHand;

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

    [Header("Canvas sub-infos")]
    public Button playButton;

    [Header("Info Level")]
    [SerializeField]
    private List<Vector3> spawnpoints = new List<Vector3>();
    [SerializeField]
    private List<int> scoreToObtain = new List<int>();
    [SerializeField]
    private List<PathFinding> ownerLvl = new List<PathFinding>();
    [SerializeField]
    private List<OwnerHand> ownerHandLvl = new List<OwnerHand>();

    // infos
    private bool inGame = false;
    private int lvl = 0;
    private Vector3 spawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerCharacterController = player.GetComponent<CharacterController>();

        actualOwner = ownerLvl[0];
        actualOwnerHand = ownerHandLvl[0];

        levelText.text = "Start a new Game !";
        Menu();
    }

    private void Update()
    {
        if (inGame)
        {
            if (actualOwnerHand.finish) // lose case
            {
                if (actualOwnerHand.lose)
                {
                    levelText.text = "You Lose";

                    NextLevel(false);//fonction ecran perdu retour au spawnpoint
                }
                else
                {
                    levelText.text = "Next Level";
                    NextLevel(true);
                }
            }
            else if (scorebar.finish) // win case
            {
                levelText.text = "You Win";
                NextLevel(true);
            }
        }

        // CheatCode
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N)) NextLevel(true);
#endif
    }

    // Switch between screen mode
    public void Game()
    {        
        //player
        playerRigidbody.useGravity = true;
        playerController.activate = true;
        playerRigidbody.constraints = RigidbodyConstraints.None;

        //owner
        foreach (PathFinding agent in ownerLvl) agent.activate = false;
        foreach (OwnerHand agentHand in ownerHandLvl) agentHand.activate = false;
        actualOwner = ownerLvl[lvl];
        actualOwnerHand = ownerHandLvl[lvl];
        actualOwner.activate = true;
        actualOwnerHand.activate = true;

        canvasMenu.localPosition = posStock;
        canvasInGame.localPosition = posUse;

        inGame = true;
    }

    public void Menu()
    {
        // arrêt mécanique de jeu
        actualOwner.activate = false;
        actualOwnerHand.activate = false;

        playerRigidbody.useGravity = false;
        playerController.activate = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // repositionnement menu
        player.position = posCameraMenu;
        player.rotation = new Quaternion();

        canvasInGame.localPosition = posStock;
        canvasMenu.localPosition = posUse;

        playButton.interactable = true;

        // reset
        inGame = false;
    }

    public void NextLevel(bool win)
    {
        playButton.interactable = false;
        if (win) lvl += 1;

        if (lvl >= spawnpoints.Count)
        {
            levelText.text = "You Finish C.A.T\nThanks for Playing !";
            lvl = 0;
            Menu();
        }
        else
        {
            spawnpoint = spawnpoints[lvl];

            playerCharacterController.enabled = false;
            player.position = spawnpoint;
            playerCharacterController.enabled = true;

            scorebar.ResetBar(scoreToObtain[lvl]);

            if (win) Game();
            else Menu();
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("[MenuManager]\n Game quit");
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 spawn in spawnpoints) Gizmos.DrawSphere(spawn, 0.75f);
    }
}
