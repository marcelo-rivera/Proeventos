using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testiculo.Application.Dtos;

namespace Testiculo.Application.Contratos
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}