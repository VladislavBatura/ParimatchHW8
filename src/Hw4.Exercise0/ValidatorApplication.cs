using Common;

namespace Hw4.Exercise0;

public sealed class ValidatorApplication
{
    /// <summary>
    /// Runs application.
    /// </summary>
    public ReturnCode Run(string[] args)
    {
        // TODO: create a person object from passed `args`
        Person? person = null;

        // validate
        var validationResult = PersonValidator.Validate(person);

        // and log validation result
        Console.WriteLine("Person {0} validation result is: {1}", person, validationResult);

        return validationResult.IsValid ? ReturnCode.Success : ReturnCode.InvalidArgs;
    }
}
