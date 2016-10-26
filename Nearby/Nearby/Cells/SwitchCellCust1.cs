using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nearby.Controls
{
    public class SwitchCellCust1 : SwitchCell
    {
        public SwitchCellCust1()
        {
            this.OnChanged += (s, e) => {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(s);
                }
            };
        }

        public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create("CommandParameter", typeof(string), typeof(SwitchCellCust1), defaultBindingMode: BindingMode.OneWay);

        public string CommandParameter
        {
            get { return (string)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create<SwitchCellCust1, ICommand>(x => x.Command, null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
