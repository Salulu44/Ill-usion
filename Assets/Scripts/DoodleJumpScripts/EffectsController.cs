using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsController : MonoBehaviour
{
    [SerializeField] DoodleScoreScript doodleScoreScript;
    private Volume volume;
    private Vignette vignette;
    ChromaticAberration chromaticAberration;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chromaticAberration);
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseEffect();
    }
    void IncreaseEffect() 
    {
        vignette.intensity.value = (float)doodleScoreScript.CurrentScore / doodleScoreScript.MaxScore;
        chromaticAberration.intensity.value = (float)doodleScoreScript.CurrentScore / doodleScoreScript.MaxScore;
    }
}
