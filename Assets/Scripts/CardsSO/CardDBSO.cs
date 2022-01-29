using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDBSO", menuName = "CardDB", order = 1)]
public class CardDB : ScriptableObject
{
    public List<CardAttributes> frontCards;
    public List<CardAttributes> backCards;

    private List<CardAttributes> consolidatedCardList;

    // public GetCardEffect(int index){

    // }
}
