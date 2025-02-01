using UnityEngine;
using UnityEngine.UI;

public class ShipsInShop : MonoBehaviour
{
    [SerializeField] GameObject[] ShipModelSelectors;
    [SerializeField] Button NextButton_Left;
    [SerializeField] Button NextButton_Right;

    private int currentIndex = 0;

    private void Start()
    {
        if (NextButton_Left != null)
        {
            NextButton_Left.onClick.AddListener(ShowPreviousShip);
        }

        if (NextButton_Right != null)
        {
            NextButton_Right.onClick.AddListener(ShowNextShip);
        }

        UpdateShipModelSelectors();
    }

    private void ShowPreviousShip()
    {
        currentIndex = (currentIndex - 1 + ShipModelSelectors.Length) % ShipModelSelectors.Length;
        UpdateShipModelSelectors();
    }

    private void ShowNextShip()
    {
        currentIndex = (currentIndex + 1) % ShipModelSelectors.Length;
        UpdateShipModelSelectors();
    }

    private void UpdateShipModelSelectors()
    {
        for (int i = 0; i < ShipModelSelectors.Length; i++)
        {
            if (i == currentIndex)
                ShipModelSelectors[i].transform.localScale = new Vector3(0.87979f, 0.87979f, 0.87979f); // Restore to original size
            else
                ShipModelSelectors[i].transform.localScale = Vector3.zero; // Scale to zero
        }
    }
}
