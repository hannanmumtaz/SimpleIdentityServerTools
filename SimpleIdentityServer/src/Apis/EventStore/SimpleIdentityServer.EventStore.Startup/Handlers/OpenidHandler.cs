using Newtonsoft.Json;
using SimpleBus.Core;
using SimpleIdentityServer.EventStore.Core.Repositories;
using SimpleIdentityServer.OpenId.Events;
using System;

namespace SimpleIdentityServer.EventStore.Startup.Handlers
{
    public class OpenidHandler : IEventHandler<GetUserInformationReceived>, IEventHandler<UserInformationReturned>, IEventHandler<OpenIdErrorReceived>, 
        IEventHandler<ResourceOwnerAuthenticated>, IEventHandler<ConsentAccepted>, IEventHandler<ConsentRejected>
    {
        private readonly IEventAggregateRepository _repository;

        public OpenidHandler(IEventAggregateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(GetUserInformationReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start user information",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "get_user_info_started",
            }).Wait();
        }

        public void Handle(UserInformationReturned evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "User information returned",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "get_user_info_finished",
            }).Wait();
        }

        public void Handle(ResourceOwnerAuthenticated evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.AggregateId,
                Payload = evt.Payload,
                Description = "Resource owner is authenticated",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "resource_owner_auth",
            }).Wait();
        }

        public void Handle(ConsentAccepted evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                CreatedOn = DateTime.UtcNow,
                Description = "Consent accepted",
                Payload = evt.Payload,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "consent_accepted"
            }).Wait();
        }

        public void Handle(ConsentRejected evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                CreatedOn = DateTime.UtcNow,
                Description = "Consent rejected",
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "consent_rejected"
            }).Wait();
        }

        public void Handle(OpenIdErrorReceived evt)
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
