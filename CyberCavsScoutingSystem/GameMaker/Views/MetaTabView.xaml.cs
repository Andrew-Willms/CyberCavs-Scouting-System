using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CCSSDomain;

namespace GameMaker.Views;

/// <summary>
/// Interaction logic for MetaTabView.xaml
/// </summary>
public partial class MetaTabView : UserControl {

	public MetaTabView() {

		DataContext = new GameEditingData();

		InitializeComponent();

	}

}