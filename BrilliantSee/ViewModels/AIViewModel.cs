﻿using BrilliantSee.Models;
using BrilliantSee.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BrilliantSee.ViewModels
{
    public partial class AIViewModel : ObservableObject
    {
        public readonly AIService _aiService;
        public readonly DBService _db;
        public readonly MessageService _ms;
        public bool hasModel { get; set; } = false;

        [ObservableProperty]
        public bool _isGettingResult = false;

        [ObservableProperty]
        public ImageSource _audioIcon = ImageSource.FromFile("disable_audio.png");

        public List<SettingItem> modelConfigs { get; set; } = new List<SettingItem>();

        public string AudioStatus { get; set; } = "false";

        public AIViewModel(DBService db, MessageService ms)
        {
            _aiService = MauiProgram.servicesProvider!.GetRequiredService<AIService>();
            _db = db;
            _ms = ms;
            hasModel = _aiService.hasModel;
            _aiService.RemovePlugins();
        }

        private async Task GetAudioStatus()
        {
            var audio = await _db.GetSettingItemsAsync("Audio");
            AudioStatus = audio[0].Value;
        }

        public async Task UpdateModel(string name, string key, string url)
        {
            IsGettingResult = true;
            _aiService.InitKernel(name, key, url);
            modelConfigs = await _db.GetSettingItemsAsync("AIModel");
            foreach (var item in modelConfigs)
            {
                switch (item.Name)
                {
                    case "ModelId":
                        item.Value = name;
                        break;

                    case "ApiKey":
                        item.Value = key;
                        break;

                    case "ApiUrl":
                        item.Value = url;
                        break;
                }
                _ = _db.UpdateSettingItemAsync(item);
            }
            hasModel = true;
            IsGettingResult = false;
            _ms.WriteMessage("模型更新成功");
        }

        public async Task<string> Chat(string prompt)
        {
            IsGettingResult = true;
            var result = string.Empty;
            result = await Task.Run(async Task<string>? () => { return await _aiService.SolvePromptAsync(prompt); });
            IsGettingResult = false;
            return result;
        }

        public bool IsAcceptableString(string[] strs)
        {
            var acceptable = true;
            foreach (var str in strs)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                    acceptable = false;
                    break;
                }
            }
            if (!acceptable)
                _ms.WriteMessage("请填写合理的内容");
            return acceptable;
        }
    }
}