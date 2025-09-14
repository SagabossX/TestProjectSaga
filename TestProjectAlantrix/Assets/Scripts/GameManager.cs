using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private List<CardData> cardDataSet;
    [SerializeField] private float revealDuration = 2f;
    [SerializeField] private float checkDelay = 0.5f;

    [Header("References")]
    [SerializeField] private DynamicGridSpawner gridSpawner;

    private List<Card> activeCards = new();
    private Card firstSelected, secondSelected;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else 
        { 
            Instance = this; 
        }
    }

    private void Start() 
    {
        GameSaveData saveData = SaveHandler.LoadGame();
        if (saveData == null)
        {
            SetupGame(); // new game if no save
            return;
        }

        gridSpawner.SpawnGridFromSave(saveData, cardDataSet);
        activeCards = gridSpawner.spawnedCards;
    }

    private void SetupGame() 
    {
        GenerateCards(RevealAllCards);
        Invoke(nameof(StartPlayPhase), revealDuration);
    }

    private void GenerateCards(System.Action callback)
    {
        List<CardData> pool = new();
        foreach (var card in cardDataSet)
        {
            pool.Add(card);
            pool.Add(card); // duplicate for pairs
        }

        Shuffle(pool);
        gridSpawner.SpawnGrid(pool);
        activeCards = gridSpawner.spawnedCards;

        callback();
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = Random.Range(0,(n--)); //get random from 0 to n-1
            (list[n], list[k]) = (list[k], list[n]); //swap the lists
        }
    }

    private void RevealAllCards()
    {
        foreach (var card in activeCards)
            card.Reveal();
    }

    private void StartPlayPhase()
    {
        foreach (var card in activeCards)
            card.Hide();
    }

    public void OnCardSelected(Card card)
    {
        if (card.IsMatched || card == firstSelected) return;

        card.Reveal();

        SoundManager.Instance.PlayFlip();

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            
            StartCoroutine(CheckPair(firstSelected, secondSelected));
            firstSelected = secondSelected = null; 
        }
    }

    private IEnumerator CheckPair(Card first, Card second)
    {
        yield return new WaitForSeconds(checkDelay);

        if (first.Id == second.Id)
        {
            first.MarkAsMatched();
            second.MarkAsMatched();

            SoundManager.Instance.PlayMatch();

            SaveHandler.SaveGame(activeCards);   //Save game after matching cards

            CheckWinCondition();
        }
        else
        {
            first.Hide();
            second.Hide();

            SoundManager.Instance.PlayMismatch();
        }
    }

    private void CheckWinCondition()
    {
        foreach (var card in activeCards)
            if (!card.IsMatched) return;

        SaveHandler.DeleteSave();

        Debug.Log("You Won!");
        SoundManager.Instance.PlayWin();
    }
}
