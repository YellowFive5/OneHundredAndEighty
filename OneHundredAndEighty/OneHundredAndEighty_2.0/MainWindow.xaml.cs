namespace OneHundredAndEighty_2._0
{
    public partial class MainWindow
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel(this);
        }
    }
}