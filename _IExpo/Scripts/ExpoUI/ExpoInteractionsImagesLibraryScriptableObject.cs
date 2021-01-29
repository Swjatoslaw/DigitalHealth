using _IExpo.Scripts.ExpoUI;
using System.Collections;
using System.Collections.Generic;
using SpaceHub.Conference;
using UnityEngine;
using Cursor = SpaceHub.Conference.Cursor;

namespace _IExpo.Scripts.ExpoUI
{
    [System.Serializable]
    public struct ExpoInteractionsImagesLibraryItem
    {
        public CursorType type;
        public Sprite sprite;
    }

    [CreateAssetMenu(fileName = "ExpoInteractionsImagesLibrary", menuName = "iExpo/ExpoInteractionsImagesLibrary")]
    public class ExpoInteractionsImagesLibraryScriptableObject : ScriptableObject
    {
        public List<ExpoInteractionsImagesLibraryItem> items;


        public Sprite GetSpriteByType(CursorType type)
        {
            return items.Find(x => x.type == type).sprite;
        }
    }
}