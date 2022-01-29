using System;
public enum CardClass {Weight, Liquid, Paper};
public enum CardType {Hit, Heal, Flip};

[Serializable]
public class CardAttributes
{
    public string name;
    public int value;
    public CardClass cardClass;
    public CardType cardType;
    public int count;
}