using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
            throw new Exception("Mismatched keys and values count after deserialization.");
        for (int i = 0; i < keys.Count; i++)
            this[keys[i]] = values[i];
    }
}

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> { }

[Serializable]
public class StringAudioDictionary : SerializableDictionary<string, AudioClip> { }

[Serializable]
public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> { }