using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ree7.Utils.UI
{
	public class GridViewItemClick
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
			"Command", 
			typeof(ICommand), 
			typeof(GridViewItemClick), 
			new PropertyMetadata(null,CommandPropertyChanged)); 
 
        public static void SetCommand(DependencyObject attached, ICommand value)
        {
            attached.SetValue(CommandProperty, value);
        } 
 
        public static ICommand GetCommand(DependencyObject attached)
        {
            return (ICommand)attached.GetValue(CommandProperty);
        }
 
        private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Attach click handler
            (d as GridView).ItemClick += gridView_ItemClick;
        }
 
        private static void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Get GridView
            var gridView = (sender as GridView);
 
            // Get command
            ICommand command = GetCommand(gridView);
 
            // Execute command
            command.Execute(e.ClickedItem);
        }
    }
}