using UnityEngine;
using DG.Tweening;

public class PulseEffectWithThreeSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] targetSprites; // Array for 3 SpriteRenderer components
    [SerializeField] private float[] scaleUpSizes; // Array to set individual scale sizes for each sprite
    [SerializeField] private float[] animationDurations; // Array to set individual animation durations for each sprite
    [SerializeField] private float[] fadeOpacities; // Array to set individual fade opacities for each sprite
    [SerializeField] private float[] resetScales; // Array to set individual reset scales for each sprite
    [SerializeField] private float[] fadeBackOpacities; // Array for individual fade-back opacities
    [SerializeField] private float delayBetweenSprites = 0.1f; // Delay between animations for each sprite

    void Start()
    {
        // Ensure that each array is the same length as the targetSprites array
        if (scaleUpSizes.Length != targetSprites.Length || animationDurations.Length != targetSprites.Length || fadeOpacities.Length != targetSprites.Length || resetScales.Length != targetSprites.Length || fadeBackOpacities.Length != targetSprites.Length)
        {
            Debug.LogError("Array lengths do not match the number of target sprites.");
            return;
        }

        for (int i = 0; i < targetSprites.Length; i++)
        {
            int index = i; // Capture the current index for closure

            DOTween.Sequence()
                .AppendInterval(delayBetweenSprites * index) // Delay based on the sprite's order
                .Append(targetSprites[index].transform.DOScale(Vector3.one * scaleUpSizes[index], animationDurations[index])) // Smooth scaling up
                .Join(targetSprites[index].DOColor(new Color(targetSprites[index].color.r, targetSprites[index].color.g, targetSprites[index].color.b, fadeOpacities[index]), animationDurations[index])) // Smooth fade
                .Append(targetSprites[index].transform.DOScale(Vector3.one * resetScales[index], animationDurations[index])) // Individual reset scale
                .Join(targetSprites[index].DOColor(new Color(targetSprites[index].color.r, targetSprites[index].color.g, targetSprites[index].color.b, fadeBackOpacities[index]), animationDurations[index])) // Individual fade-back opacity
                .SetLoops(-1, LoopType.Restart); // Infinite loop
        }
    }
}
