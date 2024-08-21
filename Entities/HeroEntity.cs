using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public class HeroEntity : AbstractEntity
    {
        private int _speed;
        public HeroEntity() 
        {
            _speed = 12;
        }
        public override int Speed 
        {
            get => _speed;
            set => _speed = value;
        }
    }
}
