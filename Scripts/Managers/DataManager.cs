using Data;
using System.Collections.Generic;
using UnityEngine;

public enum DataType
{

}

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDictionary();
}

public class DataManager : CustomSingleton<DataManager>
{
	public List<PlayerItemRes> items = new();
	public int PlayerId = 0;
	public Stat Stats { get; private set; } = new();
	public Dictionary<int, Rune> Runes { get; private set; } = new();
	public Dictionary<int, Weapon> Weapons { get; private set; } = new();
	public Dictionary<int, FarmingBox> FarmingBoxes { get; private set; } = new();

	private void Awake()
	{
		Stats = JsonUtility.FromJson<Stat>(Resources.Load<TextAsset>($"Data/Stat").text);
		// FarmingBoxes = LoadJson<ILoader<int, FarmingBox>, int, FarmingBox>("FarmingBoxInventory").MakeDictionary();
	}

	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");

		return JsonUtility.FromJson<Loader>(textAsset.text);
	}

	public BaseItem FindItem(int itemId)
	{
		BaseItem item;

		item = Weapons[itemId];
		if (item != null)
		{
			return item;
		}
		item = Runes[itemId];
		if (item != null)
		{
			return item;
		}

		Debug.Log($"ItemID ({itemId}) 는 존재하지 않는 아이템입니다.");
		return null;
	}

	// public T FindItem<T>(int itemId, Dictionary<int, T> dictionary) where T : BaseItem
	// {
	// 	if (dictionary.TryGetValue(itemId, out T item))
	// 	{
	// 		return item;
	// 	}

	// 	Debug.Log($"ItemID ({itemId})는 존재하지 않습니다.");
	// 	return null;
	// }
}