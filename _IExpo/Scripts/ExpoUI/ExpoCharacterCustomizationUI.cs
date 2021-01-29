using _IExpo.Scripts.ExpoCharacter;
using _IExpo.Scripts.ExpoUtils;
using Michsky.UI.ModernUIPack;
using SpaceHub.Conference;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _IExpo.Scripts.ExpoUI
{
    public class ExpoCharacterCustomizationUI : MonoBehaviour
    {
        [SerializeField] private SwitchManager _genderSwitch;

        [SerializeField] private GradientSlider _skinColorSlider;
        [SerializeField] private GradientSlider _hairColorSlider;
        [SerializeField] private GradientSlider _clothColorSlider;

        [Header("GenderObjects")]
        
        [SerializeField]
        private GameObject _manHairs;

        [SerializeField] private GameObject _womanHairs;

        [Header("Characters")]
        
        [SerializeField]
        private GenderCharacter[] _characters;

        private ExpoCharacterCustomizationController _currentCharacter;

        private PlayerCustomizationLocalInfo _customizationLocalInfo;

        private void Start()
        {
            _customizationLocalInfo = PlayerCustomizationLocalInfo.LoadFromFile();

            if (_customizationLocalInfo == null || !_customizationLocalInfo.Custom)
            {
                if (_customizationLocalInfo == null)
                {
                    _customizationLocalInfo = new PlayerCustomizationLocalInfo();
                }

                // Randomize
                _customizationLocalInfo.Custom = true;

                _customizationLocalInfo.Gender = Random.Range(0, 2);

                if (_customizationLocalInfo.Gender == (int) Gender.Male)
                {
                    _customizationLocalInfo.HairIndex =
                        Random.Range(0, _manHairs.GetComponentsInChildren<Button>(true).Length);
                }
                else
                {
                    _customizationLocalInfo.HairIndex =
                        Random.Range(0, _womanHairs.GetComponentsInChildren<Button>(true).Length);
                }

                _customizationLocalInfo.SkinColorSliderValue = (float) Random.Range(0f, 1f);
                _customizationLocalInfo.HairColorSliderValue = (float) Random.Range(0f, 1f);
                _customizationLocalInfo.ClothColorSliderValue = (float) Random.Range(0f, 1f);
            }

            // Apply Customization
            if (_customizationLocalInfo.Gender == (int) Gender.Female)
            {
                _genderSwitch.isOn = false;
                //Changes to opposite value and makes isOn == true
                _genderSwitch.AnimateSwitch();
            }
            else
            {
                _genderSwitch.isOn = true;
                //Changes to opposite value and makes isOn == false
                _genderSwitch.AnimateSwitch();
            }

            SetHairIndex(_customizationLocalInfo.HairIndex);
            _skinColorSlider.SetSliderValue(_customizationLocalInfo.SkinColorSliderValue);
            _hairColorSlider.SetSliderValue(_customizationLocalInfo.HairColorSliderValue);
            _clothColorSlider.SetSliderValue(_customizationLocalInfo.ClothColorSliderValue);

            OnSkinColorChange();
            OnHairColorChange();
            OnClothColorChange();
        }

        public void SetGender(int genderIndex)
        {
            Gender gender = (Gender) genderIndex;

            foreach (GenderCharacter character in _characters)
            {
                character.DisableCharacter();
            }

            if (gender == Gender.Male)
            {
                _manHairs.SetActive(true);
                _womanHairs.SetActive(false);
            }
            else
            {
                _manHairs.SetActive(false);
                _womanHairs.SetActive(true);
            }

            LocalPlayerData.CharacterData.Gender = gender;

            var currentGender = _characters.FirstOrDefault(a => a.Gender == gender);
            if (currentGender != null)
            {
                currentGender.EnableCharacter();
                _currentCharacter = currentGender.GameObject.GetComponent<ExpoCharacterCustomizationController>();
            }

            OnSkinColorChange();
            OnHairColorChange();
            OnClothColorChange();
            SetHairIndex(0);
        }

        public void SetHairIndex(int index)
        {
            if (_currentCharacter != null)
            {
                _currentCharacter.SetCurrentHair(index);
            }
            LocalPlayerData.CharacterData.HairStyle = index;
        }

        public void OnJoinClick()
        {
            JoinExpo();
        }

        private void JoinExpo()
        {
            List<Tuple<string, int>> halls = new List<Tuple<string, int>>();

            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.DigitalAutomation), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Energy), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.ExtractionProcessing), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Government), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.HSSE), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Partners), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.Telecommunication), 0));
            halls.Add(new Tuple<string, int>(ExpoSceneNames.GetNameFromEnum(ExpoSceneNames.MapType.TransportationStorage), 0));
            
            Tuple<string, int> hall = halls[UnityEngine.Random.Range(1, halls.Count)];

            string newSceneName = hall.Item1;
            int floorId = hall.Item2;

            string spawnPoint = "";

            ConferenceRoomManager.LoadRoom(newSceneName, (byte) floorId, spawnPoint, null);
        }

        public void OnSkinColorChange()
        {
            if (_currentCharacter != null)
            {
                _currentCharacter.SetSkinColor(_skinColorSlider.GetCurrentColor());
            }

            LocalPlayerData.CharacterData.SetColor(ExpoCharacterColorCategoryType.SkinColor,
                _skinColorSlider.GetCurrentColor());
        }

        public void OnHairColorChange()
        {
            if (_currentCharacter != null)
            {
                _currentCharacter.SetHairColor(_hairColorSlider.GetCurrentColor());
            }

            LocalPlayerData.CharacterData.SetColor(ExpoCharacterColorCategoryType.HairColor,
                _hairColorSlider.GetCurrentColor());
        }

        public void OnClothColorChange()
        {
            if (_currentCharacter != null)
            {
                _currentCharacter.SetClothColor(_clothColorSlider.GetCurrentColor());
            }

            LocalPlayerData.CharacterData.SetColor(ExpoCharacterColorCategoryType.ClothColor,
                _clothColorSlider.GetCurrentColor());
        }

        private void OnDisable()
        {
        }

        public void Save()
        {
            _customizationLocalInfo.Custom = true;

            _customizationLocalInfo.Gender = (int) LocalPlayerData.CharacterData.Gender;
            _customizationLocalInfo.HairIndex = LocalPlayerData.CharacterData.HairStyle;

            _customizationLocalInfo.SkinColor = _skinColorSlider.GetCurrentColor();
            _customizationLocalInfo.SkinColorSliderValue = _skinColorSlider.GetSliderValue();

            _customizationLocalInfo.HairColor = _hairColorSlider.GetCurrentColor();
            _customizationLocalInfo.HairColorSliderValue = _hairColorSlider.GetSliderValue();

            _customizationLocalInfo.ClothColor = _clothColorSlider.GetCurrentColor();
            _customizationLocalInfo.ClothColorSliderValue = _clothColorSlider.GetSliderValue();

            PlayerCustomizationLocalInfo.Save(_customizationLocalInfo);
        }
    }

    [Serializable]
    public class PlayerCustomizationLocalInfo
    {
        public bool Custom = false;
        public int Gender = 0;
        public int HairIndex = 0;
        public Color SkinColor;
        public float SkinColorSliderValue;
        public Color HairColor;
        public float HairColorSliderValue;
        public Color ClothColor;
        public float ClothColorSliderValue;

        private static string SavePath => Path.Combine(Application.persistentDataPath, "CustomizationInfo.json");

        public static PlayerCustomizationLocalInfo LoadFromFile()
        {
            try
            {
                string info = File.ReadAllText(SavePath);

                PlayerCustomizationLocalInfo file = JsonUtility.FromJson<PlayerCustomizationLocalInfo>(info);

                return file;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception reading {SavePath}: Cause {e}");
                return new PlayerCustomizationLocalInfo();
            }
        }

        public static void Save(PlayerCustomizationLocalInfo saveData)
        {
            saveData.Custom = true;

            try
            {
                string saveJson = JsonUtility.ToJson(saveData);

                File.WriteAllText(SavePath, saveJson);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Exception while saving to {SavePath}: Cause {e}");
            }
        }
    }
}