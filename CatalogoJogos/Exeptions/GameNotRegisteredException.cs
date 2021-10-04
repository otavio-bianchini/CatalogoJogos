using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Exeptions
{
    public class GameNotRegisteredException : Exception
    {
        public GameNotRegisteredException()
            : base ("Jogo não cadastrado") 
        { }
    }
}
