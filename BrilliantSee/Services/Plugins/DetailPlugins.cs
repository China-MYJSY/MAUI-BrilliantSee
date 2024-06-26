﻿using BrilliantSee.Models.Items;
using BrilliantSee.Models.Objs;
using CommunityToolkit.Maui.Alerts;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrilliantSee.Services.Plugins
{
    public sealed class DetailPlugins
    {
        public DBService _db;

        public DetailPlugins(DBService db)
        {
            _db = db;
        }

        //打开功能

        [KernelFunction, Description("打开指定的章节")]
        public async Task OpenChapterAsync(
                       [Description("要打开的章节")] Item chapter)
        {
            await Shell.Current.GoToAsync("BrowsePage", new Dictionary<string, object> { { "Chapter", chapter } });
        }

        //返回功能

        [KernelFunction, Description("返回或返回某个页面,无需关注指定哪个页面，只要涉及返回都可调用")]
        public async Task GoBackAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() => { Shell.Current.SendBackButtonPressed(); });
        }

        //查找功能

        [KernelFunction, Description("查找指定名字的章节")]
        [return: Description("获取到的章节（可能没有）")]
        public Item? FindChapterAsync(
            [Description("用于查找的漫画")] Obj comic,
                         [Description("要查找的章节名称")] string name)
        {
            var chapter = comic.Items.Where(c => c.Name.Contains(name)).FirstOrDefault();
            return chapter;
        }

        [KernelFunction, Description("查找第一个章节")]
        [return: Description("获取到的章节（可能没有）")]
        public Item? GetFirstChapterAsync(
                                             [Description("用于查找漫画")] Obj comic)
        {
            if (comic.IsReverseList == true) return comic.Items.LastOrDefault();
            else return comic.Items.FirstOrDefault();
        }

        [KernelFunction, Description("查找最后一个或最新章节")]
        [return: Description("获取到的章节（可能没有）")]
        public Item? GetLastChapterAsync(
                                                        [Description("用于查找的漫画")] Obj comic)
        {
            if (comic.IsReverseList == true) return comic.Items.FirstOrDefault();
            else return comic.Items.LastOrDefault();
        }

        [KernelFunction, Description("查找最后浏览章节")]
        [return: Description("获取到的章节（可能没有）")]
        public Item? GetLastReadChapterAsync(
                                                                   [Description("用于查找的漫画")] Obj comic)
        {
            if (comic.LastReadedItemIndex == -1) return null;
            return comic.Items.ToList()[comic.LastReadedItemIndex];
        }
    }
}