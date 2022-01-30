using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlipConfirmationButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool isPlayer;
    public void OnPointerClick(PointerEventData eventData)
    {
        BattleVisuals.Instance.HideSelectors();
        BattleVisuals.Instance.HideSelectorTriggers();
        BattleVisuals.Instance.ShowSelector(isPlayer ? EntityType.PLAYER : EntityType.ENEMY);
        BattleVisualManager.Instance.onFlipTargetChosen.Invoke(isPlayer ? EntityType.PLAYER : EntityType.ENEMY);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleVisuals.Instance.ShowSelector(isPlayer ? EntityType.PLAYER : EntityType.ENEMY);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleVisuals.Instance.ShowSelector(isPlayer ? EntityType.PLAYER : EntityType.ENEMY);
    }
}
