using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CollectionViewTest;

public partial class MainViewModel : ObservableObject
{
    private static readonly List<Person> longPeople = [
        new Person("First","Person", 0),
        new Person("Bob","Smith"),
        new Person("Jim","Brown"),
        new Person("Fred","Robinson"),
        new Person("Famous","Person"),
        new Person("Duplicate","Person"),
        new Person("Duplicate","Person"),
        new Person("Duplicate","Person"),
        new Person("Duplicate","Person"),
        new Person("Duplicate","Person"),
        new Person("Duplicate","Person"),
        new Person("Last","Person"),
    ];
    private static readonly List<Person> shortPeople = [
        new Person("First","Person", 0),
        new Person("Last","Person"),
    ];

    public ObservableCollection<Person> People { get; } = [with(shortPeople)];

    private Person? _draggedPerson;

    [RelayCommand]
    private void DragStarting(Person person) => _draggedPerson = person;

    [RelayCommand]
    private void Drop(Person targetPerson)
    {
        if (_draggedPerson is not null && _draggedPerson != targetPerson)
        {
            int fromIndex = People.IndexOf(_draggedPerson);
            int toIndex = People.IndexOf(targetPerson);
            if (fromIndex >= 0 && toIndex >= 0)
                People.Move(fromIndex, toIndex);
        }
        _draggedPerson = null;
    }

    [ObservableProperty]
    public partial string? CommandLog { get; set; } = null;

    [ObservableProperty]
    public partial Person? SelectedPerson { get; set; } = null;

    partial void OnSelectedPersonChanged(Person? value) => ShowCommandInfo("OnSelectedPersonChanged Property Notification", value);

    [ObservableProperty]
    public partial bool IsLongList { get; set; } = false;

    partial void OnIsLongListChanged(bool value)
    {
        People.Clear();
        foreach (var p in value ? longPeople : shortPeople)
            People.Add(p);
    }

    public void ShowCommandInfo(string t, object? commandParam = null)
    {
        string CommandParameter;
        if (commandParam is null)
            CommandParameter = "";
        else if (commandParam is string s)
            CommandParameter = s;
        else
            CommandParameter = "'" + (commandParam.ToString() ?? "") + "'";
        CommandLog += t + (string.IsNullOrWhiteSpace(CommandParameter) ? "" : ", " + CommandParameter) + "\n";
    }

    [RelayCommand]
    private void ClearLog() => CommandLog = null;
    [RelayCommand]
    private void ShowTouch(object commandParam) => ShowCommandInfo("Touch Command", commandParam);
    [RelayCommand]
    private void ShowTap(object commandParam) => ShowCommandInfo("Tap Command", commandParam);
    [RelayCommand]
    private void ShowSwipe(object commandParam) => ShowCommandInfo("Swipe Command", commandParam);
    [RelayCommand]
    private void ShowLongPress(object commandParam) => ShowCommandInfo("Long Press Command", commandParam);
    [RelayCommand]
    private void SelectionChanged(object commandParam) => ShowCommandInfo("SelectionChanged Control Notification", commandParam);
    [RelayCommand]
    private void ToggleSelectedPerson(object commandParam)
    {
        ShowCommandInfo("ToggleSelectedPerson Command", commandParam);
        // Deselect if the item was already selected otherwise select it
        Person? targetPerson = commandParam as Person;
        try
        {
            SelectedPerson = (SelectedPerson == targetPerson) ? null : targetPerson;
        }
        catch (Exception ex)
        {
            ShowCommandInfo("Exception", ex.Message);
        }
    }
    [RelayCommand]
    private void SelectFirst(object commandParam)
    {
        ShowCommandInfo("Select First Command");
        SelectedPerson = People.FirstOrDefault();
    }

    [RelayCommand]
    private void SelectLast(object commandParam)
    {
        ShowCommandInfo("Select Last Command");
        SelectedPerson = People.LastOrDefault();
    }
}

public class Person
{
    public Person(string firstName, string lastName)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        number = count++;
    }
    public Person(string firstName, string lastName, int number)
    {
        count = number;
        // Duplicate the simple constructor
        this.firstName = firstName;
        this.lastName = lastName;
        number = count++;
    }

    readonly string firstName;
    readonly string lastName;
    private readonly int number;
    private static int count = 0;
    public string Name => lastName + ", " + firstName + " [" + number + "]";
    public string FirstName => firstName;
    public string LastName => lastName;
    public int Number => number;
    public override string ToString() => Name;
}
