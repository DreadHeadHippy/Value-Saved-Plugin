using Playnite.SDK;
using Playnite.SDK.Plugins;
using Playnite.SDK.Data;
using Playnite.SDK.Models;
using Playnite.SDK.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace ValueSavedPlugin
{
    public class ValueSavedPlugin : GenericPlugin
    {
        private Dictionary<Guid, double> gamePrices;
        private string PricesFilePath => Path.Combine(PlayniteApi.Paths.ExtensionsDataPath, "ValueSavedPlugin", "prices.json");
        private ValueSavedSettings settings;

        public ValueSavedPlugin(IPlayniteAPI api) : base(api)
        {
            gamePrices = LoadPrices();
            settings = LoadPluginSettings<ValueSavedSettings>() ?? new ValueSavedSettings(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            // Clean up orphaned prices for deleted games after database is loaded
            var existingGameIds = new HashSet<Guid>(PlayniteApi.Database.Games.Select(g => g.Id));
            var orphanedKeys = gamePrices.Keys.Where(id => !existingGameIds.Contains(id)).ToList();
            foreach (var key in orphanedKeys)
            {
                gamePrices.Remove(key);
            }
            if (orphanedKeys.Any())
            {
                SavePrices();
            }
        }    private Dictionary<Guid, double> LoadPrices()
    {
        try
        {
            if (File.Exists(PricesFilePath))
            {
                var json = File.ReadAllText(PricesFilePath);
                return Serialization.FromJson<Dictionary<Guid, double>>(json) ?? new Dictionary<Guid, double>();
            }
        }
        catch { }
        return new Dictionary<Guid, double>();
    }

    private void SavePrices()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PricesFilePath));
            var json = Serialization.ToJson(gamePrices);
            File.WriteAllText(PricesFilePath, json);
        }
        catch { }
    }

    public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
    {
        return new List<MainMenuItem>
        {
            new MainMenuItem
            {
                MenuSection = "@Value Saved",
                Description = "Show Total Value Saved",
                Action = (menuItem) => PlayniteApi.Dialogs.ShowMessage("Total Value Saved: $" + GetTotalSaved().ToString("N2"))
            },
            new MainMenuItem
            {
                MenuSection = "@Value Saved",
                Description = "Settings",
                Action = (menuItem) => PlayniteApi.MainView.OpenPluginSettings(Id)
            }
        };
    }

    public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
    {
        return new List<GameMenuItem>
        {
            new GameMenuItem
            {
                MenuSection = "Value Saved",
                Description = "Set Original Price",
                Action = (gameMenuItem) => SetPriceForGame(args.Games.First())
            }
        };
    }

    private void SetPriceForGame(Game game)
    {
        string currentPrice = gamePrices.ContainsKey(game.Id) ? gamePrices[game.Id].ToString("0.00") : "";
        var input = PlayniteApi.Dialogs.SelectString("Enter the original price for " + game.Name, "Original Price", currentPrice);
        if (input.Result && double.TryParse(input.SelectedString, out double price))
        {
            gamePrices[game.Id] = price;
            SavePrices();
            PlayniteApi.Dialogs.ShowMessage("Price set to $" + price.ToString("N2"));
        }
        else if (input.Result == false && !string.IsNullOrEmpty(input.SelectedString))
        {
            PlayniteApi.Dialogs.ShowMessage("Invalid price entered.");
        }
    }

    private void ShowTotalSaved()
    {
        double total = gamePrices.Values.Sum();
        PlayniteApi.Dialogs.ShowMessage("Total Value Saved: $" + total.ToString("0.00"));
    }

        public double GetTotalSaved() => gamePrices.Values.Sum() * (1 + settings.TaxRate / 100);    public override IEnumerable<TopPanelItem> GetTopPanelItems()
    {
        return new List<TopPanelItem>
        {
            new TopPanelItem
            {
                Title = "Total Saved: $" + GetTotalSaved().ToString("N2"),
                Icon = @"C:\Users\Dakot\AppData\Local\Playnite\Extensions\icon.png",
                Activated = () => PlayniteApi.Dialogs.ShowMessage("Total Value Saved: $" + GetTotalSaved().ToString("N2"))
            }
        };
    }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunView)
        {
            return new ValueSavedSettingsView();
        }

        public override Guid Id => Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1");
    }
}
