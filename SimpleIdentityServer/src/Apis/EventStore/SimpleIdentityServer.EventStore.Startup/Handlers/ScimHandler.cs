using Newtonsoft.Json.Linq;
using SimpleBus.Core;
using SimpleIdentityServer.EventStore.Core.Repositories;
using SimpleIdentityServer.Scim.Events;
using System;

namespace SimpleIdentityServer.EventStore.Startup.Handlers
{
    public class ScimHandler : IEventHandler<AddGroupFinished>, IEventHandler<AddGroupReceived>, IEventHandler<AddUserFinished>, IEventHandler<AddUserReceived>,
        IEventHandler<PatchGroupFinished>, IEventHandler<PatchGroupReceived>, IEventHandler<PatchUserFinished>, IEventHandler<PatchUserReceived>,
        IEventHandler<RemoveGroupFinished>, IEventHandler<RemoveGroupReceived>, IEventHandler<RemoveUserFinished>, IEventHandler<RemoveUserReceived>,
        IEventHandler<ScimErrorReceived>, IEventHandler<UpdateGroupFinished>, IEventHandler<UpdateGroupReceived>, IEventHandler<UpdateUserFinished>,
        IEventHandler<UpdateUserReceived>
    {
        private readonly IEventAggregateRepository _repository;

        public ScimHandler(IEventAggregateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AddUserReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start add user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "add_user_started",
            }).Wait();
        }

        public void Handle(AddUserFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish add user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "add_user_finished",
            }).Wait();
        }

        public void Handle(UpdateUserReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to update the user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "update_user_started",
            }).Wait();
        }

        public void Handle(UpdateUserFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to update the user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "update_user_finished",
            }).Wait();
        }

        public void Handle(RemoveUserReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to remove user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "remove_user_started",
            }).Wait();
        }

        public void Handle(RemoveUserFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to remove user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "remove_user_finished",
            }).Wait();
        }

        public void Handle(PatchUserReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to patch user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "patch_user_started",
            }).Wait();
        }

        public void Handle(PatchUserFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to patch user",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "patch_user_finished",
            }).Wait();
        }

        public void Handle(AddGroupReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start add group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "add_group_started",
            }).Wait();
        }

        public void Handle(AddGroupFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish add group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "add_group_finished",
            }).Wait();
        }

        public void Handle(UpdateGroupReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to update the group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "update_group_started",
            }).Wait();
        }

        public void Handle(UpdateGroupFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to update the group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "update_group_finished",
            }).Wait();
        }

        public void Handle(RemoveGroupReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to remove group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "remove_group_started",
            }).Wait();
        }

        public void Handle(RemoveGroupFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to remove group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "remove_group_finished",
            }).Wait();
        }

        public void Handle(PatchGroupReceived evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Start to patch group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "patch_group_started",
            }).Wait();
        }

        public void Handle(PatchGroupFinished evt)
        {
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                Description = "Finish to patch group",
                CreatedOn = DateTime.UtcNow,
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Information,
                Key = "patch_group_finished",
            }).Wait();
        }

        public void Handle(ScimErrorReceived evt)
        {
            var jObj = new JObject();
            jObj.Add("error", evt.Message);
            _repository.Add(new Core.Models.EventAggregate
            {
                Id = evt.Id,
                AggregateId = evt.ProcessId,
                CreatedOn = DateTime.UtcNow,
                Description = "An error occured",
                Payload = jObj.ToString(),
                Order = evt.Order,
                Type = evt.ServerName,
                Verbosity = Core.Models.EventVerbosities.Error,
                Key = "error"
            }).Wait();
        }
    }
}
