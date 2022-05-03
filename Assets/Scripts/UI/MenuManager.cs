using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private PathFinding ownerPath;
    private OwnerHand ownerHand;

    [Header("UI General")]
    public Vector3 posCameraMenu;
    public List<GameObject> canvasList;

    [Header("UI InGame")]
    public SatisfactionBar scorebar;
    public TextMeshProUGUI levelText;

    //[Header("UI Menu")]

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
        ownerPath = levels[lvl].pathfindingOwner;
        ownerHand = levels[lvl].handOwner;

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
        ownerPath.activate = true;
        ownerHand.activate = true;

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
        ownerPath.activate = false;
        ownerHand.activate = false;
        levels[lvl].Reset();
        ownerPath = levels[lvl].pathfindingOwner;
        ownerHand = levels[lvl].handOwner;

        // placement camera (player)
        player.position = posCameraMenu;
        player.rotation = new Quaternion();

        CanvasChange(0);

        // other
        playButton.interactable = true;
        inGame = false;
    }

    public void NextLevel(bool win)
    {
        playButton.interactable = false;
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
