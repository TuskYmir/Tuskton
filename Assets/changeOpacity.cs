using UnityEngine;

public class ChangeOpacitys : MonoBehaviour
{
    // Reference to the material of the object
    private Material objectMaterial;

    // Target opacity value (0.0 is fully transparent, 1.0 is fully opaque)
    public float targetOpacity = 0.5f;

    void Start()
    {
        // Get the Renderer component of the object and its material
        Renderer renderer = GetComponent<Renderer>();
        objectMaterial = renderer.material;

        // Set the material to use a transparent shader
        objectMaterial.SetFloat("_Mode", 3); // This sets the shader to transparent mode (for Standard Shader)
        objectMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        objectMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        objectMaterial.SetInt("_ZWrite", 0); // Disable Z-writing for transparency
        objectMaterial.DisableKeyword("_ALPHATEST_ON");
        objectMaterial.EnableKeyword("_ALPHABLEND_ON");
        objectMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        objectMaterial.renderQueue = 3000; // Transparency queue

        // Apply the desired opacity
        ChangeOpacity(targetOpacity);
    }

    // Function to change opacity of the material
    public void ChangeOpacity(float opacity)
    {
        Color color = objectMaterial.color;
        color.a = Mathf.Clamp01(opacity);  // Ensure the alpha value stays between 0 and 1
        objectMaterial.color = color;
    }
}