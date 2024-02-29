using BrilliantComic.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace BrilliantComic.Views;

public partial class BrowsePage : ContentPage
{
    /// <summary>
    /// �Ƿ����ڼ���
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// ��ǰ��һ���ɼ�item��CollectionView�е�����
    /// </summary>
    private int crrentFirstVisibleItemIndex = 0;

    /// <summary>
    /// ��ǰ���һ���ɼ�item��CollectionView�е�����
    /// </summary>
    private int crrentLastVisibleItemIndex = 1;

    /// <summary>
    /// ��ǰλ�ڿɼ������м��item��CollectionView�е�����
    /// </summary>
    private int crrentCenterItemIndex = 0;

    private readonly BrowseViewModel _vm;

    public BrowsePage(BrowseViewModel vm)
    {
        _vm = vm;
        this.BindingContext = _vm;
        InitializeComponent();
    }

    /// <summary>
    /// ���ݹ���λ��������ǰ����
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (e.CenterItemIndex != crrentCenterItemIndex)
        {
            if (e.CenterItemIndex - crrentCenterItemIndex == -1)
            {
                _vm.CurrentPageNum--;
                if (e.CenterItemIndex != 0 && e.CenterItemIndex == _vm.utillCrrentChapterImageCount - _vm.Chapter!.PageCount - 1)
                {
                    _vm.CurrentChapterIndex--;
                    _vm.CurrentPageNum = _vm.Chapter.PageCount;
                    _ = _vm.StoreLastReadedChapterIndex();
                }
            }
            else if (e.CenterItemIndex - crrentCenterItemIndex == 1)
            {
                _vm.CurrentPageNum++;
                if (e.CenterItemIndex == _vm.utillCrrentChapterImageCount)
                {
                    _vm.CurrentChapterIndex++;
                    _vm.CurrentPageNum = 1;
                    _ = _vm.StoreLastReadedChapterIndex();
                }
            }
            crrentCenterItemIndex = e.CenterItemIndex;
        }
        if (e.FirstVisibleItemIndex != crrentFirstVisibleItemIndex)
        {
            crrentFirstVisibleItemIndex = e.FirstVisibleItemIndex;
            if (e.FirstVisibleItemIndex == 0 && !isLoading)
            {
                isLoading = true;
                _ = Toast.Make("���ڼ�����һ��...").Show();
                var result = await _vm.UpdateChapterAsync("Last");
                if (result)
                {
                    _ = Toast.Make("���سɹ�").Show();
                }
                else
                {
                    _ = Toast.Make("���ǵ�һ��").Show();
                }
                isLoading = false;
            }
        }
        if (e.LastVisibleItemIndex != crrentLastVisibleItemIndex)
        {
            crrentLastVisibleItemIndex = e.LastVisibleItemIndex;
            if (_vm.Images.ToList().Count != 0 && e.LastVisibleItemIndex == _vm.Images.LongCount() && !isLoading)
            {
                isLoading = true;
                _ = Toast.Make("���ڼ�����һ��...").Show();
                var result = await _vm.UpdateChapterAsync("Next");
                if (result)
                {
                    _ = Toast.Make("���سɹ�").Show();
                }
                else
                {
                    _ = Toast.Make("��������һ��").Show();
                }
                isLoading = false;
            }
        }
    }
}