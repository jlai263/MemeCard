using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<Card> inventory = new List<Card>(); // List to store the player's cards

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the inventory manager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add a card to the inventory
    public void AddCard(Card card)
    {
        inventory.Add(card);
        Debug.Log($"Added {card.Name} to inventory. Total cards: {inventory.Count}");
    }

    // Get all cards in the inventory
    public List<Card> GetInventory()
    {
        return inventory;
    }

    // Display inventory (optional, for debugging)
    public void DisplayInventory()
    {
        Debug.Log("Inventory Contents:");
        foreach (var card in inventory)
        {
            Debug.Log($"- {card.Name} ({card.Rarity}, {card.Border})");
        }
    }
}
