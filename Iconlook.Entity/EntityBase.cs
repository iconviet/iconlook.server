using Iconviet.Object;
using ReactiveUI;
using ServiceStack.DataAnnotations;
using System;

namespace Iconlook.Entity
{
    public abstract class EntityBase<T> : ObjectBase<T>, IEntity
    {
        
        [Ignore]
        public new IObservable<Exception> ThrownExceptions { get; set; }

        [Ignore]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed { get; set; }

        [Ignore]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing { get; set; }
    }
}