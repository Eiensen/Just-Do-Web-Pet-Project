using System.Text.RegularExpressions;

namespace ServerAPI.Servieces.Validators
{
    public class PasswordValidator: IPasswordValidator<User>
    {
        private readonly int _requiredUppercaseLetters;

        public PasswordValidator(int letters)
        {
            _requiredUppercaseLetters = letters;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            LinkedList<IdentityError> errors = new LinkedList<IdentityError>();

            string patternSeparate = @"([A-Z]{1,})";
            string patternTogether = @"([A-Z]{2,})";

            var matchSeparate = Regex.Matches(password, patternSeparate);
            var matchTogether = Regex.Matches(password, patternTogether);

            if (matchSeparate.Count < _requiredUppercaseLetters && matchTogether.Count < 1)
            {
                errors.AddLast(new IdentityError
                {
                    Description = $"The password should contain {_requiredUppercaseLetters} or more uppercase letters"
                });
            }

            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
