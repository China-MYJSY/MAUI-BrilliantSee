using BrilliantComic.ViewModels;
using System.Diagnostics;

namespace BrilliantComic.Views;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryViewModel _vm;

    public HistoryPage(HistoryViewModel vm)
    {
        _vm = vm;
        this.BindingContext = _vm;
        InitializeComponent();
    }

    /// <summary>
    /// ҳ�����ʱ������ʷ��¼������
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.OnLoadHistoryComicAsync();
        if (_vm._ai.hasModel)
        {
            _vm._ai.RemovePlugins();
            _vm._ai.ImportPlugins(new Services.Plugins.HistoryPlugins(_vm._db));
        }
        this.audio.IsVisible = await _vm._db.GetAudioStatus();
    }

    private async void CleanTapped(object sender, TappedEventArgs e)
    {
        var obj = sender! as Frame;
        var shadow = obj!.Shadow;
        obj!.Shadow = new Shadow()
        {
            Offset = new Point(0, 8),
            Opacity = (float)0.3,
            Radius = 14,
        };
        await obj!.ScaleTo(1.05, 100);
        await obj!.ScaleTo(1, 100);
        obj!.Shadow = shadow;
        bool answer = await DisplayAlert("�����ʷ��¼", "��ʷ��¼��պ��޷��ָ����Ƿ����?", "ȷ��", "ȡ��");
        if (answer)
        {
            await _vm.ClearHistoryComicsAsync();
        }
    }

    private void JumpToSettingPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("SettingPage");
    }
}