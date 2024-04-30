using FluentValidation;
using System.Text.RegularExpressions;
using visma_assignment.Contracts;

namespace visma_assignment.Model.Validations
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator(IEmployeeRepository employeeRepo)
        {
            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .NotEqual(e => e.LastName).WithMessage("First Name must be different from the Last Name")
                .MaximumLength(50).WithMessage("First Name cannot be longer than 50 characters");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .NotEqual(e => e.FirstName).WithMessage("Last Name must be different from the First Name")
                .MaximumLength(50).WithMessage("Last Name cannot be longer than 50 characters");

            RuleFor(e => e.Birthdate)
                .NotEmpty().WithMessage("Birthdate is required")
                .Must(BeAValidAge).WithMessage("Age is not valid");

            RuleFor(e => e.EmploymentDate)
                .NotEmpty().WithMessage("Employment Date is required")
                .Must(BeAValidEmployeeDate).WithMessage("Employment Date is invalid, " +
                                        "Must be greater than 01-01-2000 and cannot be a future date");

            RuleFor(e => e.HomeAddress)
                .NotEmpty().WithMessage("Home Address is required");

            RuleFor(e => e.CurrentSalary)
                .NotEmpty().WithMessage("Current Salary is required")
                .Must(SalaryNonNegative).WithMessage("Current Salary must be non-negative");

            RuleFor(e => e.Role)
                .NotEmpty().WithMessage("Role is required")
                .MustAsync(async (role, _) =>
                {
                    var employeeCEO = await employeeRepo.GetByRole(RoleEnum.CEO.ToString());
                    if (employeeCEO != null) return false;
                    return true;
                }).WithMessage("There can be only 1 employee with CEO role")
                .When(e => RoleEnum.CEO.ToString().ToLower().Equals(e.Role.ToLower()))
                .Must(ValidCEORole).WithMessage("Role is not valid, Only the CEO role has no boss");

            RuleFor(e => e.BossId)
                .MustAsync(async (bossId, _) =>
                {
                    string pattern = "^[0-9a-fA-F]{24}$";
                    if (!Regex.IsMatch(bossId, pattern)) return false;

                    var employeeBoss = await employeeRepo.GetById(bossId);
                    if (employeeBoss == null) return false;

                    return true;
                }).WithMessage("There is no boss with this Id")
                .When(e => !string.IsNullOrEmpty(e.BossId));
        }

        protected bool ValidCEORole(Employee e, string role)
        {
            if (RoleEnum.CEO.ToString().ToLower().Equals(role.ToLower())) return true;
            else if (!RoleEnum.CEO.ToString().ToLower().Equals(role.ToLower()) && !string.IsNullOrEmpty(e.BossId)) return true;
            return false;
        }

        protected bool BeAValidAge(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int age = currentYear - date.Year;

            if (age == 18 && DateTime.Now.DayOfYear > date.DayOfYear) return true;
            else if (age == 70 && DateTime.Now.DayOfYear < date.DayOfYear) return true;
            else if (age > 18 && age < 70) return true;

            return false;
        }

        protected bool BeAValidEmployeeDate(DateTime date)
        {
            if (date.Year < 2000) return false;

            if (date.Date > DateTime.Now.Date) return false;

            return true;
        }

        protected bool SalaryNonNegative(decimal salary)
        {
            return salary >= 0;
        }

    }
}
