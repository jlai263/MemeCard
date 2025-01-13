using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject InventoryCanvas; // Assign in Inspector, parent UI canvas for the inventory
    public Transform cardGrid; // Assign in Inspector, the grid where cards will be displayed
    public GameObject cardSlotPrefab; // Assign in Inspector, prefab for individual card slots
    public GameObject cardDetailsPanel; // Assign in Inspector, panel for showing detailed card info
    public Text cardDetailsText; // Assign in Inspector, text field for card info
    public Button quicksellButton; // Assign in Inspector
    public Button tradeButton; // Assign in Inspector
    public Button closeDetailsButton; // Assign in Inspector

    private Card selectedCard; // Store the currently selected card

    void Start()
    {
        Debug.Log("InventoryUIManager script initialized."); // Log initialization
        InventoryCanvas.SetActive(false); // Ensure inventory and details panel are hidden at start
        cardDetailsPanel.SetActive(false);

        // Add listener for closing details
        if (closeDetailsButton != null)
        {
            closeDetailsButton.onClick.AddListener(CloseDetailsPanel);
        }
        else
        {
            Debug.LogError("CloseDetailsButton is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I key pressed");
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        bool isActive = InventoryCanvas.activeSelf;

        // Toggle visibility
        InventoryCanvas.SetActive(!isActive);

        if (!isActive)
        {
            // Populate inventory when opening
            PopulateInventory();
        }

        // Show/hide cursor and lock state
        Cursor.visible = !isActive;
        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;

        Debug.Log(isActive ? "Inventory closed" : "Inventory opened");
    }

    public void PopulateInventory()
    {
        // Clear existing cards in the grid
        foreach (Transform child in cardGrid)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"Populating inventory with {InventoryManager.Instance.GetInventory().Count} cards.");

        // Loop through all cards in the inventory
        foreach (var card in InventoryManager.Instance.GetInventory())
        {
            Debug.Log($"Creating card slot for {card.Name}");

            // Instantiate the prefab
            GameObject cardSlot = Instantiate(cardSlotPrefab, cardGrid);

            // Find and modify the Image component directly on the cardSlotPrefab
            Image cardSlotImage = cardSlot.GetComponent<Image>();
            if (cardSlotImage != null)
            {
                Sprite cardSprite = CardManager.Instance.GetCardImage(card.Name);
                if (cardSprite != null)
                {
                    cardSlotImage.sprite = cardSprite;
                }
                else
                {
                    Debug.LogError($"Sprite for card {card.Name} not found!");
                }
            }
            else
            {
                Debug.LogError("Image component is missing on cardSlotPrefab!");
            }

            // Find and update the CardName text
            Text cardName = cardSlot.transform.Find("CardName")?.GetComponent<Text>();
            if (cardName != null)
            {
                cardName.text = card.Name;
            }
            else
            {
                Debug.LogError("CardName component is missing in prefab!");
            }

            // Add a button click listener to show card details
            Button cardButton = cardSlot.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.AddListener(() => ShowCardDetails(card));
            }
            else
            {
                Debug.LogError("Button component missing in cardSlotPrefab!");
            }
        }
    }




    public void ShowCardDetails(Card card)
    {
        selectedCard = card; // Store the selected card

        // Show card details
        cardDetailsText.text = $"{card.Name}\n{CardManager.Instance.RarityTiers[card.Rarity - 1]} - {card.Border}\nValue: {card.Value} Dogecoin";
        cardDetailsPanel.SetActive(true);

        // Add listeners to buttons
        quicksellButton.onClick.RemoveAllListeners();
        quicksellButton.onClick.AddListener(QuicksellCard);

        tradeButton.onClick.RemoveAllListeners();
        tradeButton.onClick.AddListener(() => TradeCard(card)); // Placeholder for trade logic
    }

    public void QuicksellCard()
    {
        if (selectedCard == null) return;

        // Quicksell logic
        Debug.Log($"Quicksold {selectedCard.Name} for {selectedCard.Value} Dogecoin!");
        InventoryManager.Instance.GetInventory().Remove(selectedCard);
        CurrencyManager.Instance.AddCurrency(selectedCard.Value);

        // Refresh UI
        PopulateInventory();
        CloseDetailsPanel();
    }

    public void TradeCard(Card card)
    {
        // Placeholder for trade logic
        Debug.Log($"Trade {card.Name} functionality not implemented yet.");
    }

    public void CloseDetailsPanel()
    {
        cardDetailsPanel.SetActive(false);
        selectedCard = null;
    }
}
