﻿using DiabloII.Items.Reader.Extensions;
using DiabloII.Items.Reader.Records;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiabloII.Items.Reader
{
    public class DiabloIIFilesReader
    {
        private List<string> MissingItemTypes = new List<String>();

		// TODO : 
		// Les types h2h et 2h2 ne sont pas mapper, il semble manquer des Claw / Druid helm / barb helm/ necor shield / etc..
		// Afficher tous les attributs en distinct et les convertir un nom verbeux
		// Récupérer les stats requis par niveau (il faudra aussi probablement les recaculer avec les attributs).
		// La partie avec  weaponSubCategoriesRecord.AddRange(new[]) : ne contient pas encore l'armure et les dommages et les stats requis, attack speed
		// Recalculer l'attack speed.
		// Recacluler : attack speed, dommage une à deux mains, defence, stats requis ?
		// Utiliser le nouveau document properties de Ascended pour calculer "IsPercent" et "Description" des attributs.
		public IEnumerable<Item> Read(
            string uniquesCsv,
            string weaponsCsv,
            string armorsCsv)
        {
            var weapons = ReadWeapons(weaponsCsv);
            var armors = ReadArmors(armorsCsv);
            var subCategories = ReadSubCategories(weapons, armors);
            var uniques = ReadUniques(uniquesCsv, subCategories);

            // For sanitize purpoeses and comparaison :
            var sub = string.Join("\n- ", subCategories.Select(s => s.Name).OrderBy(x => x));
            var uni = string.Join("\n- ", MissingItemTypes.Distinct().OrderBy(x => x));
            var subCategoriesEnums = string.Join(",\n", subCategories.Select(s => s.SubCategory.Replace(" ", "_")).OrderBy(x => x).Distinct());
			var allProperties = string.Join(Environment.NewLine, uniques.SelectMany(_ => _.Properties).Select(_ => _.Name).Distinct().ToList());

			return uniques;
        }

        public IEnumerable<Item> ReadUniques(string uniquesCsv, List<ItemCategoryRecord> itemCategories)
            => uniquesCsv
                .Split('\n')
                .Skip(1)
                .Select(line =>
                {
                    var itemData = line.Split(';');

                    if (itemData.Length < 4)
                        return null;

                    var properties = new List<ItemProperty>();
                    var name = itemData[0];
                    var type = itemData[3]
                        .ToTitleCase()
                        .Replace("Ancientarmor", "Ancient Armor")
                        .Replace("Battle Guantlets", "Battle Gauntlets")
                        .Replace("Cedarbow", "Cedar Bow")
                        .Replace("Hunter’S Bow", "Hunter’s Bow")
                        .Replace("Jo Stalf", "Jo Staff")
                        .Replace("2-Handed Sword", "Two-Handed Sword")
                        .Replace("Tresllised Armor", "Trellised Armor");
                    var itemCategory = itemCategories.FirstOrDefault(x => x.Name == type);

                    if (itemCategory == null)
                    {
                        MissingItemTypes.Add(type);
                        return null;
                    }

                    for (var index = 4; index < itemData.Length; index += 4)
                    {
                        if (string.IsNullOrEmpty(itemData[index]))
                            continue;

                        properties.Add(new ItemProperty
                        {
                            Name = itemData[index],
                            Par = itemData[index + 1].ParseIntOrDefault(),
                            Minimum = itemData[index + 2].ParseIntOrDefault(),
                            Maximum = itemData[index + 3].ParseIntOrDefault(),
                            IsPercent = itemData[index].Contains("%")
                        });
                    }

					var minDamage = GetPropertyValueOrDefault(properties, "dmg-min");
					var maxDamage = GetPropertyValueOrDefault(properties, "dmg-max");
					var damagePercentMinimum = GetPropertyValueOrDefault(properties, "Damage %") + 100;
					var damagePercentMaximum = GetPropertyValueOrDefault(properties, "Damage %", true) + 100;

					return new Item
                    {
                        Name = name,
                        LevelRequired = itemData[2].ParseIntOrDefault(),
                        Level = itemData[1].ParseIntOrDefault(),
                        Quality = "Unique",
                        Properties = properties,
                        Category = itemCategory?.Category,
                        SubCategory = itemCategory?.SubCategory,
						Type = type,
						// Specific to Armor :
						MinimumDefense = itemCategory?.MinimumDefense,
						MaximumDefense = itemCategory?.MaximumDefense,
						// Specific to Weapon :
						MinimumOneHandedDamageMinimum = (itemCategory?.MinimumOneHandedDamage * damagePercentMinimum) / 100 + minDamage,
						MaximumOneHandedDamageMinimum = (itemCategory?.MaximumOneHandedDamage * damagePercentMinimum) / 100 + maxDamage,
						MinimumTwoHandedDamageMinimum = (itemCategory?.MinimumTwoHandedDamage * damagePercentMinimum) / 100 + minDamage,
						MaximumTwoHandedDamageMinimum = (itemCategory?.MaximumTwoHandedDamage * damagePercentMinimum) / 100 + maxDamage,
						MinimumOneHandedDamageMaximum = (itemCategory?.MinimumOneHandedDamage * damagePercentMaximum) / 100 + minDamage,
						MaximumOneHandedDamageMaximum = (itemCategory?.MaximumOneHandedDamage * damagePercentMaximum) / 100 + maxDamage,
						MinimumTwoHandedDamageMaximum = (itemCategory?.MinimumTwoHandedDamage * damagePercentMaximum) / 100 + minDamage,
						MaximumTwoHandedDamageMaximum = (itemCategory?.MaximumTwoHandedDamage * damagePercentMaximum) / 100 + maxDamage,
						AttackSpeed = itemCategory?.AttackSpeed,
						// Stats
						StrengthRequired = itemCategory?.StrengthRequired,
						DexterityRequired = itemCategory?.DexterityRequired,
					};
                })
                .Where(item => item != null)
                .ToList();

        private List<ArmorReord> ReadArmors(string armorsCsv)
            => armorsCsv
                .Split('\n')
                .Skip(1)
                .Select(line =>
                {
                    var itemData = line.Split(';');

                    if (itemData.Length < 2)
                        return null;

                    return new ArmorReord
                    {
                        Name = itemData[0],
                        Slot = itemData[1],
						MinimumDefense = itemData[2].ParseIntOrDefault(),
						MaximumDefense = itemData[3].ParseIntOrDefault(),
						StrengthRequired = itemData[4].ParseIntOrDefault()
                    };
                })
                .Where(item => item != null)
                .ToList();

        private List<WeaponRecord> ReadWeapons(string weaponsCsv) 
            => weaponsCsv
                .Split('\n')
                .Skip(1)
                .Select(line =>
                {
                    var itemData = line.Split(';');

                    if (itemData.Length < 3)
                        return null;

					return new WeaponRecord
					{
						Name = itemData[0],
						Type = itemData[1],
						Slot = itemData[2].Replace("\r", string.Empty),
						MinimumOneHandedDamage = itemData[3].ParseIntOrDefault(),
						MaximumOneHandedDamage = itemData[4].ParseIntOrDefault(),
						MinimumTwoHandedDamage = itemData[5].ParseIntOrDefault(),
						MaximumTwoHandedDamage = itemData[6].ParseIntOrDefault(),
						StrengthRequired = itemData[7].ParseIntOrDefault(),
						DexterityRequired = itemData[8].ParseIntOrDefault(),
						AttackSpeed = itemData[9].ParseIntOrDefault(),
					};
				})
                .Where(item => item != null)
                .ToList();

        private List<ItemCategoryRecord> ReadSubCategories(
            List<WeaponRecord> weapons,
            List<ArmorReord> armors)
        {
            var armorSubCategoriesRecord = armors
                .Select(armor => new ItemCategoryRecord
                {
                    Name = armor.Name
                        .Replace("\r", string.Empty)
                            .Replace("(M)", "Medium")
                            .Replace("(H)", "Heavy")
                            .Replace("(L)", "Light")
                            .Replace("Hard Leather Armor", "Hard Leather")
                            .Replace("Gaunlets", "Gauntlets")
                            .Replace("Cap/hat", "Cap")
                            .Replace("Skull  Guard", "Skull Guard"),
                    SubCategory = armor.Slot.ToTitleCase(),
                    Category = "Armor",
					MinimumDefense = armor.MinimumDefense,
					MaximumDefense = armor.MaximumDefense,
					StrengthRequired = armor.StrengthRequired,
                })
                .Where(record => record.SubCategory != string.Empty)
                .ToList();

			var weaponSubCategoriesRecord = weapons
				.Where(weapon => !string.IsNullOrWhiteSpace(weapon.Type))
				.Select(weapon => new ItemCategoryRecord
				{
					Name = weapon.Name
						.Replace("Bec-de-Corbin", "Bec-De-Corbin")
						.Replace("Kriss", "Kris")
						.Replace("Martel de Fer", "Martel De Fer")
						.Replace("Blood Spirt", "Blood Spirit")
						.Replace("Hunter's Bow", "Hunter’s Bow")
						.Replace("MatriarchalJavelin", "Matriarchal Javelin"),
					Category = "Weapon",
					SubCategory =
						(weapon.Slot == "1h" ? weapon.Type :
						 weapon.Slot == "2h" ? $"Two Handed {weapon.Type}" :
						 $"Two And One Handed {weapon.Type}")
							.ToTitleCase()
							.Replace("Scep", "Scepter")
							.Replace("Hamm", "Hammer")
							.Replace("Swor", "Sword")
							.Replace("Knif", "Knife")
							.Replace("Jave", "Javelin")
							.Replace("Jave", "Jave")
							.Replace("Spea", "Spear")
							.Replace("Pole", "Polearm")
							.Replace("Staf", "Staff")
							.Replace("Xbow", "Crossbow")
							.Replace("Tpot", "Throwing Potions")
							.Replace("Taxe", "Throwing Axe")
							.Replace("Tkni", "Thorwing knife")
							.Replace("Abow", "Amazon bow")
							.Replace("Aspe", "Amazon spear")
							.Replace("Ajav", "Amazon Javelin")
							.Replace("H2h2", "Hand To Hand Two Handed")
							.Replace("H2h", "Hand To Hand")
							.ToTitleCase(),
					MinimumOneHandedDamage = weapon.MinimumOneHandedDamage,
					MaximumOneHandedDamage = weapon.MaximumOneHandedDamage,
					MinimumTwoHandedDamage = weapon.MinimumTwoHandedDamage,
					MaximumTwoHandedDamage = weapon.MaximumTwoHandedDamage,
					AttackSpeed = weapon.AttackSpeed,
					StrengthRequired = weapon.StrengthRequired,
					DexterityRequired = weapon.DexterityRequired
				})
                .ToList();

            weaponSubCategoriesRecord.AddRange(armorSubCategoriesRecord);
            weaponSubCategoriesRecord.AddRange(new[]
            {
				// Manque l'armure et les dommages
                    new ItemCategoryRecord { Name = "Silver-Edged Axe", Category = "Weapon", SubCategory = "Two Handed Axe"},
                    new ItemCategoryRecord { Name = "Amulet", Category = "Jewelry", SubCategory = "Amulet"},
                    new ItemCategoryRecord { Name = "Arrows", Category = "Armor", SubCategory = "Arrows"},
                    new ItemCategoryRecord { Name = "Bolts", Category = "Armor", SubCategory = "Bolts"},
                    new ItemCategoryRecord { Name = "Charm", Category = "Charm", SubCategory = "Charm"},
                    new ItemCategoryRecord { Name = "Hammer", Category = "Weapon", SubCategory = "Hammer"},
                    new ItemCategoryRecord { Name = "Jewel", Category = "Jewelry", SubCategory = "Jewel"},
                    new ItemCategoryRecord { Name = "Ring", Category = "Jewelry", SubCategory = "Ring"},
                    new ItemCategoryRecord { Name = "Staff", Category = "Weapon", SubCategory = "Staff"},
                    new ItemCategoryRecord { Name = "Conqueror Crown", Category = "Armor", SubCategory = "Barbarian Helm"},
                    new ItemCategoryRecord { Name = "Blood Spirit", Category = "Armor", SubCategory = "Druid Helm"},
                    new ItemCategoryRecord { Name = "Bracers", Category = "Armor", SubCategory = "Hands"},
                    new ItemCategoryRecord { Name = "Gloves", Category = "Armor", SubCategory = "Hands"},
                    new ItemCategoryRecord { Name = "Belt", Category = "Armor", SubCategory = "Waist"},
                    new ItemCategoryRecord { Name = "Sash", Category = "Armor", SubCategory = "Waist"},
                    new ItemCategoryRecord { Name = "Girdle", Category = "Armor", SubCategory = "Waist"},
                    new ItemCategoryRecord { Name = "Gauntlets", Category = "Armor", SubCategory = "Hands"},
            });

            return weaponSubCategoriesRecord;
        }

		private static int GetPropertyValueOrDefault(List<ItemProperty> properties, string name, bool maximum = false)
		{
			var property = properties.FirstOrDefault(_ => _.Name == name);

			return property == null ? 0 : maximum ? property.Maximum : property.Minimum;
		}
	}
}