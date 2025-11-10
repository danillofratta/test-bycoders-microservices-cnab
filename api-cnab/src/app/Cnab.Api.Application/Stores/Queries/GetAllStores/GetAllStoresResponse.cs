using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cnab.Api.Application.Stores.Queries.GetAllStores;
public sealed record GetAllStoresResponse(string Name, string Owner, decimal Balance);