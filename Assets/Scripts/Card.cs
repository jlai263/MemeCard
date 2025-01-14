using UnityEngine;

public class Card
{
    public string Name { get; private set; }
    public int Rarity { get; private set; }
    public int Border { get; private set; } // Changed from string to int
    public float Value { get; private set; }

    public Card(string name, int rarity, int border, float value) // Changed parameter type for border
    {
        Name = name;
        Rarity = rarity;
        Border = border; // Border is now stored as an int
        Value = value;
    }
}
