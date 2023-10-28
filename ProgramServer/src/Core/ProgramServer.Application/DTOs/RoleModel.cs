using FluentValidation;

namespace ProgramServer.Application.DTOs
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool IsReceiver { get; set; }

        public bool IsBroadcaster { get; set; }

        public bool IsCreator { get; set; }
    }

    public class RoleValidator : AbstractValidator<RoleModel>
    {
        public RoleValidator()
        {

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Enter the role name");

        }

    }
}

