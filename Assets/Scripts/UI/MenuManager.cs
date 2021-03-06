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
    public DioramaManager diorama;

    [Header("UI HowToPlay")]
    public List<GameObject> panelList;

    [Header("UI Credits")]
    public Credits credits;

    [Header("Canvas sub-infos")]
    public Button playButton;

    [Header("Info Level")]
    public List<LevelConfig> levels = new List<LevelConfig>();

    // infos
    private bool inTransition = false;
    private bool goGame = true;
    private bool inGame = false;
    private bool inCredits = false;
    private int lvl = 0;

    // Start is called before the first frame update
    void Start()
    {
        diorama.ActiveCam();

        playerController = player.GetComponent<FPSController>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerCharacterController = player.GetComponent<CharacterController>();

        foreach (LevelConfig l in levels) l.SetLevel();
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
        if (inTransition)
        {
            if (goGame)
            {
                if (diorama.touchTarget)
                {
                    inTransition = false;
                    Game();
                }
            }
            else
            {
                if (diorama.touchTarget)
                {
                    inTransition = false;
                    Menu();
                }
            }
        }

        if (inGame)
        {
            if (ownerInfos.gameOver || player.position.y < 0 || Input.GetKeyDown(KeyCode.R))//(ownerHand.finish) // lose case
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

        if (inCredits)
        {
            if (!credits.inMove)
            {
                inCredits = false;
                CanvasChange(0);
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
        playerCharacterController.enabled = false;
        player.position = levels[lvl].worldSpawnPlayer;
        playerCharacterController.enabled = true;

        scorebar.ResetBar(levels[lvl].scoreToGet);

        messageGame.text = "";

        var _audio = levels[lvl].tempLevel.GetComponent<AudioSource>();
        if (_audio != null)
        {
            _audio.Play();
        }
        /*
        levels[lvl].Reset();
        owner = levels[lvl].controllerOwner;
        ownerInfos = owner.infos;
        */

        //player
        playerRigidbody.useGravity = true;
        playerController.activate = true;
        playerController.ResetController();
        playerRigidbody.constraints = RigidbodyConstraints.None;

        //owner
        //ownerPath.activate = true;
        //ownerHand.activate = true;
        owner.isActive = true;
        ownerHand.isActive = true;

        CanvasChange(2);
        inGame = true;
    }

    public void ActiveTransition(bool _goGame)
    {
        goGame = _goGame;
        inTransition = true;
        CanvasChange(-1);

        if (goGame)
        {
            diorama.SetTarget(levels[lvl].levelNumber);
        }
        else
        {
            diorama.SetBase();
        }
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
        diorama.ActiveCam();
        //playButton.interactable = false;
        if (win) lvl += 1;

        if (lvl >= levels.Count)
        {
            levelText.text = "You Finish C.A.T\nThanks for Playing !";
            lvl = 0;
            DisplayCredits();
            //Menu();
        }
        else
        {
            levelText.text = "Go to the Next Level !";
            ActiveTransition(false);
            //if (win) Game();
            //else Menu();
        }

        levels[lvl].Reset();
        owner = levels[lvl].controllerOwner;
        ownerInfos = owner.infos;
    }

    public void DisplayCredits()
    {
        // stop player
        playerRigidbody.useGravity = false;
        playerController.activate = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        // stop owner
        owner.isActive = false;
        ownerHand.isActive = false;

        CanvasChange(3);

        diorama.SetBase();
        diorama.ActiveCam();
        inCredits = true;
        credits.StartCredits();
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

    public void ResetScene()
    {
        SceneManager.UnloadSceneAsync("C.A.T_Main Game");
        SceneManager.LoadScene("C.A.T_Main Game");
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
