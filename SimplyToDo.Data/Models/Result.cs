using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimplyToDo.Data.Models;

public class Result
{
    private readonly Dictionary<string, List<string>> _errors = [];

    /// <summary>
    /// Indicate if the operation succeeded or not.
    /// </summary>
    /// <value>True if the operation succeeded, otherwise false.</value>
    public bool Succeeded { get; protected set; } = true;

    /// <summary>
    /// An <see cref="IEnumerable{T}"/> of instances containing errors
    /// that occurred during the operation.
    /// </summary>
    /// <value>An <see cref="IEnumerable{T}"/> of instances.</value>
    public IEnumerable<KeyValuePair<string, List<string>>> Errors => _errors;

    /// <summary>
    /// Adds an error to the Error instances and sets the Succeeded value to false.
    /// </summary>
    /// <param name="property">The name of the error instance.</param>
    /// <param name="error">The error to add.</param>
    public void AddError(string property, string error)
    {
        Succeeded = false;

        if (!_errors.ContainsKey(property))
        {
            _errors.Add(property, [error]);
        }
        else
        {
            _errors[property].Add(error);
        }
    }

    /// <summary>
    /// Adds any error instances to the <see cref="ModelStateDictionary"/>.
    /// </summary>
    /// <param name="modelState">The <see cref="ModelStateDictionary"/> to add the error instances to.</param>
    public void MapTo(ModelStateDictionary modelState)
    {
        foreach (var error in _errors)
        {
            foreach (var errorMessage in error.Value)
            {
                _ = modelState.TryAddModelError(error.Key, errorMessage);
            }
        }

    }

    protected Result(bool succeeded = true)
    {
        Succeeded = succeeded;
    }

    public static Result Ok() => new();
    public static Result Error(string property, string error)
    {
        var result = new Result(false);
        result.AddError(property, error);
        return result;
    }
}

public class Result<T> : Result
{
    private T? _value { get; init; }
    /// <summary>
    /// The value of the Result if the operation succeeded.
    /// </summary>
    public T Value
    {
        get => Succeeded ? _value! : throw new InvalidOperationException("Unable to get Value when result did not succeed.");
    }

    protected Result(T? value, bool succeeded = true) : base(succeeded)
    {
        _value = value;
    }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Error(string error) => Error(string.Empty, error);
    public new static Result<T> Error(string property, string error)
    {
        var result = new Result<T>(default, false);
        result.AddError(property, error);
        return result;
    }
}