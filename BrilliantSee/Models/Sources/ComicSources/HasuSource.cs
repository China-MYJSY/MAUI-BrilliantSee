﻿using BrilliantSee.Models.Objs;
using BrilliantSee.Models.Objs.Comics;
using BrilliantSee.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BrilliantSee.Models.Sources.ComicSources
{
    public partial class HasuSource : Source
    {
        public HasuSource()
        {
            SetHttpClient("https://mangahasu.se/");
            Name = "HasuManga";
        }

        public override async Task<IEnumerable<Obj>> SearchAsync(string keyword)
        {
            var url = $"https://mangahasu.se/advanced-search.html?keyword={keyword}";
            var html = await GetHtmlAsync(url);
            if (html == string.Empty) { return Array.Empty<Obj>(); }

            var start = html.IndexOf("imgage");
            var end = html.IndexOf("setTimeout");
            if (start < 0 || end < 0)
            {
                throw new Exception("接口异常,请等待维护");
            }
            html = html.Substring(start, end - start);
            string pattern = "lazy[\\s\\S]*?src=\"(.*?)\"[\\s\\S]*?href=\"(.*?)\"[\\s\\S]*?h3>(.*?)<";
            if (html.IndexOf("date_") == -1)
            {
                return Array.Empty<Obj>();
            }
            var matches = Regex.Matches(html, pattern);
            var comics = new List<Obj>();
            foreach (Match match in matches)
            {
                var comic = new HasuComic()
                {
                    Url = match.Groups[2].Value,
                    Name = match.Groups[3].Value.Replace("&quot;", ""),
                    Cover = match.Groups[1].Value,
                    Source = this,
                    SourceName = Name,
                    SourceCategory = Category
                };
                comics.Add(comic);
                if (comics.Count == matches.Count - 10)
                {
                    break;
                }
            }

            return comics;
        }
    }
}