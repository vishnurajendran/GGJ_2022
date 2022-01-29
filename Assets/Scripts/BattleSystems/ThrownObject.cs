using System;
using System.Collections;
using UnityEngine;

namespace Flippards
{
    public class ThrownObject : MonoBehaviour
    {
        private float ThrowTime => isAutoTurn ? 0.04f : 1f;

        private Vector3 startPos;
        private Vector3 endPos;
        private bool isAutoTurn;

        private Action onComplete;
        public void Init(bool isAutoTurn, Vector3 startPos, Vector3 endPos, Action onComplete)
        {
            this.isAutoTurn = isAutoTurn;
            this.startPos = startPos;
            this.endPos = endPos;
            this.onComplete = onComplete;
            StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            float currTimer = 0f;

            while (currTimer < ThrowTime)
            {
                currTimer += Time.deltaTime;
                yield return null;
                transform.position = Vector3.Lerp(startPos, endPos, currTimer / ThrowTime);
            }
            onComplete?.Invoke();
            Destroy(this.gameObject);
        }
    }
}