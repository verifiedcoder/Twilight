﻿using FluentValidation;
using Twilight.CQRS.Queries;
using Twilight.CQRS.Tests.Unit.Shared;

namespace Twilight.CQRS.Messaging.InMemory.Autofac.Tests.Integration.Setup.Validators
{
    public sealed class TestQueryValidator : AbstractValidator<Query<TestParameters, QueryResponse<string>>>
    {
    }
}
