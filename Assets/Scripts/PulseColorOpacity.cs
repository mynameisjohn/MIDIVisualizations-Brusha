using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseColorOpacity : MonoBehaviour
{
    public float _PulseIncreaseAmount = 0.1f;
    public float _PulseDecayAmount = 0.98f;
    public bool _StartInverted;

    float  _invertValue;
    public float invertValue { get {  return _invertValue; } }

    Material _material;
    float _opacityValue;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        if (_StartInverted)
        {
            SetInversion(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _opacityValue *= _PulseDecayAmount;
        if (_opacityValue < 0.001f)
        {
            _opacityValue = 0f;
        }

        _material.SetFloat("_Opacity", _opacityValue);
    }

    public void PulseOpacity()
    {
        _opacityValue = Mathf.Clamp01(_opacityValue + _PulseIncreaseAmount);
    }

    public void SetInversion(float inversion)
    {
        _invertValue = Mathf.Clamp01(inversion);
        _material.SetFloat("_InversionBlend", _invertValue);
    }
}
