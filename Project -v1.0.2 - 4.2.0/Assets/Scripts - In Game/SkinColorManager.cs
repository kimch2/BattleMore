using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinColorManager : MonoBehaviour
{

	public float HueShift = 0;
	public float Saturation = 1.6f;
	public float Value = 1;
	public Shader ColorPickerShader;

	private void Awake()
	{
		if (!ColorPickerShader)
		{
			ColorPickerShader = Shader.Find("Custom/ColorPicker");
		}

	}

	public bool useColoredSkins;
	Dictionary<Material, Material> colorMapper = new Dictionary<Material, Material>();

	public void resetHue(float m)
	{
		HueShift = m;
		foreach (KeyValuePair<Material, Material> mesh in colorMapper)
		{
			mesh.Value.SetFloat("_HueShift", HueShift);
		}
	}
	public void resetSaturation(float m)
	{
		Saturation = m;
		foreach (KeyValuePair<Material, Material> mesh in colorMapper)
		{
			mesh.Value.SetFloat("_Sat", Saturation);
		}
	}

	public Material getSkin(Material toChange)
	{
		if (!useColoredSkins)
		{
			return toChange;
		}

		Material newMat;

		if (!colorMapper.ContainsKey(toChange))
		{
			newMat = new Material(toChange);
			newMat.shader = ColorPickerShader;
			newMat.SetFloat("_HueShift", HueShift);
			newMat.SetFloat("_Sat", Saturation);
			newMat.SetFloat("_Val", Value);
			newMat.renderQueue = 2000;
			colorMapper.Add(toChange, newMat);
		}
		else
		{
			newMat = colorMapper[toChange];
		}

		return newMat;
	}

}
