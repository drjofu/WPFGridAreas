# WPF Grid Areas

Implementation of CSS-like Grid Area Definitions in WPF

Usage example:
```
 <Grid Margin="7" GridExtensions.Gap="7">
    <Grid.RowDefinitions>
      <RowDefinition Height="100*"/>
      <RowDefinition Height="100*"/>
      <RowDefinition Height="100*"/>
      <RowDefinition Height="100*"/>
    </Grid.RowDefinitions>
    <Grid.ColumnDefinitions>
      <ColumnDefinition Width="100*" />
      <ColumnDefinition Width="100*"/>
      <ColumnDefinition Width="100*"/>
      <ColumnDefinition Width="100*"/>
    </Grid.ColumnDefinitions>

    <GridExtensions.AreaDefinitions>
      <AreaRows>
        <AreaRow>eins zwei zwei drei</AreaRow>
        <AreaRow>eins zwei zwei drei</AreaRow>
        <AreaRow>eins fünf fünf fünf</AreaRow>
        <AreaRow>vier vier vier vier</AreaRow>
      </AreaRows>
    </GridExtensions.AreaDefinitions>


    <Button Content="1" GridExtensions.Area="eins" />
    <Button Content="2" GridExtensions.Area="zwei" />
    <Button Content="3" GridExtensions.Area="drei" />
    <Button Content="4" GridExtensions.Area="vier" />
    <Button Content="5" GridExtensions.Area="fünf" />

  </Grid>
```
