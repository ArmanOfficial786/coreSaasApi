using UserManagement.Domain.Enum;

namespace UserManagement.Application.ViewModels;

public class ModulePermissionViewModel
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PermissionEnum Permission { get; set; }

    // Optional: Include Module name for display if needed
    public string? ModuleName { get; set; }

    public class Mapping : Profile
    {
        public Mapping()
        {
            // ─── From ModulePermission (Direct) ───
            CreateMap<ModulePermission, ModulePermissionViewModel>()
                .ForMember(
                    dest => dest.ModuleName,
                    opts => opts.MapFrom(src =>
                        src.Module != null ? src.Module.Name : null
                    )
                );

            // ─── From RoleModulePermission (System Role Assignment) ───
            CreateMap<RoleModulePermission, ModulePermissionViewModel>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.ModulePermissionId)
                )
                .ForMember(
                    dest => dest.ModuleId,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null ? src.ModulePermission.ModuleId : Guid.Empty
                    )
                )
                .ForMember(
                    dest => dest.Permission,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null ? src.ModulePermission.Permission : default
                    )
                )
                .ForMember(
                    dest => dest.ModuleName,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null && src.ModulePermission.Module != null
                            ? src.ModulePermission.Module.Name
                            : null
                    )
                );

            // ─── From UserModulePermission (User-Specific Assignment) ───
            CreateMap<UserModulePermission, ModulePermissionViewModel>()
                .ForMember(
                    dest => dest.Id,
                    opts => opts.MapFrom(src => src.ModulePermissionId)
                )
                .ForMember(
                    dest => dest.ModuleId,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null ? src.ModulePermission.ModuleId : Guid.Empty
                    )
                )
                .ForMember(
                    dest => dest.Permission,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null ? src.ModulePermission.Permission : default
                    )
                )
                .ForMember(
                    dest => dest.ModuleName,
                    opts => opts.MapFrom(src =>
                        src.ModulePermission != null && src.ModulePermission.Module != null
                            ? src.ModulePermission.Module.Name
                            : null
                    )
                );
        }
    }
}


public class ModulePermissionGroupViewModel
{
    public Guid ModuleId { get; init; }
    public string ModuleName { get; init; } = null!;
    public List<ModulePermissionViewModel> Permissions { get; init; } = [];
}
