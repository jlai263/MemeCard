using UnityEngine;
using TMPro;

public class DogecoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI dogecoinText; // Assign in Inspector
    private CurrencyManager currencyManager;

    void Start()
    {
        if (dogecoinText == null)
        {
            Debug.LogError("DogecoinText is not assigned in the Inspector!");
            return;
        }

        currencyManager = CurrencyManager.Instance;
        if (currencyManager == null)
        {
            Debug.LogError("CurrencyManager instance not found!");
            return;
        }

        UpdateDogecoinText();
    }

    void Update()
    {
        UpdateDogecoinText();
    }

    private void UpdateDogecoinText()
    {
        dogecoinText.text = $"Dogecoin: {currencyManager.GetCurrency():0.00}";
    }
}
