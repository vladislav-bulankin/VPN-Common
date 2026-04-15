using BlackProjects.Common.Services.Network;
using Microsoft.Extensions.DependencyInjection;

namespace BlackProjects.Common.Services.Extensions; 
public static class ServicesExtensions {
    public static void UseCommonServices(this IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<IDataReassembler>(
            _ => new DataReassembler()
        );
    }
}
