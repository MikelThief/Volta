﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using ElkaUWP.Infrastructure.Services;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Studia.Proxies
{
    public class LdapFormGradesStrategy : IGradesStrategy
    {
        private readonly IFlurlClient _restClient;

        private const string PlPathSegment = "pl/";

        public LdapFormGradesStrategy(IFlurlClientFactory flurlClientFactory)
        {
            _restClient = flurlClientFactory.Get(url: Constants.STUDIA_BASE_URL).EnableCookies();
        }

        /// <inheritdoc />
        public Task<HttpResponseMessage> GetAsync(string semesterLiteral, string subjectId)
        {
            var request = _restClient.Request().AppendPathSegment(segment: PlPathSegment)
                .AppendPathSegments($"{semesterLiteral.Substring(startIndex: 3)}/", subjectId, "api", "info");
            return request.GetAsync();
        }
    }
}