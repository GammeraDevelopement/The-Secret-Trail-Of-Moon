using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;

public class InstructionScreen : MonoBehaviour {

    private const string CONFIRM_BUTTON = "Square";
    private const string ALTERNATE_CONFIRM_BUTTON = "h";

    public enum InstructionFSM {
        WAITING,
        TUTORIAL,
        NOTUTORIAL,
        FINISHED
    }
    public InstructionFSM state = InstructionFSM.WAITING;

    public GameObject instruction;
    public GameObject squareButton;
    public GameObject videoPlayer;
    public bool tutoOnly = false;
    public FirstPersonController player;

    private int introCount = 0;

    // Update is called once per frame
    void Update() {

        switch (state) {
            case InstructionFSM.WAITING:
                break;
            case InstructionFSM.TUTORIAL:
                instruction.gameObject.SetActive(true);

                //Se activa el video
                if ((Input.GetButtonDown(CONFIRM_BUTTON) || Input.GetKey(ALTERNATE_CONFIRM_BUTTON)) && introCount == 0) {
                    squareButton.SetActive(false);
                    videoPlayer.SetActive(true);
                    videoPlayer.transform.GetChild(0).gameObject.SetActive(false);
                    videoPlayer.GetComponent<VideoPlayer>().Play();
                    videoPlayer.GetComponent<VideoPlayer>().loopPointReached += CheckOver;
                } else if (introCount == 1) {
                    videoPlayer.transform.GetChild(0).gameObject.SetActive(true);
                    introCount++;
                    videoPlayer.GetComponent<VideoPlayer>().loopPointReached -= CheckOver;

                } else if (Input.GetButtonDown("Cross") && introCount >= 2) {
                    videoPlayer.SetActive(false);
                    instruction.gameObject.SetActive(false);
                    player.enabled = true;
                    if (!tutoOnly) {
                        GameObject.FindGameObjectWithTag("GameController").SendMessage("StartPlaying");
                    } else {
                        SceneManager.LoadScene("Intro");
                    }
                    state = InstructionFSM.FINISHED;
                }
                break;
            case InstructionFSM.NOTUTORIAL:
                if ((Input.GetButtonDown(CONFIRM_BUTTON) || Input.GetKey(ALTERNATE_CONFIRM_BUTTON)) && introCount == 0) {
                    squareButton.SetActive(false);
                    player.enabled = true;
                    GameObject.FindGameObjectWithTag("GameController").SendMessage("StartPlaying");
                    state = InstructionFSM.FINISHED;
                }
                break;
            case InstructionFSM.FINISHED:
                

                break;
            default:
                break;
        }

    }

    public void startInstruction() {
        introCount = 0;
        squareButton.SetActive(true);
        state = InstructionFSM.TUTORIAL;
    }

    public void setInstructionVideo(VideoClip clip) {
        videoPlayer.GetComponent<VideoPlayer>().clip = clip;
    }

    public void startNoTutorial() {
        state = InstructionFSM.NOTUTORIAL;
    }

    void CheckOver(VideoPlayer vp) {
        introCount++;
    }
}
