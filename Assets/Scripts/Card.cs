using UnityEngine;

public class Card
{
    public string Name { get; private set; }
    public int Rarity { get; private set; }
    public string Border { get; private set; }
    public float Value { get; private set; }

    public Card(string name, int rarity, string border, float value)
    {
        Name = name;
        Rarity = rarity;
        Border = border;
        Value = value;
    }
}
