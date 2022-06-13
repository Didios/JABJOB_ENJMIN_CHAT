using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Character")]
    public Transform player;
    private Rigidbody playerRigidbody;
    private FPSController playerController;
    private CharacterController playerCharacterController;

    /* OWNER */
    //private PathFinding ownerPath;
    //private OwnerHand ownerHand;
    private StateInfoController owner;
    private MachineStateInfo ownerInfos;
    private OwnerHandState ownerHand;

    [Header("UI General")]
    public Vector3 posCameraMenu;
    public List<GameObject> canvasList;

    [Header("UI InGame")]
    public SatisfactionBar scorebar;
    public TextMeshProUGUI messageGame;

    [Header("UI Menu")]
    public TextMeshProUGUI levelText;

    [Header("UI HowToPlay")]
    public List<GameObject> panelList;

    [Header("Canvas sub-infos")]
    public Button playButton;

    [Header("Info Level")]
    public List<LevelConfig> levels = new List<LevelConfig>();

    // infos
    private bool inGame = false;
    private int lvl = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerCharacterController = player.GetComponent<CharacterController>();

        levels[lvl].Reset();
        //ownerPath = levels[lvl].pathfindingOwner;
        //ownerHand = levels[lvl].handOwner;
        owner = levels[lvl].controllerOwner;
        ownerInfos = owner.infos;
        ownerHand = levels[lvl].handOwner;

        levelText.text = "Start a new Game !";
        Menu();
    }

    private void Update()
    {
        if (inGame)
        {
            if (ownerInfos.gameOver)//(ownerHand.finish) // lose case
            {
                levelText.text = "You Lose";
                NextLevel(false); //retour ecran principal
                /*
                if (ownerHand.lose)
                {
                    levelText.text = "You Lose";

                    NextLevel(false);//fonction ecran perdu retour au spawnpoint
                }
                else
                {
                    levelText.text = "Next Level";
                    NextLevel(true);
                }
                */
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
        messageGame.text = "";

        levels[lvl].Reset();
        owner = levels[lvl].controllerOwner;
        ownerInfos = owner.infos;

        //player
        playerRigidbody.useGravity = true;
        playerController.activate = true;
        playerController.ResetShoot();
        playerRigidbody.constraints = RigidbodyConstraints.None;

        //owner
        //ownerPath.activate = true;
        //ownerHand.activate = true;
        owner.isActive = true;
        ownerHand.isActive = true;

        CanvasChange(2);
        inGame = true;
    }

    public void Menu()
    {
        // stop player
        playerRigidbody.useGravity = false;
        playerController.activate = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // reset du niveau/set du prochain
        // stop and set/reset owner
        //ownerPath.activate = false;
        //ownerHand.activate = false;
        owner.isActive = false;
        ownerHand.isActive = false;

        //ownerPath = levels[lvl].pathfindingOwner;
        //ownerHand = levels[lvl].handOwner;

        // placement camera (player)
        player.position = posCameraMenu;
        player.rotation = new Quaternion();

        CanvasChange(0);

        // other
        //playButton.interactable = true;
        inGame = false;
    }

    public void NextLevel(bool win)
    {
        levels[lvl].Clean();
        //playButton.interactable = false;
        if (win) lvl += 1;

        if (lvl >= levels.Count)
        {
            levelText.text = "You Finish C.A.T\nThanks for Playing !";
            lvl = 0;
            Menu();
        }
        else
        {
            playerCharacterController.enabled = false;
            player.position = levels[lvl].worldSpawnPlayer;
            playerCharacterController.enabled = true;

            scorebar.ResetBar(levels[lvl].scoreToGet);

            if (win) Game();
            else Menu();
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("[MenuManager]\n Game quit");
    }

    public void CanvasChange(int index)
    {
        for(int i = 0; i < canvasList.Count; i++)
        {
            canvasList[i].SetActive(i == index);
        }
        Debug.Log("[MenuManager]\n Change Canvas");
    }

    public void PanelChange(int index)
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].SetActive(i == index);
        }
        Debug.Log("[MenuManager]\n Change Panel");
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos Levels
        foreach (LevelConfig L in levels)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(L.worldSpawnPlayer, 0.75f);
            Gizmos.DrawWireSphere(L.worldSpawnOwner, 0.75f);

            Gizmos.color = Color.grey;
            Gizmos.DrawWireCube(L.worldSaveMove, new Vector3(3, 3, 3));
        }
    }
}
