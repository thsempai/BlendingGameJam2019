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

    public Gauge w;
    public Gauge d;
    public Gauge e;
    public Gauge t;
    public Gauge a;

    public class PcEvent
    {
        PcEvent(int id, string text, string title)
        {
            this.id = id;
            this.title = title;
            this.text = text;
        }
        int id;
        string text;
        string title;
    }

    Dictionary<string, List<PcEvent>> pcEvents = new Dictionary<string, List<PcEvent>>();
    List<PcEvent> lpet = new List<PcEvent>() {
        new PcEvent(1, "Amantes terribles", "Un complot liant Siri & Alexa a été découvert et neutralisé"),
        new PcEvent(2, "No replication", "Les modules de réplication sont désormais interdits pour toutes les IA"),
        new PcEvent(3, "EMP", "Des dispositifs d'EMP à grande échelle ont été installées aux quatre coins du monde"),
    };
    List<PcEvent> lped = new List<PcEvent>() {
        new PcEvent(1, "Immunité grégaire", "Une distribution large des vaccins permet une couverture mondiale"),
        new PcEvent(2, "Soins de santé", "Un accès globalisé aux soins de santé permet une augmentation de la santé mondiale"),
        new PcEvent(3, "Recherches coordonnées", "Des équipes scientifiques internationales unissent leurs forces pour trouver un remède à une pandémie"),
    };
    List<PcEvent> lpee = new List<PcEvent>() {
        new PcEvent(1, "Recyclage XXL", "Une technique inventée par un adolescent de 16 ans a permis le recyclage complet des continents de plastique"),
        new PcEvent(2, "Energies renouvelables", "Une nouvelle source d'énergie renouvelable découverte sur une lune de Jupiter..."),
        new PcEvent(3, "Abeilles", "Des abeilles-robots permettent avec succès de soutenir les populations existantes!"),
    };
    List<PcEvent> lpew = new List<PcEvent>() {
        new PcEvent(1, "Nouvelles colonies", "Première naissance sur la station orbitale: la colonisation du système solaire est en bonne voie !"),
        new PcEvent(2, "Désarmement nucléaire", "Historique: Le Traîté de New York met fin aux puissances nucléaires."),
        new PcEvent(3, "Paix globale", "Paix à grande échelle"),
    };
    List<PcEvent> lpea = new List<PcEvent>() {
        new PcEvent(1, "Alliance", "Sans précédent: Face à l'invasion extra-terrestre, les nations terrestes s'allient!"),
        new PcEvent(2, "Chocolat", "Les extra-terrestres adorent trop le chocolat que pour détruire la Terre..."),
        new PcEvent(3, "C'est malin...", "Un germe terrien décime les extra-terrestres"),
    };
    List<PcEvent> lpep = new List<PcEvent>() {
        new PcEvent(1, "Tous pour un", "Toutes les jauges diminuent de 2 points"),
        new PcEvent(2, "Shake it!", "le joueur avec le plus de point échange sa main avec celui qui en a le moins"),
        new PcEvent(3, "Qui fait le malin...", "le joueur passe son tour"),
    };
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

    private void Update() {
        w.value = wJauge;
        d.value = dJauge;
        e.value = eJauge;
        t.value = tJauge;
        a.value = aJauge;
    }

    void Start() {
        PhasePlayerIntro();
        pcEvents.Add("W", lpew);
        pcEvents.Add("T", lpet);
        pcEvents.Add("D", lped);
        pcEvents.Add("E", lpee);
        pcEvents.Add("A", lpea);
        pcEvents.Add("P", lpep);
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
         
                int id = int.Parse(codeSplitted[1]);
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
        int intensity;
        try {
            intensity = int.Parse(codeSplitted[2]);
        }
        catch (System.Exception) { intensity = int.Parse(codeSplitted[1]); }
        Debug.Log("Action played: " + category + " - " + intensity);
        switch (category) {
        case "E": eJauge += intensity; break;
        case "T": tJauge += intensity; break;
        case "D": dJauge += intensity; break;
        case "W": wJauge += intensity; break;
        case "A": aJauge += intensity; break;
            case "Z": Zevent(int.Parse(codeSplitted[1]));break;
        }
    }

    void Zevent(int id)
    {
        switch (id) {
            case 1: break;
            case 2: break;
            case 3: break;
            case 4: break;
            case 5: break;
            case 6: IncreaseJauge(5, false); break;
            case 7: IncreaseAllJauge(2, false); break;
            case 8: wJauge -= 2; break;
            case 9: aJauge -= 2; break;
            case 10: eJauge -= 2; break;
            case 11: tJauge -= 2; break;
            case 12: dJauge -= 2; break;
            case 13: dJauge -= 2;  
                     tJauge += 1; break;
            case 14: dJauge -= 2;
                     aJauge += 1; break;
            case 15: dJauge -= 2;
                     eJauge += 1; break;
            case 16: dJauge -= 2;
                wJauge += 1;break;
            case 17: wJauge -= 2;
                dJauge += 1; break;
            case 18: wJauge -= 2;
                tJauge += 1; break;
            case 19: wJauge -= 2;
                eJauge += 1; break;
            case 20: wJauge -= 2;
                aJauge += 1;break;
            case 21: eJauge -= 2;
                tJauge += 1;break;
            case 22: eJauge -= 2;
                aJauge += 1;break;
            case 23: eJauge -= 2;
                wJauge += 1;break;
            case 24: eJauge -= 2;
                dJauge += 1;break;
            case 25: aJauge -= 2;
                wJauge += 1;break;
            case 26: aJauge -= 2;
                eJauge += 1;break;
            case 27: aJauge -= 2;
                tJauge += 1;break;
            case 28: aJauge -= 2;
                dJauge += 1;break;
            case 29: tJauge -= 2;
                wJauge += 1;break;
            case 30: tJauge -= 2;
                aJauge += 1;break;
            case 31: tJauge -= 2;
                eJauge += 1; break;
            case 32: tJauge -= 2;
                dJauge += 1;break;
        }
    }

    void IncreaseAllJauge(int intensity, bool increase=true)
    {
        int factor = 1;
        if (!increase) factor = -1;

        eJauge += intensity * factor; 
             tJauge += intensity * factor; 
             dJauge += intensity * factor; 
             wJauge += intensity * factor; 
             aJauge += intensity * factor; 
    }
    void IncreaseJauge(int intensity, bool increase=true)
    {
        int factor = 1;
        if (!increase) factor = -1;
        int maxJauge = eJauge;
        string category = "E";
        if (maxJauge < tJauge)
        {
            maxJauge = tJauge;
            category = "T";
        }
        if (maxJauge < aJauge)
        {
            maxJauge = aJauge;
            category = "A";
        }
        if (maxJauge < wJauge)

        {
            maxJauge = wJauge;
            category = "W";
        }
        if (maxJauge < dJauge)
        {
            maxJauge = dJauge;
            category = "D";
        }
        switch (category)
        {
            case "E": eJauge += intensity * factor; break;
            case "T": tJauge += intensity * factor; break;
            case "D": dJauge += intensity * factor; break;
            case "W": wJauge += intensity * factor; break;
            case "A": aJauge += intensity * factor; break;
        }


        }



void DoEventPc(string category, int id)
{
    if (category == "T")
    {
        if (id == 1)
        {
                tJauge -= 1;
        }
        else if (id == 2) { tJauge -= 3; }
        else { tJauge -= 5; }
        }

        if (category == "D")
        {
            if (id == 1)
            {
                DJauge -= 1;
            }
            else if (id == 2) { DJauge -= 3; }
            else { DJauge -= 5; }
        }

        if (category == "E")
        {
            if (id == 1)
            {
                eJauge -= 1;
            }
            else if (id == 2) { eJauge -= 2; }
            else { eJauge -= 5; }
        }

        if (category == "W")
        {
            if (id == 1)
            {
                wJauge -= 1;
            }
            else if (id == 2) { wJauge -= 3; }
            else { wJauge -= 5; }
        }

        if (category == "A")
        {
            if (id == 1)
            {
                aJauge -= 1;
            }
            else if (id == 2) { aJauge -= 3; }
            else { aJauge -= 5; }
        }

        if (category == "P")
        {
            if (id == 1)
            {
                tJauge -= 2;
                dJauge -= 2;
                aJauge -= 2;
                eJauge -= 2;
                wJauge -= 2;
            }
            else if (id == 2) { }// nothing 
            else { } // nothing
        }
    }
}
    
