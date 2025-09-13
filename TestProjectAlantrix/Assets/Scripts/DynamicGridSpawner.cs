using UnityEngine;
using System.Collections.Generic;

public class DynamicGridSpawner : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private RectTransform gridParent;
    [SerializeField] private float spacing = 10f;
    [SerializeField] private float screenMargin = 20f; 

    public readonly List<Card> spawnedCards = new();

    public void SpawnGrid(List<CardData> cardsData) //dynamicaly spawn grid based on card numbers to best fit screen..
    {
        ClearGrid();

        int totalCards = cardsData.Count;

        if (totalCards == 0) return;

        float aspect = (float)Screen.width / Screen.height; //get screen aspect

        int cols, rows;
        if (aspect >= 1f) // landscape
        {
            cols = Mathf.CeilToInt(Mathf.Sqrt(totalCards * aspect));
            rows = Mathf.CeilToInt((float)totalCards / cols);
        }
        else // portrait
        {
            rows = Mathf.CeilToInt(Mathf.Sqrt(totalCards / aspect));
            cols = Mathf.CeilToInt((float)totalCards / rows);
        }

        Vector2 parentSize = gridParent.rect.size;

        //get available width and height to put cards in
        float availableWidth = parentSize.x - (2 * screenMargin); 
        float availableHeight = parentSize.y - (2 * screenMargin);

        //total space
        float totalSpacingX = (cols - 1) * spacing;
        float totalSpacingY = (rows - 1) * spacing;

        //calculate the width and height for cell
        float cellWidth = (availableWidth - totalSpacingX) / cols;
        float cellHeight = (availableHeight - totalSpacingY) / rows;

        float cardSize = Mathf.Min(cellWidth, cellHeight);

        float gridWidth = cols * cardSize + totalSpacingX;
        float gridHeight = rows * cardSize + totalSpacingY;
        Vector2 offset = new Vector2(-gridWidth / 2f, gridHeight / 2f);

        //spawn cards with above calculated values
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int index = row * cols + col;
                if (index >= totalCards) return;

                Card card = Instantiate(cardPrefab, gridParent);
                spawnedCards.Add(card);

                RectTransform cardRect = card.GetComponent<RectTransform>();
                cardRect.sizeDelta = new Vector2(cardSize, cardSize);

                float x = col * (cardSize + spacing) + (cardSize / 2);
                float y = -(row * (cardSize + spacing)) - (cardSize / 2);

                cardRect.anchoredPosition = offset + new Vector2(x, y);

                card.Initialize(cardsData[index]);
            }
        }
    }

    private void ClearGrid()
    {
        foreach (var card in spawnedCards)
            if (card != null) Destroy(card.gameObject);
        spawnedCards.Clear();
    }
}
