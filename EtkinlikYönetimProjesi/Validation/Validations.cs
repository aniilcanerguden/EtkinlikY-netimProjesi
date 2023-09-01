using EtkinlikYönetimProjesi.Dtos;
using FluentValidation;
using Model.Dtos;

namespace EtkinlikYönetimProjesi.Validation
{
    public class RegisterModelValidations : AbstractValidator<UserRegisterViewModel>
    {
        public RegisterModelValidations()
        {
            RuleFor(model => model.EMail).NotNull();
            RuleFor(model => model.Password).MinimumLength(8).Matches("[A-Za-z]").Matches("[0-9]").WithMessage("Şifre en az 8 karakterden oluşmalı ve hem harf hem de rakam içermelidir.");
            RuleFor(model => model.PasswordRepeat).Equal(model => model.Password).WithMessage("Passwords do not match.");
        }
    }

    public class UpdateProfileModelValidator : AbstractValidator<UserUpdateViewModel>
    {
        public UpdateProfileModelValidator()
        {
       
                RuleFor(model => model.NewPassword).MinimumLength(8).Matches("[A-Za-z]").Matches("[0-9]");
                RuleFor(model => model.PasswordRepeat).Equal(model => model.NewPassword).WithMessage("Passwords do not match.");
            
        }
    }
}