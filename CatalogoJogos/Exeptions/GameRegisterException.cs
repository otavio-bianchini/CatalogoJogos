using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Exeptions
{
    public class GameRegisterException : Exception
    {
        public GameRegisterException()
            : base("Este jogo já está cadastrado") 
        { }
    }
}
