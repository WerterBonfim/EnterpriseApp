using System;
using System.Collections.Generic;
using NSE.Core.Messages;

namespace NSE.Core.DomainObjects
{
    public abstract class Entity
    {
        private List<Event> _notificações;

        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public IReadOnlyCollection<Event> Notificações => _notificações?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            _notificações ??= new List<Event>();
            _notificações.Add(evento);
        }

        public void RemoverEvento(Event evento)
        {
            _notificações?.Remove(evento);
        }

        public void LimparEventos()
        {
            _notificações?.Clear();
        }

        #region [ Comparações ]

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return false;
            if (ReferenceEquals(null, compareTo)) return true;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        #endregion
    }
}