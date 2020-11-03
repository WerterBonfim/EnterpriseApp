using System;
using FluentValidation.Results;
using MediatR;

namespace NSE.Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public Command()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; }
        public ValidationResult ValidationResult { get; set; }

        public virtual bool EValido()
        {
            throw new NotImplementedException();
        }
    }
}