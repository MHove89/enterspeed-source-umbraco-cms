using Enterspeed.Source.Sdk.Api.Models.Properties;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Enterspeed.Source.UmbracoCms.Services
{
    public class EnterspeedValidationService : IEnterspeedValidationService
    {
        private readonly ILogger<EnterspeedValidationService> _logger;

        public EnterspeedValidationService(ILogger<EnterspeedValidationService> logger)
        {
            _logger = logger;
        }

        public void LogValidationError(IEnterspeedProperty property)
        {
            if (!property.Validation.IsValid)
            {
                foreach (var error in property.Validation.Errors)
                {
                    _logger.LogError(error);
                }
            }
        }

        public void LogValidationErrors(IEnumerable<IEnterspeedProperty> enterspeedProperties)
        {
            foreach (var property in enterspeedProperties)
            {
                LogValidationError(property);
            }
        }
    }
}
