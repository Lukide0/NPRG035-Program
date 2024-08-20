using System;
using Spectre.Console;
using task_tracker.Task;

namespace task_tracker.Model;

public static class Prompt
{

    /// <summary> Prompts user for a string.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <param name="errorMsg"> Error message.</param>
    /// <param name="validator"> Validator.</param>
    /// <param name="allowEmpty"> Allow empty string.</param>
    /// <returns> User input.</returns>
    public static string String(string name, string? errorMsg = null,
                                Func<string, bool>? validator = null,
                                bool allowEmpty = false)
    {
        return AnsiConsole.Prompt(
            GenericPrompt<string>(name, errorMsg, validator, allowEmpty));
    }

    /// <summary> Prompts user for a uint.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <param name="errorMsg"> Error message.</param>
    /// <param name="validator"> Validator.</param>
    /// <param name="allowEmpty"> Allow empty string.</param>
    /// <returns> User input.</returns>
    public static uint UInt(string name, string? errorMsg = null,
                            Func<uint, bool>? validator = null,
                            bool allowEmpty = false)
    {
        return AnsiConsole.Prompt(
            GenericPrompt<uint>(name, errorMsg, validator, allowEmpty));
    }

    /// <summary> Prompts user for a uint.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <param name="defaultValue"> Default value.</param>
    /// <param name="errorMsg"> Error message.</param>
    /// <param name="validator"> Validator.</param>
    /// <returns> User input.</returns>
    public static uint UInt(string name, uint defaultValue,
                            string? errorMsg = null,
                            Func<uint, bool>? validator = null)
    {
        return AnsiConsole.Prompt(
            GenericPrompt<uint>(name, errorMsg, validator, true)
                .DefaultValue(defaultValue));
    }

    /// <summary>Creates a generic optional prompt for the user.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <param name="errorMsg"> Error message.</param>
    /// <param name="validator"> Validator.</param>
    /// <returns> User input.</returns>
    private static TextPrompt<T> GenericPrompt<T>(string name,
                                                  string? errorMsg = null,
                                                  Func<T, bool>? validator = null,
                                                  bool allowEmpty = false)
    {
        var prompt =
            new TextPrompt<T>($"[bold]{name}:[/]").Culture(App.Config.Locale);

        if (errorMsg is not null)
        {
            prompt.ValidationErrorMessage(errorMsg);
        }

        if (validator is not null)
        {
            prompt.Validate(validator);
        }

        if (allowEmpty)
        {
            prompt.AllowEmpty();
        }

        return prompt;
    }

    /// <summary>Creates a generic optional prompt for the user.</summary>
    /// <typeparam name="T">The type of the prompt.</typeparam>
    /// <param name="name">The name of the prompt.</param>
    /// <param name="errorMsg">
    /// The error message to display if validation fails.
    /// </param>
    /// <param name="validator">
    /// The validation function to apply to the user's input.</param>
    /// <returns>
    /// A TextPrompt instance configured for the specified type and options.
    /// </returns>
    private static TextPrompt<T>
    GenericOptionalPrompt<T>(string name, string? errorMsg = null,
                             Func<T, bool>? validator = null)
    {

        var prompt = new TextPrompt<T>($"[[Optional]][bold]{name}:[/]")
                         .Culture(App.Config.Locale)
                         .AllowEmpty()
                         .ShowDefaultValue(false);

        if (errorMsg is not null)
        {
            prompt.ValidationErrorMessage(errorMsg);
        }

        if (validator is not null)
        {
            prompt.Validate(validator);
        }

        return prompt;
    }

    /// <summary> Prompts user for a date.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <returns> User input.</returns>
    public static DateTime? OptionalDate(string name)
    {
        return AnsiConsole.Prompt(GenericOptionalPrompt<DateTime?>(name).DefaultValue(null));
    }

    /// <summary> Prompts user for a uint.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <returns> User input.</returns>
    public static uint? OptionalUInt(string name)
    {
        return AnsiConsole.Prompt(GenericOptionalPrompt<uint?>(name).DefaultValue(null));
    }

    /// <summary> Prompts user for a enum.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <returns> User input.</returns>
    public static T? OptionalEnum<T>(string name)
        where T : struct, Enum
    {
        var values =
            Array.ConvertAll(System.Enum.GetValues<T>(), item => item.ToString());

        var prompt = new SelectionPrompt<string>()
                         .Title($"[[Optional]][bold]{name}:[/]")
                         .AddChoices("None")
                         .AddChoiceGroup("Option", values);

        T value;
        if (System.Enum.TryParse<T>(AnsiConsole.Prompt(prompt), out value))
        {
            return value;
        }

        return null;
    }

    /// <summary> Prompts user for a enum.</summary>
    /// <param name="name"> Name of the prompt.</param>
    /// <returns> User input.</returns>
    public static T Enum<T>(string name)
        where T : struct, Enum
    {
        var values = System.Enum.GetValues<T>();

        var prompt =
            new SelectionPrompt<T>().Title($"[bold]{name}:[/]").AddChoices(values);

        return AnsiConsole.Prompt(prompt);
    }

    /// <summary> Prmpts user for a enter key.</summary>
    public static void PressEnter()
    {
        AnsiConsole.Write(new Markup("[bold]Press ENTER to continue...[/]"));
        while (AnsiConsole.Console.Input.ReadKey(false)?.Key != ConsoleKey.Enter)
        {
        }
    }

    /// <summary> Prompts user for a task filter.</summary>
    /// <returns> User input.</returns>
    public static TaskFilterOptions TaskFilter()
    {
        TaskFilterOptions filter = new();
        filter.Id = Prompt.OptionalUInt("Id");
        filter.Priority = Prompt.OptionalEnum<TaskPriority>("Priority");
        filter.State = Prompt.OptionalEnum<TaskState>("State");
        filter.DateStart = Prompt.OptionalDate("From date");
        filter.DateEnd = Prompt.OptionalDate("To date");
        filter.Limit = Prompt.UInt("Limit", 100);
        filter.Offset = Prompt.UInt("Offset", 0);

        return filter;
    }
}
