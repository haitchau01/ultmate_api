﻿using Application.Queries;
using AutoMapper;
using Constracts;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDTO>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GetCompaniesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CompanyDTO>> Handle(GetCompaniesQuery request,
        CancellationToken cancellationToken)
        {
            var companies = await
            _repository.Company.GetAllCompaniesAsync(request.TrackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDTO>>(companies);
            return companiesDto;
        }
    }
}
