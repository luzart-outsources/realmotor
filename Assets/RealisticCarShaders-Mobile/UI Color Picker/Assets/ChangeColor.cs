using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SkrilStudio
{
    public class ChangeColor : MonoBehaviour
    {
        public bool isDragging = false;
        public bool isDecal = false;
        public bool isPearl = false;
        public bool isFlakes = false;
        public Material material;
        public Texture[] decalTextures;

        public Slider decalSizeSlider;

        public MeshRenderer[] paintableMeshes;
        public Material[] bodyMaterials;

        public CUIColorPicker CarColorPickerToUse;
        public CUIColorPicker DecalColorPickerToUse;

        // material changer buttons
        public Image[] materialChangerButtons;

        public Text shaderInfoText;

        private int currentMaterialID; // not getting saved

        void Start()
        {
            currentMaterialID = 0;
            // get and set saved colors
            // get car color
            bodyMaterials[0].SetColor("_Color", RCS_PlayerPrefsX.GetColor("CarColor"));
            bodyMaterials[currentMaterialID].SetColor("_PearlescentColor", RCS_PlayerPrefsX.GetColor("CarColor")); // set secondary color back to normal
            // get decal color
            bodyMaterials[0].SetColor("_DecalColor", RCS_PlayerPrefsX.GetColor("DecalColor"));

            // get and set saved decals
            bodyMaterials[0].SetTexture("_Decal", decalTextures[PlayerPrefs.GetInt("_Decal")]);

            // change button color
            if (materialChangerButtons.Length != 0)
                materialChangerButtons[0].color = Color.green; // set the choosed soundpack button's color to green
        }

        void Update()
        {
            if (isDragging)
            {
                if (!isDecal) // car color
                {
                    bodyMaterials[currentMaterialID].SetColor("_Color", CarColorPickerToUse.value);
                    // set pearlescent color to current color's negative
                    if (isPearl)
                        bodyMaterials[currentMaterialID].SetColor("_PearlescentColor", new Color(1 - CarColorPickerToUse.hueColor.r, 1 - CarColorPickerToUse.hueColor.g, 1 - CarColorPickerToUse.hueColor.b));
                    else
                        bodyMaterials[currentMaterialID].SetColor("_PearlescentColor", CarColorPickerToUse.value);
                }
                else // decals
                {
                    bodyMaterials[currentMaterialID].SetColor("_DecalColor", DecalColorPickerToUse.value);
                }
            }
        }

        public void SetDecal(GameObject g)
        {
            // convert string to int
            int convertedString;
            int.TryParse(g.name, out convertedString); // convert to int

            // set decals by button name
            bodyMaterials[currentMaterialID].SetTexture("_Decal", decalTextures[convertedString]);
            // save decal id
            PlayerPrefs.SetInt("DecalID", convertedString);
            PlayerPrefs.Save();
        }
        public void SetBodyMaterial(GameObject g)
        {
            // convert string to int
            int convertedString;
            int.TryParse(g.name, out convertedString); // convert to int
            currentMaterialID = convertedString; // set currently chosed material shader
            for (int i = 0; i < paintableMeshes.Length; i++)
            {
                paintableMeshes[i].material = bodyMaterials[convertedString];
            }
            // update selected material color
            bodyMaterials[convertedString].SetColor("_Color", CarColorPickerToUse.value);
            // set pearlescent color to current color's negative
            if (isPearl)
                bodyMaterials[convertedString].SetColor("_PearlescentColor", new Color(1 - CarColorPickerToUse.hueColor.r, 1 - CarColorPickerToUse.hueColor.g, 1 - CarColorPickerToUse.hueColor.b));
            else
                bodyMaterials[convertedString].SetColor("_PearlescentColor", CarColorPickerToUse.value);
            // update decals on selected material
            bodyMaterials[convertedString].SetColor("_DecalColor", DecalColorPickerToUse.value);
            bodyMaterials[convertedString].SetTexture("_Decal", decalTextures[PlayerPrefs.GetInt("_Decal")]);
            bodyMaterials[convertedString].SetFloat("_DecalUVScale", decalSizeSlider.value);
            // change Demo UI button color
            ChangeButtonColor(convertedString);
        }
        public void ChangeButtonColor(int a)
        {
            materialChangerButtons[a].color = Color.green; // set the choosed button's color to green
            for (int i = 0; i < materialChangerButtons.Length; i++)
            {
                if (i != a) // set all other buttons color to white
                    materialChangerButtons[i].color = Color.white;
            }
        }
        public void SaveColor()
        {
            RCS_PlayerPrefsX.SetColor("CarColor", CarColorPickerToUse.value);
        }
        public void SaveDecalColor()
        {
            RCS_PlayerPrefsX.SetColor("DecalColor", DecalColorPickerToUse.value);
        }
        public void IsCarColor()
        {
            isDecal = false;
        }
        public void IsDecalColor()
        {
            isDecal = true;
        }
        public void IsPearl()
        {
            isPearl = true;
        }
        public void IsNotPearl()
        {
            isPearl = false;
            bodyMaterials[currentMaterialID].SetColor("_PearlescentColor", CarColorPickerToUse.value); // set secondary color back to normal
        }
        public void IsWood() // needed to keep bumped wood material's decal
        {
            bodyMaterials[currentMaterialID].SetTexture("_Decal", decalTextures[4]);
            bodyMaterials[currentMaterialID].SetFloat("_DecalUVScale", 25);
        }
        public void SliderValueChange()
        {
            bodyMaterials[currentMaterialID].SetFloat("_DecalUVScale", decalSizeSlider.value);
        }
        public void ChangeShaderInfoText(string text)
        {
            shaderInfoText.text = text;
        }
    }
}
