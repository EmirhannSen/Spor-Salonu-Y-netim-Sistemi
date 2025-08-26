using System.Windows.Controls;
using System;

public class MainMenuItem
{
    public string Title { get; }
    public string Icon { get; }
    public Func<UserControl> PageFactory { get; }

    public MainMenuItem(string title, string icon, Func<UserControl> pageFactory)
    {
        Title = title;
        Icon = icon;
        PageFactory = pageFactory;
    }
}
