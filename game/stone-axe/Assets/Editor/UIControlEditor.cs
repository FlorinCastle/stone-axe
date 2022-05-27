using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIControl))]
public class UIControlEditor : Editor
{
    public SerializedProperty
        selectSection,
        // General UI
        mainMenuUI, gameShopUI, optionsPopup, shopUI, marketUI,
        // Shop UI
        currencyText, economicSubUI, disassembleSubUI, craftSubUI, itemCraftingUI, partCraftingUI, inventoryUI, receipeUI, skillTreeUI, questUI, shopSellMenu, shopBuyMenu, miniGameUI, questDetailUI, dialogeUI, toMarketButton, /*buyTabButton, sellTabButton, disassembleTabButton, craftTabButton,*/ itemsScrollView, itemsSortText, partsScrollView, partsSortText, matsScrollView, matsSortText, enchantsScrollView, enchantsSortText,
        // Button Group - Shop UI
        shopUIGroupScript, buyUIButton, sellUIButton, disaUIButton, craftUIButton,
        // Button Group - Market UI
        marketUIGroupScript, mSellUIButton, mQuestUIButton,
        // Button Gropu - Bottom UI
        bottomUIGroupScript, invUIButton, recipesUIButton,
        // Button Group - Inventory UI
        invUIGroupScript, itemsUIButton, partsUIButton, matsUIButton, enchUIButton,
        // Market UI
        marketEconomicSubUI, questSubUI,
        // Main Menu UI
        mainUIElements, creditsUI, creditsText, continueText, continueButton, newGameButton,loadGameButton, settingsButton, creditsButton, loadGameMenu, newGameMenu,
        // Load Game UI
        loadGameUIButton, deleteGameUIButton,
        // New Game UI
        playerName, shopName, startNewGameButton, isNotValidUI, playerSpeciesDropdown, colorMenu, colorMenuParent, colorSamplePrefab;

    private void OnEnable()
    {
        // selector
        selectSection = serializedObject.FindProperty("selectSection");
        // UI
        mainMenuUI = serializedObject.FindProperty("mainMenuUI"); gameShopUI = serializedObject.FindProperty("gameShopUI"); optionsPopup = serializedObject.FindProperty("optionsPopup"); shopUI = serializedObject.FindProperty("shopUI"); marketUI = serializedObject.FindProperty("marketUI");
        // Shop UI
        currencyText = serializedObject.FindProperty("_currencyText"); economicSubUI = serializedObject.FindProperty("economicSubUI"); disassembleSubUI = serializedObject.FindProperty("disassembleSubUI"); craftSubUI = serializedObject.FindProperty("craftSubUI"); itemCraftingUI = serializedObject.FindProperty("itemCraftingUI"); partCraftingUI = serializedObject.FindProperty("partCraftingUI"); inventoryUI = serializedObject.FindProperty("inventoryUI"); receipeUI = serializedObject.FindProperty("receipeUI"); skillTreeUI = serializedObject.FindProperty("skillTreeUI"); questUI = serializedObject.FindProperty("questUI"); shopSellMenu = serializedObject.FindProperty("shopSellMenu"); shopBuyMenu = serializedObject.FindProperty("shopBuyMenu"); miniGameUI = serializedObject.FindProperty("miniGameUI"); questDetailUI = serializedObject.FindProperty("questDetailUI"); dialogeUI = serializedObject.FindProperty("dialogeUI"); toMarketButton = serializedObject.FindProperty("_toMarketButton"); /*buyTabButton = serializedObject.FindProperty("_buyTabButton"); sellTabButton = serializedObject.FindProperty("_sellTabButton"); disassembleTabButton = serializedObject.FindProperty("_disassembleTabButton"); craftTabButton = serializedObject.FindProperty("_craftTabButton");*/ itemsScrollView = serializedObject.FindProperty("itemsScrollView"); itemsSortText = serializedObject.FindProperty("_itemsSortText"); partsScrollView = serializedObject.FindProperty("partsScrollView"); partsSortText = serializedObject.FindProperty("_partsSortText"); matsScrollView = serializedObject.FindProperty("matsScrollView"); matsSortText = serializedObject.FindProperty("_matsSortText"); enchantsScrollView = serializedObject.FindProperty("enchantsScrollView"); enchantsSortText = serializedObject.FindProperty("_enchantsSortText");
        // Button Group - Shop UI
        shopUIGroupScript = serializedObject.FindProperty("_shopUIGroupScript"); buyUIButton = serializedObject.FindProperty("_buyUIButton"); sellUIButton = serializedObject.FindProperty("_sellUIButton"); disaUIButton = serializedObject.FindProperty("_disaUIButton"); craftUIButton = serializedObject.FindProperty("_craftUIButton");
        // Button Group - Market UI
        marketUIGroupScript = serializedObject.FindProperty("_marketUIGroupScript"); mSellUIButton = serializedObject.FindProperty("_mSellUIButton"); mQuestUIButton = serializedObject.FindProperty("_mQuestUIButton");
        // Button Gropu - Bottom UI
        bottomUIGroupScript = serializedObject.FindProperty("_bottomUIGroupScript"); invUIButton = serializedObject.FindProperty("_invUIButton"); recipesUIButton = serializedObject.FindProperty("_recipesUIButton");
        // Button Group - Inventory UI
        invUIGroupScript = serializedObject.FindProperty("_invUIGroupScript"); itemsUIButton = serializedObject.FindProperty("_itemsUIButton"); partsUIButton = serializedObject.FindProperty("_partsUIButton"); matsUIButton = serializedObject.FindProperty("_matsUIButton"); enchUIButton = serializedObject.FindProperty("_enchUIButton");
        // Market UI
        marketEconomicSubUI = serializedObject.FindProperty("marketEconomicSubUI"); questSubUI = serializedObject.FindProperty("questSubUI");
        // Main Menu UI
        mainUIElements = serializedObject.FindProperty("_mainUIElements"); creditsUI = serializedObject.FindProperty("_creditsUI"); creditsText = serializedObject.FindProperty("_creditsText"); continueText = serializedObject.FindProperty("_continueText"); continueButton = serializedObject.FindProperty("_continueButton"); newGameButton = serializedObject.FindProperty("_newGameButton"); loadGameButton = serializedObject.FindProperty("_loadGameButton"); settingsButton = serializedObject.FindProperty("_settingsButton"); creditsButton = serializedObject.FindProperty("_creditsButton"); loadGameMenu = serializedObject.FindProperty("_loadGameMenu"); newGameMenu = serializedObject.FindProperty("_newGameMenu");
        // Load Game UI
        loadGameUIButton = serializedObject.FindProperty("_loadGameUIButton"); deleteGameUIButton = serializedObject.FindProperty("_deleteGameUIButton");
        // New Game UI
        playerName = serializedObject.FindProperty("_playerName"); shopName = serializedObject.FindProperty("_shopName"); startNewGameButton = serializedObject.FindProperty("_startNewGameButton"); isNotValidUI = serializedObject.FindProperty("_isNotValidUI"); playerSpeciesDropdown = serializedObject.FindProperty("_playerSpeciesDropdown"); colorMenu = serializedObject.FindProperty("_colorMenu"); colorMenuParent = serializedObject.FindProperty("_colorMenuParent"); colorSamplePrefab = serializedObject.FindProperty("_colorSamplePrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(selectSection);
        UIControl.dataSection UIC_SelectSection = (UIControl.dataSection)selectSection.enumValueIndex;

        switch(UIC_SelectSection)
        {
            case UIControl.dataSection.UI:
                EditorGUILayout.PropertyField(mainMenuUI);
                EditorGUILayout.PropertyField(gameShopUI);
                EditorGUILayout.PropertyField(optionsPopup);
                EditorGUILayout.PropertyField(shopUI);
                EditorGUILayout.PropertyField(marketUI);
                break;

            case UIControl.dataSection.ShopUI:
                EditorGUILayout.PropertyField(currencyText);
                EditorGUILayout.PropertyField(economicSubUI);
                EditorGUILayout.PropertyField(disassembleSubUI);
                EditorGUILayout.PropertyField(craftSubUI);
                EditorGUILayout.PropertyField(itemCraftingUI);
                EditorGUILayout.PropertyField(partCraftingUI);
                EditorGUILayout.PropertyField(inventoryUI);
                EditorGUILayout.PropertyField(receipeUI);
                EditorGUILayout.PropertyField(skillTreeUI);
                EditorGUILayout.PropertyField(questUI);
                EditorGUILayout.PropertyField(shopSellMenu);
                EditorGUILayout.PropertyField(shopBuyMenu);
                EditorGUILayout.PropertyField(miniGameUI);
                EditorGUILayout.PropertyField(questDetailUI);
                EditorGUILayout.PropertyField(dialogeUI);
                EditorGUILayout.PropertyField(toMarketButton);
                /*EditorGUILayout.PropertyField(buyTabButton);
                EditorGUILayout.PropertyField(sellTabButton);
                EditorGUILayout.PropertyField(disassembleTabButton);
                EditorGUILayout.PropertyField(craftTabButton); */
                EditorGUILayout.PropertyField(itemsScrollView);
                EditorGUILayout.PropertyField(itemsSortText);
                EditorGUILayout.PropertyField(partsScrollView);
                EditorGUILayout.PropertyField(partsSortText);
                EditorGUILayout.PropertyField(matsScrollView);
                EditorGUILayout.PropertyField(matsSortText);
                EditorGUILayout.PropertyField(enchantsScrollView);
                EditorGUILayout.PropertyField(enchantsSortText);
                break;

            case UIControl.dataSection.ButtonGroupShopUI:
                EditorGUILayout.PropertyField(shopUIGroupScript);
                EditorGUILayout.PropertyField(buyUIButton);
                EditorGUILayout.PropertyField(sellUIButton);
                EditorGUILayout.PropertyField(disaUIButton);
                EditorGUILayout.PropertyField(craftUIButton);
                break;

            case UIControl.dataSection.ButtonGroupMarketUI:
                EditorGUILayout.PropertyField(marketUIGroupScript);
                EditorGUILayout.PropertyField(mSellUIButton);
                EditorGUILayout.PropertyField(mQuestUIButton);
                break;

            case UIControl.dataSection.ButtonGroupBottomUI:
                EditorGUILayout.PropertyField(bottomUIGroupScript);
                EditorGUILayout.PropertyField(invUIButton);
                EditorGUILayout.PropertyField(recipesUIButton);
                break;

            case UIControl.dataSection.ButtonGroupInvUI:
                EditorGUILayout.PropertyField(invUIGroupScript);
                EditorGUILayout.PropertyField(itemsUIButton);
                EditorGUILayout.PropertyField(partsUIButton);
                EditorGUILayout.PropertyField(matsUIButton);
                EditorGUILayout.PropertyField(enchUIButton);
                break;

            case UIControl.dataSection.MarketUI:
                EditorGUILayout.PropertyField(marketEconomicSubUI);
                EditorGUILayout.PropertyField(questSubUI);
                break;

            case UIControl.dataSection.MainMenuUI:
                EditorGUILayout.PropertyField(mainUIElements);
                EditorGUILayout.PropertyField(creditsUI);
                EditorGUILayout.PropertyField(creditsText);
                EditorGUILayout.PropertyField(continueText);
                EditorGUILayout.PropertyField(continueButton);
                EditorGUILayout.PropertyField(newGameButton);
                EditorGUILayout.PropertyField(loadGameButton);
                EditorGUILayout.PropertyField(settingsButton);
                EditorGUILayout.PropertyField(creditsButton);
                EditorGUILayout.PropertyField(loadGameMenu);
                EditorGUILayout.PropertyField(newGameMenu);
                break;

            case UIControl.dataSection.LoadGameUI:
                EditorGUILayout.PropertyField(loadGameUIButton);
                EditorGUILayout.PropertyField(deleteGameUIButton);
                break;

            case UIControl.dataSection.NewGameUI:
                EditorGUILayout.PropertyField(playerName);
                EditorGUILayout.PropertyField(shopName);
                EditorGUILayout.PropertyField(startNewGameButton);
                EditorGUILayout.PropertyField(isNotValidUI);
                EditorGUILayout.PropertyField(playerSpeciesDropdown);
                EditorGUILayout.PropertyField(colorMenu);
                EditorGUILayout.PropertyField(colorMenuParent);
                EditorGUILayout.PropertyField(colorSamplePrefab);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
