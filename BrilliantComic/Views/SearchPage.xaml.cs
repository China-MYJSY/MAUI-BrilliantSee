using BrilliantComic.ViewModels;
using CommunityToolkit.Maui.Core.Platform;

namespace BrilliantComic.Views;

public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _vm;

    public SearchPage(SearchViewModel vm)
    {
        _vm = vm;
        this.BindingContext = _vm;
        InitializeComponent();

        this.Loaded += SearchPage_Loaded;
    }

    /// <summary>
    /// ҳ�����ʱ������ȡ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void SearchPage_Loaded(object? sender, EventArgs e)
    {
        this.input.Focus();
        await Task.Delay(500);
        this.input.Focus();
        this.Loaded -= SearchPage_Loaded;
    }

    private void HideKeyboard(object sender, TappedEventArgs e)
    {
        if(this.input.IsSoftKeyboardShowing())
        {
            this.input.HideKeyboardAsync(CancellationToken.None);
        }
    }
}