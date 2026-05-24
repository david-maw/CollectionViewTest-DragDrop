namespace CollectionViewTest
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel vm;
        public MainPage()
        {
            InitializeComponent();
            vm = (MainViewModel)BindingContext;
        }

        private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            vm.ShowCommandInfo("OnCollectionViewScrolled Control Notification", $"Item {e.FirstVisibleItemIndex} to Item {e.LastVisibleItemIndex}");
        }
    }
}
