using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public float Dogecoin = 100; // Starting currency

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the currency manager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add Dogecoin to the player's balance
    public void AddCurrency(float amount)
    {
        Dogecoin += amount;
        Debug.Log($"Added {amount} Dogecoin. Total: {Dogecoin}");
    }

    // Deduct Dogecoin from the player's balance
    public bool SpendCurrency(float amount)
    {
        if (Dogecoin >= amount)
        {
            Dogecoin -= amount;
            Debug.Log($"Spent {amount} Dogecoin. Remaining: {Dogecoin}");
            return true;
        }

        Debug.LogWarning("Not enough Dogecoin!");
        return false;
    }

    // Get the current Dogecoin balance
    public float GetCurrency()
    {
        return Dogecoin;
    }
}
