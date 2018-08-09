using Newtonsoft.Json;
using SimpleBus.Core;
using SimpleIdentityServer.EventStore.Core.Repositories;
using SimpleIdentityServer.OAuth.Events;
using System;

namespace SimpleIdentityServer.EventStore.Startup.Handlers
{
    public class OauthHandler : IEventHandler<TokenGranted>, IEventHandler<AuthorizationGranted>, IEventHandler<AuthorizationRequestReceived>,
        IEventHandler<GrantTokenViaAuthorizationCodeReceived>, IEventHandler<GrantTokenViaClientCredentialsReceived>, IEventHandler<GrantTokenViaRefreshTokenReceived>,
        IEventHandler<GrantTokenViaResourceOwnerCredentialsReceived>, IEventHandler<IntrospectionRequestReceived>, IEventHandler<IntrospectionResultReturned>,
        IEventHandler<OAuthErrorReceived>, IEventHandler<RegistrationReceived>, IEventHandler<RegistrationResultReceived>, IEventHandler<RevokeTokenReceived>, IEventHandler<TokenRevoked>
    {
        private readonly IEventAggregateRepository _repository;

        public OauthHandler(IEventAggregateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AuthorizationRequestReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start authorization process",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "auth_process_started"
            }).Wait();
        }

        public void Handle(AuthorizationGranted evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Authorization granted",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "auth_granted"
            }).Wait();
        }

        public void Handle(IntrospectionRequestReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start introspection",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "introspection_started"
            }).Wait();
        }

        public void Handle(IntrospectionResultReturned evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Introspection result",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "introspection_finished"
            }).Wait();
        }

        public void Handle(RegistrationReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start client registration",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "register_client_started"
            }).Wait();
        }

        public void Handle(RegistrationResultReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Client registered",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "register_client_finished"
            }).Wait();
        }

        public void Handle(GrantTokenViaAuthorizationCodeReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start grant token via authorization code",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "auth_code_grantype_started"
            }).Wait();
        }

        public void Handle(GrantTokenViaClientCredentialsReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start grant token via client credentials",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "client_credentials_grantype_started"
            }).Wait();
        }

        public void Handle(GrantTokenViaRefreshTokenReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start grant token via refresh token",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "refresh_token_grantype_started"
            }).Wait();
        }

        public void Handle(GrantTokenViaResourceOwnerCredentialsReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start grant token via resource owner credentials",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "resource_owner_grantype_started"
            }).Wait();
        }

        public void Handle(RevokeTokenReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start revoke token",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "revoke_token_started"
            }).Wait();
        }

        public void Handle(TokenGranted evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Token granted",
                CreatedOn = DateTime.UtcNow,
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "token_granted"
            }).Wait();
        }

        public void Handle(TokenRevoked evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Token revoked",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "token_revoked"
            }).Wait();
        }

        public void Handle(OAuthErrorReceived evt)
        {
            var payload = JsonConvert.SerializeObject(new
            {
                Code = evt.Code,
                Message = evt.Message,
                State = evt.State
            });
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                CreatedOn = DateTime.UtcNow,
                Description = "An error occured",
                Payload = payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Error,
                Key = "error"
            }).Wait();
        }
    }
}
