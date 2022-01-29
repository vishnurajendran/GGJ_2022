using System;

public enum CardClass
{
    Weight,
    Liquid,
    Paper
};

public enum CardType
{
    Hit,
    Heal,
    Flip
};

[Serializable]
public class CardAttributes
{
    public string name;
    public int value;
    public CardClass cardClass;
    public CardType cardType;
    public int count;
    public string imagePath;

    public CardAttributes(CardAttributes copyConstructor) : this(copyConstructor.name, copyConstructor.value,
        copyConstructor.cardClass, copyConstructor.cardType, copyConstructor.count, copyConstructor.imagePath)
    {
    }

    public CardAttributes(string name, int value, CardClass cardClass, CardType cardType, int count, string imagePath)
    {
        this.name = name;
        this.value = value;
        this.cardClass = cardClass;
        this.cardType = cardType;
        this.count = count;
        this.imagePath = imagePath;
    }
}