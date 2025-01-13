using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    public Dictionary<int, Border> Borders = new Dictionary<int, Border>();
    public string[] RarityTiers = { "Common", "Uncommon", "Rare", "Epic", "Legendary" };

    // A dictionary to store card images with their names as keys
    public Dictionary<string, Sprite> CardImages = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeBorders();
            LoadCardImages();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeBorders()
    {
        Borders[1] = new Border("Worn", 0.5f);
        Borders[2] = new Border("Scratched", 0.75f);
        Borders[3] = new Border("Pristine", 1.0f);
        Borders[4] = new Border("Shiny", 1.25f);
        Borders[5] = new Border("Mythic", 1.5f);
    }

    // Load card images into the dictionary
    private void LoadCardImages()
    {
        // Ensure the card names match exactly with the sprite file names
        CardImages.Add("The Rizzler", Resources.Load<Sprite>("CardSprites/TheRizzler"));
        CardImages.Add("Military Man", Resources.Load<Sprite>("CardSprites/MilitaryMan"));
        CardImages.Add("Conscious Baby", Resources.Load<Sprite>("CardSprites/ConsciousBaby"));
        CardImages.Add("Chill Guy", Resources.Load<Sprite>("CardSprites/ChillGuy"));
        CardImages.Add("Le-sunshine", Resources.Load<Sprite>("CardSprites/LeSunshine"));
        CardImages.Add("Turkish Quandale Dingle", Resources.Load<Sprite>("CardSprites/TurkishQuandaleDingle"));
        CardImages.Add("Moo Deng", Resources.Load<Sprite>("CardSprites/MooDeng"));
        CardImages.Add("Low Taper Fade", Resources.Load<Sprite>("CardSprites/LowTaperFade"));
        CardImages.Add("Hawk Tuah", Resources.Load<Sprite>("CardSprites/HawkTuah"));
        CardImages.Add("Steve", Resources.Load<Sprite>("CardSprites/Steve"));
    }


    // Method to get a card image by name
    public Sprite GetCardImage(string cardName)
    {
        if (CardImages.TryGetValue(cardName, out Sprite cardSprite))
        {
            return cardSprite;
        }
        else
        {
            Debug.LogError($"Card image for {cardName} not found!");
            return null;
        }
    }
}

public class Border
{
    public string Name;
    public float Modifier;

    public Border(string name, float modifier)
    {
        Name = name;
        Modifier = modifier;
    }
}
