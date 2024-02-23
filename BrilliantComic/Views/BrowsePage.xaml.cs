using BrilliantComic.ViewModels;
using CommunityToolkit.Maui.Alerts;

namespace BrilliantComic.Views;

public partial class BrowsePage : ContentPage
{
    /// <summary>
    /// ��ǰ��һ���ɼ�item��CollectionView�е�����
    /// </summary>
    private int crrentFirstVisibleItemIndex = 0;

    /// <summary>
    /// ��ǰ���һ���ɼ�item��CollectionView�е�����
    /// </summary>
    private int crrentLastVisibleItemIndex = 0;

    /// <summary>
    /// ��ǰλ�ڿɼ������м��item��CollectionView�е�����
    /// </summary>
    private int crrentCenterItemIndex = 0;

    /// <summary>
    /// �ڶ����Ѽ����½ڵ���ǰ�½�ͼƬ�����ܺ�
    /// </summary>
    private int utillCrrentChapterImageCount = 0;

    private readonly BrowseViewModel _vm;

    public BrowsePage(BrowseViewModel vm)
    {
        _vm = vm;
        this.BindingContext = _vm;
        InitializeComponent();
    }

    /// <summary>
    /// ���ݹ���λ�ü�����һ�»���һ�¼��л��½�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (e.FirstVisibleItemIndex != crrentFirstVisibleItemIndex)
        {
            crrentFirstVisibleItemIndex = e.FirstVisibleItemIndex;
            if (e.FirstVisibleItemIndex == 0)
            {
                var toast = Toast.Make("���ڼ�����һ��");
                _ = toast.Show();
                var result = await _vm.UpdateChapterAsync("Last");
                if (result)
                {
                    utillCrrentChapterImageCount += _vm.Chapter!.PageCount;
                    _vm.CurrentChapterIndex++;

                    var toast1 = Toast.Make("���سɹ�");
                    _ = toast1.Show();
                }
                else
                {
                    var toast1 = Toast.Make("���ǵ�һ��");
                    _ = toast1.Show();
                }
            }
        }
        if (e.LastVisibleItemIndex != crrentLastVisibleItemIndex)
        {
            crrentLastVisibleItemIndex = e.LastVisibleItemIndex;
            if (e.LastVisibleItemIndex == _vm.Images.ToList().Count - 1)
            {
                var toast = Toast.Make("���ڼ�����һ��");
                _ = toast.Show();
                var result = await _vm.UpdateChapterAsync("Next");
                if (result)
                {
                    var toast1 = Toast.Make("���سɹ�");
                    _ = toast1.Show();
                }
                else
                {
                    var toast1 = Toast.Make("��������һ��");
                    _ = toast1.Show();
                }
            }
        }
        if (e.CenterItemIndex != crrentCenterItemIndex)
        {
            if (e.CenterItemIndex < crrentCenterItemIndex)
            {
                _vm.CurrentPageNum--;
            }
            else
            {
                _vm.CurrentPageNum++;
            }
            if (e.CenterItemIndex == utillCrrentChapterImageCount - _vm.Chapter!.PageCount + _vm.LoadedChapter[0].PageCount - 1 && crrentCenterItemIndex > e.CenterItemIndex)
            {
                utillCrrentChapterImageCount -= _vm.Chapter.PageCount;
                _vm.CurrentChapterIndex--;
                _vm.CurrentPageNum = _vm.Chapter.PageCount;
                _ = _vm.StoreLastReadedChapterIndex();
            }
            else if (e.CenterItemIndex == utillCrrentChapterImageCount + _vm.LoadedChapter[0].PageCount && crrentCenterItemIndex < e.CenterItemIndex)
            {
                _vm.CurrentChapterIndex++;
                _vm.CurrentPageNum = 1;
                utillCrrentChapterImageCount += _vm.Chapter.PageCount;
                _ = _vm.StoreLastReadedChapterIndex();
            }
            crrentCenterItemIndex = e.CenterItemIndex;
        }
    }
}