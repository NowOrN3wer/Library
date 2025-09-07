using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.OpenApi.Models;
// <-- ControllerActionDescriptor
// <-- FeatureGateAttribute

namespace Library.WebAPI.OpenApi;

public sealed class FeatureGateTransformer(IFeatureManagerSnapshot fm)
    : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument doc, OpenApiDocumentTransformerContext ctx, CancellationToken token)
    {
        var provider = ctx.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
        var apiDescs = provider.ApiDescriptionGroups.Items.SelectMany(g => g.Items).ToList();

        var toRemove = new List<(string path, OperationType op)>();

        foreach (var (path, item) in doc.Paths)
        foreach (var opKvp in item.Operations.ToList())
        {
            var http = opKvp.Key.ToString().ToUpperInvariant();

            var match = apiDescs.FirstOrDefault(d =>
                ("/" + d.RelativePath?.TrimEnd('/')).Equals(path, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.HttpMethod, http, StringComparison.OrdinalIgnoreCase));

            if (match is null) continue;

            // ✨ FeatureGate attribute’unu action/controller/endpoint metadata’dan çek
            var gate = ResolveFeatureGate(match);
            if (gate is null) continue;

            var enabled = gate.Features.All(f => fm.IsEnabledAsync(f).GetAwaiter().GetResult());
            if (!enabled) toRemove.Add((path, opKvp.Key));
        }

        foreach (var (path, op) in toRemove)
            if (doc.Paths.TryGetValue(path, out var item))
            {
                item.Operations.Remove(op);
                if (item.Operations.Count == 0) doc.Paths.Remove(path);
            }

        return Task.CompletedTask;
    }

    private static FeatureGateAttribute? ResolveFeatureGate(ApiDescription d)
    {
        // 1) Endpoint metadata (minimal API + MVC)
        var fromEndpoint = d.ActionDescriptor.EndpointMetadata?
            .OfType<FeatureGateAttribute>().FirstOrDefault();
        if (fromEndpoint is not null) return fromEndpoint;

        // 2) MVC action attribute
        if (d.ActionDescriptor is ControllerActionDescriptor cad)
        {
            var fromAction = cad.MethodInfo
                .GetCustomAttributes(typeof(FeatureGateAttribute), true)
                .OfType<FeatureGateAttribute>()
                .FirstOrDefault();
            if (fromAction is not null) return fromAction;

            // 3) Controller attribute
            var fromController = cad.ControllerTypeInfo
                .GetCustomAttributes(typeof(FeatureGateAttribute), true)
                .OfType<FeatureGateAttribute>()
                .FirstOrDefault();
            return fromController;
        }

        return null;
    }
}