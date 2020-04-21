﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiabloII.Application.Tests.Apis.Bases
{
    public interface IApiSearch<RequestDto, ResponseDto>
    {
        Task<IReadOnlyCollection<ResponseDto>> Search(RequestDto dto);
    }
}