// --------------------------------------------------------- 
// FadeAnim.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEditor;

[ExecuteAlways]
public class FadeAnim : MonoBehaviour ,IMaterialModifier
{
    #region variable 

    [SerializeField]
    private Vector2 _tessellate = new Vector2(10F, 10F);
    [SerializeField, Range(0.0F, 1.0F)]
    private float _progress = 0.0F;
    [SerializeField]
    private bool _flipX = false;
    [SerializeField]
    private bool _flipY = false;

    [SerializeField]
    private Shader _shader = default;

    private readonly int _tessellateID = Shader.PropertyToID("_Tessellate");
    private readonly int _progressID = Shader.PropertyToID("_Progress");
    private readonly int _flipXID = Shader.PropertyToID("_FlipX");
    private readonly int _flipYID = Shader.PropertyToID("_FlipY");

    [NonSerialized] private Graphic _animGraphic;
    private Material _material;

    #endregion
    #region property

    public Graphic AnimGraphic
    {
        get
        {
            _animGraphic ??= GetComponent<Graphic>();

            return _animGraphic;
        }
    }

    #endregion
    #region method


    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return baseMaterial;
        }

        UpdateProperty(baseMaterial);
        return _material;
    }

    private void OnDidApplyAnimationProperties()
    {
        if (!isActiveAndEnabled || !_animGraphic)
        {
            return;
        }

        _animGraphic.SetMaterialDirty();
    }



    private void OnValidate()
    {
        if (!isActiveAndEnabled || AnimGraphic == null)
        {
            return;
        }

        AnimGraphic.SetMaterialDirty();
    }

    private void UpdateProperty(Material baseMaterial)
    {
        if (!_material)
        {
            _material = new Material(_shader);
            _material.CopyPropertiesFromMaterial(baseMaterial);
            _material.hideFlags = HideFlags.HideAndDontSave;
        }

        _material.SetVector(_tessellateID, new Vector4(_tessellate.x, _tessellate.y, 0.0F, 0.0F));
        _material.SetFloat(_progressID, _progress);
        _material.SetFloat(_flipXID, _flipX ? 1.0F : 0.0F);
        _material.SetFloat(_flipYID, _flipY ? 1.0F : 0.0F);
    }

    private void OnEnable()
    {
        if (!AnimGraphic)
        {
            return;
        }

        _animGraphic.SetMaterialDirty();
    }

    private void DestroyMaterial()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            DestroyImmediate(_material);
            _material = null;
            return;
        }
#endif
        Destroy(_material);
        _material = null;
    }

    private void OnDisable()
    {
        if (!_material)
        {
            DestroyMaterial();
        }

        if (!AnimGraphic)
        {
            _animGraphic.SetMaterialDirty();
        }
    }
    #endregion
}