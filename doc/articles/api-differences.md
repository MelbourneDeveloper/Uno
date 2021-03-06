# Uno.UI and UWP API or Behavior differences

For legacy, platform support or performance reasons, Uno has some notable API differences.

## DependencyObject is an interface.
`DependencyObject` is an interface to allow for XAML controls to inherit directly from their native counterpart. The implementation of the methods is done through the `DependencyObjectGenerator` source generator, automatically.

This has some implications in generic constraints which require a class, but can be worked around using the `IS_UNO` define.

## Panel.Children is exposing native views as items
This a legacy requirement which will be updated in the future. This is currently required by iOS/Android `Image` control, as well as iOS implementation of `TextBlock`.

Historically, this has been a requirement for performance reasons related to view nesting in Android 4.4 and earlier, caused by a very short UI Thread stack size.

For the time being, enumerating panel children can be done as follows, in a cross platform compatible way:

```csharp
foreach(var item in myPanel.Children)
{
    if(item is FrameworkElement fe)
    {
        // Process the item
    }
}
```

This difference is particularly visible for custom panel implementations.

## ListView implementations

The implementations of the ListView for iOS and Android use the native controls for performance reasons, see the [ListViewBase implementation documentation](controls/ListViewBase.md#internal-implementation).

## Styles

Uno Platform implements styling in XAML as per the [Microsoft UWP documentation](https://docs.microsoft.com/en-us/windows/uwp/design/controls-and-patterns/xaml-styles). You can declare a `Style` as a resource globally or inline. Uno supports implicit and explicit styling. However, in versions of Uno Platform before 3.0, resources cannot be declared as implicit and inline.

For example, this `Style` works and will apply a red background to a `ListViewItem`.

```xml
<Page.Resources>
  <Style TargetType="ListViewItem" x:Key="redlistviewitem">
    <Setter Property="Background"  Value="Red"></Setter>
  </Style>
</Page.Resources>

 <ListView ItemContainerStyle="{StaticResource redlistviewitem}"  />
```
However, this XAML will cause a compilation error

```xml
<Page.Resources>
  <Style TargetType="ListViewItem" >
    <Setter Property="Background"  Value="Red"></Setter>
  </Style>
</Page.Resources>
```

> Implicit styles in inline resources are not supported (Page, Line 1:2)

To resolve the issue, add an `x:Key` to the `Style` or move the `Style` to a global XAML location. This issue [has been fixed in V3](https://github.com/unoplatform/uno/pull/1766). At the time of writing, V3 is not in release mode.

## Themes

`FrameworkElement.RequestedTheme` is not supported yet. The `Application.Current.RequestedTheme` property
must be set at launch time. Documentation: [`Application.RequestedTheme`](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.application.requestedtheme)

### Custom Themes

On Windows, there are some _themes_ that can target, but there is no way to trigger them. The most
known is the `HighContrast` theme.

You can do something similar - and even create totally custom themes - by using the following helper:

``` csharp
  // Set current theme to High contrast
  Uno.UI.RequestedCustomTheme = "HighContrast";
```

* Beware, all themes are **CASE SENSITIVE**.
* Themed dictionaries will fall back to `Application.Current.RequestedTheme` when they are not
  defining a resource for the custom theme.
* You can put any string and create totally custom themes, but they won't be supported by UWP.

Themes [are implemented](https://calculator.platform.uno?Theme=Pink) in the Uno port of the Windows 10 calculator. See [App.xaml.cs](https://github.com/unoplatform/calculator/blob/7772a593b541edd9809bc8946ee29d6a5b29e0ff/src/Calculator.Shared/App.xaml.cs#L79) and  [Styles.xaml](https://github.com/unoplatform/calculator/blob/7772a593b541edd9809bc8946ee29d6a5b29e0ff/src/Calculator.Shared/Styles.xaml).


