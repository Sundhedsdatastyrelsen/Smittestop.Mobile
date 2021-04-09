using System.Collections.Generic;
using I18NPortable;
using NDB.Covid19.Configuration;
using NDB.Covid19.Enums;
using NDB.Covid19.Models;

namespace NDB.Covid19.ViewModels
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            SettingItemList = new List<SettingItem>
            {
                new SettingItem(SettingItemType.Intro),
                new SettingItem(SettingItemType.HowItWorks),
                new SettingItem(SettingItemType.Consent),
                new SettingItem(SettingItemType.Help),
                new SettingItem(SettingItemType.About),
                new SettingItem(SettingItemType.Settings)
            };

            if (Conf.UseDeveloperTools)
            {
                SettingItemList.Add(new SettingItem(SettingItemType.Debug));
            }
        }

        public static string SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON =>
            "SETTINGS_ITEM_ACCESSIBILITY_CLOSE_BUTTON".Translate();

        public static string SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON =>
            "SETTINGS_CHILD_PAGE_ACCESSIBILITY_BACK_BUTTON".Translate();

        public bool ShowDebugItem => Conf.UseDeveloperTools;

        public List<SettingItem> SettingItemList { get; }
    }
}