using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum PlayerState { NoCard, OneActionPlayed, TwoActionPlayed, EventPlayed }

    public PlayerState currentPlayerState;
    public Animator animator;
    public int currentPlayer;
    public int playersCount = 4;
    public List<int> playerSkip = new List<int>();
    public GameObject webcam;
    public string lastCodeReceived = "";

    public int wJauge = 0;
    public int dJauge = 0;
    public int eJauge = 0;
    public int tJauge = 0;
    public int aJauge = 0;

    public List<string> cardsplayed = new List<string>();

    public List<GameObject> avatars = new List<GameObject>();

    private void OnEnable() {
        PhaseOut.PlayerStart += PhasePlayersStart;
        PhaseOut.PlayerTurnStart += PlayerPlays;
        DisplayWebcam.QRCodeMessage += PlayerPlayCard;

        PhaseOut.Yes += EndOfTurn;
    }

    private void OnDisable() {
        PhaseOut.PlayerStart -= PhasePlayersStart;
        PhaseOut.PlayerTurnStart -= PlayerPlays;
        DisplayWebcam.QRCodeMessage -= PlayerPlayCard;
        PhaseOut.Yes -= EndOfTurn;

    }

    void Start() {
        PhasePlayerIntro();
    }

    void PhasePlayerIntro() {
        animator.SetTrigger("PlayerPhase");
    }

    void PhasePlayersStart() {
        currentPlayer = 0;
        Debug.Log("Phase Player is Started");
        NextPlayer();
        PlayerIntro();
    }

    private void EndOfTurn() {
        if (currentPlayerState != PlayerState.EventPlayed && currentPlayerState != PlayerState.TwoActionPlayed) return;
        NextPlayer();
        PlayerIntro();
    }

    private int NextPlayer() {
        currentPlayer++;
        currentPlayerState = PlayerState.NoCard;
        lastCodeReceived = "";
        if (currentPlayer > playersCount) {
            currentPlayer = 1;
        }

        while (playerSkip.Contains(currentPlayer)) {
            playerSkip.Remove(currentPlayer);
            currentPlayer++;
            if (currentPlayer > playersCount) {
                currentPlayer = 1;
            }
        }
        Debug.Log("Player " + currentPlayer.ToString() + " plays.");
        return currentPlayer;
    }

    private void PlayerIntro() {
        animator.SetTrigger("PlayerIntro");
        ActiveAvatar();
    }

    private void PlayerPlays() {
        Debug.Log("Waiting for player " + currentPlayer.ToString() + " cards.");
    }

    private void ActiveAvatar() {
        webcam.SetActive(currentPlayer > 0);

        for (int index = 0; index < avatars.Count; index++) {
            avatars[index].SetActive((index == currentPlayer - 1));
        }
    }

    private bool IsValidCard(string code) {
        string[] codeSplitted = code.Split('_');
        switch (codeSplitted[0].ToUpper()) {
        case "E":
        case "T":
        case "D":
        case "W":
        case "A":
            try {
                int id = int.Parse(codeSplitted[1]);
                if (id < 1 || id > 16) return false;

            }
            catch (System.Exception) { return false; };

            try { int.Parse(codeSplitted[2]); } catch (System.Exception) { return false; };
            return true;
        case "Z":
            try {
                int id = int.Parse(code);
                if (id < 1 || id > 40) return false;

            }
            catch (System.Exception) { return false; };
            return true;

        }
        return false;
    }

    private void PlayerPlayCard(string code) {
        if (code == lastCodeReceived) return;
        if (!IsValidCard(code)) {
            animator.SetTrigger("No");
            Debug.Log("This QRCode is not a card. (" + code + ")");
            lastCodeReceived = "";
            return;
        }

        lastCodeReceived = code;
        Debug.Log("QRCode: " + code);
        if (cardsplayed.Contains(code)) {
            animator.SetTrigger("No");
            Debug.Log("QRCode already passed. (" + code + ")");
            lastCodeReceived = "";
        }

        switch (currentPlayerState) {
        case PlayerState.NoCard:
            DoAction(code);
            if (IsEvent(code)) {
                currentPlayerState = PlayerState.EventPlayed;
                cardsplayed.Add(code);
                animator.SetTrigger("Yes");
            }
            else {
                currentPlayerState = PlayerState.OneActionPlayed;
                cardsplayed.Add(code);
                animator.SetTrigger("Yes");
            }
            break;
        case PlayerState.OneActionPlayed:
            if (IsEvent(code)) {
                animator.SetTrigger("No");
                Debug.Log("Only action as a second card (" + code + ")");
                lastCodeReceived = "";
            }
            else {
                DoAction(code);
                currentPlayerState = PlayerState.TwoActionPlayed;
                cardsplayed.Add(code);
                animator.SetTrigger("Yes");
            }
            break;

        }

    }

    private bool IsEvent(string code) {
        return code.StartsWith("Z_");
    }

    private void DoAction(string code) {
        string[] codeSplitted = code.Split('_');
        string category = codeSplitted[0];
        int intensity = int.Parse(codeSplitted[2]);
        Debug.Log("Action played: " + category + " - " + intensity);
        switch (category) {
        case "E": eJauge += intensity; break;
        case "T": tJauge += intensity; break;
        case "D": dJauge += intensity; break;
        case "W": wJauge += intensity; break;
        case "A": aJauge += intensity; break;
        }
    }
}
