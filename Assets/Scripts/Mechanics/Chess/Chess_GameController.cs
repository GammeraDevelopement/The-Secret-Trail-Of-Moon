using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Chess_GameController : MonoBehaviour
{
    private const string CONFIRM = "Cross";
    private const string CANCEL = "Circle";

    public enum ChessFSM {
        LOADING,
        INSTRUCTION,
        PLAYING,
        NEXT_LEVEL,
        FINISHED
    }
    public ChessFSM state = ChessFSM.LOADING;

    [Header("Variables del psicólogo")]
    public int nivel;

    [Header("Variables del editor")]
    public string winningPosition;
    public enum Pieza {
        PAWN,
        KNIGHT,
        BISHOP,
        TOWER,
        QUEEN,
        KING,
        CHECKS1,
        CHECKS2,
        CHECKS3,
        REPASO1,
        REPASO2
    }
    public Pieza tipoMecánica;

    public int nivelesTotales;
    public int nivelActual = 1;
    public GameObject player;
    public GameObject piecePlace;
    public GameObject nivelACargar;
    
    /*public GameObject[] playerPieces;
    public GameObject[] targetPieces;
    public GameObject[] pieces;
    public GameObject[] rows;*/

    [Header("Audio")]
    public AudioSource source;
    public AudioClip selectPiece;
    public AudioClip wrong;
    public AudioClip correct;

    [Header("GUI")]
    public Image black;
    public GameObject winCartel;
    public VideoClip[] clips;
    public GameObject instruction;

    private GameObject clone;
    private GameObject[,] gameMatrix;
    private GameObject movingPiece;
    private bool loadingScene = false;
    private bool newTuto = true;
    private int movimientoscorrectos = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        switch (tipoMecánica) {
            case Pieza.PAWN:
                nivelACargar = Resources.Load("Chess Levels/Pawn/Plevel" + nivelActual) as GameObject;
                break;
            case Pieza.KNIGHT:
                nivelACargar = Resources.Load("Chess Levels/Knight/Nlevel" + nivelActual) as GameObject;
                break;
            case Pieza.BISHOP:
                nivelACargar = Resources.Load("Chess Levels/Bishop/Blevel" + nivelActual) as GameObject;
                break;
            case Pieza.TOWER:
                nivelACargar = Resources.Load("Chess Levels/Rook/Rlevel" + nivelActual) as GameObject;
                break;
            case Pieza.QUEEN:
                nivelACargar = Resources.Load("Chess Levels/Queen/Qlevel" + nivelActual) as GameObject;
                break;
            case Pieza.KING:
                nivelACargar = Resources.Load("Chess Levels/King/Klevel" + nivelActual) as GameObject;
                break;
            case Pieza.CHECKS1:
                nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + nivelActual) as GameObject;
                break;
            case Pieza.CHECKS2:
                nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + (nivelActual + 10)) as GameObject;
                break;
            case Pieza.CHECKS3:
                nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + (nivelActual + 20)) as GameObject;
                break;
            case Pieza.REPASO1:
                nivelACargar = Resources.Load("Chess Levels/Repaso1/REPlevel" + nivelActual) as GameObject;
                break;
            case Pieza.REPASO2:
                nivelACargar = Resources.Load("Chess Levels/Repaso2/REPlevel" + nivelActual) as GameObject;
                break;
            default:
                break;
        }
        clone = Instantiate(nivelACargar, piecePlace.transform.position, piecePlace.transform.rotation);
        winCartel.SetActive(false);
    }

    private void setClips(int numberVideo) {
        Debug.Log("Show new instruction video.");
        instruction.SetActive(true);
        instruction.GetComponent<InstructionScreen>().setInstructionVideo(clips[numberVideo]);
        instruction.GetComponent<InstructionScreen>().startInstruction();
        instruction.GetComponent<InstructionScreen>().state = InstructionScreen.InstructionFSM.TUTORIAL;
        state = ChessFSM.INSTRUCTION;
    }

    private void StartPlaying() {
        state = ChessFSM.PLAYING;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case ChessFSM.LOADING:
                black.CrossFadeAlpha(0, 0.5F, true);
                break;
            case ChessFSM.INSTRUCTION:
                break;
            case ChessFSM.PLAYING:

                if (tipoMecánica == Pieza.KING) {
                    switch (nivelActual) {
                        case 5:
                            if (newTuto) {
                                newTuto = false;
                                setClips(0);
                            }
                            break;
                        case 10:
                            if (newTuto) {
                                newTuto = false;
                                setClips(1);
                            }
                            break;
                        case 15:
                            if (newTuto) {
                                newTuto = false;
                                setClips(2);
                            }
                            break;
                        default:
                            break;
                    }
                    
                }

                if (Input.GetButtonDown(CONFIRM)) {

                    //Debug.DrawRay(player.transform.position, player.transform.forward * 100);

                    Ray ray = new Ray(player.transform.position, player.transform.forward);
                    if (movingPiece != null) {
                        Material mate = movingPiece.GetComponent<Renderer>().material;
                        mate.SetColor("_EmissionColor", Color.black);
                    }

                    if (Physics.Raycast(ray, out RaycastHit hit, 100, ~(1 << 11))) {

                        Debug.Log(hit.collider.gameObject.name);

                        if (hit.collider != null) {

                            if (hit.collider.tag == "targetBlackPiece" && movingPiece != null && movingPiece.tag == "whitePiece") {
                                if (hit.collider.GetComponent<Chess_Piece>() != null) {
                                    if (movingPiece == hit.collider.GetComponent<Chess_Piece>().piezaDestructora) {
                                        movingPiece.transform.position = hit.collider.transform.position;
                                        movimientoscorrectos++;
                                        Destroy(hit.collider.gameObject);
                                        source.clip = correct;
                                        source.Play();
                                        Debug.Log("movimientos correctos " + movimientoscorrectos + "/" + GameObject.FindGameObjectsWithTag("whitePiece").Length);
                                    } else {
                                        source.clip = wrong;
                                        source.Play();
                                    }
                                }
                                if (movimientoscorrectos == GameObject.FindGameObjectsWithTag("whitePiece").Length) {
                                    //WIN CONDITION
                                    if (nivelActual == nivelesTotales) {
                                        winCartel.SetActive(true);
                                        state = ChessFSM.FINISHED;
                                    } else {
                                        Debug.Log("win");
                                        state = ChessFSM.NEXT_LEVEL;
                                    }
                                    
                                }
                                //recalcular posibilidad de movimiento de la/las piezas

                                

                            } else if (hit.collider.tag == "back" && movingPiece != null && movingPiece.tag == "whitePiece") {
                                movingPiece = null;


                            } else if (hit.collider.tag != "blackPiece" && hit.collider.tag != "targetBlackPiece"  && hit.collider.tag != "back") {
                                movingPiece = hit.collider.gameObject;

                                //Light material
                                Material mymat = movingPiece.GetComponent<Renderer>().material;
                                mymat.SetColor("_EmissionColor", Color.blue);
                                source.clip = selectPiece;
                                source.Play();

                            } else if (hit.collider.tag == "blackPiece") {
                                source.clip = wrong;
                                source.Play();
                            }

                        }
                    }
                }
                if (Input.GetButtonDown(CANCEL)) {
                    Material mate = movingPiece.GetComponent<Renderer>().material;
                    mate.SetColor("_EmissionColor", Color.black);
                    movingPiece = null;

                }

                break;

            case ChessFSM.NEXT_LEVEL:
                if (nivelActual == nivelesTotales) {
                    if (Input.GetButtonDown("Square") && !loadingScene) {
                        black.CrossFadeAlpha(1, 0.5F, true);
                        loadingScene = true;
                        AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
                    }
                } else {
                    Destroy(clone);
                    nivelActual++;
                    newTuto = true;
                    switch (tipoMecánica) {
                        case Pieza.PAWN:
                            nivelACargar = Resources.Load("Chess Levels/Pawn/Plevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.KNIGHT:
                            nivelACargar = Resources.Load("Chess Levels/Knight/Nlevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.BISHOP:
                            nivelACargar = Resources.Load("Chess Levels/Bishop/Blevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.TOWER:
                            nivelACargar = Resources.Load("Chess Levels/Rook/Rlevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.QUEEN:
                            nivelACargar = Resources.Load("Chess Levels/Queen/Qlevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.KING:
                            nivelACargar = Resources.Load("Chess Levels/King/Klevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.CHECKS1:
                            nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.CHECKS2:
                            nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + (nivelActual + 10)) as GameObject;
                            break;
                        case Pieza.CHECKS3:
                            nivelACargar = Resources.Load("Chess Levels/CheckMates/CMlevel" + (nivelActual + 20)) as GameObject;
                            break;
                        case Pieza.REPASO1:
                            nivelACargar = Resources.Load("Chess Levels/Repaso1/REPlevel" + nivelActual) as GameObject;
                            break;
                        case Pieza.REPASO2:
                            nivelACargar = Resources.Load("Chess Levels/Repaso2/REPlevel" + nivelActual) as GameObject;
                            break;
                        default:
                            break;
                    }
                    movimientoscorrectos = 0;
                    clone = Instantiate(nivelACargar, piecePlace.transform.position, piecePlace.transform.rotation);
                    state = ChessFSM.PLAYING;
                }
                break;
            case ChessFSM.FINISHED:
                //Debug.Log("Finished");
                if (Input.GetButtonDown("Square") && !loadingScene) {
                    black.CrossFadeAlpha(1, 0.5F, true);
                    loadingScene = true;
                    AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
                }
                break;
            default:
                break;
        }
    }
}
