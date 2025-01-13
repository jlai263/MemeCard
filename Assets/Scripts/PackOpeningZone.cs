using UnityEngine;
using UnityEngine.UI;

public class PackOpeningZone : MonoBehaviour
{
    public GameObject cardOpeningUI; // Assign this in the Inspector
    public Text cardNameText; // Assign this in the Inspector
    public Text cardDetailsText; // Assign this in the Inspector
    public Image cardImage; // Assign this in the Inspector
    public Button addToInventoryButton; // Assign this in the Inspector
    public Button quicksellButton; // Assign this in the Inspector
    public string packName = "2024 Pack"; // Set this to the name of the pack
    public float packCost = 50f; // Cost of the pack in Dogecoin
    public GameObject player; // Assign the player GameObject in the Inspector

    private bool playerInZone = false;
    private Card currentCard; // Store the currently opened card
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script
    private PlayerFirstPersonController playerCamera; // Reference to the camera controller

    void Start()
    {
        // Assign button functionality
        addToInventoryButton.onClick.AddListener(AddToInventory);
        quicksellButton.onClick.AddListener(Quicksell);

        // Get player movement and camera scripts
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerCamera = player.GetComponentInChildren<PlayerFirstPersonController>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player entered pack opening zone.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("Player left pack opening zone.");
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.F))
        {
            OpenPack();
        }
    }

    void OpenPack()
    {
        if (CurrencyManager.Instance.SpendCurrency(packCost))
        {
            Debug.Log($"Opening {packName}...");

            // Ensure all references are set before proceeding
            if (cardOpeningUI == null || cardNameText == null || cardDetailsText == null)
            {
                Debug.LogError("UI references are not assigned in the Inspector!");
                return;
            }

            // Card data
            string[] cardNames = { "Military Man", "Moo Deng", "Turkish Quandale Dingle", "Conscious Baby", 
                                   "Steve", "Chill Guy", "Hawk Tuah", "The Rizzler", 
                                   "Le-sunshine", "Low Taper Fade" };
            int[] rarities = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 }; // Sorted by rarity

            // Validate data
            if (cardNames.Length != rarities.Length)
            {
                Debug.LogError("Card names and rarities arrays are not the same length!");
                return;
            }

            // Weighted probabilities for rarities
            float[] rarityOdds = { 0.65f, 0.20f, 0.10f, 0.04f, 0.01f };

            // Get a random rarity based on weights
            int selectedRarity = GetWeightedRandom(rarityOdds);

            // Filter cards by selected rarity
            string[] filteredNames = FilterCardsByRarity(cardNames, rarities, selectedRarity);

            // Randomly select a card from the filtered list
            string cardName = filteredNames[Random.Range(0, filteredNames.Length)];

            // Assign a random border
            int borderKey = Random.Range(1, 6); // Randomly select a border
            if (!CardManager.Instance.Borders.ContainsKey(borderKey))
            {
                Debug.LogError($"Border with key {borderKey} not found!");
                return;
            }

            var borderInfo = CardManager.Instance.Borders[borderKey];
            float baseValue = selectedRarity * 10; // Example value calculation
            float finalValue = baseValue * borderInfo.Modifier; // Use Modifier property

            // Create the card
            currentCard = new Card(cardName, selectedRarity, borderInfo.Name, finalValue);

            // Display the card in the UI
            ShowCardUI();
            cardNameText.text = currentCard.Name;
            cardDetailsText.text = $"{CardManager.Instance.RarityTiers[selectedRarity - 1]} - {currentCard.Border}\nValue: {currentCard.Value} Dogecoin";
        }
        else
        {
            Debug.Log("Not enough Dogecoin to open the pack!");
        }
    }

    void AddToInventory()
    {
        if (currentCard == null) return;

        Debug.Log($"Added {currentCard.Name} to inventory!");
        InventoryManager.Instance.AddCard(currentCard); // Add the card to the inventory
        CloseCardUI();
    }

    void Quicksell()
    {
        if (currentCard == null) return;

        Debug.Log($"Quicksold {currentCard.Name} for {currentCard.Value} Dogecoin!");
        CurrencyManager.Instance.AddCurrency(currentCard.Value); // Add Dogecoin
        CloseCardUI();
    }

    void ShowCardUI()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        cardOpeningUI.SetActive(true); // Show the UI

        // Disable player movement and camera
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerCamera != null) playerCamera.enabled = false;
    }

    void CloseCardUI()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        cardOpeningUI.SetActive(false); // Hide the UI

        // Enable player movement and camera
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCamera != null) playerCamera.enabled = true;

        currentCard = null;
    }

    int GetWeightedRandom(float[] weights)
    {
        float total = 0;
        foreach (float weight in weights)
        {
            total += weight;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < weights.Length; i++)
        {
            if (randomPoint < weights[i])
            {
                return i + 1; // Rarities are 1-based
            }
            randomPoint -= weights[i];
        }

        return weights.Length; // Return the highest rarity by default
    }

    string[] FilterCardsByRarity(string[] cardNames, int[] rarities, int rarity)
    {
        System.Collections.Generic.List<string> filteredCards = new System.Collections.Generic.List<string>();

        for (int i = 0; i < rarities.Length; i++)
        {
            if (rarities[i] == rarity)
            {
                filteredCards.Add(cardNames[i]);
            }
        }

        return filteredCards.ToArray();
    }
}
