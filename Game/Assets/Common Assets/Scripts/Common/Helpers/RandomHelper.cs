using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class RandomHelper
{
    public static System.Random random = new System.Random();

    private static char[] numbers = new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    private static char[] alphabet = new char[26] { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };

    public static KeyCode GetRandomKeyCode(bool includeNums, bool includeAlphabet)
    {
        List<char> options = new List<char>();
        if (includeNums)
        {
            options.AddRange(numbers);
        }
        if (includeAlphabet)
        {
            options.AddRange(alphabet);
        }
        return GetKeyCodeFromOptions(options);
    }

    public static Vector3 RandomVector3(float v1, float v2, float v3, float v4, float v5, float v6)
    {
        return new Vector3(RandomFloat(v1, v2), RandomFloat(v3, v4), RandomFloat(v5, v6));
    }

    private static readonly Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

    public static KeyCode GetKeyCodeFromOptions(List<char> options)
    {
        int r = random.Next(options.Count);
        char character = options[r];
        // Get from cache if it was taken before to prevent unnecessary enum parse
        KeyCode code;
        if (_keycodeCache.TryGetValue(character, out code)) return code;
        // Cast to it's integer value
        int alphaValue = character;
        code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
        _keycodeCache.Add(character, code);
        return code;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static float RandomFloat(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float RandomFloat(Vector2 minMax)
    {
        return UnityEngine.Random.Range(minMax.x, minMax.y);
    }

    public static int RandomIntInclusive(float min, float max)
    {
        return random.Next((int)min, (int)max + 1);
    }

    public static int RandomIntExclusive(float min, float max)
    {
        return random.Next((int)min, (int)max);
    }

    public static int RandomIntInclusive(Vector2 minMax)
    {
        return random.Next((int)minMax.x, (int)minMax.y + 1);
    }

    public static int RandomIntExclusive(Vector2 minMax)
    {
        return random.Next((int)minMax.x, (int)minMax.y);
    }

    public static T GetRandomFromList<T>(List<T> list)
    {
        return list[random.Next(0, list.Count)];
    }

    public static bool RandomBool()
    {
        return UnityEngine.Random.value <= 0.5f;
    }

    public static bool RandomBool(float chance)
    {
        return UnityEngine.Random.value <= chance;
    }
}