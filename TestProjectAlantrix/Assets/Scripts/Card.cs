using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject frontImageBG;
    [SerializeField] private Image backImage;

    [Header("Animation")]
    [SerializeField] private float flipDuration = 0.3f;

    private CardData data;
    private bool isRevealed;
    private bool isMatched;
    private bool isFlipping;

    public string Id => data.Id;
    public bool IsMatched => isMatched;
    public bool IsRevealed => isRevealed;

    public void Initialize(CardData cardData)
    {
        data = cardData;
        frontImage.sprite = data.FrontSprite;
        FlipDownInstant();
        isMatched = false;
    }

    public void Initialize(CardData cardData, CardSaveData saveData)
    {
        data = cardData;
        frontImage.sprite = data.FrontSprite;

        isMatched = saveData.isMatched;
        isRevealed = saveData.isRevealed;

        if (isMatched)
        {
            frontImageBG.SetActive(true);
            backImage.gameObject.SetActive(false);
        }
        else if (isRevealed)
        {
            frontImageBG.SetActive(true);
            backImage.gameObject.SetActive(false);
        }
        else
        {
            FlipDownInstant();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isMatched && !isRevealed && !isFlipping)
            GameManager.Instance.OnCardSelected(this);
    }

    public void Reveal()
    {
        if (isRevealed || isFlipping) return;
        isRevealed = true;
        StartCoroutine(FlipCard(backImage.gameObject, frontImageBG));
    }

    public void Hide()
    {
        if (!isRevealed || isFlipping) return;
        isRevealed = false;
        StartCoroutine(FlipCard(frontImageBG, backImage.gameObject));
    }

    public void MarkAsMatched()
    {
        isMatched = true;
        isRevealed = true; // ensure revealed flag is correct
        frontImageBG.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    private void FlipDownInstant()
    {
        isRevealed = false;
        frontImageBG.SetActive(false);
        backImage.gameObject.SetActive(true);
    }

    private IEnumerator FlipCard(GameObject from, GameObject to)
    {
        isFlipping = true;
        float time = 0f;
        Vector3 originalScale = transform.localScale;

        // Shrink X to 0
        while (time < flipDuration / 2f)
        {
            time += Time.deltaTime;
            float t = time / (flipDuration / 2f);
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, t), 1f, 1f);
            yield return null;
        }

        // Swap the visible side
        from.SetActive(false);
        to.SetActive(true);

        // Expand X back to 1
        time = 0f;
        while (time < flipDuration / 2f)
        {
            time += Time.deltaTime;
            float t = time / (flipDuration / 2f);
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, t), 1f, 1f);
            yield return null;
        }

        transform.localScale = originalScale;
        isFlipping = false;
    }
}
