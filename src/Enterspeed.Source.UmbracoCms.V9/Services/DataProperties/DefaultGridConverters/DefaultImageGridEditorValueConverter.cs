﻿using Enterspeed.Source.Sdk.Api.Models.Properties;
using Enterspeed.Source.UmbracoCms.V9.Models.Grid;
using Enterspeed.Source.UmbracoCms.V9.Providers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;

namespace Enterspeed.Source.UmbracoCms.V9.Services.DataProperties.DefaultGridConverters
{
    public class DefaultImageGridEditorValueConverter : IEnterspeedGridEditorValueConverter
    {
        private readonly IUmbracoMediaUrlProvider _umbracoMediaUrlProvider;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

        public DefaultImageGridEditorValueConverter(IUmbracoMediaUrlProvider umbracoMediaUrlProvider,
            IUmbracoHelperAccessor umbracoHelperAccessor)
        {
            _umbracoMediaUrlProvider = umbracoMediaUrlProvider;
            _umbracoHelperAccessor = umbracoHelperAccessor;
        }

        public bool IsConverter(string alias)
        {
            return alias.InvariantEquals("media");
        }

        public IEnterspeedProperty Convert(GridControl editor, string culture)
        {
            Dictionary<string, IEnterspeedProperty> properties = null;
            if (editor.Value != null && editor.Value.HasValues)
            {
                properties = new Dictionary<string, IEnterspeedProperty>();

                // Focalpoint
                var focalPoint = GetFocalPoint(editor.Value);
                if (focalPoint != null)
                {
                    properties.Add("focalPoint", focalPoint);
                }

                // ImageUrl
                var mediaId = editor.Value.Value<int>("id");

                if (_umbracoHelperAccessor.TryGetUmbracoHelper(out UmbracoHelper umbracoHelper))
                {
                    var media = umbracoHelper.Media(mediaId);
                    if (media != null)
                    {
                        var imageUrl = _umbracoMediaUrlProvider.GetUrl(media);
                        if (!string.IsNullOrWhiteSpace(imageUrl))
                        {
                            properties.Add("image", new StringEnterspeedProperty(imageUrl));
                        }
                    }
                }

                // AltText
                var altText = editor.Value.Value<string>("altText");
                if (!string.IsNullOrWhiteSpace(altText))
                {
                    properties.Add("altText", new StringEnterspeedProperty(altText));
                }

                // Caption
                var caption = editor.Value.Value<string>("caption");
                if (!string.IsNullOrWhiteSpace(caption))
                {
                    properties.Add("caption", new StringEnterspeedProperty(caption));
                }
            }

            return new ObjectEnterspeedProperty(properties);
        }

        private ObjectEnterspeedProperty GetFocalPoint(JToken value)
        {
            if (value == null || !value.HasValues)
            {
                return null;
            }

            var properties = new Dictionary<string, IEnterspeedProperty>();

            var focalPointProp = value.Value<JToken>("focalPoint");
            if (focalPointProp == null)
            {
                return null;
            }

            var leftProp = focalPointProp.Value<double?>("left");
            var topProp = focalPointProp.Value<double?>("top");

            if (!leftProp.HasValue || !topProp.HasValue)
            {
                return null;
            }

            properties.Add("left", new NumberEnterspeedProperty(leftProp.Value));
            properties.Add("top", new NumberEnterspeedProperty(topProp.Value));

            return new ObjectEnterspeedProperty(properties);
        }
    }
}
