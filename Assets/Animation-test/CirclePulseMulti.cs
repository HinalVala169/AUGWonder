using UnityEngine;

public class CirclePulseMulti : MonoBehaviour
{
    [System.Serializable]
    public class Circle
    {
        public GameObject circleObject; // Reference to the circle GameObject
        public float pulseSpeed = 1f;   // Speed of the pulse
        public float maxScale = 2f;    // Maximum scale
        public float minScale = 1f;    // Minimum scale
        public float fadeSpeed = 1f;   // Speed of the fade
        public float startDelay = 0f;  // Individual start delay
    }

    public Circle[] circles; // Array of circles to animate

    private Vector3[] initialScales;  // Store initial scales
    private Material[] materials;     // Store materials for color fading
    private float[] startTimes;       // Track start times for delays

    private void Start()
    {
        // Initialize arrays
        initialScales = new Vector3[circles.Length];
        materials = new Material[circles.Length];
        startTimes = new float[circles.Length];

        // Store initial scales, materials, and initialize start times
        for (int i = 0; i < circles.Length; i++)
        {
            if (circles[i].circleObject != null)
            {
                initialScales[i] = circles[i].circleObject.transform.localScale;

                Renderer renderer = circles[i].circleObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    materials[i] = renderer.material;
                }
                else
                {
                    Debug.LogWarning($"No Renderer found on circle {i}!");
                }

                // Record the start time with the delay
                startTimes[i] = Time.time + circles[i].startDelay;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            if (circles[i].circleObject != null && Time.time >= startTimes[i])
            {
                // Pulse the scale
                float scale = Mathf.PingPong((Time.time - startTimes[i]) * circles[i].pulseSpeed, circles[i].maxScale - circles[i].minScale) + circles[i].minScale;
                circles[i].circleObject.transform.localScale = initialScales[i] * scale;

                // Fade the color
                if (materials[i] != null)
                {
                    float alpha = Mathf.PingPong((Time.time - startTimes[i]) * circles[i].fadeSpeed, 1f); // Fade between 0 and 1
                    Color color = materials[i].color;
                    color.a = alpha;
                    materials[i].color = color;
                }
            }
        }
    }
}
