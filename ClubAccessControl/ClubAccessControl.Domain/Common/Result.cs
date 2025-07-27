using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Common
{
    public class Result
    {
        public bool Sucesso { get; private set; }
        public string? Erro { get; private set; }

        protected Result(bool sucesso, string? erro = null)
        {
            Sucesso = sucesso;
            Erro = erro;
        }

        public static Result Ok() => new Result(true);
        public static Result Fail(string erro) => new Result(false, erro);
    }

    public class Result<T> : Result
    {
        public T? Valor { get; private set; }

        private Result(bool sucesso, T? valor = default, string? erro = null) : base(sucesso, erro)
        {
            Valor = valor;
        }

        public static Result<T> Ok(T valor) => new Result<T>(true, valor);
        public static new Result<T> Fail(string erro) => new Result<T>(false, default, erro);

    }
}