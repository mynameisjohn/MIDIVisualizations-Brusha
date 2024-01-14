using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PulseRandomOpacityOnNote : NoteReceiver
{
    public int _PulseOpacityNoteNumber = 60;
    public int _ToggleInversioneNoteNumber = 62;
    public int _NumberOfChildrenToPulse = 1;
    public float _PulseIncreaseAmount = 0.5f;
    public float _PulseDecayAmount = 0.98f;
    PulseColorOpacity[] _childrenToPulse;
    HashSet<int> _alreadyPulsed = new HashSet<int>();
    int _lastPulsed = -1;

    public KeyCode _PulseOpacityKey = KeyCode.Space;
    public KeyCode _ToggleInversionKey = KeyCode.UpArrow;

    public Material _PulseOpacityMaterial;
    public Texture2D[] _Textures;
    public Vector3 _ChildScale;
    public Vector3 _ChildPosition;
    public bool _StartInverted;

    // Start is called before the first frame update
    void Start()
    {
        _childrenToPulse = new PulseColorOpacity[_Textures.Length];
        for (int i = 0; i < _Textures.Length; i++)
        {
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Quad);
            child.transform.parent = transform;
            child.transform.localScale = _ChildScale;
            child.transform.localPosition = _ChildPosition;

            MeshRenderer mr = child.GetComponent<MeshRenderer>();
            mr.material = _PulseOpacityMaterial;
            mr.material.mainTexture = _Textures[i];

            PulseColorOpacity p = child.AddComponent<PulseColorOpacity>();
            p._PulseIncreaseAmount = _PulseIncreaseAmount;
            p._PulseDecayAmount = _PulseDecayAmount;
            p._StartInverted = _StartInverted;
            _childrenToPulse[i] = p;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_PulseOpacityKey))
        {
            pulseOpacity();
        }
        if (Input.GetKeyDown(_ToggleInversionKey))
        {
            toggleInversion();
        }
    }

    public override void HandleNote(int noteNumber)
    {
        if (noteNumber == _PulseOpacityNoteNumber)
        {
            pulseOpacity();
        }
        if (noteNumber == _ToggleInversioneNoteNumber)
        {
            toggleInversion();
        }
    }

    void pulseOpacity()
    {
        for (int i = 0; i < _NumberOfChildrenToPulse;)
        {
            if (_alreadyPulsed.Count == _childrenToPulse.Length)
            {
                _alreadyPulsed.Clear();
            }

            int idxToPulse = Random.Range(0, _childrenToPulse.Length);
            if (idxToPulse == _lastPulsed || _alreadyPulsed.Contains(idxToPulse))
            {
                continue;
            }
            _childrenToPulse[idxToPulse].PulseOpacity();
            _alreadyPulsed.Add(idxToPulse);
            i++;
            _lastPulsed = idxToPulse;
        }
    }

    void toggleInversion()
    {
        foreach (PulseColorOpacity p in _childrenToPulse)
        {
            p.SetInversion(1f - p.invertValue);
        }
    }
}
