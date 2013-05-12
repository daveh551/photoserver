// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Configuration;
using PhotoServer.DataAccessLayer;
using PhotoServer.DataAccessLayer.Storage;
using PhotoServer.Domain;
using StructureMap;
namespace PhotoServer.DependencyResolution {
    public static class IoC
    {
        private const string PhotoPath = "PhotosPhysicalDirectory";
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
	                        x.For<IPhotoDataSource>()
	                         .Use(() => new EFPhotoServerDataSource("DefaultConnection"));
                            if (PhotosPhysicalPathExists())
                                x.For<IStorageProvider>()
                                 .Use(
                                     () =>
                                     new FileStorageProvider(ConfigurationManager.AppSettings[PhotoPath]));
                            else
                            {
	                            x.For<IStorageProvider>()
	                             .Use(
		                             () =>
				                             new AzureStorageProvider(
					                             ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString,
					                             "images")) ;
                            }

                        });
            return ObjectFactory.Container;
        }

        private static bool PhotosPhysicalPathExists()
        {
            return  (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[PhotoPath]));
            
        }
    }
}