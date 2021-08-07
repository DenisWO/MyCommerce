using FluentValidation;
using FluentValidation.Results;
using MyCommerce.Business.Models;
using MyCommerce.Business.Notifications;

namespace MyCommerce.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;
        public BaseService(INotificator notificator)
        {
            _notificator = notificator;
        }
        protected void Notify(string message)
        {
            _notificator.Handle(new Notification(message));
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected bool ExecuteValidation<TValidation, TEntity>(TValidation validation, TEntity entity) 
            where TValidation : AbstractValidator<TEntity> where TEntity : Entity
        {
            var validator = validation.Validate(entity);

            if (!validator.IsValid)
                Notify(validator);

            return true;
        }
    }

}
