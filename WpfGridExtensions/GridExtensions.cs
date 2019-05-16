using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfGridExtensions
{
  public class GridExtensions
  {
    // Hashtable zum Verwalten aller AreaDefinitions im Programm
    private static Dictionary<DependencyObject, Dictionary<string, AreaDefinition>> AreaDefinitions
      = new Dictionary<DependencyObject, Dictionary<string, AreaDefinition>>();

    // Attached Dependency Property "Area"
    public static string GetArea(DependencyObject obj)
    {
      return (string)obj.GetValue(AreaProperty);
    }

    public static void SetArea(DependencyObject obj, string value)
    {
      obj.SetValue(AreaProperty, value);
    }

    // Using a DependencyProperty as the backing store for Area.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AreaProperty =
        DependencyProperty.RegisterAttached("Area", typeof(string), typeof(GridExtensions),
          new FrameworkPropertyMetadata(OnAreaChanged));

    // Setzen der Property führt zum Setzen der Attached Dependency Properties des Grids (Row, Column etc.)
    private static void OnAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var fe = d as FrameworkElement;

      // Parent wird benötigt, daher nur Objekte vom Typ FrameworkElement zulässig
      if (fe == null) throw new ApplicationException("Area can only be set on Frameworkelements");

      var grid = fe.Parent as Grid;

      // Parent muss ein Grid sein
      if (grid == null)
        throw new ApplicationException("Area can only be set on children of a Grid");

      // Die AreadDefinitions des Grids müssen bereits definiert sein
      if (AreaDefinitions == null || !AreaDefinitions.ContainsKey(grid))
        throw new ApplicationException("Area definitions not set");

      var areaDefs = AreaDefinitions[grid];

      // Der gewünschte Area-Name muss bereits definiert sein
      if (!areaDefs.ContainsKey((string)e.NewValue))
        throw new ApplicationException($"Area name '{e.NewValue}' not defined");

      var areaDef = areaDefs[(string)e.NewValue];

      // Setzen der Attached Dependency Properties des Grids
      d.SetValue(Grid.ColumnProperty, areaDef.Column);
      d.SetValue(Grid.RowProperty, areaDef.Row);
      d.SetValue(Grid.RowSpanProperty, areaDef.RowSpan);
      d.SetValue(Grid.ColumnSpanProperty, areaDef.ColumnSpan);

    }


    // Attached Dependency Property "AreaDefinitions"

    public static AreaRows GetAreaDefinitions(DependencyObject obj)
    {
      return (AreaRows)obj.GetValue(AreaDefinitionsProperty);
    }

    public static void SetAreaDefinitions(DependencyObject obj, AreaRows value)
    {
      obj.SetValue(AreaDefinitionsProperty, value);
    }

    // Using a DependencyProperty as the backing store for AreaDefinitions.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AreaDefinitionsProperty =
        DependencyProperty.RegisterAttached("AreaDefinitions", typeof(AreaRows), typeof(GridExtensions),
          new FrameworkPropertyMetadata(OnAreaDefinitionsChanged));

    // Setzen der Property führt zur Analyse und Anlegen der AreaDefinitions
    private static void OnAreaDefinitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      // Zieltyp muss ein Grid sein
      if (!(d is Grid)) throw new ApplicationException("Area definition can only be set on type Grid");

      // Definitionen dürfen nur einmal vorhanden sein
      if (AreaDefinitions.ContainsKey(d)) throw new ApplicationException("Only one area definition allowed");

      var areaDefs = new Dictionary<string, AreaDefinition>();
      AreaDefinitions[d] = areaDefs;
      for (int row = 0; row < ((AreaRows)e.NewValue).Count; row++)
      {
        // In einer Row enthaltene Namen
        var names = ((AreaRows)e.NewValue)[row].AreaNames.Split(' ');

        for (int col = 0; col < names.Length; col++)
        {
          var name = names[col];
          if (areaDefs.ContainsKey(name)) // Name schon verwendet?
          {
            if (row == areaDefs[name].Row) // Definition erste Zeile des Vorkommens
            {
              if (col == areaDefs[name].Column + areaDefs[name].ColumnSpan) // Folgespalte?
                areaDefs[name].ColumnSpan++;
              else
                throw new ApplicationException("Area must be rectangular");
            }
            else // Folgezeilen
            {
              // Weitere Zeile für diesen Namen?
              if (col == areaDefs[name].Column) areaDefs[name].RowSpan++;

              if (col < areaDefs[name].Column || col >= areaDefs[name].Column + areaDefs[name].ColumnSpan)
                throw new ApplicationException("Area must be rectangular");
            }
          }
          else
          {
            // Name ist neu -> neue Definition anlegen
            areaDefs[name] = new AreaDefinition { Row = row, Column = col, ColumnSpan = 1, RowSpan = 1 };
          }
        }

      }
    }



    public static Thickness GetGap(DependencyObject obj)
    {
      return (Thickness)obj.GetValue(GapProperty);
    }

    public static void SetGap(DependencyObject obj, Thickness value)
    {
      obj.SetValue(GapProperty, value);
    }

    // Using a DependencyProperty as the backing store for Gap.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty GapProperty =
        DependencyProperty.RegisterAttached("Gap", typeof(Thickness), typeof(GridExtensions), new FrameworkPropertyMetadata(new Thickness(0), OnGapChanged));

    private static void OnGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var grid = d as Grid;
      grid.Initialized += Grid_Initialized;
    }

    private static void Grid_Initialized(object sender, EventArgs e)
    {
      var grid = sender as Grid;
      var margin = GetGap(grid);
      foreach (FrameworkElement item in grid.Children)
      {
        item.Margin = margin;
      }
    }

  }

}
