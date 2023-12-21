using System.Windows;

namespace wpfCalculated
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IMemory dataBased = new DataBased();
            IMemory files = new Files();
            IMemory memory = new Memory();

            DataContext = new ViewModel(files);   
        }
    }
}
