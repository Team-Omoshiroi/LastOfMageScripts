using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	#region FarmingBox
	[Serializable]
	public class FarmingBoxItem
	{
		public int itemId;
		public int quantity;
	}

	public class FarmingBox
	{
		public int farmingBoxId;
		public List<FarmingBoxItem> farmingBoxItems;
	}

	public class FarmingBoxData : ILoader<int, FarmingBox>
	{
		public List<FarmingBox> farmingBoxes = new();

		public Dictionary<int, FarmingBox> MakeDictionary()
		{
			Dictionary<int, FarmingBox> farmingBoxDictionary = new();
			foreach (FarmingBox farmingBox in farmingBoxes)
			{
				farmingBoxDictionary.Add(farmingBox.farmingBoxId, farmingBox);
			}
			return farmingBoxDictionary;
		}
	}
	#endregion

	#region ItemData
	#region Weapon
	[Serializable]
	public class Weapon : BaseWeapon { }

	[Serializable]
	public class WeaponData : ILoader<int, Weapon>
	{
		public List<Weapon> weapons = new();

		public Dictionary<int, Weapon> MakeDictionary()
		{
			Dictionary<int, Weapon> weaponDictionary = new();
			foreach (Weapon weapon in weapons)
				weaponDictionary.Add(weapon.ItemID, weapon);
			return weaponDictionary;
		}
	}
	#endregion

	#region Rune
	[Serializable]
	public class Rune : BaseRune { }

	[Serializable]
	public class RuneData : ILoader<int, Rune>
	{
		public List<Rune> runes = new();

		public Dictionary<int, Rune> MakeDictionary()
		{
			Dictionary<int, Rune> runeDictionary = new();
			foreach (Rune rune in runes)
				runeDictionary.Add(rune.ItemID, rune);
			return runeDictionary;
		}
	}
	#endregion
	#endregion

	#region StatData
	[Serializable]
	public class Stat
	{
		public int StatID;
		public int Level;
		public int MaxHp;
		public int Def;
		public int Atk;
		public float AtkSpeed;
		public int CritRate;
		public float CritDamage;
		public float MoveSpeed;
		public float RunMultiplier;
		public float DodgeTime;
	}
	#endregion
}
