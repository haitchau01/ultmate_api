﻿using Constracts;
using Entities.Models;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        }
        public Company? GetCompany(Guid companyId, bool trackChanges)
        {
           return FindByCondition(cp => cp.Id.Equals(companyId), trackChanges).SingleOrDefault();
        }

    }
}
