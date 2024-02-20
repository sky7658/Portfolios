using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Utility
{
    public class ParticleUtil
    {
        public static Color ConvertColorHtoC(string colorName)
        {
            Color _returnColor;
            ColorUtility.TryParseHtmlString(colorName, out _returnColor);
            return _returnColor;
        }

        public static void SetParticleColor(ParticleSystem particleSystem, Color32 c1, Color32 c2)
        {
            ParticleSystem.MainModule _mainM = particleSystem.main;
            ParticleSystem.MinMaxGradient _startColor = _mainM.startColor;
            _startColor = new ParticleSystem.MinMaxGradient(c2, c1);

            _mainM.startColor = _startColor;
        }

        public static void SetParticleLoop(ParticleSystem particleSystem, bool active)
        {
            var _mainM = particleSystem.main;
            _mainM.loop = active;
        }

        public static void InitParticleColor(List<ParticleSystem> particleSystems, string particleName, Cards.CardProperty property = Cards.CardProperty.FIRE)
        {
            if(property == Cards.CardProperty.NONE)
            {
                Debug.Log("카드 속성에 문제가 있습니다.");
                return;
            }
            for(int index = 0; index < particleSystems.Count; index++)
            {
                string _searchColorName = particleName + property.ToString() + (index + 1).ToString();
                List<Color32> _list = ColorInfo.particleColor[_searchColorName];
                SetParticleColor(particleSystems[index], _list[0], _list[1]);
            }
        }

        public static IEnumerator ReturnParticle<T>(ParticleSystem particleSystem, GameObject obj, ObjectInfo info, T returnObj, string name)
        {
            while(particleSystem.isPlaying)
            {
                yield return null;
            }

            ObjectPool.Instance.ReturnObject(returnObj, name);
            UtilFunction.TurnOnOff(info, obj);
            yield break;
        }
    }

    public class ColorInfo
    {
        public static Dictionary<string, List<Color32>> particleColor = new Dictionary<string, List<Color32>>()
        {
            {"EffectFIRE1", new List<Color32>() { new Color32(255, 93, 34, 112), new Color32(255, 37, 0, 112) } },
            {"EffectFIRE2", new List<Color32>() { new Color32(240, 158, 95, 8), new Color32(208, 56, 0, 8) } },
            {"EffectICE1", new List<Color32>() { new Color32(0, 189, 255, 112), new Color32(10, 0, 255, 112) } },
            {"EffectICE2", new List<Color32>() { new Color32(0, 250, 255, 8), new Color32(0, 105, 208, 8) } },
            {"EffectPOISON1", new List<Color32>() { new Color32(93, 255, 34, 112), new Color32(37, 255, 0, 112) } },
            {"EffectPOISON2", new List<Color32>() { new Color32(158, 240, 95, 8), new Color32(56, 208, 0, 8) } },
            {"FloorFIRE1", new List<Color32>() { new Color32(255, 223, 154, 4), new Color32(255, 106, 79, 5) } },
            {"FloorFIRE2", new List<Color32>() { new Color32(255, 223, 154, 21), new Color32(255, 106, 79, 9) } },
            {"FloorFIRE3", new List<Color32>() { new Color32(255, 223, 154, 60), new Color32(255, 106, 79, 45) } },
            {"FloorFIRE4", new List<Color32>() { new Color32(255, 197, 109, 30), new Color32(255, 42, 0, 25) } },
            {"FloorICE1", new List<Color32>() { new Color32(0, 57, 255, 4), new Color32(0, 24, 255, 5) } },
            {"FloorICE2", new List<Color32>() { new Color32(0, 0, 255, 99), new Color32(0, 6, 255, 40) } },
            {"FloorICE3", new List<Color32>() { new Color32(0, 0, 255, 255), new Color32(0, 58, 255, 255) } },
            {"FloorICE4", new List<Color32>() { new Color32(109, 197, 255, 30), new Color32(0, 42, 255, 25) } },
            {"FloorPOISON1", new List<Color32>() { new Color32(0, 223, 163, 4), new Color32(0, 255, 161, 5) } },
            {"FloorPOISON2", new List<Color32>() { new Color32(0, 255, 92, 21), new Color32(106, 255, 79, 9) } },
            {"FloorPOISON3", new List<Color32>() { new Color32(0, 255, 0, 255), new Color32(39, 255, 195, 255) } },
            {"FloorPOISON4", new List<Color32>() { new Color32(87, 255, 0, 30), new Color32(42, 255, 0, 25) } }
        };
    }
}

