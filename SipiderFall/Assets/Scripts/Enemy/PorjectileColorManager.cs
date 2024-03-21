using SpriteGlow;
using UnityEngine;

public class PorjectileColorManager : MonoBehaviour
{
    SpriteGlowEffect _spriteGlow;
    float _H;

    private void Awake()
    {
        _spriteGlow = GetComponent<SpriteGlowEffect>();
        Color.RGBToHSV(_spriteGlow.GlowColor, out _H, out float S, out float V);
    }

    private void Update()
    {
        RGB();
    }

    public void RGB()
    {
        _H += Time.deltaTime;
        _spriteGlow.GlowColor = Color.HSVToRGB(_H, 1, 1);
        if(_H >= 1)
        {
            _H = 0;
        }
    }
}
