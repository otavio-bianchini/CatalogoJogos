using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public string Productor { get; set; }
        public double Price { get; set; }
    }
}
