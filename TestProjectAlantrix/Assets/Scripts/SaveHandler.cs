using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class SaveHandler
{
    private static readonly string saveFilePath =
        Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame(List<Card> activeCards, int score, int turnsRemaining)
    {
        GameSaveData saveData = new GameSaveData
        {
            score = score,
            turnsRemaining = turnsRemaining
        };

        foreach (Card card in activeCards)
        {
            saveData.cards.Add(new CardSaveData
            {
                id = card.Id,
                // only keep revealed state if it’s matched
                isRevealed = card.IsMatched ? card.IsRevealed : false,
                isMatched = card.IsMatched
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public static GameSaveData LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("No save file found.");
            return null;
        }

        string json = File.ReadAllText(saveFilePath);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("Game loaded.");
        return saveData;
    }

    public static void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save deleted.");
        }
    }
}

[System.Serializable]
public class CardSaveData
{
    public string id;
    public bool isMatched;
    public bool isRevealed;
}


[System.Serializable]
public class GameSaveData
{
    public List<CardSaveData> cards = new();
    public int score;
    public int turnsRemaining;
}
