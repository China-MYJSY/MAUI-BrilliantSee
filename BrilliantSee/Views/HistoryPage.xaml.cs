using BrilliantSee.Models.Enums;
using BrilliantSee.ViewModels;

namespace BrilliantSee.Views;

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
        await _vm.OnLoadHistoryObjAsync();
        //if (_vm._ai.hasModel)
        //{
        //    _vm._ai.RemovePlugins();
        //    _vm._ai.ImportPlugins(new Services.Plugins.HistoryPlugins(_vm._db));
        //}
        //this.audio.IsVisible = await _vm._db.GetAudioStatus();
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
            await _vm.ClearHistoryObjsAsync();
        }
    }

    private void JumpToSettingPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("SettingPage");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var button = sender! as Button;
        var text = button!.Text;
        var buttons = new List<Button>() { this.all, this.comics, this.novels, this.videos };
        foreach (var item in buttons)
        {
            item.FontSize = item.Text == text ? 18 : 14;
            item.TextColor = item.Text == text ? Color.FromArgb("#512BD4") : Color.FromArgb("#212121");
        }
        _vm.CurrentCategory = text == "ȫ��" ? SourceCategory.All : text == "����" ? SourceCategory.Comic : text == "С˵" ? SourceCategory.Novel : SourceCategory.Video;
        await _vm.OnLoadHistoryObjAsync();
    }
}