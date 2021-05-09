using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    [Header("Refferences")]
    [SerializeField] CraftingRecipeUI recipeUIPrefab;
    [SerializeField] RectTransform recipeUIParent;
    [SerializeField] List<CraftingRecipeUI> craftingRecipeUIs;


    [Header("Public Variables")]
    public Inventory ItemContainer;
    public List<CraftingRecipe> CraftingRecipes;

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        Init();
        
    }
    
    private void Init()
    {
        if (ItemContainer == null)
        {
            ItemContainer = FindObjectOfType<Inventory>();
        }
        recipeUIParent.GetComponentsInChildren<CraftingRecipeUI>(includeInactive: true, result: craftingRecipeUIs);
        UpdateCraftingRecipes();
    }

    public void UpdateCraftingRecipes()
    {
        for (int i = 0; i < CraftingRecipes.Count; i++)
        {
            if (craftingRecipeUIs.Count == i)
            {
                craftingRecipeUIs.Add(Instantiate(recipeUIPrefab, recipeUIParent, false));
            }
            else if (craftingRecipeUIs[i] == null)
            {
                craftingRecipeUIs[i] = Instantiate(recipeUIPrefab, recipeUIParent, false);
            }

            craftingRecipeUIs[i].ItemContainer = ItemContainer;
            craftingRecipeUIs[i].CraftingRecipe = CraftingRecipes[i];
        }

        for (int i = CraftingRecipes.Count; i < craftingRecipeUIs.Count; i++)
        {
            craftingRecipeUIs[i].CraftingRecipe = null;
        }
    }


}
