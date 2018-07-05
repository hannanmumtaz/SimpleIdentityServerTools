#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.Extensions.Logging;
using SimpleIdentityServer.Logging;

namespace SimpleIdentityServer.Manager.Logging
{
    public interface IManagerEventSource : IEventSource
    {		
        #region Events linked to client

        void StartToRemoveClient(string clientId);

        void FinishToRemoveClient(string clientId);

        #endregion

        #region Events linked to resource owner

        void StartToRemoveResourceOwner(string subject);

        void FinishToRemoveResourceOwner(string subject);

        #endregion

        #region Events linked to Scopes

        void StartToRemoveScope(string scope);

        void FinishToRemoveScope(string scope);

        #endregion

        #region Events linked to export

        void StartToExport();

        void FinishToExport();

        #endregion

        #region Events linked to import

        void StartToImport();

        void RemoveAllClients();

        void FinishToImport();

        #endregion
    }

    public class ManagerEventSource : BaseEventSource, IManagerEventSource
    {
        private static class Tasks
        {
            public const string Client = "Client";
            public const string ResourceOwner = "ResourceOwner";
            public const string Scope = "Scope";
            public const string Failure = "Failure";
            public const string Export = "Export";
            public const string Import = "Import";
        }

        #region Constructor

        public ManagerEventSource(ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<ManagerEventSource>())
        {
        }

		#endregion		
		
        #region Events linked to client

        public void StartToRemoveClient(string clientId)
        {
            var evt = new Event
            {
                Id = 1,
                Task = Tasks.Client,
                Message = $"Start to remove the client : {clientId}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveClient(string clientId)
        {
            var evt = new Event
            {
                Id = 2,
                Task = Tasks.Client,
                Message = $"Finish to remove the client : {clientId}"
            };

            LogInformation(evt);
        }

        #endregion

        #region Events linked to resource owner

        public void StartToRemoveResourceOwner(string subject)
        {
            var evt = new Event
            {
                Id = 3,
                Task = Tasks.ResourceOwner,
                Message = $"Start to remove the resource owner: {subject}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveResourceOwner(string subject)
        {
            var evt = new Event
            {
                Id = 4,
                Task = Tasks.ResourceOwner,
                Message = $"Finish to remove the resource owner: {subject}"
            };

            LogInformation(evt);
        }

        #endregion

        #region Events linked to Scopes

        public void StartToRemoveScope(string scope)
        {
            var evt = new Event
            {
                Id = 5,
                Task = Tasks.Scope,
                Message = $"Start to remove the scope: {scope}"
            };

            LogInformation(evt);
        }

        public void FinishToRemoveScope(string scope)
        {
            var evt = new Event
            {
                Id = 6,
                Task = Tasks.Scope,
                Message = $"Finish to remove the scope: {scope}"
            };

            LogInformation(evt);
        }

        #endregion

        #region Events linked to export operations

        public void StartToExport()
        {
            var evt = new Event
            {
                Id = 7,
                Task = Tasks.Export,
                Message = $"Start to export"
            };

            LogInformation(evt);
        }

        public void FinishToExport()
        {
            var evt = new Event
            {
                Id = 8,
                Task = Tasks.Export,
                Message = $"Finish to export"
            };

            LogInformation(evt);
        }

        #endregion

        #region Events linked to import

        public void StartToImport()
        {
            var evt = new Event
            {
                Id = 9,
                Task = Tasks.Import,
                Message = $"Start to import"
            };

            LogInformation(evt);

        }

        public void RemoveAllClients()
        {
            var evt = new Event
            {
                Id = 10,
                Task = Tasks.Import,
                Message = $"All clients have been removed"
            };

            LogInformation(evt);
        }

        public void FinishToImport()
        {
            var evt = new Event
            {
                Id = 11,
                Task = Tasks.Import,
                Message = $"Import is finished"
            };

            LogInformation(evt);
        }

        #endregion
    }
}
