using System;
public enum CardClass {Weight, Liquid, Paper};
public enum CardType {Hit, Heal};

[Serializable]
public class CardAttributes
{
    public string name;
    public float value;
    public CardClass cardClass;
    public CardType cardType;
    public int count;
}