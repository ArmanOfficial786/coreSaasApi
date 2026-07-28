namespace UserManagement.Application.ViewModels;

public record FilterDTO
{
    public uint PageNumber { get; set; } = 0;
    public uint PageSize { get; set; } = 20;
    public List<FilterParamDTO> Params { get; set; } = [];
    public List<SortParamDTO> Sort { get; set; } = [];

    public class Mapping : Profile
    {
        public Mapping()
        {
            _ = CreateMap<FilterDTO, Filter>();
        }
    }
}

public record FilterParamDTO
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int Option { get; set; }
    public FilterParamDTO(string key, string value, int option)
    {
        Key = key;
        Value = value;
        Option = option;
    }
    public class Mapping : Profile
    {
        public Mapping()
        {
            _ = CreateMap<FilterParamDTO, FilterParam>();
        }
    }
}

public record SortParamDTO
{
    public string Field { get; set; }
    public int SortOrder { get; set; }
    public SortParamDTO(string field, int sortOrder)
    {
        Field = field;
        SortOrder = sortOrder;
    }
    public class Mapping : Profile
    {
        public Mapping()
        {
            _ = CreateMap<SortParamDTO, SortParam>();
        }
    }
}

