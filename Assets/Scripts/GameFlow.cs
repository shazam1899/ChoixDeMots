//Code réaliser par Dylan LAUNAY à partir du code de base réalise par Clara DENIEL, avec l'aide de Copilot pour comprendre la logique et debugger
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameFlow : MonoBehaviour
{
    public FixedBlockInitializer initializer; 

    [Header("Ordre des niveaux")] 
    public List<int> levelOrder = new List<int>() { 0, 1, 2 }; // Ordre fixe des niveaux (0,1,2)

    [Header("Scores par niveau")]
    public List<LevelScoreConfig> levelScores; // Configurable dans l'inspecteur pour chaque niveau (Leaderboard)

    [System.Serializable]
    public class LevelScoreConfig // Classe pour configurer les scores de chaque niveau dans l'inspecteur
    {
        public int levelIndex;
        public int scoreYOU;
        public int scoreLeGoat;
        public int scoreMoulinRouge;
        public int scoreKauffy;
    }

    [Header("Debloquer !")] // Listes configurables dans l'inspecteur pour les zones de téléportation, annonces et récompenses
    public List<GameObject> teleportAreas; // Téléport Areas à activer après chaque niveau
    public List<GameObject> UIAnnonce; // Annonces à activer après chaque niveau
    public List<GameObject> Recompenses; // Récompenses à activer après chaque niveau
    public List<GameObject> FeedBackClassement; // Position du joueur dans le classement à activer à chaque niveau

    [Header("Final")] // Objets à activer pour la scène finale
    public GameObject final;
    public GameObject Thanks;
    public GameObject TxtFinal;
    public GameObject ButtonRestart;
    public float WaitTime = 2f;

    [Header("Sons")]
    public AudioClip levelCompleteSound;

    public int prog = 0;
    private int currentLevel = 0;
    public int CurrentLevel => currentLevel;

    private bool finalLaunched = false; // Flag pour éviter de lancer la scène finale plusieurs fois

    private void Start() // Initialisation du GameFlow
    {
        if (initializer == null)
        {
            Debug.LogError("initializer n'est pas assigné dans GameFlow !");
            return;
        }

        initializer.OnMiniGameCompleted += OnLevelCompleted;

        //Lance le premier niveau
        LaunchCurrentLevel();
    }

    private void OnLevelCompleted() // Appelé à la fin de chaque mini-jeu 
    {
        

        Debug.Log("Mini-jeu terminé : " + currentLevel); 

         // 🔊 Joue le son de fin de niveau
        //LevelCompleteSound.Instance.PlaySound(levelCompleteSound);

        SendScoresToLeaderboard(currentLevel); // Envoie les scores du niveau actuel au LeaderboardManager

        //Active la Teleport Area correspondante
        if (currentLevel < teleportAreas.Count && currentLevel < UIAnnonce.Count && currentLevel < Recompenses.Count)
        {
            teleportAreas[currentLevel].SetActive(true);
            UIAnnonce[currentLevel].SetActive(true);
            Recompenses[currentLevel].SetActive(true);
            Debug.Log("Activation Teleport Area : " + teleportAreas[currentLevel].name);
        }

        if (currentLevel < FeedBackClassement.Count)
        {
            FeedBackClassement[currentLevel].SetActive(true);
        }

        if (currentLevel - 1 >= 0 && currentLevel - 1 < FeedBackClassement.Count)
        {
            FeedBackClassement[currentLevel - 1].SetActive(false);
        }

        currentLevel++; // Passe au niveau suivant

        //if (currentLevel >= levelOrder.Count)
        //{
            //StartCoroutine(LaunchFinal());
        //}

        //if (prog >= 3)
        //{
            //StartCoroutine(LaunchFinal());
        //}
    }

    public void LaunchCurrentLevel() // Lance le niveau actuel en fonction de l'ordre défini dans levelOrder
    {
        if (currentLevel >= levelOrder.Count) // Vérifie si tous les niveaux ont été joués
        {
            Debug.Log("Tous les niveaux sont terminés.");
            return;
        }

        int levelIndex = levelOrder[currentLevel]; // Récupère l'index du niveau à lancer
        Debug.Log("Lancement du niveau : " + levelIndex); 

        initializer.ClearBoard(); // Nettoie le plateau avant de lancer le nouveau niveau
        initializer.Initialize(levelIndex); // Initialise le niveau en fonction de l'index défini dans levelOrder
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //insideCount++;

            //if (insideCount == 1)
            //{
                CheckFinalCondition();
            //}
        }
    }

    public void AddProgress() // Méthode pour ajouter de la progression, appelée à la fin de chaque mini-jeu
    {
        prog++; // Incrémente la progression
        Debug.Log("[GameFlow] Progression : " + prog + " | Instance ID : " + GetInstanceID()); 

        //CheckFinalCondition(); // Vérifie si la condition pour lancer la scène finale est remplie
    }

    private void CheckFinalCondition() // Vérifie si la progression a atteint le seuil pour lancer la scène finale
    {
        if (!finalLaunched && prog >= 3) // Si la progression est suffisante et que la scène finale n'a pas encore été lancée
        {
            finalLaunched = true;
            StartCoroutine(LaunchFinal()); // Lance la scène finale après un délai défini par WaitTime
        }
    }

    private IEnumerator LaunchFinal() // Coroutine pour lancer la scène finale avec un délai
    { 
        Thanks.SetActive(true); 
        yield return new WaitForSeconds(WaitTime);

        TxtFinal.SetActive(true);
        final.SetActive(true);
        ButtonRestart.SetActive(true);
    }

    private void SendScoresToLeaderboard(int level) // Envoie les scores du niveau actuel au LeaderboardManager pour mise à jour de l'affichage
    {
        var config = levelScores.Find(s => s.levelIndex == level); // Trouve la configuration de score correspondant au niveau actuel dans la liste levelScores
        if (config == null)
        {
            Debug.LogWarning("Aucun score configuré pour le niveau " + level);
            return;
        }

        Dictionary<string, int> scores = new Dictionary<string, int> // Crée un dictionnaire de scores à partir de la configuration du niveau actuel
        {
            { "TOI", config.scoreYOU },
            { "LeGoat404", config.scoreLeGoat },
            { "FouduMoulinRouge", config.scoreMoulinRouge },
            { "Kauffy", config.scoreKauffy }
        };

        LeaderboardManager.Instance.AddScoreForLevel(level, scores); // Appelle la méthode AddScoreForLevel du LeaderboardManager pour mettre à jour les scores et rafraîchir l'affichage du leaderboard
    }
}
