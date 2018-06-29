using System;
using System.Collections.Generic;
using System.Linq;

using Kotoha.Plugins;

namespace Kotoha
{
    public class WpfControlMapper : IControlMapper2
    {
        private readonly Dictionary<Type, Dictionary<object, Role>> _mapper;

        public WpfControlMapper()
        {
            _mapper = new Dictionary<Type, Dictionary<object, Role>>();
        }

        public void Register<T>(object key, Role role)
        {
            if (!_mapper.ContainsKey(typeof(T)))
                _mapper.Add(typeof(T), new Dictionary<object, Role>());
            _mapper[typeof(T)].Add(key, role);
        }

        public void Register(object key, Role role)
        {
            throw new NotSupportedException();
        }

        public object FindByRole<T>(Role role)
        {
            return _mapper[typeof(T)].SingleOrDefault(w => w.Value == role).Key;
        }

        public object FindByRole(Role role)
        {
            throw new NotSupportedException();
        }
    }
}