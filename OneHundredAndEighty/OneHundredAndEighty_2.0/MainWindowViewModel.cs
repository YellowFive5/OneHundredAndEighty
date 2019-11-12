namespace OneHundredAndEighty_2._0
{
    public class MainWindowViewModel
    {
        private readonly MainWindow window;

        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;
            var dbService = new DBService();

            dbService.SaveNewPlayer("Name1","Nickname1");
        }
    }
}