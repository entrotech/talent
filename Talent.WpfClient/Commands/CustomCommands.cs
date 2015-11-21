using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Talent.WpfClient
{
    public static class CustomCommands
    {
        // Define an Exit Command that can be used to Exit the Application
        public static readonly RoutedUICommand Exit =
            new RoutedUICommand("Exit", "Exit", typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                });

        // Define a Cancel Command for discarding any pending changes
        public static readonly RoutedUICommand Cancel =
            new RoutedUICommand("Cancel", "Cancel", typeof(CustomCommands));
    }
}

