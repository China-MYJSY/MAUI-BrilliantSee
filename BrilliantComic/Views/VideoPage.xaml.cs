namespace BrilliantSee.Views;

public partial class VideoPage : ContentPage
{
    public List<string> Urls = new List<string>()
    {
        "��һ��",
        "�ڶ���",
        "������",
        "���ļ�",
        "���弯",
    };

    public VideoPage()
    {
        this.BindingContext = this;
        InitializeComponent();
    }
}