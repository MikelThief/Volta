﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary;
using ElkaUWP.DataLayer.Propertiary.Services;
using ElkaUWP.DataLayer.Studia.Abstractions.Interfaces;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.Proxies;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using Flurl.Http.Configuration;
using Prism.Ioc;
using Prism.Modularity;
using TimetableService = ElkaUWP.DataLayer.Usos.Services.TimetableService;

namespace ElkaUWP.DataLayer
{
    public class DataLayerInitializer : IModule
    {
        /// <inheritdoc />
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register USOS request wrappers
            containerRegistry.RegisterSingleton<TimetableStudentRequestWrapper>();
            containerRegistry.RegisterSingleton<BuildingIndexRequestWrapper>();
            containerRegistry.RegisterSingleton<TimetableUpcomingICalRequestWrapper>();
            containerRegistry.RegisterSingleton<TimetableUpcomingWebCalRequestWrapper>();
            containerRegistry.RegisterSingleton<UserInfoRequestWrapper>();

            // Register services
            containerRegistry.RegisterSingleton<TimetableService>();
            containerRegistry.RegisterSingleton<UserService>();

            // Register proxies
            containerRegistry.RegisterSingleton<IGradesProxy, LdapFormGradesProxy>();

            //Register other types
            containerRegistry.RegisterSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        }

        /// <inheritdoc />
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }
    }
}
