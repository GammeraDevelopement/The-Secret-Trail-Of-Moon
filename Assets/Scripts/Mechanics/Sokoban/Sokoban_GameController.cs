using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;

public class Sokoban_GameController : MonoBehaviour {

    public enum SokobanFSM {
        LOADING,
        INSTRUCTION,
        PLAYING,
        FINISHED
    }
    public SokobanFSM state = SokobanFSM.LOADING;

    private const string X_AXIS = "DpadRight";
    private const string Y_AXIS = "DpadUp";
    private const string CONFIRM = "Cross";

    //Output
    public int[] nmaxMovs;

    //GameObject variables
    public float blockSpeed = 0.3F;
    public float raycastLength = 10000;
    public Transform cameraVR;
    public GameObject winCartel;
    public GameObject instruction;
    public FirstPersonController player;
    public GameObject videoPlayer;
    public GameObject squareButton;
    public ParticleSystem winEffect;
    public AudioSource source;
    public AudioClip moveBlock;
    public AudioClip selectBlock;
    public AudioClip winSound;
    public Image black;


    //Editor variables
    private float maxDistance = 10;
    private bool continuar = false;
    private bool x_isAxisInUse;
    private bool y_isAxisInUse;
    private bool ActiveMove;
    private bool verticalMove;
    private int nMovements = 0;
    private Vector3 FirstPos;
    private GameObject currentBlock;
    private Rigidbody blockConst;
    private Rigidbody blockPhysic;
    private Vector3 nextpos;
    private Transform blockPos;

    private Sokoban_Block oneCol;
    private Sokoban_Block twoCol;
    private bool oneBlocked = false;
    private bool twoBlocked = false;

    private int introCount = 0;
    private bool loadingScene = false;
    public bool tutorial;

    private void Start() {
        black.CrossFadeAlpha(0, 0.5F, true);
        if (PlayerPrefs.GetInt("nivelSokoban") != 1) {
            tutorial = false;
        } else {
            tutorial = true;
        }
    }

    void CheckOver(VideoPlayer vp) {
        introCount++;
    }

    private void StartPlaying() {
        state = SokobanFSM.PLAYING;
    }

    // Update is called once per frame
    void Update() {

        switch (state) {
            case SokobanFSM.LOADING:
                gameObject.GetComponent<Sokoban_LoadSokoban>().loadLevel();
                if (gameObject.GetComponent<Sokoban_LoadSokoban>().getLevel() > 1) {
                    instruction.GetComponent<InstructionScreen>().startNoTutorial();
                } else {
                    instruction.GetComponent<InstructionScreen>().startInstruction();
                }
                state = SokobanFSM.INSTRUCTION;
                break;
            case SokobanFSM.INSTRUCTION:

                instruction.gameObject.SetActive(true);

                if (tutorial) {
                    if (Input.GetButtonDown("Square") && introCount == 0) {
                        squareButton.SetActive(false);
                        videoPlayer.SetActive(true);
                        videoPlayer.transform.GetChild(0).gameObject.SetActive(false);
                        videoPlayer.GetComponent<VideoPlayer>().Play();
                        videoPlayer.GetComponent<VideoPlayer>().loopPointReached += CheckOver;
                    } else if (introCount == 1) {
                        videoPlayer.transform.GetChild(0).gameObject.SetActive(true);
                        introCount++;

                    } else if (Input.GetButtonDown("Cross") && introCount == 2) {
                        videoPlayer.SetActive(false);
                        instruction.gameObject.SetActive(false);
                        player.enabled = true;
                        gameObject.GetComponent<Sokoban_LoadSokoban>().loadLevel();
                        state = SokobanFSM.PLAYING;
                    }
                } else {
                    if (Input.GetButtonDown("Square") && introCount == 0) {
                        squareButton.SetActive(false);
                        player.enabled = true;
                        gameObject.GetComponent<Sokoban_LoadSokoban>().loadLevel();
                        state = SokobanFSM.PLAYING;
                    }
                }
                break;
            case SokobanFSM.PLAYING:

                Debug.DrawRay(cameraVR.position, cameraVR.forward * 100, Color.red);
                if (Input.GetButtonDown(CONFIRM)) {

                    Debug.DrawRay(transform.position, transform.forward * 100);

                    Ray ray = new Ray(transform.position, transform.forward);
                    if (currentBlock != null) {
                        Material mate = currentBlock.transform.GetChild(2).GetComponent<Renderer>().material;
                        mate.SetColor("_EmissionColor", Color.black);
                        source.clip = selectBlock;
                        source.Play();
                        currentBlock = null;
                    }

                    int layerMask = LayerMask.GetMask("Cubes");
                    if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask)) {

                        Debug.Log(hit.collider.gameObject.name);

                        if (hit.collider != null && hit.collider.tag != "back") {

                            currentBlock = hit.collider.gameObject;
                            //Light material
                            Material mymat = currentBlock.transform.GetChild(2).GetComponent<Renderer>().material;
                            mymat.SetColor("_EmissionColor", Color.blue);

                            //Get restraints colliders
                            oneCol = currentBlock.transform.GetChild(0).GetComponent<Sokoban_Block>();
                            twoCol = currentBlock.transform.GetChild(1).GetComponent<Sokoban_Block>();

                            nMovements++;
                            blockPhysic = currentBlock.GetComponent<Rigidbody>();  // <----------

                            nextpos = currentBlock.transform.position;
                            if (currentBlock.name == "YellowBlock" || currentBlock.name == "GreyBlock") {
                                verticalMove = true;
                            } else {
                                verticalMove = false;
                            }
                        }
                    }
                }

                if (currentBlock != null) {
                    blockPos = currentBlock.transform;
                    twoBlocked = twoCol.isColliding; //Left/up
                    oneBlocked = oneCol.isColliding; //Right/down

                    if (Input.GetAxisRaw(X_AXIS) == 1 && !x_isAxisInUse && !verticalMove) {
                        if (!twoBlocked) nextpos = new Vector3(blockPos.position.x + 1, blockPos.position.y, blockPos.position.z);
                        x_isAxisInUse = true;
                        source.clip = moveBlock;
                        source.Play();

                    } else if (Input.GetAxisRaw(X_AXIS) == -1 && !x_isAxisInUse && !verticalMove) {
                        if (!oneBlocked) nextpos = new Vector3(blockPos.position.x - 1, blockPos.position.y, blockPos.position.z);
                        x_isAxisInUse = true;
                        source.clip = moveBlock;
                        source.Play();
                    }
                    if (Input.GetAxisRaw(X_AXIS) == 0) {
                        x_isAxisInUse = false;
                    }

                    if (Input.GetAxisRaw(Y_AXIS) == 1 && !y_isAxisInUse && verticalMove) {
                        if (!oneBlocked) nextpos = new Vector3(blockPos.position.x, blockPos.position.y + 1, blockPos.position.z);
                        y_isAxisInUse = true;
                        source.clip = moveBlock;
                        source.Play();

                    } else if (Input.GetAxisRaw(Y_AXIS) == -1 && !y_isAxisInUse && verticalMove) {
                        if (!twoBlocked) nextpos = new Vector3(blockPos.position.x, blockPos.position.y - 1, blockPos.position.z);
                        y_isAxisInUse = true;
                        source.clip = moveBlock;
                        source.Play();

                    }
                    if (Input.GetAxisRaw(Y_AXIS) == 0) {
                        y_isAxisInUse = false;
                    }


                    blockPos.position = Vector3.MoveTowards(blockPos.position, nextpos, blockSpeed);
                }

                if (blockPos != null) {
                    if (blockPos.position == nextpos) {
                        nextpos = blockPos.position;
                        continuar = false;
                    }
                }
                break;
            case SokobanFSM.FINISHED:
                int gamemode = PlayerPrefs.GetInt("Gamemode");
                switch (gamemode)
                {
                    case 0:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
                        }
                        break;
                    case 1:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            gameObject.GetComponent<SceneLoader>().LoadSceneInOrder();
                        }
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }

    }

    public void sokobanWin() {
        winCartel.SetActive(true);
        winCartel.transform.GetChild(1).GetComponent<TMP_Text>().text = "" + nMovements +"/"+nmaxMovs[gameObject.GetComponent<Sokoban_LoadSokoban>().getLevel() - 1];
        winEffect.Play();
        source.clip = winSound;
        source.Play();
        state = SokobanFSM.FINISHED;
    }


}
